
using AutoMapper;
using backEnd.Dto;
using backEnd.Models;
using static backEnd.Services.BrandService.Commands.CreateBrandService;
using static backEnd.Services.CarService.Commands.CreateCarService;

namespace backEnd.Configs {

    public class AutoMapperConfig : Profile{
        
        public AutoMapperConfig() {
            
            // Mappers para brand
            CreateMap<BrandResponse, Brand>().ReverseMap();
            CreateMap<Brand, CreateBrandCommand>().ReverseMap();

            // Mappers para Payment
            CreateMap<PaymentFormat, PaymentResponse>().ReverseMap();

            // Mapper para car
            CreateMap<Car, CreateCarCommand>().ReverseMap();
            CreateMap<Car, CarResponse>()
            .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.BrandNavigation.BrandName))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.TypeNavigation.Type))
            .ForMember(dest => dest.Format, opt => opt.MapFrom(src => src.FormatNavigation.Format))
            .ReverseMap();

            // Maper para carType
            CreateMap<CarTypeResponse, CarType>().ReverseMap();

        }
    }
}