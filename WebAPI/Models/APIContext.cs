using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models
{
    public class APIContext : DbContext
    {

        public APIContext(DbContextOptions<APIContext> options) : base (options) { }

        public DbSet<Cuenta> Cuenta { get; set; }

    }
}
