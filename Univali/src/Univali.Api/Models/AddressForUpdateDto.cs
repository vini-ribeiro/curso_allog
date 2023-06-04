namespace Univali.Api.Entities;

public class AddressForUpdateDto
{
    public int Id { get; set; }
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
}