
using System.ComponentModel.DataAnnotations;

namespace EducaPlus.ViewModels;

public class UsuarioCadastroViewModel
{
    [Required(ErrorMessage = "Informe o e-mail.")]
    [EmailAddress]
    public string Email { get; set; } = "";

    [Required]
    [DataType(DataType.Password)]
    [MinLength(8)]
    public string Senha { get; set; } = "";

    [Required]
    [DataType(DataType.Password)]
    [Compare("Senha",
        ErrorMessage = "As senhas não coincidem.")]
    public string ConfirmarSenha { get; set; } = "";

    [Required]
    public string Perfil { get; set; } = "";
}

public class UsuarioResumoViewModel
{
    public string Email { get; set; } = "";
    public string Perfil { get; set; } = "";
}
