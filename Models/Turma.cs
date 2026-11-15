using System;
using System.Collections.Generic;

namespace EducaPlus.Models;

public partial class Turma
{
    public int IdTurma { get; set; }

    public string NomeTurma { get; set; } = null!;

    public string Curso { get; set; } = null!;

    public string? Periodo { get; set; }

    public bool Ativa { get; set; }

    public virtual ICollection<Aluno> Alunos { get; set; } = new List<Aluno>();

    public virtual ICollection<Disciplina> Disciplinas { get; set; } = new List<Disciplina>();
}
