
namespace EducaPlus.ViewModels
{
    public class ResumoAlunoViewModel
    {
        public int IdAluno { get; set; }
        public string NomeCompleto { get; set; } = "";
        public string NomeTurma { get; set; } = "";

        public decimal? Frequencia { get; set; }
        public decimal? MediaNotas { get; set; }

        public string StatusRisco { get; set; } = "Sem dados";
    }
}
