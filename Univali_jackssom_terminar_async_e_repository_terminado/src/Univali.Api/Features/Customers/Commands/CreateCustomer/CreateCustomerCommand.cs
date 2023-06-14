using System.ComponentModel.DataAnnotations;
using Univali.Api.ValidationAttributes;

namespace Univali.Api.Features.Customers.Commands.CreateCustomer;

public class CreateCustomerCommand
{
    [Required(ErrorMessage = "You should fill out a Name")]
    [MaxLength(100, ErrorMessage = "The name shouldn't have more than 100 characters")]
    public string Name {get; set;} = string.Empty;

    [Required(ErrorMessage = "You should fill out a Cpf")]
    //[StringLength(11, MinimumLength = 11, ErrorMessage = 
    //"The Cpf should have 11 characters")] // o CpfMustBeValid ja vai validar isso
    [CpfMustBeValid(ErrorMessage = "The provided {0} should be valid number")]
    public string Cpf {get; set;} = string.Empty; // posso colocar o virtual depois de public e fazer um override no filhos (As anotacoes ele ira pegar)
    // posso botar nos filhos {get => base.Cpf; set => base.Cpf = value;} base eh o proprio objeto do pai (tipo o super do java)
}

