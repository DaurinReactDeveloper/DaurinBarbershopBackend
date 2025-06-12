using GestorBarberia.Application.Contract;
using GestorBarberia.Application.Core;
using GestorBarberia.Application.Dtos.ClienteDto;
using GestorBarberia.Application.Validations;
using GestorBarberia.Domain.Entities;
using GestorBarberia.Persistence.Interface;
using Microsoft.Extensions.Logging;

namespace GestorBarberia.Application.Services
{
    public class ClienteServices : IClienteServices
    {

        private readonly IClienteRepository clienteRepository;
        private readonly ILogger<ClienteServices> logger;
        private readonly IEmailServices emailServices;
        private readonly IBarberiasRepository barberiaRepository;

        public ClienteServices(IClienteRepository clienteRepository, ILogger<ClienteServices> logger , IEmailServices emailServices, IBarberiasRepository barberiaRepository)
        {
            this.clienteRepository = clienteRepository;
            this.logger = logger;
            this.emailServices = emailServices;
            this.barberiaRepository = barberiaRepository;
        }

        public ServiceResult GetCliente(string name, string password)
        {
            ServiceResult result = new ServiceResult();

            try
            {

                var getCliente = this.clienteRepository.GetClienteName(name);   

                if (!ClienteValidations.ValidationNameCliente(getCliente))
                {

                    result.Success = false;
                    result.Message = "Nombre de Usuario Incorrecto.";
                    return result;

                }

                if (!AuthenticationValidations.AuthenticationValidationPassword(password, getCliente.Password))
                {

                    result.Success = false;
                    result.Message = "Contraseña incorrecta.";
                    return result;

                }

                result.Data = getCliente;
                result.Message = "Se ha encontrado correctamente el cliente.";

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo el cliente.";
                this.logger.LogError($"Ha ocurrido un error obteniendo el cliente: {ex.Message}.");
            }

            return result;


        }

        public ServiceResult GetClientes()
        {
            ServiceResult result = new ServiceResult();

            try
            {

                var clientes = this.clienteRepository.GetClientes();

                if (!ClienteValidations.ValidationsCountCliente(clientes)) {

                    result.Success = false;
                    result.Message = "No tiene clientes.";
                    return result;

                }

                result.Data = clientes;
                result.Message = "Clientes Obtenidos Correctamente.";

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo los clientes.";
                this.logger.LogError($"Ha ocurrido un error obteniendo los clientes: {ex.Message}.");
            }

            return result;

        }

        public ServiceResult GetClienteById(int id)
        {

            ServiceResult result = new ServiceResult();

            try
            {
                var clienteid = this.clienteRepository.GetById(id);

                if (!ClienteValidations.ValidationIdCliente(clienteid))
                {
                    result.Success = false;
                    result.Message = "El Cliente no existe.";
                    return result;
                }

                result.Data = clienteid;
                result.Message = "Cliente Obtenido Correctamente.";

            }

            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo el cliente.";
                this.logger.LogError($"Ha ocurrido un error obteniendo el cliente: {ex.Message}.");
            }

            return result;

        }

        public ServiceResult GetClientesbyBarberiaId(int barberiaId)
        {
            ServiceResult result = new ServiceResult();

            var BarberoByAdminId = this.barberiaRepository.GetBarberiaByAdminId(barberiaId);

            try
            {
                var clientes = this.clienteRepository.GetClientesByBarberiaId(BarberoByAdminId.BarberiasId);
                result.Data = clientes;
                result.Message = "Clientes Obtenidos Correctamente.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo el cliente.";
                this.logger.LogError($"Ha ocurrido un error obteniendo el cliente: {ex.Message}.");
            }

            return result;
        }

        public ServiceResult Add(ClienteAddDto modelDto)
        {

            ServiceResult result = new ServiceResult();


            if (!ClienteValidations.ValidationAddCliente(modelDto))
            {
                result.Success = false;
                result.Message = "Los campos para agregar una cita NO cumplen con las validaciones establecidas.";
                return result;
            }

            if (this.clienteRepository.VerifyNameCliente(modelDto.Nombre))
            {
                result.Success = false;
                result.Message = "Ya existe un cliente con ese nombre.";
                return result;
            }

            string PasswordHashed = BCrypt.Net.BCrypt.HashPassword(modelDto.Password);
            
            try
            {

                this.clienteRepository.Add(new Clientes()
                {
                    ClienteId = modelDto.ClienteId,
                    BarberiaId = modelDto.BarberiaId,
                    Email = modelDto.Email,
                    Nombre = modelDto.Nombre,
                    Telefono = modelDto.Telefono,
                    Password = PasswordHashed,
                    Imgcliente = modelDto.Imgcliente,
                    CreationDate = DateTime.Now,
                    CreationUser = modelDto.ChangeUser,

                });

                var emailModel = this.emailServices.GenerateEmailModelCliente(modelDto.Nombre, modelDto.Password);
                var emailBody = this.emailServices.RenderTemplateCliente("EmailTemplateNewUser", emailModel);
                this.emailServices.SendEmail(modelDto.Email, "¡Bienvenido a DaurinBarbershop!", emailBody, true);

                this.clienteRepository.SaveChanged();
                result.Message = "Cliente Agregado Correctamente";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error guardando el cliente.";
                this.logger.LogError($"Ha ocurrido un error guardando el cliente: {ex.Message}.");
            }

            return result;

        }

        public ServiceResult Update(ClienteUpdateDto modelDto)
        {
            ServiceResult result = new ServiceResult();

            try
            {

                var clienteUpdate = this.clienteRepository.GetById(modelDto.ClienteId);

                if (!ClienteValidations.ValidationUpdateCliente(modelDto))
                {
                    result.Success = false;
                    result.Message = "Los campos para actualizar un cliente NO cumplen con las validaciones establecidas.";
                    return result;
                }

                string PasswordHashed = BCrypt.Net.BCrypt.EnhancedHashPassword(modelDto.Password);

                clienteUpdate.ClienteId = modelDto.ClienteId;
                clienteUpdate.Nombre = modelDto.Nombre;
                clienteUpdate.Email = modelDto.Email;
                clienteUpdate.Telefono = modelDto.Telefono;
                clienteUpdate.Password = PasswordHashed;
                clienteUpdate.Imgcliente = modelDto.Imgcliente;
                clienteUpdate.ModifyDate = DateTime.Now;
                clienteUpdate.UserMod = modelDto.ChangeUser;

                this.clienteRepository.Update(clienteUpdate);
                this.clienteRepository.SaveChanged();
                result.Message = "Cliente Actualizado Correctamente.";

            }

            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error actualizando el cliente.";
                this.logger.LogError($"Ha ocurrido un error actualizando el cliente: {ex.Message}.");
            }

            return result;
        }

        public ServiceResult Remove(ClienteRemoveDto modelDto)
        {
            ServiceResult result = new ServiceResult();

            try
            {

                var clienteid = this.clienteRepository.GetById(modelDto.ClienteId);

                if (ClienteValidations.ValidationRemoveCliente(clienteid))
                {
                    result.Success = false;
                    result.Message = "Ha ocurrido un error obteniendo el cliente.";
                    return result;
                }

                clienteid.UserDeleted = modelDto.ChangeUser;

                this.clienteRepository.Remove(clienteid);
                this.clienteRepository.SaveChanged();
                result.Message = "Cliente Removido Correctamente.";

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error eliminando el cliente.";
                this.logger.LogError($"Ha ocurrido un error eliminando el cliente: {ex.Message}.");
            }

            return result;

        }

        public ServiceResult RemoveClienteByAdmin(int clienteId, int adminId)
        {
            ServiceResult result = new ServiceResult();

            try
            {
                var RemoveCliente = this.clienteRepository.RemoveClienteByAdmin(clienteId, adminId);

                if (!RemoveCliente)
                {
                    result.Success = false;
                    result.Message = "No tienes permisos para remover este barbero.";
                    return result;
                }

                result.Message = "Cliente Removido Correctamente.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error eliminando el barbero.";
                this.logger.LogError($"Ha ocurrido un error eliminando el barbero: {ex.Message}.");
            }

            return result;
        }

    }
}
