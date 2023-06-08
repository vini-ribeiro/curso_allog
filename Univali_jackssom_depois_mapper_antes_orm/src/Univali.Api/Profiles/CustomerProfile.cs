using AutoMapper;
using Univali.Api.Entities;
using Univali.Api.Models;

namespace Univali.Api.Profiles;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        /*
          1˚ arg tipo do objeto de origem
          2˚ arg tipo do objeto de destino
          Mapeia através dos nomes das propriedades
          Se a propriedade não existir é ignorada
        */
        CreateMap<Customer, CustomerDto>();
        CreateMap<CustomerForUpdateDto, Customer>();
        CreateMap<CustomerForCreationDto, Customer>();
        CreateMap<CustomerForPatchDto, Customer>();
        CreateMap<Customer, CustomerWithAddressesDto>();
        CreateMap<CustomerWithAddressesForCreationDto, Customer>().ReverseMap();
        CreateMap<CustomerWithAddressesForUpdateDto, Customer>();
    }
}