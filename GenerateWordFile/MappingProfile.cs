using AutoMapper;
using GenerateWordFile.Helpers;
using GenerateWordFile.Models;
using GenerateWordFile.ViewModels;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<PersonVM, Person>()
            .ForMember(dest=>dest.Birthday, opt=> opt.MapFrom(src=> src.Birthdate))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName.Capitalize()))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName.Capitalize()))
            .ForMember(dest => dest.FatherName, opt => opt.MapFrom(src => src.FatherName.Capitalize()))
            .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.gender.ToString()));
    }
}
