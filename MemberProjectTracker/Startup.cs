using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerProjectTracker.DatabaseSettings;
using ManagerProjectTracker.Services;
using MemberProjectTracker.DatabaseSettings;
using MemberProjectTracker.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace MemberProjectTracker
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Project Management API", Version = "v1" });

            });
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllHeaders",
                    builder =>
                    {
                        builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                    });
            });
            services.Configure<TeamMembersDatabaseSettings>(
                Configuration.GetSection(nameof(TeamMembersDatabaseSettings)));

            services.AddSingleton<ITeamMembersDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<TeamMembersDatabaseSettings>>().Value);

            services.Configure<TasksDatabaseSettings>(
                Configuration.GetSection(nameof(TasksDatabaseSettings)));

            services.AddSingleton<ITasksDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<TasksDatabaseSettings>>().Value);

            services.AddSingleton<TeamMembersService>();
            services.AddSingleton<TaskService>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
