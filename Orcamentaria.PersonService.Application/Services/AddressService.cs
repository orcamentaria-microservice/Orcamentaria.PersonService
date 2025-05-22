using AutoMapper;
using Orcamentaria.Lib.Domain.Enums;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.PersonService.Domain.DTOs.Address;
using Orcamentaria.PersonService.Domain.Models;
using Orcamentaria.PersonService.Domain.Repositories;
using Orcamentaria.PersonService.Domain.Services;
using Orcamentaria.PersonService.Domain.Validators;

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

        public Response<Address> Delete(long id)
        {
            var entity = _repository.GetById(id);

            _repository.Delete(entity);

            return new Response<Address>();
        }

        public Response<Address> GetById(long id)
            => new Response<Address>(_repository.GetById(id));

        public Response<IEnumerable<Address>> GetByPersonId(long personId)
            => new Response<IEnumerable<Address>>(_repository.GetByPersonId(personId));

        public async Task<Response<Address>> Insert(AddressInsertDTO dto)
        {
            var address = _mapper.Map<AddressInsertDTO, Address>(dto);

            var result = _validator.ValidateBeforeInsert(address);

            if (!result.IsValid)
                return new Response<Address>(result);

            try
            {
                var entity = await _repository.Insert(address);

                return new Response<Address>(entity);
            }
            catch (Exception ex)
            {
                return new Response<Address>(ResponseErrorEnum.DatabaseError, ex.Message); ;
            }
        }

        public async Task<Response<Address>> Update(long id, AddressUpdateDTO dto)
        {
            var address = _mapper.Map<AddressUpdateDTO, Address>(dto);

            address.Id = id;

            var result = _validator.ValidateBeforeUpdate(address);

            if (!result.IsValid)
                return new Response<Address>(result);

            try
            {
                var entity = await _repository.Update(id, address);

                return new Response<Address>(entity);
            }
            catch (Exception ex)
            {
                return new Response<Address>(ResponseErrorEnum.DatabaseError, ex.Message);
            }
        }
    }
}
