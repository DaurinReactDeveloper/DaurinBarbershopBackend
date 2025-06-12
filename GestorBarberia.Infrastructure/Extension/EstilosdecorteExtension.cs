using GestorBarberia.Domain.Entities;
using GestorBarberia.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Infrastructure.Extension
{
    public static class EstilosdecorteExtension
    {
        public static EstilosdecorteModel ConvertEstilosToModel(this Estilosdecortes EstilosEntity)
        {

            var estilosModel = new EstilosdecorteModel()
            {
                Nombre = EstilosEntity.Nombre,
                Descripcion = EstilosEntity.Descripcion,    
                EstiloId = EstilosEntity.EstiloId,
                Precio = EstilosEntity.Precio,
                Imgestilo = EstilosEntity.Imgestilo,
                BarberiaId = EstilosEntity.BarberiaId,  
            };

            return estilosModel;

        }
    }
}
