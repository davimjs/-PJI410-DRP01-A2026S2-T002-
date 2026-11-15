using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EducaPlus.Data;
using EducaPlus.Models;
using Microsoft.AspNetCore.Authorization;

namespace EducaPlus.Controllers
{
    [Authorize(Roles = "Coordenacao")]
    public class AlunosController : Controller
    {
        private readonly EducaPlusContext _context;

        public AlunosController(EducaPlusContext context)
        {
            _context = context;
        }

        // GET: Alunos
        public async Task<IActionResult> Index()
        {
            var educaPlusContext = _context.Alunos.Include(a => a.IdTurmaNavigation);
            return View(await educaPlusContext.ToListAsync());
        }

        // GET: Alunos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aluno = await _context.Alunos
                .Include(a => a.IdTurmaNavigation)
                .FirstOrDefaultAsync(m => m.IdAluno == id);
            if (aluno == null)
            {
                return NotFound();
            }

            return View(aluno);
        }

        // GET: Alunos/Create
        public IActionResult Create()
        {
            ViewBag.Turmas = new SelectList(
                _context.Turmas.OrderBy(t => t.NomeTurma).ToList(),
                "IdTurma",
                "NomeTurma"
            );

            return View();
        }

        // POST: Alunos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
    [Bind("IdAluno,IdTurma,Matricula,NomeCompleto,Email,DataMatricula,Ativo")] Aluno aluno)
        {
            // O relacionamento será preenchido pelo Entity Framework
            ModelState.Remove("IdTurmaNavigation");

            if (ModelState.IsValid)
            {
                _context.Alunos.Add(aluno);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // Exibe os erros de validação no console
            foreach (var campo in ModelState)
            {
                foreach (var erro in campo.Value.Errors)
                {
                    Console.WriteLine(
                        $"CAMPO: {campo.Key} | ERRO: {erro.ErrorMessage}"
                    );
                }
            }

            // Recarrega as turmas caso exista erro
            ViewBag.Turmas = new SelectList(
                _context.Turmas.OrderBy(t => t.NomeTurma).ToList(),
                "IdTurma",
                "NomeTurma",
                aluno.IdTurma
            );

            return View(aluno);
        }

        // GET: Alunos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aluno = await _context.Alunos.FindAsync(id);
            if (aluno == null)
            {
                return NotFound();
            }
            ViewData["IdTurma"] = new SelectList(_context.Turmas, "IdTurma", "IdTurma", aluno.IdTurma);
            return View(aluno);
        }

        // POST: Alunos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAluno,IdTurma,Matricula,NomeCompleto,Email,DataMatricula,Ativo")] Aluno aluno)
        {
            if (id != aluno.IdAluno)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aluno);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlunoExists(aluno.IdAluno))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdTurma"] = new SelectList(_context.Turmas, "IdTurma", "IdTurma", aluno.IdTurma);
            return View(aluno);
        }

        // GET: Alunos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aluno = await _context.Alunos
                .Include(a => a.IdTurmaNavigation)
                .FirstOrDefaultAsync(m => m.IdAluno == id);
            if (aluno == null)
            {
                return NotFound();
            }

            return View(aluno);
        }

        // POST: Alunos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aluno = await _context.Alunos.FindAsync(id);
            if (aluno != null)
            {
                _context.Alunos.Remove(aluno);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlunoExists(int id)
        {
            return _context.Alunos.Any(e => e.IdAluno == id);
        }
    }
}
