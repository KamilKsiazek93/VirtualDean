using DbUp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualDean.Data;
using Microsoft.EntityFrameworkCore;
using VirtualDean.Models.DatabaseContext;

namespace VirtualDean
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            EnsureDatabase.For.SqlDatabase(connectionString);

            var upgrader = DeployChanges.To
                .SqlDatabase(connectionString, null)
                .WithScriptsEmbeddedInAssembly(
                System.Reflection.Assembly.GetExecutingAssembly()
                )
                .WithTransaction()
                .Build();

            if(upgrader.IsUpgradeRequired())
            {
                upgrader.PerformUpgrade();
            }

            services.AddDbContext<BrotherDbContext>(options => options.UseSqlServer(connectionString));
            services.AddDbContext<ObstaclesDbContext>(options => options.UseSqlServer(connectionString));
            services.AddDbContext<ObstacleConstDbContext>(options => options.UseSqlServer(connectionString));
            services.AddDbContext<OfficeNameDbContext>(options => options.UseSqlServer(connectionString));
            services.AddDbContext<OfficeDbContext>(options => options.UseSqlServer(connectionString));
            services.AddDbContext<ObstacleBetweenOfficesDbContext>(options => options.UseSqlServer(connectionString));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "VirtualDean", Version = "v1" });
            });

            services.AddScoped<IBrothers, Brothers>();
            services.AddScoped<IOfficesManager, OfficesManager>();
            services.AddScoped<ITrayCommunionHour, TrayCommunionHour>();
            services.AddScoped<IObstacle, Obstacles>();
            services.AddScoped<IWeek, Week>();
            services.AddScoped<IAuth, Auth>();

            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
                  builder
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .WithOrigins(Configuration["Frontend"])));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "VirtualDean v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
