using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using VirtualDean.Data;
using Microsoft.EntityFrameworkCore;
using VirtualDean.Models.DatabaseContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using Microsoft.AspNetCore.Authorization;
using VirtualDean.Authorization;

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
            services.AddHttpClient("Brothers", httpClient =>
            {
                httpClient.BaseAddress = new Uri(Configuration["ServiceUri:Brothers"]);
            });

            services.AddHttpClient("Offices", httpClient =>
            {
                httpClient.BaseAddress = new Uri(Configuration["ServiceUri:Offices"]);
            });

            services.AddHttpClient("Weeks", httpClient =>
            {
                httpClient.BaseAddress = new Uri(Configuration["ServiceUri:Weeks"]);
            });

            services.AddHttpClient("TrayCommunions", httpClient =>
            {
                httpClient.BaseAddress = new Uri(Configuration["ServiceUri:TrayCommunions"]);
            });

            services.AddDbContext<VirtualDeanDbContext>(options => options.UseSqlServer(connectionString));

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

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Dean", policy =>
                {
                    policy.Requirements.Add(new ShouldBeDean());
                });
            });
            services.AddScoped<IAuthorizationHandler, ShouldBeDeanHandler>();

            services.AddHttpContextAccessor();

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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
