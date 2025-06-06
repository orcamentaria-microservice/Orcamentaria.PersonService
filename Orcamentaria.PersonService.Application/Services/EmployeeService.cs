using AutoMapper;
using Orcamentaria.Lib.Domain.Contexts;
using Orcamentaria.Lib.Domain.Enums;
using Orcamentaria.Lib.Domain.Exceptions;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Domain.Models.Exceptions;
using Orcamentaria.Lib.Domain.Validators;
using Orcamentaria.PersonService.Domain.DTOs.Employee;
using Orcamentaria.PersonService.Domain.Enums;
using Orcamentaria.PersonService.Domain.Models;
using Orcamentaria.PersonService.Domain.Repositories;
using Orcamentaria.PersonService.Domain.Services;

namespace Orcamentaria.PersonService.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUserAuthContext _userAuthContext;
        private readonly IEmployeeRepository _repository;
        private readonly IValidatorEntity<Employee> _validator;
        private readonly IMapper _mapper;

        public EmployeeService(
            IUserAuthContext userAuthContext,
            IEmployeeRepository repository, 
            IValidatorEntity<Employee> validator,
            IMapper mapper)
        {
            _userAuthContext = userAuthContext;
            _repository = repository;
            _validator = validator;
            _mapper = mapper;
        }

        public Response<EmployeeResponseDTO> GetById(long id)
        {
            try
            {
                var data = _repository.GetById(id);

                if (data is null)
                    throw new InfoException($"O {id} não foi encontrado", ErrorCodeEnum.NotFound);

                return new Response<EmployeeResponseDTO>(_mapper.Map<Employee, EmployeeResponseDTO>(data));
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

        public Response<IEnumerable<EmployeeResponseDTO>> GetByCompanyId()
        {
            try
            {
                var data = _repository.GetByCompanyId();

                if (!data.Any())
                    throw new InfoException($"Nenhum dado foi encontrado", ErrorCodeEnum.NotFound);

                return new Response<IEnumerable<EmployeeResponseDTO>>(
                    data.Select(x => _mapper.Map<Employee, EmployeeResponseDTO>(x)));
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

        public Response<IEnumerable<EmployeeResponseDTO>> GetByName(string name)
        {
            try
            {
                var data = _repository.GetByName(name);

                if (!data.Any())
                    throw new InfoException($"Nenhum dado foi encontrado", ErrorCodeEnum.NotFound);

                return new Response<IEnumerable<EmployeeResponseDTO>>(
                    data.Select(x => _mapper.Map<Employee, EmployeeResponseDTO>(x)));
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

        public async Task<Response<EmployeeResponseDTO>> Insert(EmployeeInsertDTO dto)
        {
            try
            {
                var employee = _mapper.Map<EmployeeInsertDTO, Employee>(dto);

                employee.CompanyId = _userAuthContext.UserCompanyId;
                employee.Type = PersonTypeEnum.Employee;

                var result = _validator.ValidateBeforeInsert(employee);

                if (!result.IsValid)
                    throw new ValidationException(result);

                var entity = await _repository.Insert(employee);

                return new Response<EmployeeResponseDTO>(_mapper.Map<Employee, EmployeeResponseDTO>(entity));
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

        public async Task<Response<EmployeeResponseDTO>> Update(long id, EmployeeUpdateDTO dto)
        {
            try
            {
                if (_repository.GetById(id) is null)
                    throw new InfoException($"O {id} não foi encontrado", ErrorCodeEnum.NotFound);

                var employee = _mapper.Map<EmployeeUpdateDTO, Employee>(dto);

                employee.Id = id;

                var result = _validator.ValidateBeforeUpdate(employee);

                if (!result.IsValid)
                    throw new ValidationException(result);

                var entity = await _repository.Update(id, employee);

                return new Response<EmployeeResponseDTO>(_mapper.Map<Employee, EmployeeResponseDTO>(entity));
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
