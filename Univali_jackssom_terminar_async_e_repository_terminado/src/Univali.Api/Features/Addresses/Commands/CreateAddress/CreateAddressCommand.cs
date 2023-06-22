namespace Univali.Api.Features.Addresses.Commands.CreateAddress;

public class CreateAddressCommand
{
    public string Street {set; get;} = string.Empty;
    public string City {set; get;} = string.Empty;
    public int customerId {set; get;}
}