using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace IdentityServer
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            
            services.Configure<IdentityConfig>(this.Configuration.GetSection("IdentityConfig"));
            services.AddSingleton<IdentityServerParameters>();
            
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            
            services.AddDbContext<IdentityServerDbContext>(options => options.UseNpgsql(this.Configuration.GetConnectionString("IdentityDbContext")));
            
            services.AddIdentityServer()
                .AddConfigurationStore(options =>
                {
                    options.DefaultSchema = "identity";
                    options.ConfigureDbContext = b => b.UseNpgsql(this.Configuration.GetConnectionString("IdentityDbContext"),
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddOperationalStore(options =>
                {
                    options.DefaultSchema = "identity";
                    options.ConfigureDbContext = b => b.UseNpgsql(this.Configuration.GetConnectionString("IdentityDbContext"),
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                });

            services.AddSingleton<DatabaseInitializer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IdentityServerDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var initializer = serviceScope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
                initializer.Initialize(app, context);
            }
            
            app.UseIdentityServer();
        }
    }
}