using Univali.Api.Features.Customers.Queries.GetCustomerDetail;

namespace Univali.Api.Features.Customers.Queries.GetCustomersDetail;

public class GetCustomersDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
}
