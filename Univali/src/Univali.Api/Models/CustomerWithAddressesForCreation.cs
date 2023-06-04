namespace Univali.Api.Models;

public class CustomerWithAddressesForCreationDto
{
    public string Name { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public ICollection<AddressForCreationDto> Addresses { get; set; } = new List<AddressForCreationDto>();
}