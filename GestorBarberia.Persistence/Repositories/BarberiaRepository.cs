using GestorBarberia.Domain.Entities;
using GestorBarberia.Infrastructure.Exceptions;
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
    public class BarberiaRepository : BaseRepository<Barberias>, IBarberiasRepository
    {
        private readonly DbContextBarberia dbContextBarberia;
        private readonly ILogger<BarberiaRepository> logger;

        public BarberiaRepository(DbContextBarberia dbContextBarberia, ILogger<BarberiaRepository> logger) : base(dbContextBarberia)
        {

            this.dbContextBarberia = dbContextBarberia;
            this.logger = logger;

        }

        public List<BarberiaModel> GetBarberias()
        {

            List<BarberiaModel> ListBarberias = new List<BarberiaModel>();

            try
            {

                ListBarberias = (from bs in this.dbContextBarberia.Barberias
                                 where !bs.Deleted
                                 select new BarberiaModel()
                                 {

                                     BarberiasId = bs.BarberiasId,
                                     NombreBarberia = bs.NombreBarberia,
                                     Admin = bs.Admin,

                                 }).ToList();

                return ListBarberias;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Ha ocurrido un error listando las barberias: {ex.ToString()}.");
                throw new BarberiaExceptions("Ha ocurrido un error listando las barberias");
            }

        }

        public BarberiaModel GetBarberiaByAdminId(int adminId)
        {
            try
            {

                var barberiaModel = (from c in this.dbContextBarberia.Barberias
                                 where c.Admin.Equals(adminId) && !c.Deleted
                                 select new BarberiaModel()
                                 {
                                     
                                  BarberiasId = c.BarberiasId,
                                  Admin = c.Admin,
                                  
                                 }).FirstOrDefault();

                return barberiaModel;

            }
            catch (Exception ex)
            {
                this.logger.LogError($"Ha ocurrido un error obteniendo la barberia: {ex.ToString()}.");
                throw new CitaExceptions("Ha ocurrido un error obteniendo la barberia");

            }
        }
       
        public BarberiaModel GetBarberiaById(int barberiaId)
        {
            try
            {

                var barberiaModel = (from br in this.dbContextBarberia.Barberias
                                     where br.BarberiasId == barberiaId && !br.Deleted
                                     select new BarberiaModel()
                                     {

                                         NombreBarberia = br.NombreBarberia

                                     }).FirstOrDefault();

                return barberiaModel;

            }
            catch (Exception ex)
            {
                this.logger.LogError($"Ha ocurrido un error obteniendo la barberia: {ex.ToString()}.");
                throw new CitaExceptions("Ha ocurrido un error obteniendo la barberia");

            }
        }

        public override void Add(Barberias entity)
        {
            base.Add(entity);
            base.SaveChanged();
        }

        public override void Update(Barberias entity)
        {
            try
            {
                Barberias barberiasUpdate = this.GetById(entity.BarberiasId);

                if (barberiasUpdate is null)
                {

                    throw new BarberiaExceptions("Ha ocurrido un error obteniendo el Id de la barberia.");

                }

                barberiasUpdate.NombreBarberia = entity.NombreBarberia;

                base.Update(barberiasUpdate);
                base.SaveChanged();

            }
            catch (Exception ex)
            {
                this.logger.LogError($"Ha ocurrido un error actualizando la barberia: {ex.ToString()}.");
            }
        }

        public override void Remove(Barberias entity)
        {
            try
            {

                Barberias barberiaRemove = this.GetById(entity.BarberiasId);

                if (barberiaRemove is null)
                {

                    throw new BarberiaExceptions("Ha ocurrido un error obteniendo el Id de la barberia.");

                }

                barberiaRemove.Deleted = true;
                barberiaRemove.DeletedDate = DateTime.Now;
                barberiaRemove.UserDeleted = entity.UserDeleted;


                base.Update(barberiaRemove);
                base.SaveChanged();

            }
            catch (Exception ex)
            {
                this.logger.LogError($"Ha ocurrido un error Removiendo la barberia: {ex.ToString()}.");
            }
        }

    }
}
