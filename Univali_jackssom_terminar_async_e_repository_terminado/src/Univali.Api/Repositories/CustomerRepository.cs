using Microsoft.EntityFrameworkCore;
using Univali.Api.DbContexts;
using Univali.Api.Entities;

namespace Univali.Api.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly CustomerContext _context;

    // construtor
    public CustomerRepository(CustomerContext customerContext)
    {
        _context = customerContext;
    }

    public async Task<bool> AddressExistAsync(int customerId, int addressId)
    {
        var customerFromDatabase = await _context.Customers.Include(c => c.Addresses)
            .FirstOrDefaultAsync(customer => customer.Id == customerId);
        
        if (customerFromDatabase == null) return false;

        var addressFromCustomer = customerFromDatabase.Addresses.FirstOrDefault(address => address.Id == addressId);

        if (addressFromCustomer == null) return false;

        _context.ChangeTracker.Clear();

        return true;
    }
    
    public async Task<Address> CreateAddressAsync(Address addressToCreate)
    {
        await _context.Set<Address>().AddAsync(addressToCreate);
        await _context.SaveChangesAsync();
        return addressToCreate;
    }
    
    public async Task<int> DeleteAddressAsync(Address? addressToDelete)
    {
        if (addressToDelete == null) return 0;
        _context.Set<Address>().Remove(addressToDelete);
        return await _context.SaveChangesAsync();
    }
    
    public async Task<int> UpdateAddressAsync(Address addressToUpdate)
    {
        _context.Set<Address>().Update(addressToUpdate);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> UpdateCustomerWithAddressesAsync(Customer customerToUpdate)
    {
        _context.Set<Customer>().Update(customerToUpdate);
        return await _context.SaveChangesAsync();
    }

    public async Task<Customer> CreateCustomerWithAddressesAsync(Customer customerToCreate)
    {
        await _context.Set<Customer>().AddAsync(customerToCreate);
        await _context.SaveChangesAsync();
        return customerToCreate;
    }

    public async Task DeleteCustomerAsync(Customer customerToDelete)
    {
        _context.Set<Customer>().Remove(customerToDelete);
        await _context.SaveChangesAsync();
    }

    public async Task<int> UpdateCustomerAsync(Customer customerToUpdate)
    {
        _context.Set<Customer>().Update(customerToUpdate);
        return await _context.SaveChangesAsync();
    }

    public async Task<Customer> CreateCustomerAsync(Customer customerToCreate)
    {
        await _context.Set<Customer>().AddAsync(customerToCreate);
        await _context.SaveChangesAsync();
        return customerToCreate;
    }

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task<bool> CustomerExistAsync(int customerId)
    {
        var customerFromDatabase = await _context.Customers
            .FirstOrDefaultAsync(customer => customer.Id == customerId);

        _context.ChangeTracker.Clear();

        if (customerFromDatabase == null) return false;

        return true;
    }
    
    // Jeito do professor
    public void AddCustomer(Customer customer) // adicionar nao deve ser async pois nao espera nada do bd
    {
        _context.Customers.Add(customer);
    }

    public async Task<bool> SaveChangesAsync() // save eh async
    {
        return (await _context.SaveChangesAsync()) > 0 ? true : false;
    }

    // meu jeito depois de ver o prof fazendo
    public async Task<Customer?> GetCustomerByCpfAsync(string customerCpf)
    {
        return await _context.Customers.FirstOrDefaultAsync(c => c.Cpf == customerCpf);
    }

    public async Task<Customer?> GetCustomerByIdAsync(int customerId)
    {
        return await _context.Customers.FirstOrDefaultAsync(c => c.Id == customerId);
    }

    public async Task<IEnumerable<Customer>> GetCustomersAsync() // task significa que eh retorno de um metodo async
    {
        return await _context.Customers.OrderBy(c => c.Id).ToListAsync(); // ToListAsync eh do ef
    } // em um metodo async tem que ser todo async, desde a chamada ate o retorno

    public void UpdateCustomer(Customer customer)
    {
        _context.Customers.Update(customer);
    }

    public void DeleteCustomer(Customer customer)
    {
        _context.Customers.Remove(customer);
    }

    public async Task<IEnumerable<Customer>> GetCustomersWithAddressesAsync()
    {
        return await _context.Customers.Include(c => c.Addresses).ToListAsync();
    }

    public async Task<Customer?> GetCustomerWithAddressesByIdAsync(int customerId)
    {
        return await _context.Customers.Include(c => c.Addresses)
            .FirstOrDefaultAsync(c => c.Id == customerId);
    }

    public async Task<IEnumerable<Address>> GetAddressesAsync(int customerId)
    {
        return await _context.Addresses
            .Where(address => address.CustomerId == customerId).ToListAsync();
    }
    
    public async Task<Address?> GetAddressAsync(int addressId)
    {
        return await _context.Addresses.FirstOrDefaultAsync(address => address.Id == addressId); // revisar
    }

    public void AddAddress(Address address) // adicionar nao deve ser async pois nao espera nada do bd
    {
        _context.Addresses.Add(address);
    }
}