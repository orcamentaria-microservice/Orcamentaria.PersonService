using AutoMapper;
using PersonService.BuildingBlocks.Enums;
using PersonService.BuildingBlocks.Reponses;
using PersonService.Domain.Contexts;
using PersonService.Domain.DTOs.Employee;
using PersonService.Domain.Enums;
using PersonService.Domain.Models;
using PersonService.Domain.Repositories;
using PersonService.Domain.Services;
using PersonService.Domain.Validators;

namespace PersonService.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ICompanyContext _companyContext;
        private readonly IEmployeeRepository _repository;
        private readonly IValidatorEntity<Employee> _validator;
        private readonly IMapper _mapper;

        public EmployeeService(
            ICompanyContext companyContext,
            IEmployeeRepository repository, 
            IValidatorEntity<Employee> validator,
            IMapper mapper)
        {
            _companyContext = companyContext;
            _repository = repository;
            _validator = validator;
            _mapper = mapper;
        }

        public Response<Employee> GetById(long id)
            => new Response<Employee>(_repository.GetById(id));

        public Response<IEnumerable<Employee>> GetByName(string name)
            => new Response<IEnumerable<Employee>>(_repository.GetByName(name));

        public async Task<Response<Employee>> Insert(EmployeeInsertDTO dto)
        {
            var employee = _mapper.Map<EmployeeInsertDTO, Employee>(dto);

            employee.CompanyId = _companyContext.CompanyId;
            employee.Type = PersonTypeEnum.Employee;

            var result = _validator.ValidateBeforeInsert(employee);

            if (!result.IsValid)
                return new Response<Employee>(result);

            try
            {
                var entity = await _repository.Insert(employee);

                return new Response<Employee>(entity);
            }
            catch (Exception ex)
            {
                return new Response<Employee>(ResponseErrorEnum.DatabaseError, ex.Message);
            }
        }

        public async Task<Response<Employee>> Update(long id, EmployeeUpdateDTO dto)
        {
            var employee = _mapper.Map<EmployeeUpdateDTO, Employee>(dto);

            employee.Id = id;

            var result = _validator.ValidateBeforeUpdate(employee);

            if (!result.IsValid)
                return new Response<Employee>(result);

            try
            {
                var entity = await _repository.Update(id, employee);

                return new Response<Employee>(entity);
            }
            catch (Exception ex)
            {
                return new Response<Employee>(ResponseErrorEnum.DatabaseError, ex.Message);
            }
        }
    }
}
