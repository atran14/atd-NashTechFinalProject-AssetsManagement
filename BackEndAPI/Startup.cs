using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEndAPI.DBContext;
using BackEndAPI.Filters;
using BackEndAPI.Interfaces;
using BackEndAPI.Entities;
using BackEndAPI.Repositories;
using BackEndAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace BackEndAPI
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

            services.AddDbContext<AssetsManagementDBContext>(
              opts => opts.UseLazyLoadingProxies()
                          .UseSqlServer(Configuration.GetConnectionString("SqlConnection")));

            services.AddControllers()
              .AddNewtonsoftJson(
                opts => opts.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
              );
            services.AddControllers(opts =>
            {
                opts.Filters.Add(typeof(CustomExceptionFilter));
            });
            services.AddControllers();

            services.AddTransient<IAsyncUserRepository, UserRepository>();
            services.AddTransient<IAsyncAssignmentRepository, AssignmentRepository>();
            services.AddScoped<IUserService, UserService>();

            services.AddIdentity<User, Role>()
                             .AddEntityFrameworkStores<AssetsManagementDBContext>()
                             .AddDefaultTokenProviders();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "back_end", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "back_end v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
