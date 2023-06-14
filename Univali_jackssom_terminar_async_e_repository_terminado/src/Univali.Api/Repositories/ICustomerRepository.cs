using Univali.Api.Entities;

namespace Univali.Api.Repositories;

// addressRepository vai herdar desta interface aqui
// codigo assincrono eh para codigos que rodam fora do servidor e nao no mesmo servidor (nao se usa assync para codigo no mesmo servidor, mesmo que seja demorado)

public interface ICustomerRepository
{
    Task<int> SaveAsync();
    Task<bool> CustomerExistAsync(int customerId);
    Task<IEnumerable<Customer>> GetCustomersAsync();
    Task<Customer?> GetCustomerByIdAsync(int customerId);
    Task<Customer?> GetCustomerByCpfAsync(string customerCpf);
    Task<Customer> CreateCustomerAsync(Customer customerToCreate);
    Task<int> UpdateCustomerAsync(Customer customerToUpdate);
    Task DeleteCustomerAsync(Customer customerToDelete);
    Task<IEnumerable<Customer>> GetCustomersWithAddressesAsync();
    Task<Customer?> GetCustomerWithAddressesByIdAsync(int customerId);
    Task<Customer> CreateCustomerWithAddressesAsync(Customer customerToCreate);
    Task<int> UpdateCustomerWithAddressesAsync(Customer customerToUpdate);

    Task<IEnumerable<Address>> GetAddressesAsync(int customerId);
    Task<Address?> GetAddressAsync(int addressId);
    Task<Address> CreateAddressAsync(Address addressToCreate);
    Task<int> DeleteAddressAsync(Address? addressToDelete);
    Task<int> UpdateAddressAsync(Address addressToUpdate);
    Task<bool> AddressExistAsync(int customerId, int addressId);

    // jeito do professor
    void AddCustomer(Customer customer);
    Task<bool> SaveChangesAsync();

    // meu jeito depois de ver o professor fazendo

    void UpdateCustomer(Customer customer);
    void DeleteCustomer(Customer customer);
}

