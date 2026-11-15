
using EducaPlus.Data;
using EducaPlus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace EducaPlus.Controllers
{
    [Authorize(Roles = "Coordenacao,Psicopedagogia")]
    public class OcorrenciasController : Controller
    {
        private readonly EducaPlusContext _context;

        public OcorrenciasController(EducaPlusContext context)
        {
            _context = context;
        }

        // ==========================================
        // CARREGAR ALUNOS PARA O DROPDOWN
        // ==========================================
        private void CarregarAlunos(int? idSelecionado = null)
        {
            var alunos = _context.Alunos
                .Where(a => a.Ativo)
                .OrderBy(a => a.NomeCompleto)
                .ToList();

            var lista = new SelectList(
                alunos,
                "IdAluno",
                "NomeCompleto",
                idSelecionado
            );

            ViewBag.Alunos = lista;
            ViewData["IdAluno"] = lista;
        }

        // ==========================================
        // INDEX - LISTAGEM DAS OCORRÊNCIAS
        // ==========================================
        public async Task<IActionResult> Index()
        {
            var ocorrencias = await _context
                .Set<OcorrenciasRisco>()
                .Include(o => o.IdAlunoNavigation)
                .OrderByDescending(o => o.DataAlerta)
                .ToListAsync();

            return View(ocorrencias);
        }

        // ==========================================
        // DETAILS - DETALHES DE UMA OCORRÊNCIA
        // ==========================================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var ocorrencia = await _context
                .Set<OcorrenciasRisco>()
                .Include(o => o.IdAlunoNavigation)
                .FirstOrDefaultAsync(o => o.IdOcorrencia == id);

            if (ocorrencia == null)
                return NotFound();

            return View(ocorrencia);
        }

        // ==========================================
        // GET - CADASTRAR OCORRÊNCIA
        // ==========================================
        [HttpGet]
        public IActionResult Create(int? idAluno)
        {
            CarregarAlunos(idAluno);

            var ocorrencia = new OcorrenciasRisco
            {
                IdAluno = idAluno ?? 0,
                NivelRisco = "",
                MotivoRisco = "",
                StatusOcorrencia = "Aberta",
                DataAlerta = DateTime.Now
            };

            return View(ocorrencia);
        }

        // ==========================================
        // POST - SALVAR OCORRÊNCIA
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("IdAluno,NivelRisco,MotivoRisco,AcaoRealizada")]
            OcorrenciasRisco ocorrencia)
        {
            // Dados controlados pelo sistema
            ocorrencia.DataAlerta = DateTime.Now;

            bool possuiIntervencao =
                !string.IsNullOrWhiteSpace(
                    ocorrencia.AcaoRealizada
                );

            ocorrencia.StatusOcorrencia = possuiIntervencao
                ? "Em andamento"
                : "Aberta";

            ocorrencia.DataIntervencao = possuiIntervencao
                ? DateTime.Now
                : null;

            // O relacionamento será resolvido pelo EF
            ModelState.Remove("IdAlunoNavigation");
            ModelState.Remove("DataAlerta");
            ModelState.Remove("DataIntervencao");
            ModelState.Remove("StatusOcorrencia");

            // Validação do nível de risco
            if (ocorrencia.NivelRisco != "Atenção" &&
                ocorrencia.NivelRisco != "Crítico")
            {
                ModelState.AddModelError(
                    "NivelRisco",
                    "Selecione um nível de risco válido."
                );
            }

            // Validação do motivo
            if (string.IsNullOrWhiteSpace(
                ocorrencia.MotivoRisco))
            {
                ModelState.AddModelError(
                    "MotivoRisco",
                    "Informe o motivo da ocorrência."
                );
            }

            // Verifica se o aluno está ativo
            bool alunoExiste = await _context.Alunos
                .AnyAsync(a =>
                    a.IdAluno == ocorrencia.IdAluno &&
                    a.Ativo);

            if (!alunoExiste)
            {
                ModelState.AddModelError(
                    "IdAluno",
                    "Selecione um aluno ativo."
                );
            }

            if (ModelState.IsValid)
            {
                _context.Set<OcorrenciasRisco>()
                    .Add(ocorrencia);

                await _context.SaveChangesAsync();

                TempData["Sucesso"] =
                    "Ocorrência registrada com sucesso!";

                return RedirectToAction(nameof(Index));
            }

            CarregarAlunos(ocorrencia.IdAluno);

            return View(ocorrencia);
        }

        // ==========================================
        // GET - EDITAR OCORRÊNCIA
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var ocorrencia = await _context
                .Set<OcorrenciasRisco>()
                .FindAsync(id);

            if (ocorrencia == null)
                return NotFound();

            CarregarAlunos(ocorrencia.IdAluno);

            return View(ocorrencia);
        }

        // ==========================================
        // POST - ATUALIZAR OCORRÊNCIA
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("IdOcorrencia,IdAluno,NivelRisco,MotivoRisco,StatusOcorrencia,AcaoRealizada")]
            OcorrenciasRisco dados)
        {
            if (id != dados.IdOcorrencia)
                return NotFound();

            var ocorrencia = await _context
                .Set<OcorrenciasRisco>()
                .FindAsync(id);

            if (ocorrencia == null)
                return NotFound();

            ModelState.Remove("IdAlunoNavigation");
            ModelState.Remove("DataAlerta");
            ModelState.Remove("DataIntervencao");

            if (dados.NivelRisco != "Atenção" &&
                dados.NivelRisco != "Crítico")
            {
                ModelState.AddModelError(
                    "NivelRisco",
                    "Selecione um nível de risco válido."
                );
            }

            if (!new[] {
                "Aberta", "Em andamento", "Concluída"
            }.Contains(dados.StatusOcorrencia))
            {
                ModelState.AddModelError(
                    "StatusOcorrencia",
                    "Selecione um status válido."
                );
            }

            if (string.IsNullOrWhiteSpace(dados.MotivoRisco))
            {
                ModelState.AddModelError(
                    "MotivoRisco",
                    "Informe o motivo da ocorrência."
                );
            }

            bool alunoExiste = await _context.Alunos
                .AnyAsync(a =>
                    a.IdAluno == dados.IdAluno &&
                    a.Ativo);

            if (!alunoExiste)
            {
                ModelState.AddModelError(
                    "IdAluno",
                    "Selecione um aluno ativo."
                );
            }

            if (ModelState.IsValid)
            {
                bool novaIntervencao =
                    string.IsNullOrWhiteSpace(
                        ocorrencia.AcaoRealizada
                    ) &&
                    !string.IsNullOrWhiteSpace(
                        dados.AcaoRealizada
                    );

                ocorrencia.IdAluno = dados.IdAluno;
                ocorrencia.NivelRisco = dados.NivelRisco;
                ocorrencia.MotivoRisco = dados.MotivoRisco;
                ocorrencia.StatusOcorrencia =
                    dados.StatusOcorrencia;

                ocorrencia.AcaoRealizada =
                    dados.AcaoRealizada;

                if (novaIntervencao)
                {
                    ocorrencia.DataIntervencao =
                        DateTime.Now;
                }

                await _context.SaveChangesAsync();

                TempData["Sucesso"] =
                    "Ocorrência atualizada com sucesso!";

                return RedirectToAction(nameof(Index));
            }

            // Preserva os dados originais da ocorrência
            dados.DataAlerta = ocorrencia.DataAlerta;
            dados.DataIntervencao =
                ocorrencia.DataIntervencao;

            CarregarAlunos(dados.IdAluno);

            return View(dados);
        }

        // ==========================================
        // GET - CONFIRMAR EXCLUSÃO
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var ocorrencia = await _context
                .Set<OcorrenciasRisco>()
                .Include(o => o.IdAlunoNavigation)
                .FirstOrDefaultAsync(o =>
                    o.IdOcorrencia == id);

            if (ocorrencia == null)
                return NotFound();

            return View(ocorrencia);
        }

        // ==========================================
        // POST - EXCLUIR OCORRÊNCIA
        // ==========================================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ocorrencia = await _context
                .Set<OcorrenciasRisco>()
                .FindAsync(id);

            if (ocorrencia != null)
            {
                _context.Set<OcorrenciasRisco>()
                    .Remove(ocorrencia);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
