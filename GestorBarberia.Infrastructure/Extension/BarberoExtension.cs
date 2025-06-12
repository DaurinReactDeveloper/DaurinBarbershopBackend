using GestorBarberia.Domain.Entities;
using GestorBarberia.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Infrastructure.Extension
{
    public static class BarberoExtension
    {

        public static BarberoModel ConverteBarberoToModel(this Barberos BarberoEntity)
        {

            var barberoModel = new BarberoModel()
            {
                BarberoId = BarberoEntity.BarberoId,
                Nombre = BarberoEntity.Nombre,
                Telefono = BarberoEntity.Telefono,
                Email = BarberoEntity.Email,
                Password = BarberoEntity.Password,
                Imgbarbero = BarberoEntity.Imgbarbero
            };

            return barberoModel;
        }


    }
}
