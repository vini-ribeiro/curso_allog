namespace Univali.Api.Features.Customers.Queries.GetCustomersWithAddressesDetail;

public interface IGetCustomersWithAddressesDetailQueryHandler
{
    Task<IEnumerable<GetCustomersWithAddressesDetailDto>> Handle();
}