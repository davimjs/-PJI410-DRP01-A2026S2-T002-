using System;
using System.Collections.Generic;

namespace EducaPlus.Models;

public partial class Aula
{
    public int IdAula { get; set; }

    public int IdDisciplina { get; set; }

    public DateOnly DataAula { get; set; }

    public string? Conteudo { get; set; }

    public virtual ICollection<Frequencia> Frequencia { get; set; } = new List<Frequencia>();

    public virtual Disciplina IdDisciplinaNavigation { get; set; } = null!;
}
