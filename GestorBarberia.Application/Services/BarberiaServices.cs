using GestorBarberia.Application.Contract;
using GestorBarberia.Application.Core;
using GestorBarberia.Application.Dtos.BarberiaDto;
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
    public class BarberiaServices : IBarberiaServices
    {

        private readonly ILogger<BarberiaServices> logger;
        private readonly IBarberiasRepository barberiaRepository;
        private readonly IEmailServices emailServices;
        private readonly IAdministradorServices adminServices;

        public BarberiaServices(ILogger<BarberiaServices> logger, IBarberiasRepository barberiaRepository, IEmailServices emailServices, IAdministradorServices adminServices)
        {
            this.logger = logger;
            this.barberiaRepository = barberiaRepository;
            this.emailServices = emailServices;
            this.adminServices = adminServices;
        }

        public ServiceResult GetBarberias()
        {
            ServiceResult result = new ServiceResult();

            try
            {

                var barberias = this.barberiaRepository.GetBarberias();

                if (!BarberiaValidations.ValidationCountBarberia(barberias))
                {

                    result.Success = false;
                    result.Message = "No hay Barberias";
                    return result;
                }


                result.Data = barberias;
                result.Message = "Barberias Obtenidas Correctamente.";

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo las barberias.";
                this.logger.LogError($"Ha ocurrido un error obteniendo las barberias: {ex.Message}.");
            }

            return result;

        }

        public ServiceResult GetBarberiaById(int barberiaId)
        {
            ServiceResult result = new ServiceResult();

            try
            {

                var getBarberiaName = this.barberiaRepository.GetBarberiaById(barberiaId);

                if (!BarberiaValidations.ValidationBarberiaName(getBarberiaName))
                {
                    result.Success = false;
                    result.Message = "No se pudo obtener la barberia seleccionado.";
                    return result;
                }

                result.Data = getBarberiaName;
                result.Message = "Se ha encontrado correctamente la barberia.";

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo la barbera.";
                this.logger.LogError($"Ha ocurrido un error obteniendo la barberia: {ex.Message}.");
            }

            return result;
        }

        public ServiceResult Add(BarberiaAddDto modelDto)
        {
            ServiceResult result = new ServiceResult();

            try
            {

                if (!BarberiaValidations.ValidationAddBarberia(modelDto))
                {
                    result.Success = false;
                    result.Message = "Los campos para agregar una barberia NO cumplen con las validaciones establecidas.";
                    return result;
                }


                this.barberiaRepository.Add(new Barberias()
                {

                    NombreBarberia = modelDto.NombreBarberia,
                    Admin = modelDto.Admin,
                    CreationDate = DateTime.Now,
                    CreationUser = modelDto.ChangeUser,

                });

                var GetAdminId = this.adminServices.GetAdministradorById(modelDto.Admin);

                var emailModel = this.emailServices.GenerateEmailModelAdminSucursal(GetAdminId.Data.Nombre, modelDto.NombreBarberia);
                var emailBody = this.emailServices.RenderTemplateAdminSucursal("EmailTemplateNewAdmin", emailModel);
                this.emailServices.SendEmail(GetAdminId.Data.Email, "¡Bienvenido Administrador!", emailBody, true);

                this.barberiaRepository.SaveChanged();
                result.Message = "Se ha Agregado Correctamente la barberia.";

            }

            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error guardando la barberia.";
                this.logger.LogError($"Ha ocurrido un error guardando la barberia + {ex.Message}.");
            }

            return result;

        }

        public ServiceResult Update(BarberiaUpdateDto modelDto)
        {
            ServiceResult result = new ServiceResult();

            try
            {

                var BarberiaExistente = this.barberiaRepository.GetById(modelDto.BarberiasId);


                if (!BarberiaValidations.ValidationUpdateBarberia(modelDto))
                {
                    result.Success = false;
                    result.Message = "Los campos para agregar una barberia NO cumplen con las validaciones establecidas.";
                    return result;
                }

                BarberiaExistente.NombreBarberia = modelDto.NombreBarberia;
                BarberiaExistente.ModifyDate = DateTime.Now;
                BarberiaExistente.UserMod = modelDto.ChangeUser;

                this.barberiaRepository.Update(BarberiaExistente);
                this.barberiaRepository.SaveChanged();
                result.Message = "Barberia Actualizada Correctamente.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error actualizando la barberia.";
                this.logger.LogError($"Ha ocurrido un error actualizando la barberia: {ex.Message}.");
            }

            return result;
        }

        public ServiceResult Remove(BarberiaRemoveDto modelDto)
        {
            ServiceResult result = new ServiceResult();

            try
            {

                var BarberiaId = this.barberiaRepository.GetById(modelDto.BarberiasId);

                if (!BarberiaValidations.ValidationRemoveBarberia(BarberiaId))
                {
                    result.Success = false;
                    result.Message = "Ha ocurrido un error obteniendo el id de la barberia.";
                    return result;
                }

                BarberiaId.UserDeleted = modelDto.ChangeUser;

                this.barberiaRepository.Remove(BarberiaId);
                this.barberiaRepository.SaveChanged();
                result.Message = "Barberia Removida Correctamente.";
            }
            catch (Exception ex)
            {

                result.Success = false;
                result.Message = "Ha ocurrido un error eliminando la barberia.";
                this.logger.LogError($"Ha ocurrido un error eliminando la barberia: {ex.Message}.");

            }

            return result;
        }

    }
}
