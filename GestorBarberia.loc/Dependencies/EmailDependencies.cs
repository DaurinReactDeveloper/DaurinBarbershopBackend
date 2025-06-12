using GestorBarberia.Application.Core;
using GestorBarberia.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GestorBarberia.loc.Dependencies
{
    public static class EmailDependencies
    {
        public static void AddDependenciesEmail(this IServiceCollection servicesCollection)
        {
            servicesCollection.AddTransient<IEmailServices>(provider =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var emailSettings = configuration.GetSection("Email");

                return new EmailService(
                    host: emailSettings["Host"],
                    port: int.Parse(emailSettings["Port"]),
                    enableSsl: bool.Parse(emailSettings["EnableSsl"]),
                    userName: emailSettings["UserName"],
                    appPassword: emailSettings["AppPassword"],
                    fromAddress: emailSettings["FromAddress"]
                );
            });
        }
    }
}
