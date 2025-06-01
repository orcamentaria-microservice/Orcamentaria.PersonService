using AutoMapper;
using Orcamentaria.Lib.Domain.Contexts;
using Orcamentaria.Lib.Domain.Enums;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Domain.Validators;
using Orcamentaria.PersonService.Domain.DTOs.Person;
using Orcamentaria.PersonService.Domain.Models;
using Orcamentaria.PersonService.Domain.Repositories;
using Orcamentaria.PersonService.Domain.Services;

namespace Orcamentaria.PersonService.Application.Services
{
    public class PersonService : IPersonService
    {
        private readonly ICompanyContext _companyContext;
        private readonly IPersonRepository _repository;
        private readonly IValidatorEntity<Person> _validator;
        private readonly IMapper _mapper;

        public PersonService(
            ICompanyContext companyContext,
            IPersonRepository repository, 
            IValidatorEntity<Person> validator,
            IMapper mapper)
        {
            _companyContext = companyContext;
            _repository = repository;
            _validator = validator;
            _mapper = mapper;
        }

        public Response<PersonResponseDTO> GetById(long id)
            => new Response<PersonResponseDTO>(
                _mapper.Map<Person, PersonResponseDTO>(_repository.GetById(id)));

        public Response<IEnumerable<PersonResponseDTO>> GetByName(string name)
            => new Response<IEnumerable<PersonResponseDTO>>(
                _repository.GetByName(name)
                .Select(x => _mapper.Map<Person, PersonResponseDTO>(x)));

        public async Task<Response<PersonResponseDTO>> Insert(PersonInsertDTO dto)
        {
            var person = _mapper.Map<PersonInsertDTO, Person>(dto);

            person.CompanyId = _companyContext.CompanyId;

            var result = _validator.ValidateBeforeInsert(person);

            if (!result.IsValid)
                return new Response<PersonResponseDTO>(result);

            try
            {
                var entity = await _repository.Insert(person);

                return new Response<PersonResponseDTO>(_mapper.Map<Person, PersonResponseDTO>(entity));
            }
            catch (Exception ex)
            {
                return new Response<PersonResponseDTO>(ResponseErrorEnum.DatabaseError, ex.Message);
            }
        }

        public async Task<Response<PersonResponseDTO>> Update(long id, PersonUpdateDTO dto)
        {
            var person = _mapper.Map<PersonUpdateDTO, Person>(dto);

            person.Id = id;

            var result = _validator.ValidateBeforeUpdate(person);

            if (!result.IsValid)
                return new Response<PersonResponseDTO>(result);

            try
            {
                var entity = await _repository.Update(id, person);

                return new Response<PersonResponseDTO>(_mapper.Map<Person, PersonResponseDTO>(entity));
            }
            catch (Exception ex)
            {
                return new Response<PersonResponseDTO>(ResponseErrorEnum.DatabaseError, ex.Message);
            }
        }
    }
}
