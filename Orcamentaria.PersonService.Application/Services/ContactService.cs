using AutoMapper;
using Orcamentaria.Lib.Domain.Enums;
using Orcamentaria.Lib.Domain.Exceptions;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Domain.Models.Exceptions;
using Orcamentaria.Lib.Domain.Models.Responses;
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

        public async Task<Contact?> GetByIdAsync(long id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
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

        public async Task<Response<IEnumerable<ContactResponseDTO>>> GetAsync(GridParams gridParams)
        {
            try
            {
                var (data, pagination) = await _repository.GetAsync(gridParams);

                if (!data.Any())
                    throw new InfoException($"Nenhum dado foi encontrado.", ErrorCodeEnum.NotFound);

                return new Response<IEnumerable<ContactResponseDTO>>(
                    data.Select(x => _mapper.Map<Contact, ContactResponseDTO>(x)), pagination);
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

        public async Task<Response<ContactResponseDTO>> InsertAsync(ContactInsertDTO dto)
        {
            try
            {
                var contact = _mapper.Map<ContactInsertDTO, Contact>(dto);

                var result = _validator.ValidateBeforeInsert(contact);

                if (!result.IsValid)
                    throw new ValidationException(result);

                var entity = await _repository.InsertAsync(contact);

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

        public async Task<Response<ContactResponseDTO>> UpdateAsync(long id, ContactUpdateDTO dto)
        {
            try
            {
                var contact = _mapper.Map<ContactUpdateDTO, Contact>(dto);

                contact.Id = id;

                var result = _validator.ValidateBeforeUpdate(contact);

                if (!result.IsValid)
                    throw new ValidationException(result);

                var entity = await _repository.UpdateAsync(id, contact);

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

        public async Task<Response<ContactResponseDTO>> DeleteAsync(long id)
        {
            try
            {
                if (await _repository.GetByIdAsync(id) is null)
                    throw new InfoException($"O {id} nao foi encontrado.", ErrorCodeEnum.NotFound);

                await _repository.DeleteAsync(id);

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
    }
}
