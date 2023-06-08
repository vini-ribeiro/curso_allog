namespace Univali.Api.Entities;

public class Customer
{
    public int Id {get; set;}
    public string Name {get; set;} = string.Empty;
    public string Cpf {get; set;} = string.Empty;
    public IList<Address> Addresses {get; set;} = new List<Address>();
}