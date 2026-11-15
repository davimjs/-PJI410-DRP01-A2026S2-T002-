using System;
using System.Collections.Generic;

namespace EducaPlus.Models;

public partial class Nota
{
    public int IdNota { get; set; }

    public int IdAvaliacao { get; set; }

    public int IdAluno { get; set; }

    public decimal Nota1 { get; set; }

    public virtual Aluno IdAlunoNavigation { get; set; } = null!;

    public virtual Avaliacao IdAvaliacaoNavigation { get; set; } = null!;
}
