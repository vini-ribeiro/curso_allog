using AutoMapper;
using Univali.Api.Entities;
using Univali.Api.Repositories;

namespace Univali.Api.Features.Addresses.Commands.DeleteAddress;

public class CreateAddressCommandHandler : ICreateAddressCommandHandler
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public CreateAddressCommandHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<CreateAddressCommandDto> Handle(CreateAddressCommand request)
    {
        var addressEntity = _mapper.Map<Address>(request);
        addressEntity.CustomerId = request.customerId;
        _customerRepository.AddAddress(addressEntity);
        await _customerRepository.SaveChangesAsync();
        var addressToReturn = _mapper.Map<CreateAddressCommandDto>(addressEntity);
        return addressToReturn;
    }
}