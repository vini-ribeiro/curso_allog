using Microsoft.AspNetCore.Mvc;
using Univali.Api.Models;
using Univali.Api.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Univali.Api.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<CustomerDTO>> GetCustomers()
    {
        var customersToReturn = Data.Instance.Customers.Select
        (
            customer => new CustomerDTO
            {
                Id = customer.Id,
                Name = customer.Name,
                Cpf = customer.Cpf
            }
        ); // eu preciso usar o ToList para forcar ela a virar uma lista. sem ele o ok vai transformar isso em lista mesmo sem o ToList (nao e necessario)
        // eu poderia fazer um foreach e criar um customerDTO para cada customer na minha singleton e botar em uma lista

        return Ok(customersToReturn);
    }

    // podemos tirar o [FromRoute] pois o [ApiController] ja faz o trabalho
    //public ActionResult<Customer> GetCustomer([FromRoute] int id)
    //[HttpGet("{id:int:min(1)}", Name = "GetCustomerById")] // resolve o problema do GetCustomer pelo cpf
    [HttpGet("{id}", Name = "GetCustomerById")] //retirei para funcionar o cpf (na verdade era so mudar a rota do cpf para que nao de o erro de ambiguidade de rota, eu poderia ter deixado assim)
    // se passar do tamanho do int ele cai em cpf
    public ActionResult<CustomerDTO> GetCustomerByID(int id)
    {
        var customerFromDatabase =
            Data.Instance.Customers.FirstOrDefault<Customer>(n => n.Id == id);

        if (customerFromDatabase == null) return NotFound();

        CustomerDTO customerToReturn = new CustomerDTO
        {
            Id = customerFromDatabase.Id,
            Name = customerFromDatabase.Name,
            Cpf = customerFromDatabase.Cpf
        };

        return Ok(customerToReturn);
    }

    // da erro por multiplos acessos a endpoints. Para reolver usamos restricoes
    /*
    [HttpGet("{cpf}")]
    public ActionResult<Customer> GetCustomer(string cpf) 
    {
        var customer = Data.Instance.Customers.FirstOrDefault<Customer>(n => n.Cpf == cpf);

        if (customer == null) return NotFound();

        return Ok(customer);
    }
    */

    //agora para chamar o metodo preciso de /cpf/ na frente da url
    // mudar o nome do metodo nao muda que cham. Eh os parametros
    [HttpGet("cpf/{cpf}")]
    public ActionResult<CustomerDTO> GetCustomerByCpf(string cpf)
    {
        var customerFromDatabase =
            Data.Instance.Customers.FirstOrDefault<Customer>(c => c.Cpf == cpf);

        if (customerFromDatabase == null) return NotFound();

        CustomerDTO customerToReturn = new CustomerDTO
        {
            Id = customerFromDatabase.Id,
            Name = customerFromDatabase.Name,
            Cpf = customerFromDatabase.Cpf
        };

        return Ok(customerToReturn);
    }

    [HttpPost]
    public ActionResult<CustomerDTO> CreateCustomer
    (
        CustomerForCreationDto customerForCreationDto
    )  // [FromBody] pode deixar sem isso, api controller ja faz isso para nos
    {
        // o ideal e nao retornar um BadRequest
        if (!ModelState.IsValid)
        {
            Response.ContentType = "application/problem+json";
            //cria a fabrica de um objeto de detalhes de problema de validadcao
            var problemDetailsFactory = HttpContext.RequestServices
                .GetRequiredService<ProblemDetailsFactory>();
            var validationProblemDetails = problemDetailsFactory.CreateValidationProblemDetails(HttpContext, ModelState);

            //atribui o status 422 no corpo do response
            validationProblemDetails.Status = StatusCodes.Status422UnprocessableEntity;

            return UnprocessableEntity(validationProblemDetails);
        }

        // sempre construir quando decebe e sempre controi quando vai enviar
        var customerEntity = new Customer()
        {
            Id = Data.Instance.Customers.Max(c => c.Id) + 1,
            Name = customerForCreationDto.Name,
            Cpf = customerForCreationDto.Cpf   // nao preciso passar o cpf
        };

        Data.Instance.Customers.Add(customerEntity);

        // poderia usar var aqui embaixo
        CustomerDTO customerToReturn = new CustomerDTO()
        {
            Id = customerEntity.Id,
            Name = customerForCreationDto.Name,
            Cpf = customerForCreationDto.Cpf
        };
        //return Ok(newCustomer);
        // retornamos uma rota
        return CreatedAtRoute
        (
            "GetCustomerById", // ele busca pelo nome do metodo (o que esta na rota do metodo)
            new { id = customerToReturn.Id },
            customerToReturn
        );
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteCustomerById(int id)
    {
        var customerFromDataBase = Data.Instance.Customers
            .FirstOrDefault<Customer>(customer => customer.Id == id);

        if (customerFromDataBase == null) return NotFound();

        Data.Instance.Customers.Remove(customerFromDataBase);

        return NoContent();
    }

    /*
        [HttpDelete("deleteByCpf/{cpf}")]
        public ActionResult DeleteCustomerByCpf(string cpf)
        {
            var customerFromDataBase = Data.Instance.Customers
                .FirstOrDefault<Customer>(customer => customer.Cpf == cpf);

            if (customerFromDataBase == null) return NotFound();

            Data.Instance.Customers.Remove(customerFromDataBase);

            return NoContent();
        }
    */
    [HttpPut("{id}")]
    public ActionResult UpdateCustomerById
    (
        int id, CustomerForUpdateDto customerForUpdateDto
    )
    {
        if (id != customerForUpdateDto.Id) return BadRequest();

        var customerFromDataBase = Data.Instance.Customers
            .FirstOrDefault<Customer>(customer => customer.Id == id);

        if (customerFromDataBase == null) return NotFound();

        customerFromDataBase.Name = customerForUpdateDto.Name;
        customerFromDataBase.Cpf = customerForUpdateDto.Cpf;

        return NoContent();
    }

    [HttpPatch("{id}")]
    public ActionResult PartiallyUpdateCustomer(
        [FromBody] JsonPatchDocument<CustomerForPatchDto> patchDocument,
        [FromRoute] int id)
    {
        var customerFromDataBase = Data.Instance.Customers
            .FirstOrDefault(customer => customer.Id == id);

        if (customerFromDataBase == null) return NotFound();

        var customerToPatch = new CustomerForPatchDto
        {
            Name = customerFromDataBase.Name,
            Cpf = customerFromDataBase.Cpf
        };

        patchDocument.ApplyTo(customerToPatch);

        customerFromDataBase.Name = customerToPatch.Name;
        customerFromDataBase.Cpf = customerToPatch.Cpf;

        return NoContent();
    }

    [HttpGet("with-address")]
    public ActionResult<IEnumerable<CustomerWithAddressesDto>> GetCustomersWithAddress()
    {
        var customersFromDataBase = Data.Instance.Customers;

        var customersToReturn = customersFromDataBase
            .Select(customer => new CustomerWithAddressesDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Cpf = customer.Cpf,
                Address = customer.Addresses
                    .Select(address => new AddressDto()
                    {
                        Id = address.Id,
                        City = address.City,
                        Street = address.Street
                    }).ToList()
            });

        return Ok(customersToReturn);
    }


}