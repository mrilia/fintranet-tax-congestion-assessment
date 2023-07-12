using AutoMapper;
using Congestion.Tax.API.MapperProfile;
using Congestion.Tax.Business;
using Congestion.Tax.Business.Models.Entrance;
using Congestion.Tax.Business.Models.Tax;
using Congestion.Tax.Business.Models.Vehicles;
using Congestion.Tax.Persistence.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Congestion.Tax.Tests
{
    public class TaxCalculationTests
    {
        List<Entrance> sampleEntrances;
        IEntranceRepository _entranceRepo;
        ITaxCalculator _taxCalculator;
        private readonly IServiceProvider _serviceProvider;

        public TaxCalculationTests()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<EntranceProfile>();
            });

            IMapper mapper = mapperConfig.CreateMapper();

            var services = new ServiceCollection();
            services.AddSingleton(mapper);
            services.AddSingleton<IEntranceRepository, EntranceRepository>();
            services.AddSingleton<ITaxCalculator, CongestionTaxCalculator>();

            services.AddDbContext<TaxDbContext>(options =>
                options.UseInMemoryDatabase(databaseName: "MyDatabaseName"));

            _serviceProvider = services.BuildServiceProvider();
        }

        TaxCalculationRequest sampleBusTaxRequest = new TaxCalculationRequest
        {
            From = new DateTime(2013, 1, 1, 0, 0, 1),
            To = new DateTime(2014, 1, 1, 23, 59, 59),
            VehicleNumberPlate = "B123"
        };

        TaxCalculationRequest sampleCarTaxRequest = new TaxCalculationRequest
        {
            From = new DateTime(2013, 1, 1, 0, 0, 1),
            To = new DateTime(2014, 1, 1, 23, 59, 59),
            VehicleNumberPlate = "A123"
        };

        TaxCalculationRequest sampleTruckTaxRequest = new TaxCalculationRequest
        {
            From = new DateTime(2013, 1, 1, 0, 0, 1),
            To = new DateTime(2014, 1, 1, 23, 59, 59),
            VehicleNumberPlate = "T123"
        };

        TaxCalculationRequest sampleCarTaxRequestInHolyDay = new TaxCalculationRequest
        {
            From = new DateTime(2013, 12, 25, 0, 0, 1),
            To = new DateTime(2014, 12, 26, 23, 59, 59),
            VehicleNumberPlate = "A123"
        };
        
        TaxCalculationRequest sampleTruckTaxRequestInDayBeforeHolyDay = new TaxCalculationRequest
        {
            From = new DateTime(2013, 12, 24, 0, 0, 1),
            To = new DateTime(2014, 12, 26, 23, 59, 59),
            VehicleNumberPlate = "T123"
        };

        [SetUp]
        public void Setup()
        {
            _entranceRepo = _serviceProvider.GetService<IEntranceRepository>();
            _taxCalculator = _serviceProvider.GetService<ITaxCalculator>(); 

            sampleEntrances = new List<Entrance> {
                // Car Entrance
                new Entrance
                {
                    Datetime = DateTime.ParseExact("2013-01-14 21:00:00", "yyyy-MM-dd HH:mm:ss", null),//+0 >>>> 0
                    VehicleNumberPlate = "A123",
                    VehicleType = VehicleType.Car
                },
                new Entrance
                {
                    Datetime = DateTime.ParseExact("2013-01-15 21:00:00", "yyyy-MM-dd HH:mm:ss", null),//+0 >>>> 0
                    VehicleNumberPlate = "A123",
                    VehicleType = VehicleType.Car
                },
                new Entrance
                {
                    Datetime = DateTime.ParseExact("2013-02-07 06:23:27", "yyyy-MM-dd HH:mm:ss", null),//+8
                    VehicleNumberPlate = "A123",
                    VehicleType = VehicleType.Car
                },
                new Entrance
                {
                    Datetime = DateTime.ParseExact("2013-02-07 15:27:00", "yyyy-MM-dd HH:mm:ss", null),//+13 >>>> 21
                    VehicleNumberPlate = "A123",
                    VehicleType = VehicleType.Car
                },
                new Entrance
                {
                    Datetime = DateTime.ParseExact("2013-02-08 06:20:27", "yyyy-MM-dd HH:mm:ss", null),//8
                    VehicleNumberPlate = "A123",
                    VehicleType = VehicleType.Car
                },
                new Entrance
                {
                    Datetime = DateTime.ParseExact("2013-02-08 06:27:00", "yyyy-MM-dd HH:mm:ss", null),//+8
                    VehicleNumberPlate = "A123",
                    VehicleType = VehicleType.Car
                },
                new Entrance
                {
                    Datetime = DateTime.ParseExact("2013-02-08 14:35:00", "yyyy-MM-dd HH:mm:ss", null),//8
                    VehicleNumberPlate = "A123",
                    VehicleType = VehicleType.Car
                },
                new Entrance
                {
                    Datetime = DateTime.ParseExact("2013-02-08 15:29:00", "yyyy-MM-dd HH:mm:ss", null),//+13
                    VehicleNumberPlate = "A123",
                    VehicleType = VehicleType.Car
                },
                new Entrance
                {
                    Datetime = DateTime.ParseExact("2013-02-08 15:47:00", "yyyy-MM-dd HH:mm:ss", null),//18
                    VehicleNumberPlate = "A123",
                    VehicleType = VehicleType.Car
                },
                new Entrance
                {
                    Datetime = DateTime.ParseExact("2013-02-08 16:01:00", "yyyy-MM-dd HH:mm:ss", null),//+18
                    VehicleNumberPlate = "A123",
                    VehicleType = VehicleType.Car
                },
                new Entrance
                {
                    Datetime = DateTime.ParseExact("2013-02-08 16:48:00", "yyyy-MM-dd HH:mm:ss", null),//+18
                    VehicleNumberPlate = "A123",
                    VehicleType = VehicleType.Car
                },
                new Entrance
                {
                    Datetime = DateTime.ParseExact("2013-02-08 17:49:00", "yyyy-MM-dd HH:mm:ss", null),//+13
                    VehicleNumberPlate = "A123",
                    VehicleType = VehicleType.Car
                },
                new Entrance
                {
                    Datetime = DateTime.ParseExact("2013-02-08 18:29:00", "yyyy-MM-dd HH:mm:ss", null),//8
                    VehicleNumberPlate = "A123",
                    VehicleType = VehicleType.Car
                },
                new Entrance
                {
                    Datetime = DateTime.ParseExact("2013-02-08 18:35:00", "yyyy-MM-dd HH:mm:ss", null),//0 >>>> 70 => 60
                    VehicleNumberPlate = "A123",
                    VehicleType = VehicleType.Car
                },
                new Entrance
                {
                    Datetime = DateTime.ParseExact("2013-03-26 14:25:00", "yyyy-MM-dd HH:mm:ss", null),//+8 >>>> 8
                    VehicleNumberPlate = "A123",
                    VehicleType = VehicleType.Car
                },
                new Entrance
                {
                    Datetime = DateTime.ParseExact("2013-03-28 14:07:27", "yyyy-MM-dd HH:mm:ss", null),//+8 >>>> 0 Day Before Holyday
                    VehicleNumberPlate = "A123",
                    VehicleType = VehicleType.Car
                },

                // Bus Entrance
                new Entrance
                {
                    Datetime = DateTime.ParseExact("2013-01-07 06:23:27", "yyyy-MM-dd HH:mm:ss", null),
                    VehicleNumberPlate = "B123",
                    VehicleType = VehicleType.Bus
                },
                new Entrance
                {
                    Datetime = DateTime.ParseExact("2013-01-07 15:27:00", "yyyy-MM-dd HH:mm:ss", null),
                    VehicleNumberPlate = "B123",
                    VehicleType = VehicleType.Bus
                },
                new Entrance
                {
                    Datetime = DateTime.ParseExact("2013-01-08 06:20:27", "yyyy-MM-dd HH:mm:ss", null),
                    VehicleNumberPlate = "B123",
                    VehicleType = VehicleType.Bus
                },
                new Entrance
                {
                    Datetime = DateTime.ParseExact("2013-01-08 06:27:00", "yyyy-MM-dd HH:mm:ss", null),
                    VehicleNumberPlate = "B123",
                    VehicleType = VehicleType.Bus
                },
                new Entrance
                {
                    Datetime = DateTime.ParseExact("2013-02-08 14:35:00", "yyyy-MM-dd HH:mm:ss", null),
                    VehicleNumberPlate = "B123",
                    VehicleType = VehicleType.Bus
                },
                new Entrance
                {
                    Datetime = DateTime.ParseExact("2013-03-08 15:29:00", "yyyy-MM-dd HH:mm:ss", null),
                    VehicleNumberPlate = "B123",
                    VehicleType = VehicleType.Bus
                },
                new Entrance
                {
                    Datetime = DateTime.ParseExact("2013-04-08 15:47:00", "yyyy-MM-dd HH:mm:ss", null),
                    VehicleNumberPlate = "B123",
                    VehicleType = VehicleType.Bus
                },

                //Truck Entrance
                new Entrance
                {
                    Datetime = DateTime.ParseExact("2013-02-08 16:48:00", "yyyy-MM-dd HH:mm:ss", null),//+18
                    VehicleNumberPlate = "T123",
                    VehicleType = VehicleType.Truck
                },
                new Entrance
                {
                    Datetime = DateTime.ParseExact("2013-02-08 17:49:00", "yyyy-MM-dd HH:mm:ss", null),//+13
                    VehicleNumberPlate = "T123",
                    VehicleType = VehicleType.Truck
                },
                new Entrance
                {
                    Datetime = DateTime.ParseExact("2013-02-08 18:29:00", "yyyy-MM-dd HH:mm:ss", null),//8
                    VehicleNumberPlate = "T123",
                    VehicleType = VehicleType.Truck
                },
                new Entrance
                {
                    Datetime = DateTime.ParseExact("2013-02-08 18:35:00", "yyyy-MM-dd HH:mm:ss", null),//0 
                    VehicleNumberPlate = "T123",
                    VehicleType = VehicleType.Truck
                },
                new Entrance
                {
                    Datetime = DateTime.ParseExact("2013-03-26 14:25:00", "yyyy-MM-dd HH:mm:ss", null),//+8 >>>> 8
                    VehicleNumberPlate = "T123",
                    VehicleType = VehicleType.Truck
                },
                new Entrance
                {
                    Datetime = DateTime.ParseExact("2013-12-24 14:07:27", "yyyy-MM-dd HH:mm:ss", null),//+8
                    VehicleNumberPlate = "T123",
                    VehicleType = VehicleType.Truck
                },
            };

            foreach (var entrance in sampleEntrances)
            {
                _entranceRepo.AddAsync(entrance);
            }
        }

        [Test]
        public void CalculateTaxExemptVehicles_ShouldBeZero()
        {
            //arrange

            //act
            var busTax = _taxCalculator.GetTax(sampleBusTaxRequest);

            //assert
            Assert.That(0, Is.EqualTo(busTax));
        }

        [Test]
        public void CalculateTaxForHolydays_ShouldBeZero()
        {
            //arrange

            //act
            var tax = _taxCalculator.GetTax(sampleCarTaxRequestInHolyDay);

            //assert
            Assert.That(0, Is.EqualTo(tax));
        }

        [Test]
        public void CalculateTaxForDayBeforeHolydays_ShouldBeZero()
        {
            //arrange

            //act
            var tax = _taxCalculator.GetTax(sampleTruckTaxRequestInDayBeforeHolyDay);

            //assert
            Assert.That(0, Is.EqualTo(tax));
        }

        [Test]
        public void CalculateTaxForMultipleDays_ShouldBeCorrect_TestCase5()
        {
            //arrange

            //act
            var tax = _taxCalculator.GetTax(sampleCarTaxRequest);

            //assert
            Assert.That(tax, Is.EqualTo(89.0));
        }
    }

}