using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Univali.Api.DbContexts;
using Univali.Api.Entities;
using Univali.Api.Features.Addresses.Commands.CreateAddress;
using Univali.Api.Features.Addresses.Queries.GetAddressDetail;
using Univali.Api.Features.Addresses.Queries.GetAddressesDetail;
using Univali.Api.Models;
using Univali.Api.Repositories;

namespace Univali.Api.Controllers;

// [ApiController] podemos tirar pois ja estamos herdando da MainController
[Route("api/customers/{customerId}/addresses")]
public class AddressesController : MainController
{
    private readonly Data _data;
    private readonly IMapper _mapper;
    private readonly CustomerContext _context;
    private readonly ICustomerRepository _customerRepository;

    public AddressesController(Data data, IMapper mapper, CustomerContext context, ICustomerRepository customerRepository)
    {
        _data = data ?? throw new ArgumentNullException(nameof(data));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AddressDto>>> GetAddresses(int customerId,
        [FromServices] IGetAddressesDetailQueryHandler handler
    )
    {
        // var customerFromDatabase = _context.Customers.Include(c => c.Addresses)
        //     .FirstOrDefault(customer => customer.Id == customerId);
        // var addressesFromCustomerFromDatabse = _context.Addresses
        //     .Select(address => address.CustomerId == customerId).ToList(); select funciona apenas no linq e nao no banco
        // var addressesFromCustomerFromDatabse = _context.Addresses
        //     .Where(address => address.CustomerId == customerId).ToList(); // para funcinar no banco

        // var addressesFromCustomerFromDatabse = await _customerRepository.GetAddressesAsync(customerId);

        // if (addressesFromCustomerFromDatabse == null) return NotFound();

        // // var addressesToReturn = new List<AddressDto>();

        // // foreach (var address in customerFromDatabase.Addresses)
        // // {
        // //     addressesToReturn.Add(new AddressDto
        // //     {
        // //         Id = address.Id,
        // //         Street = address.Street,
        // //         City = address.City
        // //     });
        // // }

        // var addressesToReturn = _mapper.Map<IEnumerable<AddressDto>>(addressesFromCustomerFromDatabse);

        var getAddressesDetailQuery = new GetAddressesDetailQuery {Id = customerId};

        var addressesToReturn = await handler.Handle(getAddressesDetailQuery);

        return Ok(addressesToReturn);
    }

    [HttpGet("{addressId}", Name = "GetAddress")]
    public async Task<ActionResult<AddressDto>> GetAddress
    (
        int customerId, 
        int addressId,
        [FromServices] IGetAddressDetailQueryHandler handler
    ) // aqui customerId se torna inutil
    {
        // var customerFromDatabase = _context.Customers.Include(c => c.Addresses)
        //     .FirstOrDefault(customer => customer.Id == customerId);

        // if (customerFromDatabase == null) return NotFound();

        // var addressFromDatabase = customerFromDatabase.Addresses
        //     .FirstOrDefault(address => address.Id == addressId);
        // var addressFromDatabase = await _customerRepository.GetAddressAsync(addressId);

        // if (addressFromDatabase == null) return NotFound();

        // // var addressToReturn = new AddressDto
        // // {
        // //     Id = addressFromDatabase.Id,
        // //     Street = addressFromDatabase.Street,
        // //     City = addressFromDatabase.City
        // // };

        // var addressToReturn = _mapper.Map<AddressDto>(addressFromDatabase);

        var getAddressDetailQuery = new GetAddressDetailQuery {Id = addressId};

        var addressToReturn = await handler.Handle(getAddressDetailQuery);

        if (addressToReturn == null) return NotFound();

        return Ok(addressToReturn);
    }

    [HttpPost]
    public async Task<ActionResult<AddressDto>> CreateAddress(
       int customerId,
       AddressForCreationDto addressForCreationDto,
       [FromServices] ICreateAddressCommandHandler handler)
    {
        // var customerFromDatabase = _context.Customers.Include(c => c.Addresses)
        // //     .FirstOrDefault(c => c.Id == customerId);

        // bool customerExist = await _customerRepository.CustomerExistAsync(customerId);

        // if (!customerExist) return NotFound();

        // // var maxAddressId = _data.Customers
        // //     .SelectMany(c => c.Addresses).Max(a => a.Id);

        // var addressEntity = _mapper.Map<Address>(addressForCreationDto);
        // addressEntity.CustomerId = customerId;
        // addressEntity.Customer = await _customerRepository.GetCustomerByIdAsync(customerId);
        // // addressEntity.Id = maxAddressId + 1;

        // Address addressCreated = await _customerRepository.CreateAddressAsync(addressEntity);
        // var addressToReturn = _mapper.Map<AddressDto>(addressCreated);

        // var addressToReturn = _mapper.Map<AddressDto>(addressEntity);

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

        var createAddressCommand = new CreateAddressCommand 
        {
            Street = addressForCreationDto.Street,
            City = addressForCreationDto.City,
            customerId = customerId
        };

        var addressToReturn = await handler.Handle(createAddressCommand);

        return CreatedAtRoute("GetAddress",
            new
            {
                customerId = customerId,
                addressId = addressToReturn.Id
            },
            addressToReturn
        );
    }

    [HttpPut("{addressId}")]
    public async Task<ActionResult> UpdateAddress(int customerId, int addressId,
       AddressForUpdateDto addressForUpdateDto)
    {
        if (addressForUpdateDto.Id != addressId) return BadRequest();

        bool addressExist = await _customerRepository.AddressExistAsync(customerId, addressId);

        if (!addressExist) return NotFound();

        /*
        var customerFromDatabase = _context.Customers.Include(c => c.Addresses)
            .FirstOrDefault(c => c.Id == customerId);

        if (customerFromDatabase == null) return NotFound();

        var addressFromDatabase = customerFromDatabase.Addresses
            .FirstOrDefault(a => a.Id == addressId);

        if (addressFromDatabase == null) return NotFound();
        // addressFromDatabase.City = addressForUpdateDto.City;
        // addressFromDatabase.Street = addressForUpdateDto.Street;

        _mapper.Map(addressForUpdateDto, addressFromDatabase);
        */
        
        var addressToUpdate = _mapper.Map<Address>(addressForUpdateDto);
        addressToUpdate.CustomerId = customerId;
        await _customerRepository.UpdateAddressAsync(addressToUpdate);
        await _customerRepository.SaveAsync();

        return NoContent();
    }

    [HttpDelete("{addressId}")]
    public async Task<ActionResult> DeleteAddress(int customerId, int addressId)
    {
        /*
        var customerFromDatabase = _context.Customers.Include(c => c.Addresses)
            .FirstOrDefault(customer => customer.Id == customerId);

        if (customerFromDatabase == null) return NotFound();

        var addressFromDatabase = customerFromDatabase.Addresses
            .FirstOrDefault(address => address.Id == addressId);

        if (addressFromDatabase == null) return NotFound();

        customerFromDatabase.Addresses.Remove(addressFromDatabase);

        _context.SaveChanges();
        */

        var addressExist = await _customerRepository.AddressExistAsync(customerId, addressId);

        if (!addressExist) return NotFound();

        var addressToDelete = await _customerRepository.GetAddressAsync(addressId);

        await _customerRepository.DeleteAddressAsync(addressToDelete);

        await _customerRepository.SaveAsync();

        return NoContent();
    }
}
