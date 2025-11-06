using AutoMapper;
using Orcamentaria.Lib.Domain.Contexts;
using Orcamentaria.Lib.Domain.Enums;
using Orcamentaria.Lib.Domain.Exceptions;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Domain.Models.Exceptions;
using Orcamentaria.Lib.Domain.Models.Responses;
using Orcamentaria.Lib.Domain.Validators;
using Orcamentaria.PersonService.Domain.DTOs.Person;
using Orcamentaria.PersonService.Domain.Models;
using Orcamentaria.PersonService.Domain.Repositories;
using Orcamentaria.PersonService.Domain.Services;

namespace Orcamentaria.PersonService.Application.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _repository;
        private readonly IValidatorEntity<Person> _validator;
        private readonly IMapper _mapper;

        public PersonService(
            IPersonRepository repository, 
            IValidatorEntity<Person> validator,
            IMapper mapper,
            IRequestContext requestContext)
        {
            _repository = repository;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<PersonResponseDTO>>?> GetAsync(GridParams gridParams)
        {
            try
            {
                var (data, pagination) = await _repository.GetAsync(gridParams,
                    p => p.Contacts,
                    p => p.Addresses);

                if (!data.Any())
                    throw new InfoException($"Nenhum dado foi encontrado.", ErrorCodeEnum.NotFound);

                return new Response<IEnumerable<PersonResponseDTO>>(
                    data.Select(x => _mapper.Map<Person, PersonResponseDTO>(x)), pagination);
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

        public async Task<Person?> GetByIdAsync(long id)
        {
            try
            {
                return await _repository.GetByIdAsync(id,
                    p => p.Contacts,
                    p => p.Addresses);
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

        public async Task<Response<PersonResponseDTO>> InsertAsync(PersonInsertDTO dto)
        {
            try
            {
                var person = _mapper.Map<PersonInsertDTO, Person>(dto);

                var result = _validator.ValidateBeforeInsert(person);

                if (!result.IsValid)
                    throw new ValidationException(result);

                var entity = await _repository.InsertAsync(person);

                return new Response<PersonResponseDTO>(_mapper.Map<Person, PersonResponseDTO>(entity));
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

        public async Task<Response<PersonResponseDTO>> UpdateAsync(long id, PersonUpdateDTO dto)
        {
            try
            {
                var person = _mapper.Map<PersonUpdateDTO, Person>(dto);

                person.Id = id;

                var result = _validator.ValidateBeforeUpdate(person);

                if (!result.IsValid)
                    throw new ValidationException(result);

                var entity = await _repository.UpdateAsync(id, person);

                return new Response<PersonResponseDTO>(_mapper.Map<Person, PersonResponseDTO>(entity));
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
