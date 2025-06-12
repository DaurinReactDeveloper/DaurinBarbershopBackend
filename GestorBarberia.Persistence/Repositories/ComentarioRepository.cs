using GestorBarberia.Domain.Entities;
using GestorBarberia.Domain.Repository;
using GestorBarberia.Infrastructure.Exceptions;
using GestorBarberia.Infrastructure.Extension;
using GestorBarberia.Infrastructure.Models;
using GestorBarberia.Persistence.Context;
using GestorBarberia.Persistence.Core;
using GestorBarberia.Persistence.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Persistence.Repositories
{
    public class ComentarioRepository : BaseRepository<Comentarios>, IComentarioRepository
    {

        private readonly DbContextBarberia contextBarberia;
        private readonly ILogger<Comentarios> logger;

        public ComentarioRepository(DbContextBarberia contextBarberia, ILogger<Comentarios> logger) : base(contextBarberia)
        {
            this.contextBarberia = contextBarberia;
            this.logger = logger;
        }

        public List<ComentarioModel> GetComentarios()
        {

            List<ComentarioModel> ListComentarios = new List<ComentarioModel>();

            try
            {

                ListComentarios = (from c in this.contextBarberia.Comentarios
                                   where !c.Deleted
                                   select new ComentarioModel() 
                                   {
                                       IdComentarios = c.IdComentarios,
                                       IdCita = c.IdCita,
                                       IdCliente = c.IdCliente,
                                       IdCorte = c.IdCorte,
                                       IdBarbero = c.IdBarbero,
                                       Calificacion = c.Calificacion,
                                       Comentario = c.Comentario
                                   }).ToList();

            }
            catch (Exception ex)
            {

                this.logger.LogError($"Ha ocurrido un error listando los comentarios: {ex.ToString()}.");

            }

            return ListComentarios;
        }

        public List<ComentarioModel> GetComentsByBarberoId(int id)
        {

            List<ComentarioModel> comentarioModelBarberoId = new List<ComentarioModel>();

            try
            {
                comentarioModelBarberoId = (from c in this.contextBarberia.Comentarios
                                            join cl in contextBarberia.Clientes on c.IdCliente equals cl.ClienteId into clc
                                            from cl in clc.DefaultIfEmpty()
                                            join co in contextBarberia.Estilosdecortes on c.IdCorte equals co.EstiloId into coc
                                            from co in coc.DefaultIfEmpty()
                                            where c.IdBarbero.Equals(id) && !c.Deleted
                                            select new ComentarioModel()
                                            {
                                                IdComentarios = c.IdComentarios,
                                                IdCita = c.IdCita, 
                                                IdCliente = c.IdCliente,
                                                IdCorte = c.IdCorte,
                                                Calificacion = c.Calificacion,
                                                Comentario = c.Comentario,
                                                IdClienteNavigation = cl,
                                                IdCorteNavigation = co
                                                
                                            }).ToList();

            }
            catch (Exception ex)
            {

                this.logger.LogError($"Ha Ocurrido un error listando los comentarios del barbero, {ex.ToString()}.");
            }

            return comentarioModelBarberoId;
        }

        public List<ComentarioModel> GetComentsByClienteId(int id)
        {
            List<ComentarioModel> comentarioModelClienteId = new List<ComentarioModel>();

            try
            {
                comentarioModelClienteId = (from c in this.contextBarberia.Comentarios
                                            where c.IdCliente.Equals(id) && !c.Deleted
                                            select new ComentarioModel()
                                            {
                                              IdComentarios = c.IdComentarios
                                            }).ToList();
            }
            catch (Exception ex)
            {

                this.logger.LogError($"Ha Ocurrido un error listando los comentarios del cliente, {ex.ToString()}.");
            }

            return comentarioModelClienteId;
        }

        public override void Add(Comentarios entity)
        {
            base.Add(entity);
            base.SaveChanged();
        }

        public override void Update(Comentarios entity)
        {

            var ComentarioUpdate = this.GetById(entity.IdComentarios);

            try
            {

                if (ComentarioUpdate is null)
                {
                    throw new ComentarioExceptions("No se ha podido obtener el id del comentario.");
                }

                ComentarioUpdate.Calificacion = entity.Calificacion;
                ComentarioUpdate.Comentario = entity.Comentario;

                base.Update(entity);
                base.SaveChanged();

            }
            catch (Exception ex)
            {

                this.logger.LogError($"Ha ocurrido un error actualizando el comentario: {ex.ToString()}.");

            }
        }

        public override void Remove(Comentarios entity)
        {
            var ComentarioRemove = (from c in this.contextBarberia.Comentarios
                                    where c.IdComentarios.Equals(entity.IdComentarios)
                                    select c).FirstOrDefault();
            try
            {

                if (ComentarioRemove is null)
                {
                    throw new ComentarioExceptions("No se ha podido obtener el id del comentario.");
                }


                ComentarioRemove.Deleted = true;
                ComentarioRemove.DeletedDate = DateTime.Now;
                ComentarioRemove.UserDeleted = entity.UserDeleted;

                base.Update(ComentarioRemove);
                base.SaveChanged();

            }
            catch (Exception ex)
            {

                this.logger.LogError($"Ha ocurrido un error actualizando el comentario: {ex.ToString()}.");

            }
        }
    }
}