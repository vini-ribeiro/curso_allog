using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Univali.Api.DbContexts;
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
using Univali.Api.Repositories;

namespace Univali.Api.Controllers;

// [ApiController] podemos tirar pois estamos herdando da mainController
[Route("api/customers")]
public class CustomersController : MainController
{
    private readonly Data _data;
    private readonly IMapper _mapper;
    private readonly CustomerContext _context;
    private readonly ICustomerRepository _customerRepository;

    public CustomersController(Data data, IMapper mapper, CustomerContext context, ICustomerRepository customerRepository)
    {
        _data = data ?? throw new ArgumentNullException(nameof(data));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
    }

    [HttpGet]
    public async Task<ActionResult<GetCustomersDetailDto>> GetCustomers(
        [FromServices] IGetCustomersDetailQueryHandler handler
    )
    {
        // usando o context pegamos do banco de dados (precisamos usar o tolist no final se nao nao eh executado) (na vdd o ok ou o mapper cria a lista)
        // var customersFromDatabase = _context.Customers.OrderBy(c => c.Name).ToList(); // esamos ordenando pelo nome
        // var customersFromDatabase = await _customerRepository.GetCustomersAsync(); // esamos ordenando pelo nome
        // var customersToReturn = _mapper
        //     .Map<IEnumerable<CustomerDto>>(customersFromDatabase);

        var customersToReturn = await handler.Handle();

        return Ok(customersToReturn);
    }

    [HttpGet("{customerId}", Name = "GetCustomerById")]
    public async Task<ActionResult<GetCustomerDetailDto>> GetCustomerById(
        [FromServices] IGetCustomerDetailQueryHandler handler,
        int customerId)
    {
        //var customerFromDatabase = _data.Customers.Where(c => c.Id == id).FirstOrDefault(); nao faca isso

        // var customerFromDatabase = await _customerRepository.GetCustomerByIdAsync(customerId);
        var getCustomerDetailQuery = new GetCustomerDetailQuery { Id = customerId }; // eu poderia botar isso no parametro no lugar o int

        var customerToReturn = await handler.Handle(getCustomerDetailQuery);

        if (customerToReturn == null) return NotFound();

        return Ok(customerToReturn);
    }

    [HttpGet("cpf/{customerCpf}")]
    public async Task<ActionResult<CustomerDto>> GetCustomerByCpf(
        [FromServices] IGetCustomerDetailByCpfQueryHandler handler,
        string customerCpf)
    {
        // abaixo ficava as operacoes que agora ficam no repository
        // var customerFromDatabase = await _customerRepository.GetCustomerByCpfAsync(customerCpf);

        var getCustomerDetailByCpfQuery = new GetCustomerDetailByCpfQuery { Cpf = customerCpf };

        var customerToReturn = await handler.Handle(getCustomerDetailByCpfQuery);

        if (customerToReturn == null) return NotFound();

        /*
        CustomerDto customerToReturn = new CustomerDto
        {
            Id = customerFromDatabase.Id,
            Name = customerFromDatabase.Name,
            Cpf = customerFromDatabase.Cpf
        };
        */

        // CustomerDto customerToReturn = _mapper
        //     .Map<CustomerDto>(customerFromDatabase);

        return Ok(customerToReturn);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> CreateCustomer(
        CreateCustomerCommand createCustomerCommand,
        [FromServices] ICreateCustomerCommandHandler handler // ao inves de botar no construtor eu passo pelo FromServices pois ele eh transient e so usa no metodo
        )
    {
        var customerToReturn = await handler.Handle(createCustomerCommand);

        return CreatedAtRoute
        (
            nameof(GetCustomerById), // ou "GerCustomerById"
            new { customerId = customerToReturn.Id }, // o nome da propriedade do objeto precisa ser igual ao da rota
            customerToReturn
        );

        // /*
        // configuracao para validacao de dados
        // if (!ModelState.IsValid)
        // {
        //     Response.ContentType = "application/problem+json";

        //     var problemDetailsFactory = HttpContext.RequestServices
        //         .GetRequiredService<ProblemDetailsFactory>();

        //     var validationProblemDetails = problemDetailsFactory
        //         .CreateValidationProblemDetails(HttpContext, ModelState);

        //     validationProblemDetails.Status = StatusCodes.Status422UnprocessableEntity;

        //     return UnprocessableEntity(validationProblemDetails);
        // }
        // */

        // /*
        // var customerEntity = new Customer()
        // {
        //     Id = _data.Customers.Max(c => c.Id) + 1,
        //     Name = customerForCreationDto.Name,
        //     Cpf = customerForCreationDto.Cpf
        // };
        // */

        // Customer customerEntity = _mapper.Map<Customer>(customerForCreationDto);

        // // await _context.Customers.Add(customerEntity);
        // // await _context.SaveChanges(); // o add nao salva no banco ele so salva na memoria (entao precisamos fazer o saveChanges) 
        // // pois o banco teria que abrir e fechar o tempo todo
        // customerEntity = await _customerRepository.CreateCustomerAsync(customerEntity);

        // /*
        // var customerToReturn = new CustomerDto
        // {
        //     Id = customerEntity.Id,
        //     Name = customerForCreationDto.Name,
        //     Cpf = customerForCreationDto.Cpf
        // };
        // */

        // var customerToReturn = _mapper.Map<CustomerDto>(customerEntity);

        // return CreatedAtRoute
        // (
        //     nameof(GetCustomerById), // ou "GerCustomerById"
        //     new { customerId = customerToReturn.Id }, // o nome da propriedade do objeto precisa ser igual ao da rota
        //     customerToReturn
        // );
    }

    [HttpPut("{customerId}")]
    public async Task<ActionResult> UpdateCustomer(int customerId,
        UpdateCustomerCommand updateCustomerCommand,
        [FromServices] IUpdateCustomerCommandHandler handler)
    {
        if (customerId != updateCustomerCommand.Id) return BadRequest();

        bool changed = await handler.Handle(updateCustomerCommand);

        if (!changed) return BadRequest();

        return NoContent();

        // var customerExist = await _customerRepository.CustomerExistAsync(customerId);

        // if (!customerExist) return NotFound();

        // var CustomerForUpdate = _mapper.Map<Customer>(customerForUpdateDto);

        // await _customerRepository.UpdateCustomerAsync(CustomerForUpdate);

        // var customerFromDatabase = _context.Customers
        //     .FirstOrDefault(customer => customer.Id == customerId);

        /*
        var customerFromDatabase = await _customerRepository.GetCustomerByIdAsync(customerId);

        if (customerFromDatabase == null) return NotFound();

        _mapper.Map(customerForUpdateDto, customerFromDatabase);
        _context.SaveChanges();
        */
    }

    [HttpDelete("{customerId}")]
    public async Task<ActionResult> DeleteCustomer
    (
        int customerId,
        [FromServices] IDeleteCustomerCommandHandler handler
    )
    {
        var deleteCustomerCommand = new DeleteCustomerCommand { Id = customerId };

        bool deleted = await handler.Handle(deleteCustomerCommand);

        if (!deleted) return BadRequest();

        return NoContent();
    }

    [HttpPatch("{customerId}")]
    public async Task<ActionResult> PartiallyUpdateCustomer(
        [FromBody] JsonPatchDocument<CustomerForPatchDto> patchDocument,
        [FromRoute] int customerId)
    {
        var customerFromDatabase = await _customerRepository.GetCustomerByIdAsync(customerId);

        if (customerFromDatabase == null) return NotFound();

        // depois eu faco o mapper para o patch
        // var customerToPatch = new CustomerForPatchDto
        // {
        //     Name = customerFromDatabase.Name,
        //     Cpf = customerFromDatabase.Cpf
        // };

        var customerToPatch = _mapper.Map<CustomerForPatchDto>(customerFromDatabase);

        patchDocument.ApplyTo(customerToPatch, ModelState);

        if (!TryValidateModel(customerToPatch))
        {
            // return UnprocessableEntity(ModelState); isso daqui nao gera cabealho. pra gerar temos que chamar ValidationProblem
            return ValidationProblem(ModelState);
        }

        // customerFromDatabase.Name = customerToPatch.Name;
        // customerFromDatabase.Cpf = customerToPatch.Cpf;

        _mapper.Map(customerToPatch, customerFromDatabase);
        await _customerRepository.SaveAsync();

        return NoContent();
    }

    [HttpGet("with-addresses")]
    public async Task<ActionResult<IEnumerable<CustomerWithAddressesDto>>> GetCustomersWithAddresses
    (
        [FromServices] IGetCustomersWithAddressesDetailQueryHandler handler
    )
    {
        // var customersFromDatabase = _context.Customers.Include(c => c.Addresses).OrderBy(c => c.Name).ToList(); // posso fazer um orderby 
        // var customersFromDatabase = _context.Customers.Include(c => c.Addresses).ToList(); // .OrderBy(c => c.Name) depois do include para ordenar a lista
        // var customersFromDatabase = await _customerRepository.GetCustomersWithAddressesAsync();

        // // var customersToReturn = customersFromDatabase
        // //     .Select(customer => new CustomerWithAddressesDto
        // //     {
        // //         Id = customer.Id,
        // //         Name = customer.Name,
        // //         Cpf = customer.Cpf,
        // //         Addresses = customer.Addresses
        // //             .Select(address => new AddressDto
        // //             {
        // //                 Id = address.Id,
        // //                 City = address.City,
        // //                 Street = address.Street
        // //             }).ToList()
        // //     });

        // var customersToReturn = _mapper
        //     .Map<IEnumerable<CustomerWithAddressesDto>>(customersFromDatabase);

        var customersToReturn = await handler.Handle();

        return Ok(customersToReturn);
    }

    [HttpGet("with-addresses/{customerId}", Name = "GetCustomerWithAddressesById")]
    public async Task<ActionResult<CustomerWithAddressesDto>> GetCustomerWithAddressesById
    (
        int customerId,
        [FromServices] IGetCustomerWithAddressesDetailQueryHandler handler
    )
    {
        // // var customerFromDatabase = _context
        // //     .Customers.Include(c => c.Addresses).FirstOrDefault(c => c.Id == customerId);

        // var customerFromDatabase = await _customerRepository.GetCustomerWithAddressesByIdAsync(customerId);

        // if (customerFromDatabase == null) return NotFound();

        // // var addressesDto = customerFromDatabase
        // //     .Addresses.Select(address =>
        // //     new AddressDto
        // //     {
        // //         Id = address.Id,
        // //         City = address.City,
        // //         Street = address.Street
        // //     }
        // // ).ToList();

        // // var customerToReturn = new CustomerWithAddressesDto
        // // {
        // //     Id = customerFromDatabase.Id,
        // //     Name = customerFromDatabase.Name,
        // //     Cpf = customerFromDatabase.Cpf,
        // //     Addresses = addressesDto
        // // };

        // var customersToReturn = _mapper
        //     .Map<CustomerWithAddressesDto>(customerFromDatabase);
        var getCustomerWithAddressesDetailQuery = new GetCustomerWithAddressesDetailQuery
        {
            Id = customerId
        };
        var customerToReturn = await handler.Handle(getCustomerWithAddressesDetailQuery);

        return Ok(customerToReturn);
    }

    [HttpPost("with-addresses")]
    public async Task<ActionResult<CustomerWithAddressesDto>> CreateCustomerWithAddresses(
       CustomerWithAddressesForCreationDto customerWithAddressesForCreationDto)
    {
        /*
        // var maxAddressId = _data.Customers
        //     .SelectMany(c => c.Addresses).Max(c => c.Id);

        // List<Address> AddressesEntity = customerWithAddressesForCreationDto.Addresses
        //     .Select(address =>
        //         new Address
        //         {
        //             Id = ++maxAddressId,
        //             Street = address.Street,
        //             City = address.City
        //         }).ToList();

        // var customerEntity = new Customer
        // {
        //     Id = _data.Customers.Max(c => c.Id) + 1, // Obtém id do customer
        //     Name = customerWithAddressesForCreationDto.Name,
        //     Cpf = customerWithAddressesForCreationDto.Cpf,
        //     Addresses = AddressesEntity // Atribui o Address mapeado
        // };
        */

        var customerEntity = _mapper
            .Map<Customer>(customerWithAddressesForCreationDto);

        /*
        // customerEntity.Id = _data.Customers.Max(customer => customer.Id) + 1;

        // for (int i = 0; i < customerEntity.Addresses.Count; i++)
        // {
        //     customerEntity.Addresses[i].Id = ++maxAddressId;
        // }

        // _context.Customers.Add(customerEntity);
        // _context.SaveChanges();
        */

        customerEntity = await _customerRepository.CreateCustomerWithAddressesAsync(customerEntity);

        /*
        // List<AddressDto> addressesDto = customerEntity.Addresses
        //     .Select(address =>
        //         new AddressDto
        //         {
        //             Id = address.Id,
        //             Street = address.Street,
        //             City = address.City
        //         }).ToList();

        // var customerToReturn = new CustomerWithAddressesDto
        // {
        //     Id = customerEntity.Id,
        //     Name = customerEntity.Name,
        //     Cpf = customerEntity.Cpf,
        //     Addresses = addressesDto
        // };
        */

        var customerToReturn = _mapper
            .Map<CustomerWithAddressesDto>(customerEntity);

        return CreatedAtRoute
        (
            "GetCustomerWithAddressesById",
            new { customerId = customerToReturn.Id },
            customerToReturn
        );
    }

    [HttpPut("with-addresses/{customerId}")]
    public async Task<ActionResult> UpdateCustomerWithAddresses(int customerId,
       CustomerWithAddressesForUpdateDto customerWithAddressesForUpdateDto)
    {
        if (customerId != customerWithAddressesForUpdateDto.Id) return BadRequest();

        var CustomerWithAddressesForUpdate = _mapper.Map<Customer>(customerWithAddressesForUpdateDto);

        bool customerExist = await _customerRepository.CustomerExistAsync(customerId);

        if (!customerExist) return NotFound();

        await _customerRepository.UpdateCustomerWithAddressesAsync(CustomerWithAddressesForUpdate);

        // .Include(c => c.Addresses) se eu botar o include ele tras a lista e o mapper substitui ela
        /*
        var customerFromDatabase = _context.Customers 
            .FirstOrDefault(c => c.Id == customerId);

        if (customerFromDatabase == null) return NotFound();

        // customerFromDatabase.Name = customerWithAddressesForUpdateDto.Name;
        // customerFromDatabase.Cpf = customerWithAddressesForUpdateDto.Cpf;
        // preciso pegar o maior id antes de fazer o map pois irei adicionar um id externo na base de dados
        // var maxAddressId = _data.Customers
        //     .SelectMany(c => c.Addresses)
        //     .Max(c => c.Id);

        _context.SaveChanges();
        // customerFromDatabase.Addresses = customerWithAddressesForUpdateDto
        //                                 .Addresses.Select(
        //                                     address =>
        //                                     new Address()
        //                                     {
        //                                         Id = ++maxAddressId,
        //                                         City = address.City,
        //                                         Street = address.Street
        //                                     }
        //                                 ).ToList();

        // for (int i = 0; i < customerFromDatabase.Addresses.Count; i++)
        // {
        //     customerFromDatabase.Addresses[i].Id = ++maxAddressId;
        // }
        */

        return NoContent();
    }

    // este codigo gera o cabecalho dos erros para quando nos digitamos o cpf errado no patch
    // public override ActionResult ValidationProblem(
    //     [ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
    // {
    //     var options = HttpContext.RequestServices
    //         .GetRequiredService<IOptions<ApiBehaviorOptions>>();

    //     return (ActionResult)options.Value
    //         .InvalidModelStateResponseFactory(ControllerContext);
    // }

}