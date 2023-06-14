using AutoMapper;
using Univali.Api.Repositories;

namespace Univali.Api.Features.Customers.Queries.GetCustomersDetail;

public class GetCustomersDetailQueryHandler : IGetCustomersDetailQueryHandler
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public GetCustomersDetailQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetCustomersDetailDto>> Handle()
    {
        var customerFromDatabase = await _customerRepository.GetCustomersAsync();
        return _mapper.Map<IEnumerable<GetCustomersDetailDto>>(customerFromDatabase);
    }
}