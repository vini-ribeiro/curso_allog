namespace Univali.Api.Features.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommand
{
    public int Id {set; get;}
    public string Name {set; get;} = string.Empty;
    public string Cpf {set; get;} = string.Empty;
}