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
    public class ClienteRepository : BaseRepository<Clientes>, IClienteRepository
    {

        private readonly DbContextBarberia dbContextBarberia;
        private readonly ILogger<ClienteRepository> logger;
        private readonly IAdministradorRepository administradorRepository;
        private readonly IBarberiasRepository barberiasRepository;

        public ClienteRepository(DbContextBarberia dbContextBarberia, ILogger<ClienteRepository> logger, IAdministradorRepository administradorRepository, IBarberiasRepository barberiasRepository) : base(dbContextBarberia)
        {
            this.dbContextBarberia = dbContextBarberia;
            this.logger = logger;
            this.administradorRepository = administradorRepository;
            this.barberiasRepository = barberiasRepository;
        }

        public List<ClienteModel> GetClientes()
        {

            List<ClienteModel> clienteModels = new List<ClienteModel>();

            try
            {

                clienteModels = base.GetEntities()
                                .Select(cm => cm.ConvertClienteToModel())
                                .ToList();
            }

            catch (Exception ex)

            {
                this.logger.LogError($"Ha ocurrido un error listando los clientes: {ex.ToString()}.");
            }

            return clienteModels;

        }

        public List<ClienteModel> GetClientesByBarberiaId(int barberiaId)
        {

            List<ClienteModel> clienteModels = new List<ClienteModel>();

            try
            {

                clienteModels = (from bm in this.dbContextBarberia.Clientes
                                 where bm.BarberiaId.Equals(barberiaId) && !bm.Deleted
                                 select new ClienteModel()
                                 {

                                     BarberiaId = bm.BarberiaId,
                                     ClienteId = bm.ClienteId,
                                     Email = bm.Email,
                                     Imgcliente = bm.Imgcliente,
                                     Nombre = bm.Nombre,
                                     Telefono = bm.Telefono

                                 }).ToList();
            }

            catch (Exception ex)

            {
                this.logger.LogError($"Ha ocurrido un error listando los clientes: {ex.ToString()}.");
            }

            return clienteModels;
        }

        public ClienteModel GetClienteName(string name)
        {
            try
            {
                var model = (from cl in this.dbContextBarberia.Clientes
                             where cl.Nombre.Equals(name) && !cl.Deleted
                             select new ClienteModel()
                             {
                                 ClienteId = cl.ClienteId,
                                 Nombre = cl.Nombre,
                                 Password = cl.Password,
                                 BarberiaId = cl.BarberiaId

                             }).FirstOrDefault();

                return model;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Ha ocurrido un error obteniendo el cliente: {ex.ToString()}.");
                throw new ClienteExceptions("Ha ocurrido un error obteniendo el cliente.");
            }
        }

        public override void Add(Clientes entity)
        {
            base.Add(entity);
            base.SaveChanged();
        }

        public override void Update(Clientes entity)
        {
            try
            {

                Clientes clienteEntity = this.GetById(entity.ClienteId);

                if (clienteEntity is null)
                {
                    throw new ClienteExceptions("Ha ocurrido un error obteniendo el Id del cliente.");
                }

                clienteEntity.ClienteId = entity.ClienteId;
                clienteEntity.Nombre = entity.Nombre;
                clienteEntity.Email = entity.Email;
                clienteEntity.Telefono = entity.Telefono;
                clienteEntity.Password = entity.Password;
                clienteEntity.Imgcliente = entity.Imgcliente;

                base.Update(clienteEntity);
                base.SaveChanged();
            }

            catch (Exception ex)
            {

                this.logger.LogError($"Ha ocurrido un error actualizando el cliente: {ex.ToString()}.");

            }

        }

        public override void Remove(Clientes entity)
        {

            try
            {

                Clientes clienteRemove = this.GetById(entity.ClienteId);

                if (clienteRemove is null)
                {
                    throw new ClienteExceptions("Ha ocurrido un error obteniendo el Id del cliente.");
                }

                clienteRemove.Deleted = true;
                clienteRemove.UserDeleted = entity.UserDeleted;
                clienteRemove.DeletedDate = entity.DeletedDate;

                base.Update(clienteRemove);
                base.SaveChanged();

            }

            catch (Exception ex)
            {

                this.logger.LogError($"Ha ocurrido un error eliminando el cliente: {ex.ToString()}.");

            }

        }

        public bool VerifyNameCliente(string nameCliente)
        {
            try
            {
                var nameData = (from n in this.dbContextBarberia.Clientes
                                where n.Nombre.Equals(nameCliente) && !n.Deleted
                                select n).FirstOrDefault();

                return nameData != null;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Ha ocurrido un error verificando el nombre del cliente: {ex.ToString()}.");
                throw new ClienteExceptions("Ha ocurrido un error verificando el nombre del cliente.");
            }
        }

        public bool VerifyPermissionsCliente(int clienteId, int adminId)
        {
            // 1. Conseguir el Administrador
            var admin = this.administradorRepository.GetById(adminId);
            if (admin == null)
            {
                throw new ClienteExceptions("Administrador no encontrado.");
            }

            // 2. Conseguir el cliente
            var cliente = this.GetById(clienteId);
            if (cliente == null)
            {
                throw new ClienteExceptions("Cliente no encontrado.");
            }

            // 3. Conseguir la barbería
            var barberia = this.barberiasRepository.GetBarberiaByAdminId(adminId);
            if (barberia == null)
            {
                throw new ClienteExceptions("Barbería no encontrada.");
            }

            return admin.AdministradoresId == barberia.Admin && barberia.BarberiasId == cliente.BarberiaId;
        }

        public bool RemoveClienteByAdmin(int clienteId, int adminId)
        {
            try
            {
                if (VerifyPermissionsCliente(clienteId, adminId))
                {
                    Clientes clienteRemove = this.GetById(clienteId);

                    if (clienteRemove is null)
                    {
                        throw new ClienteExceptions("Cliente no encontrado.");
                    }

                    clienteRemove.Deleted = true;
                    clienteRemove.UserDeleted = adminId;
                    clienteRemove.DeletedDate = DateTime.Now;


                    base.Update(clienteRemove);
                    base.SaveChanged();
                    return true;
                }
                else
                {
                    throw new ClienteExceptions("No tienes permiso para eliminar este cliente.");
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Ha ocurrido un error eliminando el cliente: {ex.ToString()}.");
                return false;
            }
        }

    }

}

