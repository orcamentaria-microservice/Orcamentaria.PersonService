using AutoMapper;
using Orcamentaria.Lib.Domain.Enums;
using Orcamentaria.Lib.Domain.Exceptions;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Domain.Models.Exceptions;
using Orcamentaria.Lib.Domain.Models.Responses;
using Orcamentaria.Lib.Domain.Validators;
using Orcamentaria.PersonService.Domain.DTOs.Address;
using Orcamentaria.PersonService.Domain.Models;
using Orcamentaria.PersonService.Domain.Repositories;
using Orcamentaria.PersonService.Domain.Services;

namespace Orcamentaria.PersonService.Application.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository<Address> _repository;
        private readonly IValidatorEntity<Address> _validator;
        private readonly IMapper _mapper;

        public AddressService(
            IAddressRepository<Address> repository,
            IValidatorEntity<Address> validator,
            IMapper mapper)
        {
            _repository = repository;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<Address?> GetByIdAsync(long id)
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

        public async Task<Response<IEnumerable<AddressResponseDTO>>> GetAsync(GridParams gridParams)
        {
            try
            {
                var (data, pagination) = await _repository.GetAsync(gridParams);

                if (!data.Any())
                    throw new InfoException($"Nenhum dado foi encontrado.", ErrorCodeEnum.NotFound);

                return new Response<IEnumerable<AddressResponseDTO>>(
                    data.Select(x => _mapper.Map<Address, AddressResponseDTO>(x)), pagination);
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

        public async Task<Response<AddressResponseDTO>> InsertAsync(AddressInsertDTO dto)
        {
            try
            {
                var address = _mapper.Map<AddressInsertDTO, Address>(dto);

                var result = _validator.ValidateBeforeInsert(address);

                if (!result.IsValid)
                    throw new ValidationException(result);

                    var entity = await _repository.InsertAsync(address);

                    return new Response<AddressResponseDTO>(_mapper.Map<Address, AddressResponseDTO>(entity));
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

        public async Task<Response<AddressResponseDTO>> UpdateAsync(long id, AddressUpdateDTO dto)
        {
            try
            {
                var address = _mapper.Map<AddressUpdateDTO, Address>(dto);

                address.Id = id;

                var result = _validator.ValidateBeforeUpdate(address);

                if (!result.IsValid)
                    throw new ValidationException(result);

                var entity = await _repository.UpdateAsync(id, address);

                return new Response<AddressResponseDTO>(_mapper.Map<Address, AddressResponseDTO>(entity));
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

        public async Task<Response<AddressResponseDTO>> DeleteAsync(long id)
        {
            try
            {
                if (await _repository.GetByIdAsync(id) is null)
                    throw new InfoException($"O {id} nao foi encontrado.", ErrorCodeEnum.NotFound);

                await _repository.DeleteAsync(id);

                return new Response<AddressResponseDTO>();
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
