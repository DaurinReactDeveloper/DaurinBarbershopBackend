using GestorBarberia.Application.Contract;
using GestorBarberia.Application.Services;
using GestorBarberia.Persistence.Interface;
using GestorBarberia.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.loc.Dependencies
{
    public static class AdministradorDependencies
    {

        public static void AddDependenciesAdministrador(this IServiceCollection servicesCollection)
        {

            servicesCollection.AddScoped<IAdministradorRepository, AdministradorRepository>();
            servicesCollection.AddTransient<IAdministradorServices, AdministradorServices>();

        }


    }
}
