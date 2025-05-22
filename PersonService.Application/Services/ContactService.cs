using AutoMapper;
using PersonService.BuildingBlocks.Enums;
using PersonService.BuildingBlocks.Reponses;
using PersonService.Domain.DTOs.Contact;
using PersonService.Domain.Models;
using PersonService.Domain.Repositories;
using PersonService.Domain.Services;
using PersonService.Domain.Validators;

namespace PersonService.Application.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _repository;
        private readonly IValidatorEntity<Contact> _validator;
        private readonly IMapper _mapper;

        public ContactService(
            IContactRepository repository,
            IValidatorEntity<Contact> validator,
            IMapper mapper)
        {
            _repository = repository;
            _validator = validator;
            _mapper = mapper;
        }

        public Response<Contact> Delete(long id)
        {
            var entity = _repository.GetById(id);

            _repository.Delete(entity);

            return new Response<Contact>();
        }

        public Response<Contact> GetById(long id)
            => new Response<Contact>(_repository.GetById(id));

        public Response<IEnumerable<Contact>> GetByPersonId(long personId)
            => new Response<IEnumerable<Contact>>(_repository.GetByPersonId(personId));

        public async Task<Response<Contact>> Insert(ContactInsertDTO dto)
        {
            var contact = _mapper.Map<ContactInsertDTO, Contact>(dto);

            var result = _validator.ValidateBeforeInsert(contact);

            if (!result.IsValid)
                return new Response<Contact>(result);

            try
            {
                var entity = await _repository.Insert(contact);

                return new Response<Contact>(entity);
            }
            catch (Exception ex)
            {
                return new Response<Contact>(ResponseErrorEnum.DatabaseError, ex.Message);
            }
        }

        public async Task<Response<Contact>> Update(long id, ContactUpdateDTO dto)
        {
            var contact = _mapper.Map<ContactUpdateDTO, Contact>(dto);

            contact.Id = id;

            var result = _validator.ValidateBeforeUpdate(contact);

            if (!result.IsValid)
                return new Response<Contact>(result);

            try
            {
                var entity = await _repository.Update(id, contact);
                return new Response<Contact>(entity);
            }
            catch (Exception ex)
            {
                return new Response<Contact>(ResponseErrorEnum.DatabaseError, ex.Message);
            }
        }
    }
}
