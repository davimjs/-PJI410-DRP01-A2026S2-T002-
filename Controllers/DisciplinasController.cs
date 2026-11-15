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
    public class DisciplinasController : Controller
    {
        private readonly EducaPlusContext _context;

        public DisciplinasController(EducaPlusContext context)
        {
            _context = context;
        }

        // GET: Disciplinas
        public async Task<IActionResult> Index()
        {
            var educaPlusContext = _context.Disciplinas.Include(d => d.IdTurmaNavigation);
            return View(await educaPlusContext.ToListAsync());
        }

        // GET: Disciplinas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var disciplina = await _context.Disciplinas
                .Include(d => d.IdTurmaNavigation)
                .FirstOrDefaultAsync(m => m.IdDisciplina == id);
            if (disciplina == null)
            {
                return NotFound();
            }

            return View(disciplina);
        }

        // GET: Disciplinas/Create

        public IActionResult Create()
        {
            ViewBag.Turmas = new SelectList(
                _context.Turmas.OrderBy(t => t.NomeTurma).ToList(),
                "IdTurma",
                "NomeTurma"
            );

            return View();
        }

        // POST: Disciplinas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
    [Bind("IdDisciplina,IdTurma,NomeDisciplina")] Disciplina disciplina)
        {
            // Evita exigir o objeto Turma completo no formulário
            ModelState.Remove("IdTurmaNavigation");

            if (ModelState.IsValid)
            {
                _context.Disciplinas.Add(disciplina);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // Recarrega as turmas se houver erro de validação
            ViewBag.Turmas = new SelectList(
                _context.Turmas.OrderBy(t => t.NomeTurma).ToList(),
                "IdTurma",
                "NomeTurma",
                disciplina.IdTurma
            );

            return View(disciplina);
        }


        // GET: Disciplinas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var disciplina = await _context.Disciplinas.FindAsync(id);
            if (disciplina == null)
            {
                return NotFound();
            }
            ViewData["IdTurma"] = new SelectList(_context.Turmas, "IdTurma", "IdTurma", disciplina.IdTurma);
            return View(disciplina);
        }

        // POST: Disciplinas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDisciplina,IdTurma,NomeDisciplina")] Disciplina disciplina)
        {
            if (id != disciplina.IdDisciplina)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(disciplina);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DisciplinaExists(disciplina.IdDisciplina))
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
            ViewData["IdTurma"] = new SelectList(_context.Turmas, "IdTurma", "IdTurma", disciplina.IdTurma);
            return View(disciplina);
        }

        // GET: Disciplinas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var disciplina = await _context.Disciplinas
                .Include(d => d.IdTurmaNavigation)
                .FirstOrDefaultAsync(m => m.IdDisciplina == id);
            if (disciplina == null)
            {
                return NotFound();
            }

            return View(disciplina);
        }

        // POST: Disciplinas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var disciplina = await _context.Disciplinas.FindAsync(id);
            if (disciplina != null)
            {
                _context.Disciplinas.Remove(disciplina);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DisciplinaExists(int id)
        {
            return _context.Disciplinas.Any(e => e.IdDisciplina == id);
        }
    }
}
