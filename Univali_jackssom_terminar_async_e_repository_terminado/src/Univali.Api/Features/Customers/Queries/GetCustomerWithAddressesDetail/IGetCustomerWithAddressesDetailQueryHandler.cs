namespace Univali.Api.Features.Customers.Queries.GetCustomerWithAddressesDetail;

public interface IGetCustomerWithAddressesDetailQueryHandler
{
    Task<GetCustomerWithAddressesDetailDto> Handle(GetCustomerWithAddressesDetailQuery request);
}