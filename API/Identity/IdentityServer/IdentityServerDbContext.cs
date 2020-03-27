using Microsoft.EntityFrameworkCore;

namespace IdentityServer
{
    public class IdentityServerDbContext : DbContext
    {
        public IdentityServerDbContext(DbContextOptions<IdentityServerDbContext> options) 
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {    
            builder.HasDefaultSchema("identity");
            base.OnModelCreating(builder);
        }
    }
}