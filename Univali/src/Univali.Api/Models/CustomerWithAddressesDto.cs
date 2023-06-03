namespace Univali.Api.Models;

public class CustomerWithAddressesDto
{
    public int Id {set; get;}
    public string Name {set; get;} = string.Empty;
    public string Cpf {set; get;} = string.Empty;
    public ICollection<AddressDto> Address {set; get;} = new List<AddressDto>();
}