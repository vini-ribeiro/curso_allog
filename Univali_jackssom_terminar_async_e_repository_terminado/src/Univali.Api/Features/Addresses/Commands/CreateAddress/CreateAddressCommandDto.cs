namespace Univali.Api.Features.Addresses.Commands.CreateAddress;

public class CreateAddressCommandDto
{
    public int Id {set; get;}
    public string Street {set; get;} = string.Empty;
    public string City {set; get;} = string.Empty;
}