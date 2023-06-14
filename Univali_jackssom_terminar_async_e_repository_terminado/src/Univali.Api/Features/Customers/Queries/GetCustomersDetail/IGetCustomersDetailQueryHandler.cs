namespace Univali.Api.Features.Customers.Queries.GetCustomersDetail;

public interface IGetCustomersDetailQueryHandler
{
    Task<IEnumerable<GetCustomersDetailDto>> Handle();
}