using AutoMapper;
using Univali.Api.Entities;
using Univali.Api.Models;

namespace Univali.Api.Profiles;

public class AddressProfile : Profile
{
    public AddressProfile()
    {
        CreateMap<Address, AddressDto>().ReverseMap();
        CreateMap<AddressForUpdateDto, Address>();
        CreateMap<AddressForCreationDto, Customer>();
    }
}