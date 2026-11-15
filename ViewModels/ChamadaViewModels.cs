
namespace EducaPlus.ViewModels
{
    public class ChamadaViewModel
    {
        public int IdAula { get; set; }

        public List<AlunoPresencaViewModel> Alunos
        { get; set; } = new();
    }

    public class AlunoPresencaViewModel
    {
        public int IdAluno { get; set; }

        public string? NomeCompleto { get; set; }

        public bool Presente { get; set; }
    }
}
