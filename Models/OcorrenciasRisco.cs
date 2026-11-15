using System;
using System.Collections.Generic;

namespace EducaPlus.Models;

public partial class OcorrenciasRisco
{
    public int IdOcorrencia { get; set; }

    public int IdAluno { get; set; }

    public string NivelRisco { get; set; } = null!;

    public string MotivoRisco { get; set; } = null!;

    public DateTime DataAlerta { get; set; }

    public string StatusOcorrencia { get; set; } = null!;

    public string? AcaoRealizada { get; set; }

    public DateTime? DataIntervencao { get; set; }

    public virtual Aluno IdAlunoNavigation { get; set; } = null!;
}
