using AutoMapper;
using Orcamentaria.PersonService.Domain.DTOs.Employee;
using Orcamentaria.PersonService.Domain.Models;

namespace Orcamentaria.PersonService.Domain.Mappers
{
    public class EmployeeMapper : Profile
    {
        public EmployeeMapper() 
        {
            CreateMap<Employee, EmployeeInsertDTO>()
                .ForMember(s => s.Name, opt => opt.MapFrom(d => d.Name))
                .ForMember(s => s.Rg, opt => opt.MapFrom(d => d.Rg))
                .ForMember(s => s.Cpf, opt => opt.MapFrom(d => d.Cpf))
                .ForMember(s => s.Active, opt => opt.MapFrom(d => d.Active))
                .ForMember(s => s.Post, opt => opt.MapFrom(d => d.Post))
                .ForMember(s => s.AdmissionDate, opt => opt.MapFrom(d => d.AdmissionDate))
                .ForMember(s => s.ValuePerDay, opt => opt.MapFrom(d => d.ValuePerDay))
                .ReverseMap();

            CreateMap<Employee, EmployeeUpdateDTO>()
                .ForMember(s => s.Name, opt => opt.MapFrom(d => d.Name))
                .ForMember(s => s.Rg, opt => opt.MapFrom(d => d.Rg))
                .ForMember(s => s.Cpf, opt => opt.MapFrom(d => d.Cpf))
                .ForMember(s => s.Active, opt => opt.MapFrom(d => d.Active))
                .ForMember(s => s.Post, opt => opt.MapFrom(d => d.Post))
                .ForMember(s => s.AdmissionDate, opt => opt.MapFrom(d => d.AdmissionDate))
                .ForMember(s => s.ValuePerDay, opt => opt.MapFrom(d => d.ValuePerDay))
                .ReverseMap();

            CreateMap<EmployeeResponseDTO, Employee>()
                .ForMember(s => s.Id, opt => opt.MapFrom(d => d.Id))
                .ForMember(s => s.CompanyId, opt => opt.MapFrom(d => d.CompanyId))
                .ForMember(s => s.Name, opt => opt.MapFrom(d => d.Name))
                .ForMember(s => s.Rg, opt => opt.MapFrom(d => d.Rg))
                .ForMember(s => s.Cpf, opt => opt.MapFrom(d => d.Cpf))
                .ForMember(s => s.Active, opt => opt.MapFrom(d => d.Active))
                .ForMember(s => s.Post, opt => opt.MapFrom(d => d.Post))
                .ForMember(s => s.AdmissionDate, opt => opt.MapFrom(d => d.AdmissionDate))
                .ForMember(s => s.ValuePerDay, opt => opt.MapFrom(d => d.ValuePerDay))
                .ForMember(s => s.Addresses, opt => opt.MapFrom(d => d.Addresses))
                .ForMember(s => s.Contacts, opt => opt.MapFrom(d => d.Contacts))
                .ReverseMap();
        }
    }
}
