namespace Univali.Api.Entities;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public ICollection<Address> Addresses { get; set; } = new List<Address>(); //usamos o I por ser uma interface e ser mais facil trabalhar somente com a interface do que om a propria classe
}