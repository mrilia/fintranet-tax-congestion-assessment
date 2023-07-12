using System;
using System.Collections.Generic;
using System.Linq;
using Congestion.Tax;
using Congestion.Tax.Business.Configs;
using Congestion.Tax.Business.Models.Entrance;
using Congestion.Tax.Business.Models.Tax;
using Congestion.Tax.Business.Models.Vehicles;
using  Congestion.Tax.Business.Utils;

namespace Congestion.Tax.Business {
    public class CongestionTaxCalculator : ITaxCalculator
    {
        TaxRulesConfiguration _taxConfigs;
        private readonly IEntranceRepository _entranceRepo;

        public CongestionTaxCalculator(IEntranceRepository entranceRepo)
        {
            _taxConfigs = TaxConfigurationUtility.GetConfigs();
            _entranceRepo = entranceRepo;
        }

        public double GetTax(TaxCalculationRequest request)
        {
            var entrances = _entranceRepo?.GetAllAsync()?.Result
                ?.Where(i =>
                            i.Datetime >= request.From &&
                            i.Datetime <= request.To &&
                            i.VehicleNumberPlate.Trim().ToLower() == request.VehicleNumberPlate.Trim().ToLower())
                ?.ToList()
                ?? new List<Entrance>();

            var result = entrances.GroupBy(g => g.VehicleType).Sum(vehicleType =>
            {
                return CalculateTax(vehicleType.Key, entrances.Where(i => i.VehicleType == vehicleType.Key)?.Select(s => s.Datetime)?.ToList() ?? new List<DateTime>());
            });

            return result;
        }


        private double CalculateTax(VehicleType vehicleType, List<DateTime> entrances)
        {
            if (entrances == null || entrances.Count == 0 || IsTaxFreeVehicle(vehicleType))
                return 0;

            var dailyEntrances = entrances.OrderBy(e => e.Date)
                .ThenBy(o => o.TimeOfDay)
                .GroupBy(e => e.Date)
                .ToList();

            double totalFee = dailyEntrances.Sum(entranceGroup =>
            {
                DateTime intervalStart = entranceGroup.First();

                if (IsTaxFreeDate(intervalStart))
                    return 0.0;

                double totalFeeInDay = entranceGroup.Aggregate(0.0, (fee, currentEntrance) =>
                {
                    var tempFee = GetTaxOfEntrance(intervalStart);
                    var nextFee = GetTaxOfEntrance(currentEntrance);

                    double diffInMinutes = (currentEntrance - intervalStart).TotalMinutes;

                    if (diffInMinutes <= _taxConfigs.MultipleTaxStationPassingDurationInMinute)
                    {
                        if (fee > 0)
                            fee -= tempFee;

                        tempFee = Math.Max(tempFee, nextFee);
                        fee += tempFee;
                    }
                    else
                    {
                        fee += nextFee;
                        intervalStart = currentEntrance;
                    }

                    return fee;
                });

                return Math.Min(totalFeeInDay, _taxConfigs.MaxTaxPerDay);
            });

            return totalFee;
        }

        private bool IsTaxFreeVehicle(VehicleType vehicleType)
        {
            return _taxConfigs.TaxFreeVehicles?.Any(a => a.Trim().ToLower() == vehicleType.ToString().ToLower()) ?? false;
        }

        private double GetTaxOfEntrance(DateTime date)
        {
            return _taxConfigs?.TaxFees?.FirstOrDefault(i => date.TimeOfDay >= i.From && date.TimeOfDay <= i.To)?.Fee ?? 0.0;
        }

        private Boolean IsTaxFreeDate(DateTime date)
        {
            return DateUtility.IsWeekend(date) ||
                    DateUtility.IsInFreeMonth(date) ||
                    DateUtility.IsPublicHolyDay(date) ||
                    DateUtility.IsDayBeforeOfPublicHolyday(date);
        }
    }
}