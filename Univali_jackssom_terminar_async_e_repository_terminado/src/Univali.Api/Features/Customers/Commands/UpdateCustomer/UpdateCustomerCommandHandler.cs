using AutoMapper;
using Univali.Api.Entities;
using Univali.Api.Repositories;

namespace Univali.Api.Features.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommandHandler : IUpdateCustomerCommandHandler
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public UpdateCustomerCommandHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<bool> Handle(UpdateCustomerCommand request)
    {
        var customerEntity = _mapper.Map<Customer>(request);
        _customerRepository.UpdateCustomer(customerEntity);
        return await _customerRepository.SaveChangesAsync();
    }
}