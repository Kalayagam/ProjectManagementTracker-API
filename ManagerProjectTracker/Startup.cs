using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerProjectTracker.DatabaseSettings;
using ManagerProjectTracker.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace ManagerProjectTracker
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllHeaders",
                    builder =>
                    {
                        builder.WithMethods("GET", "POST", "PATCH", "PUT", "DELETE", "OPTIONS", "HEAD").WithHeaders("Access-Control-Allow-Headers", "Origin", "X-Requested-With", "Content-Type", "Accept").AllowAnyOrigin();
                    });
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Project Management API", Version = "v1" });

            });
            services.Configure<TeamMembersDatabaseSettings>(
                Configuration.GetSection(nameof(TeamMembersDatabaseSettings)));

            services.AddSingleton<ITeamMembersDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<TeamMembersDatabaseSettings>>().Value);

            services.AddSingleton<TeamMembersService>();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Project Management API V1");
            });   
           
            app.UseRouting();
            app.UseCors("AllowAllHeaders");
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
               endpoints.MapControllers();
            });
        }
    }
}
