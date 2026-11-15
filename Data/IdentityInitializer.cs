
using Microsoft.AspNetCore.Identity;

namespace EducaPlus.Data
{
    public static class IdentityInitializer
    {
        public static async Task InitializeAsync(
            IServiceProvider services,
            IConfiguration configuration)
        {
            using var scope = services.CreateScope();

            var roleManager = scope.ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            var userManager = scope.ServiceProvider
                .GetRequiredService<UserManager<IdentityUser>>();

            // Criar os perfis, caso não existam
            string[] perfis =
            {
                "Coordenacao",
                "Professor",
                "Psicopedagogia"
            };

            foreach (var perfil in perfis)
            {
                if (!await roleManager.RoleExistsAsync(perfil))
                {
                    var resultado = await roleManager
                        .CreateAsync(new IdentityRole(perfil));

                    if (!resultado.Succeeded)
                        throw new InvalidOperationException(
                            string.Join("; ",
                                resultado.Errors.Select(e => e.Description))
                        );
                }
            }

            // Credenciais temporárias de inicialização
            var email = configuration["BootstrapAdmin:Email"];
            var senha = configuration["BootstrapAdmin:Password"];

            if (string.IsNullOrWhiteSpace(email) &&
                string.IsNullOrWhiteSpace(senha))
                return;

            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(senha))
                throw new InvalidOperationException(
                    "Configure o e-mail e a senha do administrador."
                );

            var usuario = await userManager.FindByEmailAsync(email);

            if (usuario != null)
            {
                if (!await userManager.IsInRoleAsync(
                    usuario, "Coordenacao"))
                    throw new InvalidOperationException(
                        "O e-mail já pertence a um usuário sem perfil de coordenação."
                    );

                return;
            }

            usuario = new IdentityUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var criacao = await userManager
                .CreateAsync(usuario, senha);

            if (!criacao.Succeeded)
                throw new InvalidOperationException(
                    string.Join("; ",
                        criacao.Errors.Select(e => e.Description))
                );

            var associacao = await userManager
                .AddToRoleAsync(usuario, "Coordenacao");

            if (!associacao.Succeeded)
                throw new InvalidOperationException(
                    string.Join("; ",
                        associacao.Errors.Select(e => e.Description))
                );
        }
    }
}
