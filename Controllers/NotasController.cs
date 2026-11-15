
using EducaPlus.Data;
using EducaPlus.Models;
using EducaPlus.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace EducaPlus.Controllers
{
    [Authorize(Roles = "Coordenacao,Professor")]
    public class NotasController : Controller
    {
        private readonly EducaPlusContext _context;

        public NotasController(EducaPlusContext context)
        {
            _context = context;
        }

        // GET: Notas/Lancamento?idAvaliacao=1
        [HttpGet]
        public async Task<IActionResult> Lancamento(int idAvaliacao)
        {
            var avaliacao = await _context.Set<Avaliacao>()
                .FindAsync(idAvaliacao);

            if (avaliacao == null)
                return NotFound();

            var disciplina = await _context.Set<Disciplina>()
                .FindAsync(avaliacao.IdDisciplina);

            if (disciplina == null)
                return NotFound();

            var alunos = await _context.Alunos
                .Where(a => a.IdTurma == disciplina.IdTurma
                         && a.Ativo)
                .OrderBy(a => a.NomeCompleto)
                .ToListAsync();

            var notas = await _context.Set<Nota>()
                .Where(n => n.IdAvaliacao == idAvaliacao)
                .ToDictionaryAsync(
                    n => n.IdAluno,
                    n => n.Nota1
                );

            var modelo = new LancamentoNotasViewModel
            {
                IdAvaliacao = idAvaliacao,

                DescricaoAvaliacao =
                    $"{disciplina.NomeDisciplina} - " +
                    $"{avaliacao.Descricao}",

                Alunos = alunos.Select(a =>
                    new NotaAlunoViewModel
                    {
                        IdAluno = a.IdAluno,
                        NomeCompleto = a.NomeCompleto,
                        ValorNota = notas.TryGetValue(
                            a.IdAluno, out var nota
                        ) ? nota : null
                    }).ToList()
            };

            return View(modelo);
        }

        // POST: Salvar notas
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Lancamento(
            LancamentoNotasViewModel modelo)
        {
            var avaliacao = await _context.Set<Avaliacao>()
                .FindAsync(modelo.IdAvaliacao);

            if (avaliacao == null)
                return NotFound();

            var disciplina = await _context.Set<Disciplina>()
                .FindAsync(avaliacao.IdDisciplina);

            if (disciplina == null)
                return NotFound();

            // Valida se as notas pertencem aos alunos da turma
            var idsValidos = await _context.Alunos
                .Where(a => a.IdTurma == disciplina.IdTurma
                         && a.Ativo)
                .Select(a => a.IdAluno)
                .ToListAsync();

            var idsRecebidos = modelo.Alunos
                .Select(a => a.IdAluno)
                .ToList();

            if (idsRecebidos.Count != idsValidos.Count ||
                idsRecebidos.Distinct().Count() != idsRecebidos.Count ||
                idsRecebidos.Except(idsValidos).Any() ||
                idsValidos.Except(idsRecebidos).Any())
            {
                return BadRequest("Lista de alunos inválida.");
            }

            // Valida o intervalo das notas
            if (modelo.Alunos.Any(a =>
                a.ValorNota.HasValue &&
                (a.ValorNota.Value < 0 ||
                 a.ValorNota.Value > 10)))
            {
                return BadRequest(
                    "As notas devem estar entre 0 e 10."
                );
            }

            var existentes = await _context.Set<Nota>()
                .Where(n => n.IdAvaliacao == modelo.IdAvaliacao)
                .ToDictionaryAsync(n => n.IdAluno);

            foreach (var aluno in modelo.Alunos)
            {
                // Nota não preenchida: não altera o registro
                if (!aluno.ValorNota.HasValue)
                    continue;

                if (existentes.TryGetValue(
                    aluno.IdAluno, out var notaExistente))
                {
                    notaExistente.Nota1 = aluno.ValorNota.Value;
                }
                else
                {
                    _context.Set<Nota>().Add(new Nota
                    {
                        IdAvaliacao = modelo.IdAvaliacao,
                        IdAluno = aluno.IdAluno,
                        Nota1 = aluno.ValorNota.Value
                    });
                }
            }

            await _context.SaveChangesAsync();

            TempData["Sucesso"] =
                "Notas registradas com sucesso!";

            return RedirectToAction(
                nameof(Lancamento),
                new { idAvaliacao = modelo.IdAvaliacao }
            );
        }
    }
}
