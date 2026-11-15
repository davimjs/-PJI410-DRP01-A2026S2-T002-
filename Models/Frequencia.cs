using System;
using System.Collections.Generic;

namespace EducaPlus.Models;

public partial class Frequencia
{
    public int IdFrequencia { get; set; }

    public int IdAula { get; set; }

    public int IdAluno { get; set; }

    public bool Presente { get; set; }

    public virtual Aluno IdAlunoNavigation { get; set; } = null!;

    public virtual Aula IdAulaNavigation { get; set; } = null!;
}
