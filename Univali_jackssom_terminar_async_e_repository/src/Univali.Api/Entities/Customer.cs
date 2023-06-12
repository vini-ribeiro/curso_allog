using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Univali.Api.Entities;

public class Customer
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // gera as id automaticamente
    public int Id {get; set;}
    [Required]
    [MaxLength(50)]
    public string Name {get; set;} = string.Empty;
    [Required]
    [MaxLength(11)]
    public string Cpf {get; set;} = string.Empty;
    public IList<Address> Addresses {get; set;} = new List<Address>();
}