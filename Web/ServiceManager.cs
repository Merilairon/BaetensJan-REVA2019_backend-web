using System.Text;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Web
{
    public class ServiceManager
    {
        private readonly IServiceCollection _services;
        private readonly IConfiguration _configuration;

        public ServiceManager(IServiceCollection services, IConfiguration configuration)
        {
            _services = services;
            _configuration = configuration;
        }

        public void AddMvc()
        {
            _services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void AddDatabase()
        {
            _services.AddDbContext<ApplicationDbContext>(
                options =>
                    //options.UseSqlite(_configuration.GetConnectionString("DefaultConnectionSqLite"),
                    options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"),
                    //options.UseSqlServer(_configuration.GetConnectionString("DefaultConnectionPublish"),
                        b => b.MigrationsAssembly("Web")));
        }

        public void AddIdentity()
        {
            _services.AddIdentity<ApplicationUser, IdentityRole>(
                    option =>
                    {
                        option.Password.RequireDigit = false;
                        option.Password.RequiredLength = 6;
                        option.Password.RequireNonAlphanumeric = false;
                        option.Password.RequireUppercase = false;
                        option.Password.RequireLowercase = false;
                        option.User.AllowedUserNameCharacters = string.Empty;
                    })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            _services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = "http://xyz.com",
                    ValidIssuer = "http://xyz.com",
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AppSettings:Secret"]))
                };
            });
        }

        public void AddScopes()
        {
            _services.AddSingleton<IConfiguration>(_configuration);
            _services.AddScoped<ICategoryRepository, CategoryRepository>();
            _services.AddScoped<IGroupRepository, GroupRepository>();
            _services.AddScoped<IExhibitorRepository, ExhibitorRepository>();
            _services.AddScoped<IAssignmentRepository, AssignmentRepository>();
            _services.AddScoped<IGroupRepository, GroupRepository>();
            _services.AddScoped<ICategoryExhibitorRepository, CategoryExhibitorRepository>();
            _services.AddScoped<IImageWriter, ImageWriter>();
            _services.AddScoped<IQuestionRepository, QuestionRepository>();
            _services.AddScoped<ISchoolRepository, SchoolRepository>();
            _services.AddScoped<IAuthenticationManager, AuthenticationManager>();
        }
    }
}