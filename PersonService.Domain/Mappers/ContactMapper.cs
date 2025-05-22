using AutoMapper;
using PersonService.Domain.DTOs.Contact;
using PersonService.Domain.DTOs.Person;
using PersonService.Domain.Models;

namespace PersonService.Domain.Mappers
{
    public class ContactMapper : Profile
    {
        public ContactMapper() 
        {
            CreateMap<Contact, ContactInsertDTO>()
                .ForMember(s => s.ContactDescription, opt => opt.MapFrom(d => d.ContactDescription))
                .ForMember(s => s.Type, opt => opt.MapFrom(d => d.Type))
                .ForMember(s => s.Default, opt => opt.MapFrom(d => d.Default))
                .ForMember(s => s.PersonId, opt => opt.MapFrom(d => d.PersonId))
                .ReverseMap();

            CreateMap<Contact, ContactUpdateDTO>()
                .ForMember(s => s.ContactDescription, opt => opt.MapFrom(d => d.ContactDescription))
                .ForMember(s => s.Default, opt => opt.MapFrom(d => d.Default))
                .ReverseMap();
        }
    }
}
