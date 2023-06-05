namespace Univali.Api.Models;

public class CustomerWithAddressesForUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public ICollection<AddressForUpdateWithCustomerDto> Addresses { get; set; } = new List<AddressForUpdateWithCustomerDto>();

    public class AddressForUpdateWithCustomerDto
    {
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
    }
}