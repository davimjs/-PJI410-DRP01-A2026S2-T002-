
namespace EducaPlus.ViewModels
{
    public class HomeDashboardViewModel
    {
        // Indicadores acadêmicos
        public int AlunosAtivos { get; set; }
        public int TurmasAtivas { get; set; }
        public int DisciplinasCadastradas { get; set; }
        public int AulasCadastradas { get; set; }
        public int AvaliacoesCadastradas { get; set; }

        // Ocorrências registradas e ainda em acompanhamento
        public int OcorrenciasEmAcompanhamento { get; set; }
        public int OcorrenciasAtencao { get; set; }
        public int OcorrenciasCriticas { get; set; }
    }
}
