namespace Univali.Api.Features.Customers.Commands.DeleteCustomer;

public interface IDeleteCustomerCommandHandler
{
    Task<bool> Handle(DeleteCustomerCommand request);
}