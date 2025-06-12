using GestorBarberia.Domain.Entities;
using GestorBarberia.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Infrastructure.Extension
{
    public static class BarberiaExtension
    {

        public static BarberiaModel ConverteBarberoToModel(this Barberias BarberiaEntity)
        {

            var barberiaModel = new BarberiaModel()
            {

             BarberiasId = BarberiaEntity.BarberiasId,
             NombreBarberia = BarberiaEntity.NombreBarberia,
             Admin = BarberiaEntity.Admin

            };

            return barberiaModel;
        }

    }
}
