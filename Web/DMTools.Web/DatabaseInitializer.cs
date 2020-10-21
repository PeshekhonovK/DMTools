using System;
using DMTools.Web.Areas.Identity.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DMTools.Web
{
    public class DatabaseInitializer
    {
        private ILogger Logger { get; }
        
        public DatabaseInitializer(ILogger<DatabaseInitializer> logger)
        {
            Logger = logger;
        }

        public void Initialize(IApplicationBuilder app, IdentityDataContext context)
        {
            try
            {
                this.Logger.LogInformation($"Started initialization of database");
                
                context.Database.EnsureCreated();

                Initialize(app);

                context.SaveChanges();
                
                this.Logger.LogInformation($"Finished initialization of database");
            }
            catch (Exception ex)
            {
                this.Logger.LogCritical(ex, "Unexpected error occured during database initialization");
            }
        }

        private static void Initialize(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            
            scope.ServiceProvider.GetRequiredService<IdentityDataContext>().Database.Migrate();
        }
    }
}