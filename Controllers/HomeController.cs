
using System.Diagnostics;
using EducaPlus.Data;
using EducaPlus.Models;
using EducaPlus.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducaPlus.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EducaPlusContext _context;

        public HomeController(
            ILogger<HomeController> logger,
            EducaPlusContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var modelo = new HomeDashboardViewModel();

            bool coordenacao = User.IsInRole("Coordenacao");
            bool professor = User.IsInRole("Professor");
            bool psicopedagogia = User.IsInRole("Psicopedagogia");

            // Coordenação e Psicopedagogia
            if (coordenacao || psicopedagogia)
            {
                modelo.AlunosAtivos = await _context.Alunos
                    .CountAsync(a => a.Ativo);

                var ocorrencias = _context
                    .Set<OcorrenciasRisco>()
                    .Where(o =>
                        o.StatusOcorrencia == "Aberta" ||
                        o.StatusOcorrencia == "Em andamento");

                modelo.OcorrenciasEmAcompanhamento =
                    await ocorrencias.CountAsync();

                modelo.OcorrenciasAtencao =
                    await ocorrencias.CountAsync(
                        o => o.NivelRisco == "Atenção");

                modelo.OcorrenciasCriticas =
                    await ocorrencias.CountAsync(
                        o => o.NivelRisco == "Crítico");
            }

            // Indicadores exclusivos da Coordenação
            if (coordenacao)
            {
                modelo.TurmasAtivas = await _context.Turmas
                    .CountAsync(t => t.Ativa);
            }

            // Indicadores acadêmicos
            if (coordenacao || professor)
            {
                modelo.DisciplinasCadastradas =
                    await _context.Disciplinas.CountAsync();

                modelo.AulasCadastradas =
                    await _context.Aulas.CountAsync();

                modelo.AvaliacoesCadastradas =
                    await _context.Avaliacoes.CountAsync();
            }

            return View(modelo);
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id
                    ?? HttpContext.TraceIdentifier
            });
        }
    }
}
