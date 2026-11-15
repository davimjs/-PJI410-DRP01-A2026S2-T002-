using System;
using System.Collections.Generic;
using EducaPlus.Models;
using Microsoft.EntityFrameworkCore;

namespace EducaPlus.Data;

public partial class EducaPlusContext : DbContext
{
    public EducaPlusContext(DbContextOptions<EducaPlusContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aluno> Alunos { get; set; }

    public virtual DbSet<Aula> Aulas { get; set; }

    public virtual DbSet<Avaliacao> Avaliacoes { get; set; }

    public virtual DbSet<Disciplina> Disciplinas { get; set; }

    public virtual DbSet<Frequencia> Frequencias { get; set; }

    public virtual DbSet<Nota> Notas { get; set; }

    public virtual DbSet<OcorrenciasRisco> OcorrenciasRiscos { get; set; }

    public virtual DbSet<Turma> Turmas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Aluno>(entity =>
        {
            entity.HasKey(e => e.IdAluno).HasName("PK__Alunos__8092FCB35807F344");

            entity.HasIndex(e => e.Matricula, "UQ__Alunos__0FB9FB4F37E1E257").IsUnique();

            entity.Property(e => e.Ativo).HasDefaultValue(true);
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Matricula)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.NomeCompleto).HasMaxLength(150);

            entity.HasOne(d => d.IdTurmaNavigation).WithMany(p => p.Alunos)
                .HasForeignKey(d => d.IdTurma)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Alunos_Turmas");
        });

        modelBuilder.Entity<Aula>(entity =>
        {
            entity.HasKey(e => e.IdAula).HasName("PK__Aulas__2C89573C957E80CB");

            entity.Property(e => e.Conteudo).HasMaxLength(255);

            entity.HasOne(d => d.IdDisciplinaNavigation).WithMany(p => p.Aulas)
                .HasForeignKey(d => d.IdDisciplina)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Aulas_Disciplinas");
        });

        modelBuilder.Entity<Avaliacao>(entity =>
        {
            entity.HasKey(e => e.IdAvaliacao).HasName("PK__Avaliaco__78C432D857C6E34C");

            entity.Property(e => e.Descricao).HasMaxLength(150);

            entity.HasOne(d => d.IdDisciplinaNavigation).WithMany(p => p.Avaliacos)
                .HasForeignKey(d => d.IdDisciplina)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Avaliacoes_Disciplinas");
        });

        modelBuilder.Entity<Disciplina>(entity =>
        {
            entity.HasKey(e => e.IdDisciplina).HasName("PK__Discipli__97D8A467093E3521");

            entity.Property(e => e.NomeDisciplina).HasMaxLength(120);

            entity.HasOne(d => d.IdTurmaNavigation).WithMany(p => p.Disciplinas)
                .HasForeignKey(d => d.IdTurma)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Disciplinas_Turmas");
        });

        modelBuilder.Entity<Frequencia>(entity =>
        {
            entity.HasKey(e => e.IdFrequencia).HasName("PK__Frequenc__71701F33D963DD44");

            entity.HasIndex(e => new { e.IdAula, e.IdAluno }, "UQ_Frequencia_Aluno_Aula").IsUnique();

            entity.HasOne(d => d.IdAlunoNavigation).WithMany(p => p.Frequencia)
                .HasForeignKey(d => d.IdAluno)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Frequencias_Alunos");

            entity.HasOne(d => d.IdAulaNavigation).WithMany(p => p.Frequencia)
                .HasForeignKey(d => d.IdAula)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Frequencias_Aulas");
        });

        modelBuilder.Entity<Nota>(entity =>
        {
            entity.HasKey(e => e.IdNota).HasName("PK__Notas__4B2ACFF2F714F58B");

            entity.HasIndex(e => new { e.IdAvaliacao, e.IdAluno }, "UQ_Nota_Aluno_Avaliacao").IsUnique();

            entity.Property(e => e.Nota1)
                .HasColumnType("decimal(4, 2)")
                .HasColumnName("Nota");

            entity.HasOne(d => d.IdAlunoNavigation).WithMany(p => p.Nota)
                .HasForeignKey(d => d.IdAluno)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notas_Alunos");

            entity.HasOne(d => d.IdAvaliacaoNavigation).WithMany(p => p.Nota)
                .HasForeignKey(d => d.IdAvaliacao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notas_Avaliacoes");
        });

        modelBuilder.Entity<OcorrenciasRisco>(entity =>
        {
            entity.HasKey(e => e.IdOcorrencia).HasName("PK__Ocorrenc__831D923921E5313E");

            entity.ToTable("OcorrenciasRisco");

            entity.Property(e => e.AcaoRealizada).HasMaxLength(500);
            entity.Property(e => e.DataAlerta).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.MotivoRisco).HasMaxLength(500);
            entity.Property(e => e.NivelRisco).HasMaxLength(20);
            entity.Property(e => e.StatusOcorrencia)
                .HasMaxLength(20)
                .HasDefaultValue("Aberta");

            entity.HasOne(d => d.IdAlunoNavigation).WithMany(p => p.OcorrenciasRiscos)
                .HasForeignKey(d => d.IdAluno)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ocorrencias_Alunos");
        });

        modelBuilder.Entity<Turma>(entity =>
        {
            entity.HasKey(e => e.IdTurma).HasName("PK__Turmas__C1ECFFC9D0046000");

            entity.Property(e => e.Ativa).HasDefaultValue(true);
            entity.Property(e => e.Curso).HasMaxLength(150);
            entity.Property(e => e.NomeTurma).HasMaxLength(100);
            entity.Property(e => e.Periodo).HasMaxLength(30);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
