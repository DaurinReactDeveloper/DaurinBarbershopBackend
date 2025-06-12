using GestorBarberia.Application.Dtos.ComentarioDto;
using GestorBarberia.Domain.Entities;
using GestorBarberia.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Application.Validations
{
    public static class ComentarioValidations
    {

        public static bool ValidationAddComentario(ComentarioaddDto comentarioAddDto)
        {

            if (comentarioAddDto.IdComentarios < 0 || comentarioAddDto.IdCliente <= 0
                || comentarioAddDto.IdCorte <= 0 || comentarioAddDto.IdBarbero <= 0 || comentarioAddDto.Calificacion <= 0
                || string.IsNullOrEmpty(comentarioAddDto.Comentario)
                || string.IsNullOrWhiteSpace(comentarioAddDto.Comentario)
                || comentarioAddDto.Comentario.Length >= 44 || comentarioAddDto.Comentario.Length <= 10) 
                
            {
                return false;
            }

        
            return true;

        }

        public static bool VerifyClienteAdd(ComentarioaddDto comentarioAddDto, CitaModel citaId)
        {

            if (comentarioAddDto.IdCliente != citaId.ClienteId)
            {
                return false;
            }

            return true;
        }
       
        public static bool VerifyClienteRemove(ComentarioRemoveDto comentarioRemove, CitaModel citaId)
        {

            if (comentarioRemove.IdCliente != citaId.ClienteId)
            {
                return false;
            }

            return true;
        }

        public static bool ValidationUpdateComentario(ComentarioUpdateDto comentarioUpdateDto)
        {

            if (comentarioUpdateDto.Calificacion <= 0
                || string.IsNullOrEmpty(comentarioUpdateDto.Comentario)
                || string.IsNullOrWhiteSpace(comentarioUpdateDto.Comentario)
                || comentarioUpdateDto.Comentario.Length >= 44)
            {
                return false;
            }

            return true;

        }

        public static bool ValidationRemoveComentario(Comentarios comentarioRemoveDto)
        {


            if (comentarioRemoveDto.IdComentarios <= 0)
            {
                return false;
            }


            return true;

        }

        public static bool VerifyIdComentario(int id)
        {

            if (id <= 0)
            {

                return false;

            }

            return true;

        }

        public static bool VerifyNullListComentario(List<ComentarioModel> comentarioModels)
        {
            if (comentarioModels == null || comentarioModels.Count <= 0)
            {

                return false;

            }

            return true;
        }
    }
}
