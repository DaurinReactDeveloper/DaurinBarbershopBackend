using GestorBarberia.Domain.Entities;
using GestorBarberia.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Infrastructure.Extension
{
    public static class ComentarioExtension
    {

        public static ComentarioModel ConvertComentarioToModel(this Comentarios comentariosEntity)
        {
            ComentarioModel comentarioModel = new ComentarioModel()

            {
                IdComentarios = comentariosEntity.IdComentarios,
                IdCita = comentariosEntity.IdCita,
                IdCliente = comentariosEntity.IdCliente,
                IdCorte = comentariosEntity.IdCorte,
                IdBarbero = comentariosEntity.IdBarbero,
                Calificacion = comentariosEntity.Calificacion,
                Comentario = comentariosEntity.Comentario,
                IdClienteNavigation = comentariosEntity.IdClienteNavigation,
                IdCorteNavigation = comentariosEntity.IdCorteNavigation,
                IdCitasNavigation = comentariosEntity.IdCitaNavigation,
            };

            return comentarioModel;

        }

    }
}
