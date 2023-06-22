namespace Univali.Api.Features.Addresses.Commands.DeleteAddress;

public interface ICreateAddressCommandHandler
{
    Task<CreateAddressCommandDto> Handle(CreateAddressCommand request);
}