
using EducaPlus.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducaPlus.Controllers;

[Authorize(Roles = "Coordenacao")]
public class UsuariosController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;

    public UsuariosController(
        UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    // Listagem de usuários
    public async Task<IActionResult> Index()
    {
        var usuarios = await _userManager.Users
            .OrderBy(u => u.Email)
            .ToListAsync();

        var lista = new List<UsuarioResumoViewModel>();

        foreach (var usuario in usuarios)
        {
            var perfis = await _userManager
                .GetRolesAsync(usuario);

            lista.Add(new UsuarioResumoViewModel
            {
                Email = usuario.Email ?? "",
                Perfil = string.Join(", ", perfis)
            });
        }

        return View(lista);
    }

    // Formulário de cadastro
    [HttpGet]
    public IActionResult Create()
    {
        return View(new UsuarioCadastroViewModel());
    }

    // Salvar novo usuário
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        UsuarioCadastroViewModel modelo)
    {
        // Somente os perfis permitidos
        if (modelo.Perfil != "Professor" &&
            modelo.Perfil != "Psicopedagogia")
        {
            ModelState.AddModelError(
                nameof(modelo.Perfil),
                "Selecione um perfil válido."
            );
        }

        if (!ModelState.IsValid)
            return View(modelo);

        var existente = await _userManager
            .FindByEmailAsync(modelo.Email);

        if (existente != null)
        {
            ModelState.AddModelError(
                nameof(modelo.Email),
                "Este e-mail já está cadastrado."
            );

            return View(modelo);
        }

        var usuario = new IdentityUser
        {
            UserName = modelo.Email,
            Email = modelo.Email,
            // Apenas para o protótipo com dados fictícios
            EmailConfirmed = true
        };

        var resultado = await _userManager
            .CreateAsync(usuario, modelo.Senha);

        if (!resultado.Succeeded)
        {
            foreach (var erro in resultado.Errors)
            {
                ModelState.AddModelError(
                    "", erro.Description);
            }

            return View(modelo);
        }

        var perfil = await _userManager
            .AddToRoleAsync(usuario, modelo.Perfil);

        if (!perfil.Succeeded)
        {
            // Evita manter uma conta sem perfil
            await _userManager.DeleteAsync(usuario);

            foreach (var erro in perfil.Errors)
            {
                ModelState.AddModelError(
                    "", erro.Description);
            }

            return View(modelo);
        }

        TempData["Sucesso"] =
            "Usuário cadastrado com sucesso!";

        return RedirectToAction(nameof(Index));
    }
}
