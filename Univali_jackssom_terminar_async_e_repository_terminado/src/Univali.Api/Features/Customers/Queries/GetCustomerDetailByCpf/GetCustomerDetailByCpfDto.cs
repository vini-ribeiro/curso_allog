namespace Univali.Api.Features.Customers.Queries.GetCustomerDetailByCpf;
   
public class GetCustomerDetailByCpfDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
}
