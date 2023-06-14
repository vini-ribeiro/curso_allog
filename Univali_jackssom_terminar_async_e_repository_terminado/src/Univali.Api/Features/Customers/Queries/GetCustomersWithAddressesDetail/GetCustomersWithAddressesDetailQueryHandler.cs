using AutoMapper;
using Univali.Api.Repositories;

namespace Univali.Api.Features.Customers.Queries.GetCustomersWithAddressesDetail;

public class GetCustomersWithAddressesDetailQueryHandler : IGetCustomersWithAddressesDetailQueryHandler
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public GetCustomersWithAddressesDetailQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetCustomersWithAddressesDetailDto>> Handle()
    {
        var customersFromDatabase = await _customerRepository.GetCustomersWithAddressesAsync();
        return _mapper.Map<IEnumerable<GetCustomersWithAddressesDetailDto>>(customersFromDatabase);
    }
}