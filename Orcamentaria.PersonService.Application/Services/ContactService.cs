using AutoMapper;
using Orcamentaria.Lib.Domain.Enums;
using Orcamentaria.Lib.Domain.Exceptions;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Domain.Models.Exceptions;
using Orcamentaria.Lib.Domain.Validators;
using Orcamentaria.PersonService.Domain.DTOs.Address;
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
            try
            {
                var entity = _repository.GetById(id);

                if (entity is null)
                    throw new InfoException($"O {id} não foi encontrado", ErrorCodeEnum.NotFound);

                _repository.Delete(entity);

                return new Response<ContactResponseDTO>();

            }
            catch (DefaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new UnexpectedException(ex.Message, ex);
            }
        }

        public Response<ContactResponseDTO> GetById(long id)
        {
            try
            {
                var data = _repository.GetById(id);

                if (data is null)
                    throw new InfoException($"O {id} não foi encontrado", ErrorCodeEnum.NotFound);

                return new Response<ContactResponseDTO>(_mapper.Map<Contact, ContactResponseDTO>(data));
            }
            catch (DefaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new UnexpectedException(ex.Message, ex);
            }
        }

        public Response<IEnumerable<ContactResponseDTO>> GetByPersonId(long personId)
        {
            try
            {
                var data = _repository.GetByPersonId(personId);

                if (!data.Any())
                    throw new InfoException($"O {personId} não foi encontrado", ErrorCodeEnum.NotFound);

                return new Response<IEnumerable<ContactResponseDTO>>(
                    data.Select(x => _mapper.Map<Contact, ContactResponseDTO>(x)));
            }
            catch (DefaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new UnexpectedException(ex.Message, ex);
            }
        }

        public async Task<Response<ContactResponseDTO>> Insert(ContactInsertDTO dto)
        {
            try
            {
                var contact = _mapper.Map<ContactInsertDTO, Contact>(dto);

                var result = _validator.ValidateBeforeInsert(contact);

                if (!result.IsValid)
                    throw new ValidationException(result);

                var entity = await _repository.Insert(contact);

                return new Response<ContactResponseDTO>(_mapper.Map<Contact, ContactResponseDTO>(entity));
            }
            catch (DefaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new UnexpectedException(ex.Message, ex);
            }
        }

        public async Task<Response<ContactResponseDTO>> Update(long id, ContactUpdateDTO dto)
        {
            try
            {
                if (_repository.GetById(id) is null)
                    throw new InfoException($"O {id} não foi encontrado", ErrorCodeEnum.NotFound);

                var contact = _mapper.Map<ContactUpdateDTO, Contact>(dto);

                contact.Id = id;

                var result = _validator.ValidateBeforeUpdate(contact);

                if (!result.IsValid)
                    throw new ValidationException(result);

                var entity = await _repository.Update(id, contact);

                return new Response<ContactResponseDTO>(_mapper.Map<Contact, ContactResponseDTO>(entity));
            }
            catch (DefaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new UnexpectedException(ex.Message, ex);
            }
        }
    }
}
