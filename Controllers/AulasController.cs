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
    [Authorize(Roles = "Coordenacao,Professor")]
    public class AulasController : Controller
    {
        private readonly EducaPlusContext _context;

        public AulasController(EducaPlusContext context)
        {
            _context = context;
        }

        // GET: Aulas
        public async Task<IActionResult> Index()
        {
            var educaPlusContext = _context.Aulas.Include(a => a.IdDisciplinaNavigation);
            return View(await educaPlusContext.ToListAsync());
        }

        // GET: Aulas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aula = await _context.Aulas
                .Include(a => a.IdDisciplinaNavigation)
                .FirstOrDefaultAsync(m => m.IdAula == id);
            if (aula == null)
            {
                return NotFound();
            }

            return View(aula);
        }

        // GET: Aulas/Create
        
        public IActionResult Create()
        {
            CarregarDisciplinas();
            return View();
        }

        // POST: Aulas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("IdAula,IdDisciplina,DataAula,Conteudo")] Aula aula)
        {
            // O EF preencherá a propriedade de navegação
            ModelState.Remove("IdDisciplinaNavigation");

            if (ModelState.IsValid)
            {
                _context.Aulas.Add(aula);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            CarregarDisciplinas(aula.IdDisciplina);

            return View(aula);
        }


        // GET: Aulas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aula = await _context.Aulas.FindAsync(id);
            if (aula == null)
            {
                return NotFound();
            }
            ViewData["IdDisciplina"] = new SelectList(_context.Disciplinas, "IdDisciplina", "IdDisciplina", aula.IdDisciplina);
            return View(aula);
        }

        // POST: Aulas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAula,IdDisciplina,DataAula,Conteudo")] Aula aula)
        {
            if (id != aula.IdAula)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aula);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AulaExists(aula.IdAula))
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
            ViewData["IdDisciplina"] = new SelectList(_context.Disciplinas, "IdDisciplina", "IdDisciplina", aula.IdDisciplina);
            return View(aula);
        }

        // GET: Aulas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aula = await _context.Aulas
                .Include(a => a.IdDisciplinaNavigation)
                .FirstOrDefaultAsync(m => m.IdAula == id);
            if (aula == null)
            {
                return NotFound();
            }

            return View(aula);
        }

        // POST: Aulas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aula = await _context.Aulas.FindAsync(id);
            if (aula != null)
            {
                _context.Aulas.Remove(aula);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AulaExists(int id)
        {
            return _context.Aulas.Any(e => e.IdAula == id);
        }

        private void CarregarDisciplinas(int? idSelecionado = null)
        {
            var disciplinas = (
                from d in _context.Disciplinas
                join t in _context.Turmas
                    on d.IdTurma equals t.IdTurma
                orderby t.NomeTurma, d.NomeDisciplina
                select new
                {
                    d.IdDisciplina,
                    Descricao = d.NomeDisciplina + " - " + t.NomeTurma
                }
            ).ToList();

            ViewBag.Disciplinas = new SelectList(
                disciplinas,
                "IdDisciplina",
                "Descricao",
                idSelecionado
            );
        }

    }
}
