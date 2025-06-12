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
using System.Xml.Linq;

namespace GestorBarberia.Persistence.Repositories
{
    public class AdministradorRepository : BaseRepository<Administradores>, IAdministradorRepository
    {

        private readonly DbContextBarberia dbContextBarberia;
        private readonly ILogger<AdministradorRepository> logger;

        public AdministradorRepository(DbContextBarberia dbContextBarberia, ILogger<AdministradorRepository> logger) : base(dbContextBarberia)
        {

            this.dbContextBarberia = dbContextBarberia;
            this.logger = logger;

        }

        public List<AdministradorModel> GetAdministradores()
        {

            try
            {

                var AdministradoresModel = (from am in dbContextBarberia.Administradores
                                            where !am.Deleted
                                            select new AdministradorModel()
                                            {
                                                AdministradoresId = am.AdministradoresId,
                                                Email = am.Email,
                                                Nombre = am.Nombre,
                                                Telefono = am.Telefono,
                                                Tipo = am.Tipo
                                            }

                                        ).ToList();


                return AdministradoresModel;


            }
            catch (Exception ex)
            {
                this.logger.LogError($"Ha ocurrido un error obteniendo los administradores: {ex.ToString()}.");
                throw new AdministradorExceptions("Ha ocurrido un error obteniendo los administradores.");
            }

        }

        public AdministradorModel GetAdministrador(string name)
        {
            try
            {
                var model = (from ad in this.dbContextBarberia.Administradores
                             where ad.Nombre.Equals(name) && !ad.Deleted
                             select new AdministradorModel()
                             {
                                 AdministradoresId = ad.AdministradoresId,
                                 Nombre = ad.Nombre,
                                 Password = ad.Password,
                                 Tipo = ad.Tipo,

                             }).FirstOrDefault();

                return model;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Ha ocurrido un error obteniendo el administrador: {ex.ToString()}.");
                throw new AdministradorExceptions("Ha ocurrido un error obteniendo el administrador.");
            }
        }

        public AdministradorModel GetAdministradorById(int adminId)
        {
            try
            {
                var model = (from ad in this.dbContextBarberia.Administradores
                             where ad.AdministradoresId.Equals(adminId) && !ad.Deleted
                             select new AdministradorModel()
                             {
                                 AdministradoresId = ad.AdministradoresId,
                                 Nombre = ad.Nombre,
                                 Email = ad.Email

                             }).FirstOrDefault();

                return model;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Ha ocurrido un error obteniendo el administrador: {ex.ToString()}.");
                throw new AdministradorExceptions("Ha ocurrido un error obteniendo el administrador.");
            }
        }

        public bool VerifyNameAdmin(string name)
        {
            try
            {
                var model = (from ad in this.dbContextBarberia.Administradores
                             where ad.Nombre.Equals(name) && !ad.Deleted
                             select new AdministradorModel()
                             {
                                 Nombre = ad.Nombre,

                             }).FirstOrDefault();

                return model is null;

            }
            catch (Exception ex)
            {
                this.logger.LogError($"Ha ocurrido un error obteniendo el administrador: {ex.ToString()}.");
                throw new AdministradorExceptions("Ha ocurrido un error obteniendo el administrador.");
            }
        }

        public override void Add(Administradores entity)
        {
            base.Add(entity);
            base.SaveChanged();
        }

        public override void Update(Administradores entity)
        {
            try
            {

                Administradores administradorUpdate = base.GetById(entity.AdministradoresId);

                if (administradorUpdate is null)
                {
                    throw new AdministradorExceptions("Ha ocurrido un error obteniendo el Id del Administrador.");
                }

                administradorUpdate.Nombre = entity.Nombre;
                administradorUpdate.Telefono = entity.Telefono;
                administradorUpdate.Email = entity.Email;
                administradorUpdate.Password = entity.Password;

                base.Update(administradorUpdate);
                base.SaveChanged();

            }
            catch (Exception ex)
            {
                this.logger.LogError($"Ha ocurrido un error actualizando el administrador: {ex.ToString()}.");
            }
        }

        public override void Remove(Administradores entity)
        {

            try
            {

                Administradores administradorRemove = this.GetById(entity.AdministradoresId);

                if (administradorRemove is null)
                {

                    throw new AdministradorExceptions("Ha ocurrido un error obteniendo el Id del Administrador.");
                }


                administradorRemove.Deleted = true;
                administradorRemove.DeletedDate = DateTime.Now;
                administradorRemove.UserDeleted = entity.UserDeleted;


                base.Update(administradorRemove);
                base.SaveChanged();

            }

            catch (Exception ex)
            {

                this.logger.LogError($"Ha ocurrido un error elimiando el administrador: {ex.ToString()}.");

            }


        }

    }
}
