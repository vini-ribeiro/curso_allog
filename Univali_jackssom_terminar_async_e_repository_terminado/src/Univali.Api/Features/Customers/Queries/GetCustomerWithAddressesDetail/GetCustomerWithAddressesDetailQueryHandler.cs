using AutoMapper;
using Univali.Api.Repositories;

namespace Univali.Api.Features.Customers.Queries.GetCustomerWithAddressesDetail;

public class GetCustomerWithAddressesDetailQueryHandler : IGetCustomerWithAddressesDetailQueryHandler
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public GetCustomerWithAddressesDetailQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<GetCustomerWithAddressesDetailDto> Handle(GetCustomerWithAddressesDetailQuery request)
    {
        var customerFromDatabase = await _customerRepository
            .GetCustomerWithAddressesByIdAsync(request.Id);
        return _mapper.Map<GetCustomerWithAddressesDetailDto>(customerFromDatabase);
    }
}