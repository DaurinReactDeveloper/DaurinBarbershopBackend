using GestorBarberia.Domain.Entities;
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
    public class BarberoRepository : BaseRepository<Barberos>, IBarberoRepository
    {
        private readonly DbContextBarberia dbContextBarberia;
        private readonly ILogger<BarberoRepository> logger;
        private readonly IAdministradorRepository administradorRepository;
        private readonly IBarberiasRepository barberiasRepository;

        public BarberoRepository(DbContextBarberia dbContextBarberia, ILogger<BarberoRepository> logger, IAdministradorRepository administradorRepository, IBarberiasRepository barberiasRepository) : base(dbContextBarberia)
        {
            this.logger = logger;
            this.dbContextBarberia = dbContextBarberia;
            this.administradorRepository = administradorRepository;
            this.barberiasRepository = barberiasRepository;
        }

        public BarberoModel GetBarberoName(string name)
        {
            try
            {
                var model = (from ba in this.dbContextBarberia.Barberos
                             where ba.Nombre.Equals(name) && !ba.Deleted
                             select new BarberoModel()
                             {
                                 BarberoId = ba.BarberoId,
                                 Nombre = ba.Nombre,
                                 Password = ba.Password,
                                 BarberiaId = ba.BarberiaId
                             }).FirstOrDefault();

                return model;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Ha ocurrido un error obteniendo el barbero: {ex.ToString()}.");
                throw new BarberoExceptions("Ha ocurrido un error obteniendo el barbero.");
            }
        }

        public List<BarberoModel> GetBarberos()
        {

            List<BarberoModel> barberoModels = new List<BarberoModel>();

            try
            {

                barberoModels = (from bm in dbContextBarberia.Barberos
                                 where !bm.Deleted
                                 select new BarberoModel(){

                                     BarberoId = bm.BarberoId,
                                     BarberiaId = bm.BarberiaId,
                                     Email = bm.Email,
                                     Imgbarbero = bm.Imgbarbero,
                                     Nombre = bm.Nombre,
                                     Password = bm.Password,
                                     Telefono = bm.Telefono,

                                 }).ToList();
                                 
                               
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Ha ocurrido un error obteniendo los barberos : {ex.ToString()}.");
                throw new BarberoExceptions("Ha ocurrido un error obteniendo los barbero.");
            }

            return barberoModels;

        }

        public List<BarberoModel> GetBarberosByBarberiaId(int barberiaId)
        {

            List<BarberoModel> barberoModels = new List<BarberoModel>();


            try
            {

                barberoModels = (from bm in this.dbContextBarberia.Barberos
                                 where bm.BarberiaId == barberiaId && !bm.Deleted
                                 select new BarberoModel()
                                 {
                                     BarberoId = bm.BarberoId,
                                     Nombre = bm.Nombre,
                                     Email = bm.Email,
                                     Imgbarbero = bm.Imgbarbero,
                                     Telefono = bm.Telefono,
                                     BarberiaId = barberiaId,

                                 }).ToList();


            }
            catch (Exception ex)
            {
                this.logger.LogError($"Ha ocurrido un error obteniendo los barberos : {ex.ToString()}.");
                throw new BarberoExceptions("Ha ocurrido un error obteniendo los barbero.");
            }

            return barberoModels;

        }

        public bool VerifyNameBarbero(string nameBarbero)
        {
            try
            {
                var nameData = (from n in this.dbContextBarberia.Barberos
                                where n.Nombre.Equals(nameBarbero) && !n.Deleted
                                select n).FirstOrDefault();

                return nameData != null;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Ha ocurrido un error verificando el nombre del barbero: {ex.ToString()}.");
                throw new BarberoExceptions("Ha ocurrido un error verificando el nombre del barbero.");
            }
        }

        public override void Add(Barberos entity)
        {
            base.Add(entity);
            base.SaveChanged();
        }

        public override void Update(Barberos entity)
        {

            try
            {

                Barberos barberoUpdate = base.GetById(entity.BarberoId);

                if (barberoUpdate is null)
                {
                    throw new BarberoExceptions("Ha ocurrido un error obteniendo el Id del Barbero.");
                }

                barberoUpdate.Nombre = entity.Nombre;
                barberoUpdate.Telefono = entity.Telefono;
                barberoUpdate.Email = entity.Email;
                barberoUpdate.Password = entity.Password;
                barberoUpdate.Imgbarbero = entity.Imgbarbero;

                base.Update(barberoUpdate);
                base.SaveChanged();

            }
            catch (Exception ex)
            {
                this.logger.LogError($"Ha ocurrido un error actualizando el barbero: {ex.ToString()}.");
            }

        }

        public override void Remove(Barberos entity)
        {

            try
            {

                Barberos barberoRemove = this.GetById(entity.BarberoId);

                if (barberoRemove is null)
                {

                    throw new BarberoExceptions("Ha ocurrido un error obteniendo el Id del Barbero.");
                }

                barberoRemove.Deleted = true;
                barberoRemove.DeletedDate = DateTime.Now;
                barberoRemove.UserDeleted = entity.UserDeleted;

                base.Update(barberoRemove);
                base.SaveChanged();

            }

            catch (Exception ex)
            {

                this.logger.LogError($"Ha ocurrido un error elimiando el barbero: {ex.ToString()}.");

            }
        }

        public bool VerifyPermissionsBarbero(int barberoId, int adminId)
        {
            // 1. Conseguir el Administrador
            var admin = this.administradorRepository.GetById(adminId);
            if (admin == null)
            {
                throw new BarberoExceptions("Administrador no encontrado.");
            }

            // 2. Conseguir el barbero
            var barbero = this.GetById(barberoId);
            if (barbero == null)
            {
                throw new BarberoExceptions("Barbero no encontrado.");
            }

            // 3. Conseguir la barbería
            var barberia = this.barberiasRepository.GetBarberiaByAdminId(adminId);
            if (barberia == null)
            {
                throw new BarberoExceptions("Barbería no encontrada.");
            }

            return admin.AdministradoresId == barberia.Admin && barberia.BarberiasId == barbero.BarberiaId;
        }

        public bool RemoveBarberoByAdmin(int barberoId, int adminId)
        {
            try
            {

                if (VerifyPermissionsBarbero(barberoId, adminId))
                {
                    Barberos barberoRemove = this.GetById(barberoId);

                    if (barberoRemove is null)
                    {
                        throw new BarberoExceptions("Barbero no encontrado.");
                    }


                    barberoRemove.Deleted = true;
                    barberoRemove.DeletedDate = DateTime.Now;
                    barberoRemove.UserDeleted = adminId;

                    base.Update(barberoRemove);
                    base.SaveChanged();

                    return true;
                }
                else
                {
                    throw new BarberoExceptions("No tienes permiso para eliminar este barbero.");
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Ha ocurrido un error eliminando el barbero: {ex.ToString()}.");
                return false;
            }
        }
    }
}
