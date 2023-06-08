using System.ComponentModel.DataAnnotations;

namespace Univali.Api.Models;

public class CustomerForCreationDto
{
    [Required(ErrorMessage = "You should fill out a Name")] // personalizar as menssagens de erro retornadas ao usuario
    [MaxLength(100, ErrorMessage = "The name shouldn't have more than 100 characters")]
    public string Name {get; set;} = string.Empty;

    [Required(ErrorMessage = "You should fill out a Cpf")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = 
    "The Cpf should have 11 charcteres")]
    public string Cpf {get; set;} = string.Empty;
}