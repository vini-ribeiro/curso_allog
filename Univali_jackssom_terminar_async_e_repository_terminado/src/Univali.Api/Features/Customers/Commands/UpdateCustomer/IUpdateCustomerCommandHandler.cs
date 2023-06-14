namespace Univali.Api.Features.Customers.Commands.UpdateCustomer;

public interface IUpdateCustomerCommandHandler
{
    Task<bool> Handle(UpdateCustomerCommand request);
}