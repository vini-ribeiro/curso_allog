namespace Univali.Api.Features.Addresses.Queries.GetAddressDetail;

public class GetAddressDetailDto
{
    public int Id {get; set;}
    public string Street {get; set;} = string.Empty;
    public string City {get; set;} = string.Empty;
}