using AutoMapper;
using Orcamentaria.Lib.Domain.Enums;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Domain.Validators;
using Orcamentaria.PersonService.Domain.DTOs.Contact;
using Orcamentaria.PersonService.Domain.Models;
using Orcamentaria.PersonService.Domain.Repositories;
using Orcamentaria.PersonService.Domain.Services;

namespace Orcamentaria.PersonService.Application.Services
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

        public Response<ContactResponseDTO> Delete(long id)
        {
            var entity = _repository.GetById(id);

            _repository.Delete(entity);

            return new Response<ContactResponseDTO>();
        }

        public Response<ContactResponseDTO> GetById(long id)
            => new Response<ContactResponseDTO>(
                _mapper.Map<Contact, ContactResponseDTO>(_repository.GetById(id)));

        public Response<IEnumerable<ContactResponseDTO>> GetByPersonId(long personId)
            => new Response<IEnumerable<ContactResponseDTO>>(
                _repository.GetByPersonId(personId)
                .Select(x => _mapper.Map<Contact, ContactResponseDTO>(x)));

        public async Task<Response<ContactResponseDTO>> Insert(ContactInsertDTO dto)
        {
            var contact = _mapper.Map<ContactInsertDTO, Contact>(dto);

            var result = _validator.ValidateBeforeInsert(contact);

            if (!result.IsValid)
                return new Response<ContactResponseDTO>(result);

            try
            {
                var entity = await _repository.Insert(contact);

                return new Response<ContactResponseDTO>(_mapper.Map<Contact, ContactResponseDTO>(entity));
            }
            catch (Exception ex)
            {
                return new Response<ContactResponseDTO>(ResponseErrorEnum.DatabaseError, ex.Message);
            }
        }

        public async Task<Response<ContactResponseDTO>> Update(long id, ContactUpdateDTO dto)
        {
            var contact = _mapper.Map<ContactUpdateDTO, Contact>(dto);

            contact.Id = id;

            var result = _validator.ValidateBeforeUpdate(contact);

            if (!result.IsValid)
                return new Response<ContactResponseDTO>(result);

            try
            {
                var entity = await _repository.Update(id, contact);

                return new Response<ContactResponseDTO>(_mapper.Map<Contact, ContactResponseDTO>(entity));
            }
            catch (Exception ex)
            {
                return new Response<ContactResponseDTO>(ResponseErrorEnum.DatabaseError, ex.Message);
            }
        }
    }
}
