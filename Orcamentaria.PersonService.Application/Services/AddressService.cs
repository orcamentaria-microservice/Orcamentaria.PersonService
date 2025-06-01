using AutoMapper;
using Orcamentaria.Lib.Domain.Enums;
using Orcamentaria.Lib.Domain.Models;
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
            var entity = _repository.GetById(id);

            _repository.Delete(entity);

            return new Response<AddressResponseDTO>();
        }

        public Response<AddressResponseDTO> GetById(long id)
            => new Response<AddressResponseDTO>(
                _mapper.Map<Address, AddressResponseDTO>(_repository.GetById(id)));

        public Response<IEnumerable<AddressResponseDTO>> GetByPersonId(long personId)
            => new Response<IEnumerable<AddressResponseDTO>>(
                _repository.GetByPersonId(personId)
                .Select(x => _mapper.Map<Address, AddressResponseDTO>(x)));

        public async Task<Response<AddressResponseDTO>> Insert(AddressInsertDTO dto)
        {
            var address = _mapper.Map<AddressInsertDTO, Address>(dto);

            var result = _validator.ValidateBeforeInsert(address);

            if (!result.IsValid)
                return new Response<AddressResponseDTO>(result);

            try
            {
                var entity = await _repository.Insert(address);

                return new Response<AddressResponseDTO>(_mapper.Map<Address, AddressResponseDTO>(entity));
            }
            catch (Exception ex)
            {
                return new Response<AddressResponseDTO>(ResponseErrorEnum.DatabaseError, ex.Message); ;
            }
        }

        public async Task<Response<AddressResponseDTO>> Update(long id, AddressUpdateDTO dto)
        {
            var address = _mapper.Map<AddressUpdateDTO, Address>(dto);

            address.Id = id;

            var result = _validator.ValidateBeforeUpdate(address);

            if (!result.IsValid)
                return new Response<AddressResponseDTO>(result);

            try
            {
                var entity = await _repository.Update(id, address);

                return new Response<AddressResponseDTO>(_mapper.Map<Address, AddressResponseDTO>(entity));
            }
            catch (Exception ex)
            {
                return new Response<AddressResponseDTO>(ResponseErrorEnum.DatabaseError, ex.Message);
            }
        }
    }
}
