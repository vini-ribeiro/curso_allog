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
        /*
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
        */

        var addressesOfCustomerFromDataBase = Data.Instance.Customers
            .FirstOrDefault(customer => customer.Id == customerId)?.Addresses;

        if (addressesOfCustomerFromDataBase == null) return NotFound();

        var addressesToReturn = addressesOfCustomerFromDataBase
            .Select // e interessando usar o Select ao inves do foreach (ou for) por questoes de desempenho
            (
                address => new AddressDto()
                {
                    Id = address.Id,
                    Street = address.Street,
                    City = address.City
                }
            ); // poderia adicionar um .ToList() no final para trnasformar para lista (implicitamente o Ok faz isso, entao nao e necessario)

        return Ok(addressesToReturn);
    }

    [HttpGet("{addressId}")]
    public ActionResult<AddressDto> GetAddress(int customerId, int addressId)
    {
        var addressOfCustomerFromDataBase = Data.Instance.Customers
            .FirstOrDefault(customer => customerId == customer.Id)
            ?.Addresses.FirstOrDefault(address => address.Id == addressId); // operador de propagacao nula e o nome do ? antes do ponto que evita desrreferenciar um objeto null

        if (addressOfCustomerFromDataBase == null) return NotFound();

        // Criacao da dto para retornar o address
        var addressToReturn = new AddressDto()
        {
            Id = addressOfCustomerFromDataBase.Id,
            Street = addressOfCustomerFromDataBase.Street,
            City = addressOfCustomerFromDataBase.City
        };

        return Ok(addressToReturn);
    }

    [HttpPost]
    public ActionResult CreateAddress
    (
        int customerId,
        AddressForCreationDto addressForCreationDto
    )
    {
        var customerFromDataBase = Data.Instance.Customers
            .FirstOrDefault(customer => customer.Id == customerId);

        if (customerFromDataBase == null) return NotFound();

        Address addressEntity = new Address()
        {
            Id = Data.Instance.Customers.SelectMany(customers => customers.Addresses)
                    .Max(address => address.Id) + 1,
            Street = addressForCreationDto.Street,
            City = addressForCreationDto.City
        };

        customerFromDataBase.Addresses.Add(addressEntity);

        return NoContent();
    }

    [HttpDelete("{addressId}")]
    public ActionResult DeleteAddress(int customerId, int addressId)
    {
        var customerFromDataBase = Data.Instance.Customers
            .FirstOrDefault(customer => customer.Id == customerId);

        if (customerFromDataBase == null) return NotFound();

        var addressToDelete = customerFromDataBase.Addresses
            .FirstOrDefault(address => address.Id == addressId);

        if (addressToDelete == null) return NotFound();

        customerFromDataBase.Addresses.Remove(addressToDelete);

        /*
        var addressOfCustomerFromDataBase = Data.Instance.Customers
            .FirstOrDefault(customer => customer.Id == customerId)
            ?.Addresses.FirstOrDefault(address => address.Id == addressId);
        
        if (addressOfCustomerFromDataBase == null) return NotFound();

        Data.Instance.Customers
            .FirstOrDefault(customer => customer.Id == customerId)
            ?.Addresses.Remove(addressOfCustomerFromDataBase);
        */

        return NoContent();
    }

    [HttpPut("{addressId}")]
    public ActionResult UpdateAddress
    (
        int customerId,
        int addressId,
        AddressForUpdateDto addressForUpdateDto
    )
    {
        if (addressId != addressForUpdateDto.Id) return BadRequest();

        var addressOfCustomerFromDataBase = Data.Instance.Customers
            .FirstOrDefault(customer => customer.Id == customerId)
            ?.Addresses.FirstOrDefault(address => address.Id == addressId);

        if (addressOfCustomerFromDataBase == null) return NotFound();

        addressOfCustomerFromDataBase.City = addressForUpdateDto.City;
        addressOfCustomerFromDataBase.Street = addressForUpdateDto.Street;

        return NoContent();
    }

    /*
    [HttpPut]
    public ActionResult UpdateCustomerAddresses
    (
        int customerId,
        IEnumerable<AddressForCreationDto> addressForCreationDto
    )
    {
        var customerFromDataBase = Data.Instance.Customers
            .FirstOrDefault(customer => customer.Id == customerId);

        if (customerFromDataBase == null) return NotFound();

        int maxId = Data.Instance.Customers.SelectMany(customers => customers.Addresses)
                        .Max(address => address.Id) + 1;

        List<Address> addressesEntity = addressForCreationDto.Select
        (
            addressFromInput => new Address()
            {
                Id = maxId++,
                Street = addressFromInput.Street,
                City = addressFromInput.City
            }
        ).ToList();

        customerFromDataBase.Addresses = addressesEntity;

        return NoContent();
    }
    */
}