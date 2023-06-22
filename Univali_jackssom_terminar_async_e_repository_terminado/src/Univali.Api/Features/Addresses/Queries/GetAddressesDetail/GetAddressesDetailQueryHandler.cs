using AutoMapper;
using Univali.Api.Repositories;

namespace Univali.Api.Features.Addresses.Queries.GetAddressesDetail;

public class GetAddressesDetailQueryHandler : IGetAddressesDetailQueryHandler
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public GetAddressesDetailQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetAddressesDetailDto>> Handle(GetAddressesDetailQuery request)
    {
        var addressesFromDatabase = await _customerRepository.GetAddressesAsync(request.Id);
        return _mapper.Map<IEnumerable<GetAddressesDetailDto>>(addressesFromDatabase);
    }
}
