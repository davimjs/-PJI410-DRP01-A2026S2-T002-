using System;
using System.Collections.Generic;

namespace EducaPlus.Models;

public partial class Disciplina
{
    public int IdDisciplina { get; set; }

    public int IdTurma { get; set; }

    public string NomeDisciplina { get; set; } = null!;

    public virtual ICollection<Aula> Aulas { get; set; } = new List<Aula>();

    public virtual ICollection<Avaliacao> Avaliacos { get; set; } = new List<Avaliacao>();

    public virtual Turma IdTurmaNavigation { get; set; } = null!;
}
