using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using Services;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Identity;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Services.Interfaces;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.SwaggerUI;
using Database;
using Bizico_Project.Mappings;
using Controllers;
using FluentValidation.AspNetCore;
using Dtos;

namespace Bizico_Project
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
            services.AddCors();

            services.Configure<TokenSettings>(Configuration.GetSection("TokenSettings"));

            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<FreelanceContext>(options =>
                options.UseSqlServer(connection));
            services.AddMvc().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RequestValidator>());
            
            services.AddOptions<TokenSettings>("TokenSettings");

            services.AddTransient<RequestValidator>();
            services.AddTransient<ProjectValidator>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRequestService, RequestService>();
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<IProfileService, ProfileService>();
            services.AddSingleton<CurrentUserInfo>();
            services.AddIdentity<User, Role>().AddEntityFrameworkStores<FreelanceContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
            });

            var config = new AutoMapper.MapperConfiguration(c =>
            {
                c.AddProfile(new ApplicationProfile());
            });

            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            var key = Encoding.UTF8.GetBytes(Configuration["TokenSettings:JWT_Secret"].ToString());

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.FromMinutes(20),
                    RequireSignedTokens = true,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                };
            });


            services.AddTransient<Seeder>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(security);
                c.DescribeAllEnumsAsStrings();
            });

            
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, Seeder seeder)
        {
            
            if (env.IsDevelopment())
            { 
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.DocumentTitle = "Title Documentation";
                c.DocExpansion(DocExpansion.None);

            });

            app.UseAuthentication();
            app.UseHttpsRedirection();

            seeder.Seed().Wait();
            

            app.UseMvc();


        }
    }
}
