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
    public class CitaRepository : BaseRepository<Citas>, ICitaRepository
    {

        private readonly DbContextBarberia dbContextBarberia;
        private readonly ILogger<CitaRepository> logger;
        private readonly IBarberiasRepository barberiasRepository;

        public CitaRepository(DbContextBarberia dbContextBarberia, ILogger<CitaRepository> logger, IBarberiasRepository barberiasRepository) : base(dbContextBarberia)
        {
            this.dbContextBarberia = dbContextBarberia; 
            this.logger = logger;
            this.barberiasRepository = barberiasRepository; 
            
        }

        public CitaModel GetCitaById(int citaId)
        {

            try
            {

               var citaModel = (from c in this.dbContextBarberia.Citas
                                 where c.CitaId.Equals(citaId) && !c.Deleted
                                 select new CitaModel()
                                 {
                                     CitaId = c.CitaId,
                                     BarberoId = c.BarberoId,
                                     EstiloId = c.EstiloId,
                                     ClienteId = c.ClienteId,

                                 }).FirstOrDefault();

                return citaModel;

            }
            catch (Exception ex)
            {
                this.logger.LogError($"Ha ocurrido un error obteniendo la citas: {ex.ToString()}.");
                throw new CitaExceptions("Ha ocurrido un error obteniendo la Cita");

            }

        }

        public List<CitaModel> GetCitaByBarberoId(int barberoId)
        {

            List<CitaModel> listCita = new List<CitaModel>();

            try
            {
                 listCita = (from c in this.dbContextBarberia.Citas
                                join b in dbContextBarberia.Barberos on c.BarberoId equals b.BarberoId into bj
                                from b in bj.DefaultIfEmpty()
                                join e in dbContextBarberia.Estilosdecortes on c.EstiloId equals e.EstiloId into es
                                from e in es.DefaultIfEmpty()
                                join cl in dbContextBarberia.Clientes on c.ClienteId equals cl.ClienteId into clj
                                from cl in clj.DefaultIfEmpty()
                                where c.BarberoId.Equals(barberoId) && !c.Deleted
                                select new CitaModel()
                                {
                                    CitaId = c.CitaId,
                                    Fecha = c.Fecha,
                                    Hora = c.Hora,
                                    ClienteId = c.ClienteId,
                                    BarberoId = c.BarberoId,
                                    Estado = c.Estado,
                                    Barbero = b,
                                    Estilo = e,
                                    Cliente = cl,
                                }).OrderByDescending(c => c.Fecha).ToList();


            }
            catch (Exception ex)
            {
                this.logger.LogError($"Ha ocurrido un error listando las citas: {ex.ToString()}.");
                throw new CitaExceptions("Ha ocurrido un error listando las citas");
            }

            return listCita;

        }

        public List<CitaModel> GetCitaByClienteId(int clienteId)
        {

            List<CitaModel> listCita = new List<CitaModel>();   

            try
            {
                 listCita = (from c in this.dbContextBarberia.Citas
                                join cl in dbContextBarberia.Clientes on c.ClienteId equals cl.ClienteId into cid
                                from cl in cid.DefaultIfEmpty()
                                join e in dbContextBarberia.Estilosdecortes on c.EstiloId equals e.EstiloId into es
                                from e in es.DefaultIfEmpty()
                                where c.ClienteId.Equals(clienteId) && !c.Deleted
                                select new CitaModel()
                                {
                                    CitaId = c.CitaId,
                                    Fecha = c.Fecha,
                                    Hora = c.Hora,
                                    BarberoId = c.BarberoId,
                                    Estado = c.Estado,
                                    Cliente = cl,
                                    Estilo = e
                                }).OrderByDescending(c => c.Fecha).ToList();

            }
            catch (Exception ex)
            {
                this.logger.LogError($"Ha ocurrido un error listando las citas: {ex.ToString()}.");
                throw new CitaExceptions("Ha ocurrido un error listando las citas.");
            }

            return listCita;

        }

        public bool VerifyCita(DateTime fecha, TimeSpan hora)
        {
            try
            {
                var horaInicio = hora.Add(TimeSpan.FromMinutes(-30));
                var horaFin = hora.Add(TimeSpan.FromMinutes(30));

                var citaExistente = this.dbContextBarberia.Citas
                    .FirstOrDefault(c => c.Fecha.Date == fecha.Date && c.Hora >= horaInicio && c.Hora <= horaFin);

                return citaExistente != null;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Ha ocurrido un error verificando si ya existe una cita programada en el rango de 30 minutos: {ex.ToString()}.");
                throw new CitaExceptions("Ha ocurrido un error verificando si existe una cita programada en el rango de 30 minutos.");
            }
        }

        public override void Add(Citas entity)
        {
            base.Add(entity);
            base.SaveChanged();
        }

        public override void Update(Citas entity)
        {

            try
            {
                Citas citaUpdate = this.GetById(entity.CitaId);

                if (citaUpdate is null)
                {

                    throw new CitaExceptions("Ha ocurrido un error obteniendo el Id de la Cita.");

                }

                citaUpdate.Barbero = entity.Barbero;
                citaUpdate.Estilo = entity.Estilo;
                citaUpdate.Estado = entity.Estado;
                citaUpdate.Fecha = entity.Fecha;
                citaUpdate.BarberoId = entity.BarberoId;
                citaUpdate.EstiloId = entity.EstiloId;
                citaUpdate.CitaId = entity.CitaId;
                
                base.Update(citaUpdate);
                base.SaveChanged();

            }
            catch (Exception ex)
            {
                this.logger.LogError($"Ha ocurrido un error actualizando las citas: {ex.ToString()}.");
            }
        }

        public void UpdateEstado(Citas citaUpdate)
        {
            try
            {
                Citas citaUpdateEstado = this.GetById(citaUpdate.CitaId);

                if (citaUpdateEstado is null)
                {
                    throw new CitaExceptions("Ha ocurrido un error obteniendo el Id de la Cita.");
                }

                citaUpdateEstado.CitaId = citaUpdate.CitaId;
                citaUpdateEstado.Estado = citaUpdate.Estado;

                base.Update(citaUpdateEstado);
                base.SaveChanged();

            }
            catch (Exception ex)
            {
                this.logger.LogError($"Ha ocurrido un error actualizando las citas: {ex.ToString()}.");
            }
        }

        public override void Remove(Citas entity)
        {

            try
            {

                Citas citaRemove = this.GetById(entity.CitaId);

                if (citaRemove is null)
                {

                    throw new CitaExceptions("Ha ocurrido un error obteniendo el Id de la Cita.");

                }

                citaRemove.Deleted = true;
                citaRemove.DeletedDate = DateTime.Now; 
                citaRemove.UserDeleted = entity.UserDeleted;

                base.Update(citaRemove);
                base.SaveChanged();

            }
            catch (Exception ex)
            {
                this.logger.LogError($"Ha ocurrido un error Removiendo la cita: {ex.ToString()}.");
            }


        }

    }
}
