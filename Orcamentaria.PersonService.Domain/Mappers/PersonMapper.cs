using AutoMapper;
using Orcamentaria.PersonService.Domain.DTOs.Person;
using Orcamentaria.PersonService.Domain.Models;

namespace Orcamentaria.PersonService.Domain.Mappers
{
    public class PersonMapper : Profile
    {
        public PersonMapper() 
        {
            CreateMap<Person, PersonInsertDTO>()
                .ForMember(s => s.Name, opt => opt.MapFrom(d => d.Name))
                .ForMember(s => s.Rg, opt => opt.MapFrom(d => d.Rg))
                .ForMember(s => s.Cpf, opt => opt.MapFrom(d => d.Cpf))
                .ForMember(s => s.Cnpj, opt => opt.MapFrom(d => d.Cnpj))
                .ForMember(s => s.Type, opt => opt.MapFrom(d => d.Type))
                .ForMember(s => s.Active, opt => opt.MapFrom(d => d.Active))
                .ReverseMap();

            CreateMap<Person, PersonUpdateDTO>()
                .ForMember(s => s.Name, opt => opt.MapFrom(d => d.Name))
                .ForMember(s => s.Rg, opt => opt.MapFrom(d => d.Rg))
                .ForMember(s => s.Cpf, opt => opt.MapFrom(d => d.Cpf))
                .ForMember(s => s.Cnpj, opt => opt.MapFrom(d => d.Cnpj))
                .ForMember(s => s.Type, opt => opt.MapFrom(d => d.Type))
                .ForMember(s => s.Active, opt => opt.MapFrom(d => d.Active))
                .ReverseMap();

            CreateMap<PersonResponseDTO, Person>()
                .ForMember(s => s.Id, opt => opt.MapFrom(d => d.Id))
                .ForMember(s => s.CompanyId, opt => opt.MapFrom(d => d.CompanyId))
                .ForMember(s => s.Name, opt => opt.MapFrom(d => d.Name))
                .ForMember(s => s.Rg, opt => opt.MapFrom(d => d.Rg))
                .ForMember(s => s.Cpf, opt => opt.MapFrom(d => d.Cpf))
                .ForMember(s => s.Cnpj, opt => opt.MapFrom(d => d.Cnpj))
                .ForMember(s => s.Type, opt => opt.MapFrom(d => d.Type))
                .ForMember(s => s.Active, opt => opt.MapFrom(d => d.Active))
                .ForMember(s => s.Addresses, opt => opt.MapFrom(d => d.Addresses))
                .ForMember(s => s.Contacts, opt => opt.MapFrom(d => d.Contacts))
                .ReverseMap();
        }
    }
}
