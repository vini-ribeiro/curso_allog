using AutoMapper;
using Univali.Api.Entities;
using Univali.Api.Features.Addresses.Commands.CreateAddress;
using Univali.Api.Features.Addresses.Queries.GetAddressDetail;
using Univali.Api.Features.Addresses.Queries.GetAddressesDetail;
using Univali.Api.Models;

namespace Univali.Api.Profiles;

public class AddressProfile : Profile
{
    public AddressProfile()
    {
        CreateMap<Address, AddressDto>().ReverseMap();
        CreateMap<AddressForUpdateDto, Address>();
        CreateMap<AddressForCreationDto, Address>();
        
        CreateMap<Address, GetAddressesDetailDto>();
        CreateMap<Address, GetAddressDetailDto>();
        CreateMap<Address, CreateAddressCommandDto>();
        CreateMap<CreateAddressCommand, Address>();
    }
}