
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EducaPlus.Data
{
    public class EducaPlusIdentityContext
        : IdentityDbContext<IdentityUser>
    {
        public EducaPlusIdentityContext(
            DbContextOptions<EducaPlusIdentityContext> options)
            : base(options)
        {
        }
    }
}
