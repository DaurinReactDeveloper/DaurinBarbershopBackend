using GestorBarberia.Application.Contract;
using GestorBarberia.Application.Core;
using GestorBarberia.Application.Dtos.ComentarioDto;
using GestorBarberia.Application.Validations;
using GestorBarberia.Domain.Entities;
using GestorBarberia.Persistence.Interface;
using GestorBarberia.Persistence.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Application.Services
{
    public class ComentarioServices : IComentarioService
    {

        private readonly IComentarioRepository comentarioRepository;
        private readonly ICitaRepository citaRepository;
        private readonly ILogger<ComentarioServices> logger;

        public ComentarioServices(IComentarioRepository comentarioRepository, ILogger<ComentarioServices> logger, ICitaRepository citaRepository)
        {

            this.comentarioRepository = comentarioRepository;
            this.logger = logger;
            this.citaRepository = citaRepository;

        }

        public ServiceResult GetComentarios()
        {
            ServiceResult result = new ServiceResult();

            try
            {

                var Comentarios = this.comentarioRepository.GetComentarios();

                if (!ComentarioValidations.VerifyNullListComentario(Comentarios))
                {

                    result.Success = false;
                    result.Message = "No hay comentarios.";
                    return result;

                }

                result.Data = Comentarios;
                result.Message = "Comentarios Obtenidos Correctamente.";

            }

            catch (Exception ex)
            {

                result.Success = false;
                result.Message = "Ha ocurrido un error listando los comentarios.";
                this.logger.LogError($"Ha ocurrido un error listando los comentarios: {ex.Message}.");

            }

            return result;

        }
        
        public ServiceResult GetComentsByBarbero(int barberoId)
        {

            ServiceResult result = new ServiceResult();

            try
            {
                var ComentariosBarbero = this.comentarioRepository.GetComentsByBarberoId(barberoId);

                if (!ComentarioValidations.VerifyNullListComentario(ComentariosBarbero))
                {

                    result.Success = false;
                    result.Message = "El barbero no tiene comentarios.";
                    return result;

                }

                result.Data = ComentariosBarbero;
                result.Message = "Comentarios del Barbero Obtenidos Correctamente.";
            }
            catch (Exception ex)
            {

                result.Success = false;
                result.Message = "Ha ocurrido un error listando los comentarios del barbero.";
                this.logger.LogError($"Ha ocurrido un error listando los comentarios del barbero: {ex.Message}.");
            }

            return result;
        }

        public ServiceResult GetComentsByCliente(int clienteId)
        {
            ServiceResult result = new ServiceResult();

            try
            {
                var ComentariosCliente = this.comentarioRepository.GetComentsByClienteId(clienteId);

                if (!ComentarioValidations.VerifyNullListComentario(ComentariosCliente))
                {

                    result.Success = false;
                    result.Message = "El cliente no ha realizado comentarios.";
                    return result;

                }

                result.Data = ComentariosCliente;
                result.Message = "Comentarios del Cliente Obtenidos Correctamente.";
            }
            catch (Exception ex)
            {

                result.Success = false;
                result.Message = "Ha ocurrido un error listando los comentarios del cliente.";
                this.logger.LogError($"Ha ocurrido un error listando los comentarios del cliente: {ex.Message}.");
            }

            return result;
        }

        public ServiceResult Add(ComentarioaddDto modelDto)
        {

            ServiceResult result = new ServiceResult();

            try
            {

                if (!ComentarioValidations.ValidationAddComentario(modelDto))
                {
                    result.Success = false;
                    result.Message = "El comentario no cumple con las validaciones para agregarlo.";
                    return result;
                }

                var citaClienteid = this.citaRepository.GetCitaById(modelDto.IdCita);

                if (!ComentarioValidations.VerifyClienteAdd(modelDto, citaClienteid))
                {
                    result.Success = false;
                    result.Message = "El servicio que intenta comentar no corresponde a su historial.";
                    return result;
                }

                this.comentarioRepository.Add(new Comentarios()
                {
                    IdComentarios = modelDto.IdComentarios,
                    IdCita = modelDto.IdCita,
                    IdCorte = modelDto.IdCorte,
                    IdCliente = modelDto.IdCliente,
                    IdBarbero = modelDto.IdBarbero,
                    Calificacion = modelDto.Calificacion,
                    Comentario = modelDto.Comentario,
                    CreationDate = DateTime.Now,
                    CreationUser = modelDto.ChangeUser,
                });

                this.comentarioRepository.SaveChanged();
                result.Message = "Comentario Agregado Correctamente.";

            }

            catch (Exception ex)
            {

                result.Success = false;
                result.Message = "Ha ocurrido un error guardando el comentario.";
                this.logger.LogError($"Ha ocurrido un error guardando el comentario: {ex.Message}.");

            }

            return result;

        }

        public ServiceResult Update(ComentarioUpdateDto modelDto)
        {
            ServiceResult result = new ServiceResult();

            try
            {
                var comentarioUpdate = this.comentarioRepository.GetById(modelDto.IdComentarios);

                if (!ComentarioValidations.VerifyIdComentario(modelDto.IdComentarios))
                {

                    result.Success = false;
                    result.Message = "No se ha podido encontrar el id del comentario.";
                    return result;

                }


                if (!ComentarioValidations.ValidationUpdateComentario(modelDto))
                {
                    result.Success = false;
                    result.Message = "El comentario no cumple con las validaciones para actualizarlo.";
                    return result;
                }


                comentarioUpdate.Comentario = modelDto.Comentario;
                comentarioUpdate.Calificacion = modelDto.Calificacion;
                comentarioUpdate.ModifyDate = DateTime.Now;
                comentarioUpdate.UserMod = modelDto.ChangeUser;

                this.comentarioRepository.Update(comentarioUpdate);
                this.comentarioRepository.SaveChanged();
                result.Message = "Comentario Actualizado Correctamente.";

            }
            catch (Exception ex)
            {

                result.Success = false;
                result.Message = "Ha ocurrido un error actualizando el comentario.";
                this.logger.LogError($"Ha ocurrido un error actualizando el comentario: {ex.Message}.");

            }

            return result;
        }

        public ServiceResult Remove(ComentarioRemoveDto modelDto)
        {

            ServiceResult result = new ServiceResult();

            try
            {

                var RemoveComentario = this.comentarioRepository.GetById(modelDto.IdComentarios);

                var citaId = this.citaRepository.GetCitaById(modelDto.IdCita);

                if (!ComentarioValidations.ValidationRemoveComentario(RemoveComentario))
                {
                    result.Success = false;
                    result.Message = "Ha ocurrido un error obteniendo el comentario.";
                    return result;
                }

                if (!ComentarioValidations.VerifyClienteRemove(modelDto, citaId))
                {
                    result.Success = false;
                    result.Message = "El servicio que intenta eliminar no corresponde a su historial.";
                    return result;
                }

                RemoveComentario.UserDeleted = modelDto.ChangeUser;

                this.comentarioRepository.Remove(RemoveComentario);
                this.comentarioRepository.SaveChanged();
                result.Message = "Comentario Removido Correctamente.";

            }
            catch (Exception ex)
            {

                result.Success = false;
                result.Message = "Ha ocurrido un error eliminando el comentario.";
                this.logger.LogError($"Ha ocurrido un error eliminando el comentario: {ex.Message}.");

            }

            return result;

        }

    }
}
