using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using GotExplorer.BLL.Services.Interfaces;
using GotExplorer.BLL.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using GotExplorer.DAL;
using Microsoft.EntityFrameworkCore;
using GotExplorer.DAL.Models;
namespace GotExplorer.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
            var jwtAudience = builder.Configuration.GetSection("Jwt:Audience").Get<string>();
            var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                };
            });

            // TODO: Connect database
            builder.Services.AddDbContext<AppDbContext>();

            builder.Services.AddIdentity<User, UserRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
            }).AddEntityFrameworkStores<AppDbContext>();


            // Add CORS
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            // Add services to the container.
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddControllers();
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

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseHttpsRedirection();

            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
