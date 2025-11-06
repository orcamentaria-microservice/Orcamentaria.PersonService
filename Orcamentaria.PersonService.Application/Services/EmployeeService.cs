using AutoMapper;
using Orcamentaria.Lib.Domain.Contexts;
using Orcamentaria.Lib.Domain.Enums;
using Orcamentaria.Lib.Domain.Exceptions;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Domain.Models.Exceptions;
using Orcamentaria.Lib.Domain.Models.Responses;
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

        public async Task<Employee?> GetByIdAsync(long id)
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

        public async Task<Response<IEnumerable<EmployeeResponseDTO>>?> GetAsync(GridParams gridParams)
        {
            try
            {
                var (data, pagination) = await _repository.GetAsync(gridParams,
                    p => p.Contacts,
                    p => p.Addresses);

                if (!data.Any())
                    throw new InfoException($"Nenhum dado foi encontrado.", ErrorCodeEnum.NotFound);

                return new Response<IEnumerable<EmployeeResponseDTO>>(
                    data.Select(x => _mapper.Map<Employee, EmployeeResponseDTO>(x)), pagination);
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

        public async Task<Response<EmployeeResponseDTO>> InsertAsync(EmployeeInsertDTO dto)
        {
            try
            {
                var employee = _mapper.Map<EmployeeInsertDTO, Employee>(dto);

                employee.Type = PersonTypeEnum.Employee;

                var result = _validator.ValidateBeforeInsert(employee);

                if (!result.IsValid)
                    throw new ValidationException(result);

                var entity = await _repository.InsertAsync(employee);

                return new Response<EmployeeResponseDTO>(_mapper.Map<Employee, EmployeeResponseDTO>(entity));
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

        public async Task<Response<EmployeeResponseDTO>> UpdateAsync(long id, EmployeeUpdateDTO dto)
        {
            try
            {
                var employee = _mapper.Map<EmployeeUpdateDTO, Employee>(dto);

                employee.Id = id;

                var result = _validator.ValidateBeforeUpdate(employee);

                if (!result.IsValid)
                    throw new ValidationException(result);

                var entity = await _repository.UpdateAsync(id, employee);

                return new Response<EmployeeResponseDTO>(_mapper.Map<Employee, EmployeeResponseDTO>(entity));
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
