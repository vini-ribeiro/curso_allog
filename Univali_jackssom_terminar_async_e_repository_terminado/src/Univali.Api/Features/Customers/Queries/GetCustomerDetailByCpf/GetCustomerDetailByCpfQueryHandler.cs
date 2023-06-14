using AutoMapper;
using Univali.Api.Repositories;

namespace Univali.Api.Features.Customers.Queries.GetCustomerDetailByCpf;

public class GetCustomerDetailByCpfQueryHandler : IGetCustomerDetailByCpfQueryHandler
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public GetCustomerDetailByCpfQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<GetCustomerDetailByCpfDto?> Handle(GetCustomerDetailByCpfQuery request)
    {
        var customerFromDatabase = await _customerRepository.GetCustomerByCpfAsync(request.Cpf);
        return _mapper.Map<GetCustomerDetailByCpfDto>(customerFromDatabase);
    }
}