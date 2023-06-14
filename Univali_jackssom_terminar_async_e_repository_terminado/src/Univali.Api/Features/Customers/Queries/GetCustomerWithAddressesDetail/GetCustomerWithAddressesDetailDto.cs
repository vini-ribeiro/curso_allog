using Univali.Api.Models;

namespace Univali.Api.Features.Customers.Queries.GetCustomerWithAddressesDetail;

public class GetCustomerWithAddressesDetailDto
{
    public int Id {set; get;}
    public string Name {set; get;} = string.Empty;
    public string Cpf {set; get;} = string.Empty;
    public IEnumerable<AddressDto>? Addresses {set; get;}
}