using System;
using System.Collections.Generic;

namespace EducaPlus.Models;

public partial class Aluno
{
    public int IdAluno { get; set; }

    public int IdTurma { get; set; }

    public string Matricula { get; set; } = null!;

    public string NomeCompleto { get; set; } = null!;

    public string? Email { get; set; }

    public DateOnly? DataMatricula { get; set; }

    public bool Ativo { get; set; }

    public virtual ICollection<Frequencia> Frequencia { get; set; } = new List<Frequencia>();

    public virtual Turma IdTurmaNavigation { get; set; } = null!;

    public virtual ICollection<Nota> Nota { get; set; } = new List<Nota>();

    public virtual ICollection<OcorrenciasRisco> OcorrenciasRiscos { get; set; } = new List<OcorrenciasRisco>();
}
