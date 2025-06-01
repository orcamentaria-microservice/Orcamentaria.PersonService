using AutoMapper;
using Orcamentaria.PersonService.Domain.DTOs.Address;
using Orcamentaria.PersonService.Domain.Models;

namespace Orcamentaria.PersonService.Domain.Mappers
{
    public class AddressMapper : Profile
    {
        public AddressMapper() 
        {
            CreateMap<Address, AddressInsertDTO>()
                .ForMember(s => s.Street, opt => opt.MapFrom(d => d.Street))
                .ForMember(s => s.ZipCode, opt => opt.MapFrom(d => d.ZipCode))
                .ForMember(s => s.Number, opt => opt.MapFrom(d => d.Number))
                .ForMember(s => s.Complement, opt => opt.MapFrom(d => d.Complement))
                .ForMember(s => s.Neihborhood, opt => opt.MapFrom(d => d.Neihborhood))
                .ForMember(s => s.City, opt => opt.MapFrom(d => d.City))
                .ForMember(s => s.State, opt => opt.MapFrom(d => d.State))
                .ForMember(s => s.Uf, opt => opt.MapFrom(d => d.Uf))
                .ForMember(s => s.Default, opt => opt.MapFrom(d => d.Default))
                .ForMember(s => s.PersonId, opt => opt.MapFrom(d => d.PersonId))
                .ReverseMap();

            CreateMap<Address, AddressUpdateDTO>()
                .ForMember(s => s.Street, opt => opt.MapFrom(d => d.Street))
                .ForMember(s => s.ZipCode, opt => opt.MapFrom(d => d.ZipCode))
                .ForMember(s => s.Number, opt => opt.MapFrom(d => d.Number))
                .ForMember(s => s.Complement, opt => opt.MapFrom(d => d.Complement))
                .ForMember(s => s.Neihborhood, opt => opt.MapFrom(d => d.Neihborhood))
                .ForMember(s => s.City, opt => opt.MapFrom(d => d.City))
                .ForMember(s => s.State, opt => opt.MapFrom(d => d.State))
                .ForMember(s => s.Uf, opt => opt.MapFrom(d => d.Uf))
                .ForMember(s => s.Default, opt => opt.MapFrom(d => d.Default))
                .ReverseMap();

            CreateMap<AddressResponseDTO, Address>()
                .ForMember(s => s.Id, opt => opt.MapFrom(d => d.Id))
                .ForMember(s => s.Street, opt => opt.MapFrom(d => d.Street))
                .ForMember(s => s.ZipCode, opt => opt.MapFrom(d => d.ZipCode))
                .ForMember(s => s.Number, opt => opt.MapFrom(d => d.Number))
                .ForMember(s => s.Complement, opt => opt.MapFrom(d => d.Complement))
                .ForMember(s => s.Neihborhood, opt => opt.MapFrom(d => d.Neihborhood))
                .ForMember(s => s.City, opt => opt.MapFrom(d => d.City))
                .ForMember(s => s.State, opt => opt.MapFrom(d => d.State))
                .ForMember(s => s.Uf, opt => opt.MapFrom(d => d.Uf))
                .ForMember(s => s.Default, opt => opt.MapFrom(d => d.Default))
                .ReverseMap();
        }
    }
}
