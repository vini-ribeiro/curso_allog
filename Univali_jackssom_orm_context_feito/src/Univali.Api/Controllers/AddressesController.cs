using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Univali.Api.DbContexts;
using Univali.Api.Entities;
using Univali.Api.Models;

namespace Univali.Api.Controllers;

// [ApiController] podemos tirar pois ja estamos herdando da MainController
[Route("api/customers/{customerId}/addresses")]
public class AddressesController : MainController
{
    private readonly Data _data;
    private readonly IMapper _mapper;
    private readonly CustomerContext _context;

    public AddressesController(Data data, IMapper mapper, CustomerContext context)
    {
        _data = data ?? throw new ArgumentNullException(nameof(data));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    [HttpGet]
    public ActionResult<IEnumerable<AddressDto>> GetAddresses(int customerId)
    {
        // var customerFromDatabase = _context.Customers.Include(c => c.Addresses)
        //     .FirstOrDefault(customer => customer.Id == customerId);
        var addressesFromCustomerFromDatabse = _context.Addresses
            .Select(address => address.CustomerId == customerId).ToList();

        if (addressesFromCustomerFromDatabse == null) return NotFound();

        // var addressesToReturn = new List<AddressDto>();

        // foreach (var address in customerFromDatabase.Addresses)
        // {
        //     addressesToReturn.Add(new AddressDto
        //     {
        //         Id = address.Id,
        //         Street = address.Street,
        //         City = address.City
        //     });
        // }

        var addressesToReturn = _mapper.Map<IEnumerable<AddressDto>>(addressesFromCustomerFromDatabse);

        return Ok(addressesToReturn);
    }

    [HttpGet("{addressId}", Name = "GetAddress")]
    public ActionResult<AddressDto> GetAddress(int customerId, int addressId)
    {
        var customerFromDatabase = _context.Customers
            .FirstOrDefault(customer => customer.Id == customerId);

        if (customerFromDatabase == null) return NotFound();


        var addressFromDatabase = customerFromDatabase.Addresses
            .FirstOrDefault(address => address.Id == addressId);

        if (addressFromDatabase == null) return NotFound();

        // var addressToReturn = new AddressDto
        // {
        //     Id = addressFromDatabase.Id,
        //     Street = addressFromDatabase.Street,
        //     City = addressFromDatabase.City
        // };

        var addressToReturn = _mapper.Map<AddressDto>(addressFromDatabase);

        return Ok(addressToReturn);
    }

    [HttpPost]
    public ActionResult<AddressDto> CreateAddress(
       int customerId,
       AddressForCreationDto addressForCreationDto)
    {
        var customerFromDatabase = _data.Customers
            .FirstOrDefault(c => c.Id == customerId);

        if (customerFromDatabase == null) return NotFound();

        var maxAddressId = _data.Customers
            .SelectMany(c => c.Addresses).Max(a => a.Id);

        var addressEntity = _mapper.Map<Address>(addressForCreationDto);

        addressEntity.Id = maxAddressId + 1;

        customerFromDatabase.Addresses.Add(addressEntity);

        var addressToReturn = _mapper.Map<AddressDto>(addressEntity);

        // var addressEntity = new Address()
        // {
        //     Id = ++maxAddressId,
        //     Street = addressForCreationDto.Street,
        //     City = addressForCreationDto.City
        // };

        // customerFromDatabase.Addresses.Add(addressEntity);

        // var addressToReturn = new AddressDto()
        // {
        //     Id = addressEntity.Id,
        //     City = addressEntity.City,
        //     Street = addressEntity.Street
        // };



        return CreatedAtRoute("GetAddress",
            new
            {
                customerId = customerFromDatabase.Id,
                addressId = addressToReturn.Id
            },
            addressToReturn
        );
    }

    [HttpPut("{addressId}")]
    public ActionResult UpdateAddress(int customerId, int addressId,
       AddressForUpdateDto addressForUpdateDto)
    {
        if (addressForUpdateDto.Id != addressId) return BadRequest();

        var customerFromDatabase = _data.Customers
            .FirstOrDefault(c => c.Id == customerId);

        if (customerFromDatabase == null) return NotFound();

        var addressFromDatabase = customerFromDatabase.Addresses
            .FirstOrDefault(a => a.Id == addressId);

        if (addressFromDatabase == null) return NotFound();

        // addressFromDatabase.City = addressForUpdateDto.City;
        // addressFromDatabase.Street = addressForUpdateDto.Street;

        _mapper.Map(addressForUpdateDto, addressFromDatabase);

        return NoContent();
    }

    [HttpDelete("{addressId}")]
    public ActionResult DeleteAddress(int customerId, int addressId)
    {
        var customerFromDatabase = _data.Customers
            .FirstOrDefault(customer => customer.Id == customerId);

        if (customerFromDatabase == null) return NotFound();

        var addressFromDatabase = customerFromDatabase.Addresses
            .FirstOrDefault(address => address.Id == addressId);

        if (addressFromDatabase == null) return NotFound();

        customerFromDatabase.Addresses.Remove(addressFromDatabase);

        return NoContent();
    }
}
