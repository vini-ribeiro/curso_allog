using AutoMapper;
using Univali.Api.Entities;
using Univali.Api.Repositories;

namespace Univali.Api.Features.Customers.Commands.DeleteCustomer;

public class DeleteCustomerCommandHandler : IDeleteCustomerCommandHandler
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public DeleteCustomerCommandHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<bool> Handle(DeleteCustomerCommand request)
    {
        var customerFromDatabase = await _customerRepository.GetCustomerByIdAsync(request.Id);
        var customerEntity = _mapper.Map<Customer>(customerFromDatabase);
        
        _customerRepository.DeleteCustomer(customerEntity);
        return await _customerRepository.SaveChangesAsync();
    }
}