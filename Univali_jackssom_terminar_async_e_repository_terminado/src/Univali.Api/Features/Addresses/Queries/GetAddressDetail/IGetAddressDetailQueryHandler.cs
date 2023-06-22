namespace Univali.Api.Features.Addresses.Queries.GetAddressDetail;

public interface IGetAddressDetailQueryHandler
{
    Task<GetAddressDetailDto> Handle(GetAddressDetailQuery request);
}
