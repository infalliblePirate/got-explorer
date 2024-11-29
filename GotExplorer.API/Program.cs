using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using GotExplorer.BLL.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using GotExplorer.DAL;
using Microsoft.EntityFrameworkCore;
using GotExplorer.DAL.Entities;
using GotExplorer.API.Middleware;
using GotExplorer.BLL.Mapper;
using GotExplorer.BLL.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Cors.Infrastructure;
using System.Text.Json;
using GotExplorer.API.Configuration;
using GotExplorer.BLL.Options;
using FluentValidation;
using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Validators;
namespace GotExplorer.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var jwtSection = builder.Configuration.GetSection("Jwt");
            var jwtOptions = jwtSection.Get<JwtOptions>();
            builder.Services.Configure<JwtOptions>(jwtSection);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                };
            });

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddIdentity<User, UserRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
            }).AddRoles<UserRole>()
              .AddEntityFrameworkStores<AppDbContext>()
              .AddDefaultTokenProviders();

            // Add CORS
            var corsSettings = builder.Configuration.GetSection("Cors").Get<CorsSettings>();
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(options =>
                {
                    options.WithOrigins(corsSettings.Origins);
                    options.WithMethods(corsSettings.Methods);
                    options.WithHeaders(corsSettings.Headers);
                });
            });

            // Add services to the container.
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddAutoMapper(typeof(MapperProfile));

            builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("Smtp"));

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();     
            
            builder.Services.AddProblemDetails();

            builder.Services.AddControllers();

            // Add Validators
            builder.Services.AddScoped<IValidator<RegisterDTO>, RegisterDtoValidator>();
            builder.Services.AddScoped<IValidator<LoginDTO>, LoginDtoValidator>();
            builder.Services.AddScoped<IValidator<UpdateUserDTO>, UpdateUserDtoValidator>();
            builder.Services.AddScoped<IValidator<UpdateUserPasswordDTO>, UpdateUserPasswordDtoValidator>();
            builder.Services.AddScoped<IValidator<ResetPasswordDTO>, ResetPasswordDtoValidator>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();


            var executingAssembly = Assembly.GetExecutingAssembly();

            // Swagger docs generation, which includes xml comments
            builder.Services.AddSwaggerGen(options =>
            {

                options.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Version = "1.0.0",
                    Title = "GotExplorer API",
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                // Include docs from current API assembly (as described in MS Docs)
                var executingAssembly = Assembly.GetExecutingAssembly();
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{executingAssembly.GetName().Name}.xml"));

                // Additionally include the documentation of all other "relevant" projects
                var referencedProjectsXmlDocPaths = executingAssembly.GetReferencedAssemblies()
                    .Where(assembly => assembly.Name != null && assembly.Name.StartsWith("My.Example.Project", StringComparison.InvariantCultureIgnoreCase))
                    .Select(assembly => Path.Combine(AppContext.BaseDirectory, $"{assembly.Name}.xml"))
                    .Where(path => File.Exists(path));

                foreach (var xmlDocPath in referencedProjectsXmlDocPaths)
                {
                    options.IncludeXmlComments(xmlDocPath);
                }
            });

            builder.Services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
            });

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseExceptionHandler();
            app.UseHttpsRedirection();

            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MapFallbackToFile("/index.html");



            using (var scope = app.Services.CreateScope())
            {
                CreateRoles(scope.ServiceProvider).Wait();
            }
            app.Run();
        }

        public static async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var roles = serviceProvider.GetRequiredService<IConfiguration>().GetSection("Roles").Get<string[]>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<UserRole>>();
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new UserRole(role));
                }
            }
        }
    }
}
