using AutoMapper;
using Univali.Api.Repositories;

namespace Univali.Api.Features.Addresses.Queries.GetAddressDetail;

public class GetAddressDetailQueryHandler : IGetAddressDetailQueryHandler
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public GetAddressDetailQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<GetAddressDetailDto> Handle(GetAddressDetailQuery request)
    {
        var addressesFromDatabase = await _customerRepository.GetAddressAsync(request.Id);
        return _mapper.Map<GetAddressDetailDto>(addressesFromDatabase);
    }
}
