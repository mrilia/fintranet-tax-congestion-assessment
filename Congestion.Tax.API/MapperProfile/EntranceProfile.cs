using AutoMapper;
using Congestion.Tax.API.Models;
using Congestion.Tax.Business.Models.Entrance;
using Congestion.Tax.Business.Models.Tax;
using Congestion.Tax.Business.Models.Vehicles;

namespace Congestion.Tax.API.MapperProfile
{
    public class EntranceProfile : Profile
    {
        public EntranceProfile()
        {
            CreateMap<TaxCalculationRequestDto, TaxCalculationRequest>();

            CreateMap<Entrance, EntranceDto>()
                .ForMember(dest => dest.VehicleType, opt => opt.MapFrom(src => src.VehicleType.ToString()));

            CreateMap<EntranceDto, Entrance>()
                .ForMember(dest => dest.VehicleType, opt => opt.MapFrom(src => Enum.Parse(typeof(VehicleType), src.VehicleType)));
        }
    }
}
