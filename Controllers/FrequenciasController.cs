
using EducaPlus.Data;
using EducaPlus.Models;
using EducaPlus.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace EducaPlus.Controllers
{
    [Authorize(Roles = "Coordenacao,Professor")]
    public class FrequenciasController : Controller
    {
        private readonly EducaPlusContext _context;

        public FrequenciasController(EducaPlusContext context)
        {
            _context = context;
        }

        // Exibe os alunos da aula selecionada
        [HttpGet]
        public async Task<IActionResult> Chamada(int idAula)
        {
            var aula = await _context.Aulas
                .FindAsync(idAula);

            if (aula == null)
                return NotFound();

            var disciplina = await _context.Disciplinas
                .FindAsync(aula.IdDisciplina);

            if (disciplina == null)
                return NotFound();

            var alunos = await _context.Alunos
                .Where(a => a.IdTurma == disciplina.IdTurma
                         && a.Ativo)
                .OrderBy(a => a.NomeCompleto)
                .ToListAsync();

            var frequencias = await _context.Frequencias
                .Where(f => f.IdAula == idAula)
                .ToDictionaryAsync(
                    f => f.IdAluno,
                    f => f.Presente
                );

            var modelo = new ChamadaViewModel
            {
                IdAula = idAula,
                Alunos = alunos.Select(a =>
                    new AlunoPresencaViewModel
                    {
                        IdAluno = a.IdAluno,
                        NomeCompleto = a.NomeCompleto,
                        Presente = frequencias.TryGetValue(
                            a.IdAluno, out var presente
                        ) && presente
                    }).ToList()
            };

            ViewBag.DescricaoAula =
                $"{disciplina.NomeDisciplina} - " +
                $"{aula.DataAula:dd/MM/yyyy}";

            return View(modelo);
        }

        // Salva a chamada completa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Chamada(
            ChamadaViewModel modelo)
        {
            var aula = await _context.Aulas
                .FindAsync(modelo.IdAula);

            if (aula == null)
                return NotFound();

            var disciplina = await _context.Disciplinas
                .FindAsync(aula.IdDisciplina);

            if (disciplina == null)
                return NotFound();

            // Valida se todos os alunos pertencem à turma
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

            var existentes = await _context.Frequencias
                .Where(f => f.IdAula == modelo.IdAula)
                .ToDictionaryAsync(f => f.IdAluno);

            foreach (var aluno in modelo.Alunos)
            {
                if (existentes.TryGetValue(
                    aluno.IdAluno, out var frequencia))
                {
                    frequencia.Presente = aluno.Presente;
                }
                else
                {
                    _context.Frequencias.Add(new Frequencia
                    {
                        IdAula = modelo.IdAula,
                        IdAluno = aluno.IdAluno,
                        Presente = aluno.Presente
                    });
                }
            }

            await _context.SaveChangesAsync();

            TempData["Sucesso"] =
                "Frequência registrada com sucesso!";

            return RedirectToAction(
                nameof(Chamada),
                new { idAula = modelo.IdAula }
            );
        }
    }
}
