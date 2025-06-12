using GestorBarberia.Application.Contract;
using GestorBarberia.Application.Core;
using GestorBarberia.Application.Dtos.CitaDto;
using GestorBarberia.Application.Validations;
using GestorBarberia.Domain.Entities;
using GestorBarberia.Infrastructure.Models;
using GestorBarberia.Persistence.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Application.Services
{
    public class CitaServices : ICitaServices
    {

        private readonly ICitaRepository citaRepository;
        private readonly ILogger<CitaServices> logger;
        private readonly IClienteServices clienteServices;
        private readonly IEmailServices emailServices;
        private readonly IBarberoServices barberoServices;
        private readonly IBarberiasRepository barberiasRepository;


        public CitaServices(ICitaRepository citaRepository, ILogger<CitaServices> logger, IClienteServices clienteServices, IEmailServices emailServices, IBarberoServices barberoServices, IBarberiasRepository barberiasRepository)
        {
            this.citaRepository = citaRepository;
            this.logger = logger;
            this.clienteServices = clienteServices;
            this.emailServices = emailServices;
            this.barberoServices = barberoServices;
            this.barberiasRepository = barberiasRepository;
        }

        public ServiceResult GetCitaById(int citaId)
        {
            ServiceResult result = new ServiceResult();

            try
            {
                var Cita = this.citaRepository.GetCitaById(citaId);

                if (!CitaValidations.ValidationsCitaId(Cita)) { 
                
                    result.Success = false;
                    result.Message = "La Cita no Existe.";
                    return result;
                
                }

                result.Data = Cita;
                result.Message = "Cita Obtenida Correctamente.";

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo las cita.";
                this.logger.LogError($"Ha ocurrido un error obteniendo la cita: {ex.Message}.");
            }

            return result;
        }

        public ServiceResult GetCitaByBarberoId(int barberoId)
        {

            ServiceResult result = new ServiceResult();

            try
            {

                var CitasBarbero = this.citaRepository.GetCitaByBarberoId(barberoId);

                if (!CitaValidations.ValidationsCountCita(CitasBarbero))
                {

                    result.Success = false;
                    result.Message = "Actualmente no tiene citas programadas.";
                    return result;

                }

                result.Data = CitasBarbero;
                result.Message = "Citas Obtenidas Correctamente.";
            }

            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo las citas.";
                this.logger.LogError($"Ha ocurrido un error obteniendo las citas: {ex.Message}.");
            }

            return result;

        }

        public ServiceResult GetCitaByClienteId(int clienteId)
        {
            ServiceResult result = new ServiceResult();

            try
            {
                var CitasCliente = this.citaRepository.GetCitaByClienteId(clienteId);

                if (!CitaValidations.ValidationsCountCita(CitasCliente))
                {

                    result.Success = false;
                    result.Message = "Actualmente no tiene citas programadas.";
                    return result;

                }

                result.Data = CitasCliente;
                result.Message = "Citas Obtenidas Correctamente.";
            }

            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo las citas.";
                this.logger.LogError($"Ha ocurrido un error obteniendo las citas: {ex.Message}.");
            }

            return result;
        }
        
        public ServiceResult Add(CitaAddDto modelDto)
        {

            ServiceResult result = new ServiceResult();

            if (!CitaValidations.ValidationAddCita(modelDto))
            {
                result.Success = false;
                result.Message = "Los campos para agregar una cita NO cumplen con las validaciones establecidas.";
                return result;
            }

            if (this.citaRepository.VerifyCita(modelDto.Fecha, modelDto.Hora))
            {
                result.Success = false;
                result.Message = "Ya existe una cita programada para este cliente en la misma fecha y hora.";
                return result;
            }

            try
            {
                this.citaRepository.Add(new Citas()
                {

                    CitaId = modelDto.CitaId,
                    ClienteId = modelDto.ClienteId,
                    BarberoId = modelDto.BarberoId,
                    EstiloId = modelDto.EstiloId,
                    Fecha = modelDto.Fecha,
                    Estado = modelDto.Estado,
                    Hora = modelDto.Hora,
                    BarberiaId = modelDto.BarberiaId,
                    CreationDate = DateTime.Now,
                    CreationUser = modelDto.ChangeUser,

                });

                var BarberoId = this.barberoServices.GetBarberobyId(modelDto.BarberoId);
                var ClienteId = this.clienteServices.GetClienteById(modelDto.ClienteId);

                if (BarberoId is null || ClienteId is null)
                {
                    result.Success = false;
                    result.Message = "El Barbero asociado a la cita no existe.";
                    return result;
                }

                var emailModel = this.emailServices.GenerateEmailModelCita(ClienteId.Data.Nombre, modelDto.Estado, modelDto.Fecha, modelDto.Hora, BarberoId.Data.Nombre);
                var emailBody = this.emailServices.RenderTemplateCita("EmailTemplateAdd", emailModel);
                this.emailServices.SendEmail(BarberoId.Data.Email, "Nueva Peticion de Cita", emailBody, true);

                this.citaRepository.SaveChanged();
                result.Message = "Cita Agregada Correctamente.";

            }
            catch (Exception ex)
            {

                result.Success = false;
                result.Message = "Ha ocurrido un error agregando la cita.";
                this.logger.LogError($"Ha ocurrido un error agregando la cita: {ex.Message}.");

            }

            return result;
        }

        public ServiceResult Update(CitaUpdateDto modelDto)
        {

            ServiceResult result = new ServiceResult();

            try
            {
                var citaUpdate = this.citaRepository.GetById(modelDto.CitaId);

                if (!CitaValidations.ValidationUpdateCita(modelDto))
                {
                    result.Success = false;
                    result.Message = "Los campos para actualizar una cita NO cumplen con las validaciones establecidas.";
                    return result;
                }

                citaUpdate.Estado = modelDto.Estado;
                citaUpdate.Fecha = modelDto.Fecha;
                citaUpdate.BarberoId = modelDto.BarberoId;
                citaUpdate.EstiloId = modelDto.EstiloId;
                citaUpdate.CitaId = modelDto.CitaId;
                citaUpdate.ModifyDate = DateTime.Now;
                citaUpdate.UserMod = modelDto.ChangeUser;

                this.citaRepository.Update(citaUpdate);
                this.citaRepository.SaveChanged();
                result.Message = "Cita Actualizada Correctamente.";
            }

            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error actualizando la cita.";
                this.logger.LogError($"Ha ocurrido un error actualizando la cita: {ex.Message}.");
            }

            return result;

        }

        public ServiceResult UpdateEstado(CitaUpdateDto updateEstado)
        {
            ServiceResult result = new ServiceResult();

            try
            {
                var citaUpdate = this.citaRepository.GetById(updateEstado.CitaId);

                if (!CitaValidations.ValidationIdCita(citaUpdate))
                {
                    result.Success = false;
                    result.Message = "La cita no existe.";
                    return result;
                }

                citaUpdate.CitaId = updateEstado.CitaId;
                citaUpdate.Estado = updateEstado.Estado;
                citaUpdate.ModifyDate = DateTime.Now;
                citaUpdate.UserMod = updateEstado.ChangeUser;

                if (updateEstado.Estado == "Realizada")
                {
                    this.citaRepository.UpdateEstado(citaUpdate);
                    this.citaRepository.SaveChanged();
                    result.Message = "Estado de la Cita Cambiado Correctamente.";
                    return result;
                }

                var cliente = this.clienteServices.GetClienteById(citaUpdate.ClienteId);
                var Barbero = this.barberoServices.GetBarberobyId(citaUpdate.BarberoId);

                if (cliente is null)
                {
                    result.Success = false;
                    result.Message = "El cliente asociado a la cita no existe.";
                    return result;
                }

                if (updateEstado.Estado == "Cancelada")
                {
                    var emailModelCancelada = this.emailServices.GenerateEmailModelCita(cliente.Data.Nombre, citaUpdate.Estado, citaUpdate.Fecha, citaUpdate.Hora,Barbero.Data.Nombre);

                    var emailBodyCancelada = this.emailServices.RenderTemplateCita("EmailTemplate", emailModelCancelada);

                    this.emailServices.SendEmail(Barbero.Data.Email, "Actualización de Estado de Cita", emailBodyCancelada, true);
                    
                    this.citaRepository.UpdateEstado(citaUpdate);
                    this.citaRepository.SaveChanged();

                    result.Message = "Estado de la Cita Cambiado Correctamente y Notificación Enviada.";
                    return result;
                }

                var emailModel = this.emailServices.GenerateEmailModelCita(cliente.Data.Nombre, citaUpdate.Estado, citaUpdate.Fecha, citaUpdate.Hora);

                var emailBody = this.emailServices.RenderTemplateCita("EmailTemplate", emailModel);

                this.emailServices.SendEmail(cliente.Data.Email, "Actualización de Estado de Cita", emailBody, true);

                this.citaRepository.UpdateEstado(citaUpdate);
                this.citaRepository.SaveChanged();
                result.Message = "Estado de la Cita Cambiado Correctamente y Notificación Enviada.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error actualizando la cita.";
                this.logger.LogError($"Ha ocurrido un error actualizando la cita: {ex.Message}.");
            }

            return result;
        }

        public ServiceResult Remove(CitaRemoveDto modelDto)
        {

            ServiceResult result = new ServiceResult();

            try
            {
                var citaRemove = this.citaRepository.GetById(modelDto.CitaId);

                if (!CitaValidations.ValidationIdCita(citaRemove))
                {
                    result.Success = false;
                    result.Message = "Ha ocurrido un error obteniendo la cita.";
                    return result;
                }

                if (!CitaValidations.ValidationRemoveCita(modelDto))
                {
                    result.Success = false;
                    result.Message = "Solo puedes eliminar las rechazadas, realizadas o cancelada.";
                    return result;
                }

                citaRemove.UserDeleted = modelDto.ChangeUser;

                this.citaRepository.Remove(citaRemove);
                this.citaRepository.SaveChanged();
                result.Message = "Cita Removida Correctamente.";

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error eliminado la cita.";
                this.logger.LogError($"Ha ocurrido un error eliminando la cita: {ex.Message}.");
            }

            return result;

        }

    }
}
