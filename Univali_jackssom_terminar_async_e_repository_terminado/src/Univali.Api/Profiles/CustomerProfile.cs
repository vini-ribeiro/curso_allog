using AutoMapper;
using Univali.Api.Entities;
using Univali.Api.Features.Customers.Commands.CreateCustomer;
using Univali.Api.Features.Customers.Commands.DeleteCustomer;
using Univali.Api.Features.Customers.Commands.UpdateCustomer;
using Univali.Api.Features.Customers.Queries.GetCustomerDetail;
using Univali.Api.Features.Customers.Queries.GetCustomerDetailByCpf;
using Univali.Api.Features.Customers.Queries.GetCustomersDetail;
using Univali.Api.Features.Customers.Queries.GetCustomersWithAddressesDetail;
using Univali.Api.Features.Customers.Queries.GetCustomerWithAddressesDetail;
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
        // velhos
        CreateMap<Customer, CustomerDto>();
        CreateMap<CustomerForUpdateDto, Customer>();
        CreateMap<CustomerForCreationDto, Customer>();
        CreateMap<CustomerForPatchDto, Customer>().ReverseMap();
        CreateMap<Customer, CustomerWithAddressesDto>();
        CreateMap<CustomerWithAddressesForCreationDto, Customer>().ReverseMap();
        CreateMap<CustomerWithAddressesForUpdateDto, Customer>();

        // novos
        CreateMap<Customer, GetCustomerDetailDto>();
        CreateMap<Customer, GetCustomerDetailByCpfDto>();
        CreateMap<Customer, GetCustomersDetailDto>();
        CreateMap<CreateCustomerCommand, Customer>();
        CreateMap<Customer, CreateCustomerDto>();
        CreateMap<UpdateCustomerCommand, Customer>();
        CreateMap<DeleteCustomerCommand, Customer>();
        
        CreateMap<Customer, GetCustomersWithAddressesDetailDto>();
        CreateMap<Customer, GetCustomerWithAddressesDetailDto>();
    }
}