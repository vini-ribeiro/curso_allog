namespace Univali.Api.Features.Addresses.Queries.GetAddressesDetail;

public interface IGetAddressesDetailQueryHandler
{
    Task<IEnumerable<GetAddressesDetailDto>> Handle(GetAddressesDetailQuery request);
}
