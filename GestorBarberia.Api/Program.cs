using GestorBarberia.Application.Services;
using GestorBarberia.loc.Dependencies;
using GestorBarberia.Persistence.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GestorBarberia.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Dependencies
            builder.Services.AddDependenciesBarbero();
            builder.Services.AddDependenciesBarberias();
            builder.Services.AddDependenciesCita();
            builder.Services.AddDependenciesCliente();
            builder.Services.AddDependenciesEstilosdecorte();
            builder.Services.AddDependenciesAdministrador();
            builder.Services.AddDependenciesEmail();
            builder.Services.AddDependenciesComentario();
            builder.Services.AddDependenciesReportes();

            //DataBase
            builder.Services.AddDbContext<DbContextBarberia>(options => options.UseMySql(builder.Configuration.GetConnectionString("DbContextBarberia"), new MySqlServerVersion(new Version(8, 0, 23))));

            //Configurar CORS
            var frontendUrl = builder.Configuration.GetValue<string>("FrontendUrl");

            if (string.IsNullOrEmpty(frontendUrl))
            {

                throw new ArgumentException("FrontendUrl" + "El valor de FrontendUrl no puede ser nulo o vacío.");

            }

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                   {
                       policy.WithOrigins(frontendUrl)
                             .AllowAnyMethod()
                             .AllowAnyHeader()
                             .AllowCredentials();
                   });
            });

            //Configuración del Json Web Token -- Mas adelante
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))

                }; 
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                // Habilita Swagger para producción si es necesario
                app.UseSwagger();
                app.UseSwaggerUI(c => {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                    c.RoutePrefix = string.Empty;
                });
            }

            app.UseHttpsRedirection();

            app.UseCors();

            //Para que funcione el Json Web Token
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
