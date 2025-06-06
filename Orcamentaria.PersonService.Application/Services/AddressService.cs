using AutoMapper;
using Orcamentaria.Lib.Domain.Enums;
using Orcamentaria.Lib.Domain.Exceptions;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Domain.Models.Exceptions;
using Orcamentaria.Lib.Domain.Validators;
using Orcamentaria.PersonService.Domain.DTOs.Address;
using Orcamentaria.PersonService.Domain.Models;
using Orcamentaria.PersonService.Domain.Repositories;
using Orcamentaria.PersonService.Domain.Services;

namespace Orcamentaria.PersonService.Application.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _repository;
        private readonly IValidatorEntity<Address> _validator;
        private readonly IMapper _mapper;

        public AddressService(
            IAddressRepository repository,
            IValidatorEntity<Address> validator,
            IMapper mapper)
        {
            _repository = repository;
            _validator = validator;
            _mapper = mapper;
        }

        public Response<AddressResponseDTO> Delete(long id)
        {
            try
            {
                var entity = _repository.GetById(id);

                if (entity is null)
                    throw new InfoException($"O {id} não foi encontrado", ErrorCodeEnum.NotFound);

                _repository.Delete(entity);

                return new Response<AddressResponseDTO>();

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

        public Response<AddressResponseDTO> GetById(long id)
        { 
            try
            {
                var data = _repository.GetById(id);

                if(data is null)
                    throw new InfoException($"O {id} não foi encontrado", ErrorCodeEnum.NotFound);

                return new Response<AddressResponseDTO>(_mapper.Map<Address, AddressResponseDTO>(data));
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

        public Response<IEnumerable<AddressResponseDTO>> GetByPersonId(long personId)
        {
            try
            {
                var data = _repository.GetByPersonId(personId);

                if (!data.Any())
                    throw new InfoException($"O {personId} não foi encontrado", ErrorCodeEnum.NotFound);

                return new Response<IEnumerable<AddressResponseDTO>>(
                    data.Select(x => _mapper.Map<Address, AddressResponseDTO>(x)));
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

        public async Task<Response<AddressResponseDTO>> Insert(AddressInsertDTO dto)
        {
            try
            {
                var address = _mapper.Map<AddressInsertDTO, Address>(dto);

                var result = _validator.ValidateBeforeInsert(address);

                if (!result.IsValid)
                    throw new ValidationException(result);

                    var entity = await _repository.Insert(address);

                    return new Response<AddressResponseDTO>(_mapper.Map<Address, AddressResponseDTO>(entity));
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

        public async Task<Response<AddressResponseDTO>> Update(long id, AddressUpdateDTO dto)
        {
            try
            {
                if (_repository.GetById(id) is null)
                    throw new InfoException($"O {id} não foi encontrado", ErrorCodeEnum.NotFound);

                var address = _mapper.Map<AddressUpdateDTO, Address>(dto);

                address.Id = id;

                var result = _validator.ValidateBeforeUpdate(address);

                if (!result.IsValid)
                    throw new ValidationException(result);

                var entity = await _repository.Update(id, address);

                return new Response<AddressResponseDTO>(_mapper.Map<Address, AddressResponseDTO>(entity));
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
