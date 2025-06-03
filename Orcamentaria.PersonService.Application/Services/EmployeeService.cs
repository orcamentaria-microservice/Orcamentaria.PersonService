using AutoMapper;
using Orcamentaria.Lib.Domain.Contexts;
using Orcamentaria.Lib.Domain.Enums;
using Orcamentaria.Lib.Domain.Models;
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
            => new Response<EmployeeResponseDTO>(_mapper.Map<Employee, EmployeeResponseDTO>(_repository.GetById(id)));

        public Response<IEnumerable<EmployeeResponseDTO>> GetByCompanyId()
            => new Response<IEnumerable<EmployeeResponseDTO>>(
                _repository.GetByCompanyId()
                .Select(x => _mapper.Map<Employee, EmployeeResponseDTO>(x)));

        public Response<IEnumerable<EmployeeResponseDTO>> GetByName(string name)
            => new Response<IEnumerable<EmployeeResponseDTO>>(
                _repository.GetByName(name)
                .Select(x => _mapper.Map<Employee, EmployeeResponseDTO>(x)));

        public async Task<Response<EmployeeResponseDTO>> Insert(EmployeeInsertDTO dto)
        {
            var employee = _mapper.Map<EmployeeInsertDTO, Employee>(dto);

            employee.CompanyId = _userAuthContext.UserCompanyId;
            employee.Type = PersonTypeEnum.Employee;

            var result = _validator.ValidateBeforeInsert(employee);

            if (!result.IsValid)
                return new Response<EmployeeResponseDTO>(result);

            try
            {
                var entity = await _repository.Insert(employee);

                return new Response<EmployeeResponseDTO>(_mapper.Map<Employee, EmployeeResponseDTO>(entity));
            }
            catch (Exception ex)
            {
                return new Response<EmployeeResponseDTO>(ResponseErrorEnum.DatabaseError, ex.Message);
            }
        }

        public async Task<Response<EmployeeResponseDTO>> Update(long id, EmployeeUpdateDTO dto)
        {
            var employee = _mapper.Map<EmployeeUpdateDTO, Employee>(dto);

            employee.Id = id;

            var result = _validator.ValidateBeforeUpdate(employee);

            if (!result.IsValid)
                return new Response<EmployeeResponseDTO>(result);

            try
            {
                var entity = await _repository.Update(id, employee);

                return new Response<EmployeeResponseDTO>(_mapper.Map<Employee, EmployeeResponseDTO>(entity));
            }
            catch (Exception ex)
            {
                return new Response<EmployeeResponseDTO>(ResponseErrorEnum.DatabaseError, ex.Message);
            }
        }
    }
}
