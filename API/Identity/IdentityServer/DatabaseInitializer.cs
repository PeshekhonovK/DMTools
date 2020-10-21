using System;
using System.Linq;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IdentityServer
{
    public class DatabaseInitializer
    {
        private ILogger Logger { get; }
        
        public DatabaseInitializer(ILogger<DatabaseInitializer> logger)
        {
            Logger = logger;
        }

        public void Initialize(IApplicationBuilder app, IdentityServerDbContext context)
        {
            try
            {
                this.Logger.LogInformation($"Started initialization of database");
                
                context.Database.EnsureCreated();

                InitializeTokenServerConfigurationDatabase(app);

                context.SaveChanges();
                
                this.Logger.LogInformation($"Finished initialization of database");
            }
            catch (Exception ex)
            {
                this.Logger.LogCritical(ex, "Unexpected error occured during database initialization");
            }
        }

        private static void InitializeTokenServerConfigurationDatabase(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();

            var config = app.ApplicationServices.GetService<IdentityServerParameters>();
            
            scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>()
                .Database.Migrate();

            var context = scope.ServiceProvider
                .GetRequiredService<ConfigurationDbContext>();
            context.Database.Migrate();
            if (!context.Clients.Any())
            {
                foreach (var client in config.GetClients())
                {
                    context.Clients.Add(client.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in config.GetIdentityResources())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.ApiResources.Any())
            {
                foreach (var resource in config.GetApiResources())
                {
                    context.ApiResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
        }
    }
}