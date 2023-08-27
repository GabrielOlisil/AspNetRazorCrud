using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace TesteConhecimentos.Models;

public class Fornecedor
{
    public int Id { get; set; }

    [MaxLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
    [Required(ErrorMessage = "O campo Nome é obrigatório")]
    public string? Nome { get; set; }

    [RegularExpression("^[0-9]{14}$", ErrorMessage = "Cnpj deve conter 14 algarismos numéricos")]
    [MaxLength(14)]
    [Required(ErrorMessage = "O campo CNPJ é obrigatório")]
    [DisplayName("CNPJ")]
    public string? Cnpj { get; set; }

    [Required(ErrorMessage = "O campo Especialidade é obrigatório")]
    public string Especialidade { get; set; }

    [RegularExpression("^[0-9]{8}$", ErrorMessage = "CEP deve conter 8 algarismos numéricos")]
    [MaxLength(8)]
    [Required(ErrorMessage = "O campo CEP é obrigatório")]
    [DisplayName("CEP")]
    public string? Cep { get; set; }

    [MaxLength(255)] public string? Endereco { get; set; }
}