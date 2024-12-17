using ApiPeliculas.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Data
{
    public class ApiDBContext:IdentityDbContext
    {
        public ApiDBContext(DbContextOptions<ApiDBContext> options):base(options) { }
       
       public DbSet<Categoria> Categorias { get; set; }
    }
}
