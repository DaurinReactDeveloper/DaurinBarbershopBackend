using GestorBarberia.Domain.Entities;
using GestorBarberia.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Infrastructure.Extension
{
    public static class AdministradorExtension
    {

        public static AdministradorModel ConvertAdministradorToModel(this Administradores administradorEntity)
        {

          var administradorModel = new AdministradorModel()
            {

                AdministradoresId = administradorEntity.AdministradoresId,
                Email = administradorEntity.Email,
                Nombre = administradorEntity.Nombre,
                Password = administradorEntity.Password,
                Telefono = administradorEntity.Telefono,
                Tipo = administradorEntity.Tipo,

            };
            
            return administradorModel;
        }


    }
}
