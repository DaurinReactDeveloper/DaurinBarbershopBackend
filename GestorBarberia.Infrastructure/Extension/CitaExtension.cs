using GestorBarberia.Domain.Entities;
using GestorBarberia.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Infrastructure.Extension
{
    public static class CitaExtension
    {
        public static CitaModel ConvertCitaToModel(this Citas CitaEntity)
        {
            var citaModel = new CitaModel()
            {
                BarberoId = CitaEntity.BarberoId,
                BarberiaId = CitaEntity.BarberiaId,
                CitaId = CitaEntity.CitaId,
                ClienteId = CitaEntity.ClienteId,
                Estado = CitaEntity.Estado,
                EstiloId = CitaEntity.EstiloId,
                Fecha = CitaEntity.Fecha,
                Hora = CitaEntity.Hora,                
            };

            return citaModel;

        }
    }
}
