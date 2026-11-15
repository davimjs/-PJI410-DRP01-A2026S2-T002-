
namespace EducaPlus.ViewModels
{
    public class LancamentoNotasViewModel
    {
        public int IdAvaliacao { get; set; }

        public string? DescricaoAvaliacao { get; set; }

        public List<NotaAlunoViewModel> Alunos { get; set; }
            = new();
    }

    public class NotaAlunoViewModel
    {
        public int IdAluno { get; set; }

        public string? NomeCompleto { get; set; }

        public decimal? ValorNota { get; set; }
    }
}
