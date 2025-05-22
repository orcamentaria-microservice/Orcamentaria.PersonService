using AutoMapper;
using PersonService.Domain.DTOs.Employee;
using PersonService.Domain.Models;

namespace PersonService.Domain.Mappers
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
        }
    }
}
