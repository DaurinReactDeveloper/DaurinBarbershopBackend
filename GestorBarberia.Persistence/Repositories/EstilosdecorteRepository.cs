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

namespace GestorBarberia.Persistence.Repositories
{
    public class EstilosdecorteRepository : BaseRepository<Estilosdecortes>, IEstilosdecorteRepository 
    {

        private readonly DbContextBarberia dbContextBarberia;
        private readonly ILogger<EstilosdecorteRepository> logger;
        private readonly IAdministradorRepository administradorRepository;
        private readonly IBarberiasRepository barberiasRepository;

        public EstilosdecorteRepository(DbContextBarberia dbContextBarberia, ILogger<EstilosdecorteRepository> logger, IAdministradorRepository administradorRepository, IBarberiasRepository barberiasRepository) :base(dbContextBarberia) 
        { 
            this.dbContextBarberia = dbContextBarberia;
            this.logger = logger;   
            this.administradorRepository = administradorRepository; 
            this.barberiasRepository = barberiasRepository;
        }

        public List<EstilosdecorteModel> GetEstilosdecorte()
        {
            List<EstilosdecorteModel> EstilosModels = new List<EstilosdecorteModel>();

            try
            {

                EstilosModels = (from em in dbContextBarberia.Estilosdecortes
                                 where !em.Deleted
                                 select new EstilosdecorteModel()
                                 {

                                     EstiloId = em.EstiloId,
                                     BarberiaId = em.BarberiaId,
                                     Imgestilo = em.Imgestilo,
                                     Nombre = em.Nombre,
                                     Descripcion = em.Descripcion,  
                                     Precio = em.Precio
                                     
                                 }).ToList();
                
            }
            catch (Exception ex)
            {

                this.logger.LogError($"Ha ocurrido un error listando los estilos de corte: {ex.ToString()}.");
            }

            return EstilosModels;

        }

        public List<EstilosdecorteModel> GetEstilosdecorteByBarberiaId(int barberiaId)
        {
            List<EstilosdecorteModel> estiloModels = new List<EstilosdecorteModel>();


            try
            {

                estiloModels = (from bm in this.dbContextBarberia.Estilosdecortes
                                where bm.BarberiaId == barberiaId && !bm.Deleted
                                select new EstilosdecorteModel()
                                {
                                    BarberiaId = bm.BarberiaId,
                                    Descripcion = bm.Descripcion,
                                    EstiloId = bm.EstiloId,
                                    Imgestilo = bm.Imgestilo,
                                    Nombre = bm.Nombre,
                                    Precio = bm.Precio

                                }).ToList();


            }
            catch (Exception ex)
            {
                this.logger.LogError($"Ha ocurrido un error obteniendo los Estilos : {ex.ToString()}.");
                throw new EstilosdecorteExceptions("Ha ocurrido un error obteniendo los Estilos.");
            }

            return estiloModels;
        }

        public override void Add(Estilosdecortes entity)
        {
            base.Add(entity);
            base.SaveChanged();
        }

        public override void Update(Estilosdecortes entity)
        {
            try
            {
                Estilosdecortes EstiloCorteId = this.GetById(entity.EstiloId);

                if (EstiloCorteId is null)
                {
                    throw new EstilosdecorteExceptions("Ha ocurrido un error obteniendo el Id del estilo de corte.");
                }

                EstiloCorteId.Nombre = entity.Nombre;
                EstiloCorteId.Descripcion = entity.Descripcion;
                EstiloCorteId.Precio = entity.Precio;
                EstiloCorteId.Imgestilo = entity.Imgestilo;
                EstiloCorteId.EstiloId = entity.EstiloId;
                EstiloCorteId.Cita = entity.Cita;

                base.Update(EstiloCorteId);
                base.SaveChanged();

            }
            catch (Exception ex)
            {
                this.logger.LogError($"Ha ocurrido un error actualizando el estilo de corte: {ex.ToString()}.");
            }

        }

        public override void Remove(Estilosdecortes entity)
        {

            try
            {
                Estilosdecortes EstiloCorteId = this.GetById(entity.EstiloId);

                if(EstiloCorteId is null)
                {
                    throw new EstilosdecorteExceptions("Ha ocurrido un error obteniendo el Id del estilo de corte.");
                }

                EstiloCorteId.Deleted = true;
                EstiloCorteId.UserDeleted = entity.UserDeleted;
                EstiloCorteId.DeletedDate = DateTime.Now;

                base.Update(EstiloCorteId);
                base.SaveChanged();

            }
            catch (Exception ex)
            {
                this.logger.LogError($"Ha ocurrido un error elimando el estilo de corte: {ex.ToString()}.");
            }

        }

        public bool VerifyPermissionsEstilos(int estiloId, int adminId)
        {
            // 1. Conseguir el Administrador
            var admin = this.administradorRepository.GetById(adminId);
            if (admin == null)
            {
                throw new EstilosdecorteExceptions("Administrador no encontrado.");
            }

            // 2. Conseguir el estilo
            var estilo = this.GetById(estiloId);
            if (estilo == null)
            {
                throw new EstilosdecorteExceptions("estilo no encontrado.");
            }

            // 3. Conseguir la barbería 
            var barberia = this.barberiasRepository.GetBarberiaByAdminId(adminId);
            if (barberia == null)
            {
                throw new EstilosdecorteExceptions("Barbería no encontrada.");
            }

            return admin.AdministradoresId == barberia.Admin && barberia.BarberiasId == estilo.BarberiaId;
        }

        public bool RemoveEstilosByAdmin(int estiloId, int adminId)
        {
            try
            {

                if (VerifyPermissionsEstilos(estiloId, adminId))
                {
                    Estilosdecortes estiloRemove = this.GetById(estiloId);

                    if (estiloRemove is null)
                    {
                        throw new EstilosdecorteExceptions("estilo de corte no encontrado.");
                    }

                    estiloRemove.Deleted = true;
                    estiloRemove.UserDeleted = adminId;
                    estiloRemove.DeletedDate = DateTime.Now;

                    base.Update(estiloRemove);
                    base.SaveChanged();
                    return true;
                }
                else
                {
                    throw new EstilosdecorteExceptions("No tienes permiso para eliminar este estilo de corte.");
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Ha ocurrido un error eliminando el estilo de corte: {ex.ToString()}.");
                return false;
            }
        }

    }
}

