using Microsoft.AspNetCore.Mvc;
using Univali.Api.Entities;
using Univali.Api.Models;

namespace Univali.Api.Controllers;

[ApiController]
[Route("api/customers/{customerId}/addresses")]
public class AddressesController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Address>> GetAddresses(int customerId)
    {
        var customerFromDataBase = Data.Instance.Customers
            .FirstOrDefault(customer => customer.Id == customerId);
        
        if (customerFromDataBase == null) return NotFound();

        var addressesToReturn = new List<AddressDto>();

        foreach (var address in customerFromDataBase.Addresses)
        {
            addressesToReturn.Add(new AddressDto
            {
                Id = address.Id,
                Street = address.Street,
                City = address.City
            });
        }

        return Ok(addressesToReturn);
    }

    [HttpGet("{addressId}")]
    public ActionResult<AddressDto> GetAddres(int customerId, int addressId)
    {
        var addressToReturn = Data.Instance.Customers
            .FirstOrDefault(customer => customerId == customer.Id)
            ?.Addresses.FirstOrDefault(address => address.Id == addressId); // operador de propagacao nula e o nome do ? antes do ponto

        return addressToReturn != null ? Ok(addressToReturn) : NotFound();

        //if (customersFromDataBase == null) return NotFound();
    }
}