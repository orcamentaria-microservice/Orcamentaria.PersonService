using AutoMapper;
using Orcamentaria.Lib.Domain.Contexts;
using Orcamentaria.Lib.Domain.Enums;
using Orcamentaria.Lib.Domain.Exceptions;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Domain.Models.Exceptions;
using Orcamentaria.Lib.Domain.Validators;
using Orcamentaria.PersonService.Domain.DTOs.Person;
using Orcamentaria.PersonService.Domain.Models;
using Orcamentaria.PersonService.Domain.Repositories;
using Orcamentaria.PersonService.Domain.Services;
using System.Xml.Linq;

namespace Orcamentaria.PersonService.Application.Services
{
    public class PersonService : IPersonService
    {
        private readonly IUserAuthContext _userAuthContext;
        private readonly IPersonRepository _repository;
        private readonly IValidatorEntity<Person> _validator;
        private readonly IMapper _mapper;

        public PersonService(
            IUserAuthContext userAuthContext,
            IPersonRepository repository, 
            IValidatorEntity<Person> validator,
            IMapper mapper)
        {
            _userAuthContext = userAuthContext;
            _repository = repository;
            _validator = validator;
            _mapper = mapper;
        }

        public Response<IEnumerable<PersonResponseDTO>> GetByCompanyId()
        {
            try
            {
                var data = _repository.GetByCompanyId();

                if (!data.Any())
                    throw new InfoException($"Nenhum dado foi encontrado", ErrorCodeEnum.NotFound);

                return new Response<IEnumerable<PersonResponseDTO>>(
                    data.Select(x => _mapper.Map<Person, PersonResponseDTO>(x)));
            }
            catch (DatabaseException)
            {
                throw;
            }
            catch (InfoException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new UnexpectedException(ex.Message, ex);
            }
        }

        public Response<PersonResponseDTO> GetById(long id)
        {
            try
            {
                var data = _repository.GetById(id);

                if (data is null)
                    throw new InfoException($"O {id} não foi encontrado", ErrorCodeEnum.NotFound);

                return new Response<PersonResponseDTO>(_mapper.Map<Person, PersonResponseDTO>(data));
            }
            catch (DatabaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new UnexpectedException(ex.Message, ex);
            }
        }

        public Response<IEnumerable<PersonResponseDTO>> GetByName(string name)
        {
            try
            {
                var data = _repository.GetByName(name);

                if (!data.Any())
                    throw new InfoException($"Nenhum dado foi encontrado", ErrorCodeEnum.NotFound);

                return new Response<IEnumerable<PersonResponseDTO>>(
                    data.Select(x => _mapper.Map<Person, PersonResponseDTO>(x)));
            }
            catch (DatabaseException)
            {
                throw;
            }
            catch (InfoException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new UnexpectedException(ex.Message, ex);
            }
        }

        public async Task<Response<PersonResponseDTO>> Insert(PersonInsertDTO dto)
        {
            try
            {
                var person = _mapper.Map<PersonInsertDTO, Person>(dto);

                person.CompanyId = _userAuthContext.UserCompanyId;

                var result = _validator.ValidateBeforeInsert(person);

                if (!result.IsValid)
                    throw new ValidationException(result);

                var entity = await _repository.Insert(person);

                return new Response<PersonResponseDTO>(_mapper.Map<Person, PersonResponseDTO>(entity));
            }
            catch (DatabaseException)
            {
                throw;
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new UnexpectedException(ex.Message, ex);
            }
        }

        public async Task<Response<PersonResponseDTO>> Update(long id, PersonUpdateDTO dto)
        {
            try
            {
                if (_repository.GetById(id) is null)
                    throw new InfoException($"O {id} não foi encontrado", ErrorCodeEnum.NotFound);

                var person = _mapper.Map<PersonUpdateDTO, Person>(dto);

                person.Id = id;

                var result = _validator.ValidateBeforeUpdate(person);

                if (!result.IsValid)
                    throw new ValidationException(result);

                var entity = await _repository.Update(id, person);

                return new Response<PersonResponseDTO>(_mapper.Map<Person, PersonResponseDTO>(entity));
            }
            catch (DatabaseException)
            {
                throw;
            }
            catch (InfoException)
            {
                throw;
            }
            catch (ValidationException)
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
