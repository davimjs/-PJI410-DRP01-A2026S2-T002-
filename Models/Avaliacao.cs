using System;
using System.Collections.Generic;

namespace EducaPlus.Models;

public partial class Avaliacao
{
    public int IdAvaliacao { get; set; }

    public int IdDisciplina { get; set; }

    public string Descricao { get; set; } = null!;

    public DateOnly DataAvaliacao { get; set; }

    public virtual Disciplina IdDisciplinaNavigation { get; set; } = null!;

    public virtual ICollection<Nota> Nota { get; set; } = new List<Nota>();
}
