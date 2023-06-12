using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Univali.Api.DbContexts;
using Univali.Api.Entities;
using Univali.Api.Models;

namespace Univali.Api.Controllers;

// [ApiController] podemos tirar pois estamos herdando da mainController
[Route("api/customers")]
public class CustomersController : MainController
{
    private readonly Data _data;
    private readonly IMapper _mapper;
    private readonly CustomerContext _context;

    public CustomersController(Data data, IMapper mapper, CustomerContext context)
    {
        _data = data ?? throw new ArgumentNullException(nameof(data));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    [HttpGet]
    public ActionResult<IEnumerable<CustomerDto>> GetCustomers()
    {
        // usando o context pegamos do banco de dados (precisamos usar o tolist no final se nao nao eh executado) (na vdd o ok ou o mapper cria a lista)
        // var customersFromDatabase = _context.Customers.OrderBy(c => c.Name).ToList(); // esamos ordenando pelo nome
        var customersFromDatabase = _context.Customers.OrderBy(c => c.Id).ToList(); // esamos ordenando pelo nome
        var customersToReturn = _mapper
            .Map<IEnumerable<CustomerDto>>(customersFromDatabase);

        return Ok(customersToReturn);
    }

    [HttpGet("{id}", Name = "GetCustomerById")]
    public ActionResult<CustomerDto> GetCustomerById(int id)
    {
        //var customerFromDatabase = _data.Customers.Where(c => c.Id == id).FirstOrDefault(); nao faca isso

        var customerFromDatabase = _context
            .Customers.FirstOrDefault(c => c.Id == id);

        if (customerFromDatabase == null) return NotFound();

        /*
        CustomerDto customerToReturn = new CustomerDto
        {
            Id = customerFromDatabase.Id,
            Name = customerFromDatabase.Name,
            Cpf = customerFromDatabase.Cpf
        };
        */

        var customerToReturn = _mapper
            .Map<CustomerDto>(customerFromDatabase);

        return Ok(customerToReturn);
    }

    [HttpGet("cpf/{cpf}")]
    public ActionResult<CustomerDto> GetCustomerByCpf(string cpf)
    {
        var customerFromDatabase = _context.Customers
            .FirstOrDefault(c => c.Cpf == cpf);

        if (customerFromDatabase == null)
        {
            return NotFound();
        }

        /*
        CustomerDto customerToReturn = new CustomerDto
        {
            Id = customerFromDatabase.Id,
            Name = customerFromDatabase.Name,
            Cpf = customerFromDatabase.Cpf
        };
        */

        CustomerDto customerToReturn = _mapper
            .Map<CustomerDto>(customerFromDatabase);

        return Ok(customerToReturn);
    }

    [HttpPost]
    public ActionResult<CustomerDto> CreateCustomer(
        CustomerForCreationDto customerForCreationDto)
    {
        /*
        configuracao para validacao de dados
        if (!ModelState.IsValid)
        {
            Response.ContentType = "application/problem+json";

            var problemDetailsFactory = HttpContext.RequestServices
                .GetRequiredService<ProblemDetailsFactory>();

            var validationProblemDetails = problemDetailsFactory
                .CreateValidationProblemDetails(HttpContext, ModelState);

            validationProblemDetails.Status = StatusCodes.Status422UnprocessableEntity;

            return UnprocessableEntity(validationProblemDetails);
        }
        */

        /*
        var customerEntity = new Customer()
        {
            Id = _data.Customers.Max(c => c.Id) + 1,
            Name = customerForCreationDto.Name,
            Cpf = customerForCreationDto.Cpf
        };
        */

        var customerEntity = _mapper.Map<Customer>(customerForCreationDto);

        _context.Customers.Add(customerEntity);
        _context.SaveChanges(); // o add nao salva no banco ele so salva na memoria (entao precisamos fazer o saveChanges) 
                                // pois o banco teria que abrir e fechar o tempo todo

        /*
        var customerToReturn = new CustomerDto
        {
            Id = customerEntity.Id,
            Name = customerForCreationDto.Name,
            Cpf = customerForCreationDto.Cpf
        };
        */

        var customerToReturn = _mapper.Map<CustomerDto>(customerEntity);

        return CreatedAtRoute
        (
            "GetCustomerById",
            new { id = customerToReturn.Id },
            customerToReturn
        );
    }

    [HttpPut("{id}")]
    public ActionResult UpdateCustomer(int id,
        CustomerForUpdateDto customerForUpdateDto)
    {
        if (id != customerForUpdateDto.Id) return BadRequest();

        var customerFromDatabase = _context.Customers
            .FirstOrDefault(customer => customer.Id == id);

        if (customerFromDatabase == null) return NotFound();

        _mapper.Map(customerForUpdateDto, customerFromDatabase);
        _context.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteCustomer(int id)
    {
        var customerFromDatabase = _context.Customers
            .FirstOrDefault(customer => customer.Id == id);

        if (customerFromDatabase == null) return NotFound();

        _context.Customers.Remove(customerFromDatabase);
        _context.SaveChanges();

        return NoContent();
    }

    [HttpPatch("{id}")]
    public ActionResult PartiallyUpdateCustomer(
        [FromBody] JsonPatchDocument<CustomerForPatchDto> patchDocument,
        [FromRoute] int id)
    {
        var customerFromDatabase = _context.Customers
            .FirstOrDefault(customer => customer.Id == id);

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
        _context.SaveChanges();

        return NoContent();
    }

    [HttpGet("with-addresses")]
    public ActionResult<IEnumerable<CustomerWithAddressesDto>> GetCustomersWithAddresses()
    {
        var customersFromDatabase = _context.Customers.Include(c => c.Addresses).ToList();

        // var customersToReturn = customersFromDatabase
        //     .Select(customer => new CustomerWithAddressesDto
        //     {
        //         Id = customer.Id,
        //         Name = customer.Name,
        //         Cpf = customer.Cpf,
        //         Addresses = customer.Addresses
        //             .Select(address => new AddressDto
        //             {
        //                 Id = address.Id,
        //                 City = address.City,
        //                 Street = address.Street
        //             }).ToList()
        //     });

        var customersToReturn = _mapper
            .Map<IEnumerable<CustomerWithAddressesDto>>(customersFromDatabase);

        return Ok(customersToReturn);
    }

    [HttpGet("with-addresses/{customerId}", Name = "GetCustomerWithAddressesById")]
    public ActionResult<CustomerWithAddressesDto> GetCustomerWithAddressesById(int customerId)
    {
        var customerFromDatabase = _context
            .Customers.Include(c => c.Addresses).FirstOrDefault(c => c.Id == customerId);

        if (customerFromDatabase == null) return NotFound();

        // var addressesDto = customerFromDatabase
        //     .Addresses.Select(address =>
        //     new AddressDto
        //     {
        //         Id = address.Id,
        //         City = address.City,
        //         Street = address.Street
        //     }
        // ).ToList();

        // var customerToReturn = new CustomerWithAddressesDto
        // {
        //     Id = customerFromDatabase.Id,
        //     Name = customerFromDatabase.Name,
        //     Cpf = customerFromDatabase.Cpf,
        //     Addresses = addressesDto
        // };

        var customersToReturn = _mapper
            .Map<CustomerWithAddressesDto>(customerFromDatabase);

        return Ok(customersToReturn);
    }

    [HttpPost("with-addresses")]
    public ActionResult<CustomerWithAddressesDto> CreateCustomerWithAddresses(
       CustomerWithAddressesForCreationDto customerWithAddressesForCreationDto)
    {
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

        var customerEntity = _mapper
            .Map<Customer>(customerWithAddressesForCreationDto);

        // customerEntity.Id = _data.Customers.Max(customer => customer.Id) + 1;

        // for (int i = 0; i < customerEntity.Addresses.Count; i++)
        // {
        //     customerEntity.Addresses[i].Id = ++maxAddressId;
        // }

        _context.Customers.Add(customerEntity);
        _context.SaveChanges();

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
    public ActionResult UpdateCustomerWithAddresses(int customerId,
       CustomerWithAddressesForUpdateDto customerWithAddressesForUpdateDto)
    {
        if (customerId != customerWithAddressesForUpdateDto.Id) return BadRequest();

        var customerFromDatabase = _context.Customers.Include(c => c.Addresses)
            .FirstOrDefault(c => c.Id == customerId);

        if (customerFromDatabase == null) return NotFound();

        // customerFromDatabase.Name = customerWithAddressesForUpdateDto.Name;
        // customerFromDatabase.Cpf = customerWithAddressesForUpdateDto.Cpf;

        // preciso pegar o maior id antes de fazer o map pois irei adicionar um id externo na base de dados
        // var maxAddressId = _data.Customers
        //     .SelectMany(c => c.Addresses)
        //     .Max(c => c.Id);

        _mapper.Map(customerWithAddressesForUpdateDto, customerFromDatabase);
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