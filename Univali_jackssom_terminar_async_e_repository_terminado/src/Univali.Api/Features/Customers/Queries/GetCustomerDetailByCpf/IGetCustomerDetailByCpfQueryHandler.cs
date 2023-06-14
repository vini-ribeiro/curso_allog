namespace Univali.Api.Features.Customers.Queries.GetCustomerDetailByCpf;

public interface IGetCustomerDetailByCpfQueryHandler
{
    Task<GetCustomerDetailByCpfDto?> Handle(GetCustomerDetailByCpfQuery request);
}