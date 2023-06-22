namespace Univali.Api.Features.Addresses.Commands.CreateAddress;

public interface ICreateAddressCommandHandler
{
    Task<CreateAddressCommandDto> Handle(CreateAddressCommand request);
}