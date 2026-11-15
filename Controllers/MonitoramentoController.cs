
using EducaPlus.Data;
using EducaPlus.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace EducaPlus.Controllers
{
    [Authorize(Roles = "Coordenacao,Psicopedagogia")]
    public class MonitoramentoController : Controller
    {
        private readonly EducaPlusContext _context;

        public MonitoramentoController(EducaPlusContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Alunos ativos e suas turmas
            var alunos = await (
                from a in _context.Alunos
                join t in _context.Turmas
                    on a.IdTurma equals t.IdTurma
                where a.Ativo
                select new
                {
                    a.IdAluno,
                    a.NomeCompleto,
                    t.NomeTurma
                }
            ).ToListAsync();

            // Presenças por aluno
            var frequencias = await _context.Frequencias
                .GroupBy(f => f.IdAluno)
                .Select(g => new
                {
                    IdAluno = g.Key,
                    Total = g.Count(),
                    Presencas = g.Sum(f => f.Presente ? 1 : 0)
                })
                .ToDictionaryAsync(x => x.IdAluno);

            // Média das avaliações por aluno
            var notas = await _context.Notas
                .GroupBy(n => n.IdAluno)
                .Select(g => new
                {
                    IdAluno = g.Key,
                    Media = g.Average(n => n.Nota1)
                })
                .ToDictionaryAsync(x => x.IdAluno);

            var resultado = new List<ResumoAlunoViewModel>();

            foreach (var aluno in alunos)
            {
                decimal? frequencia = null;
                decimal? media = null;

                if (frequencias.TryGetValue(
                    aluno.IdAluno, out var freq))
                {
                    if (freq.Total > 0)
                    {
                        frequencia = Math.Round(
                            100m * freq.Presencas / freq.Total, 2
                        );
                    }
                }

                if (notas.TryGetValue(
                    aluno.IdAluno, out var nota))
                {
                    media = Math.Round(nota.Media, 2);
                }

                string risco = ClassificarRisco(frequencia, media);

                resultado.Add(new ResumoAlunoViewModel
                {
                    IdAluno = aluno.IdAluno,
                    NomeCompleto = aluno.NomeCompleto,
                    NomeTurma = aluno.NomeTurma,
                    Frequencia = frequencia,
                    MediaNotas = media,
                    StatusRisco = risco
                });
            }

            return View(resultado.OrderBy(
                x => x.StatusRisco == "Crítico" ? 0 :
                     x.StatusRisco == "Atenção" ? 1 :
                     x.StatusRisco == "Estável" ? 2 : 3
            ).ToList());
        }

        private static string ClassificarRisco(
            decimal? frequencia, decimal? media)
        {
            if (!frequencia.HasValue || !media.HasValue)
                return "Sem dados";

            if (frequencia < 75 || media < 5)
                return "Crítico";

            if (frequencia < 80 || media < 6)
                return "Atenção";

            return "Estável";
        }
    }
}
