using GestorBarberia.Application.Contract;
using GestorBarberia.Application.Core;
using GestorBarberia.Application.Dtos.BarberoDto;
using GestorBarberia.Application.Validations;
using GestorBarberia.Domain.Entities;
using GestorBarberia.Persistence.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Application.Services
{
    public class BarberoServices : IBarberoServices
    {

        private readonly IBarberoRepository BarberoRepository;
        private readonly ILogger<BarberoServices> logger;
        private readonly IEmailServices emailServices;
        private readonly IBarberiaServices barberiaServices;
        private readonly IBarberiasRepository barberiaRepository;

        public BarberoServices(IBarberoRepository BarberoRepository, ILogger<BarberoServices> logger, IEmailServices emailServices, IBarberiaServices barberiaServices, IBarberiasRepository barberiaRepository)
        {
            this.BarberoRepository = BarberoRepository;
            this.logger = logger;
            this.emailServices = emailServices;
            this.barberiaServices = barberiaServices;
            this.barberiaRepository = barberiaRepository;
        }

        public ServiceResult GetBarberos()
        {
            ServiceResult result = new ServiceResult();

            try
            {
                var barberos = this.BarberoRepository.GetBarberos();

                if (!BarberoValidations.ValidationCountBarbero(barberos))
                {

                    result.Success = false;
                    result.Message = "No hay Barberos";
                    return result;

                }

                result.Data = barberos;
                result.Message = "Barberos Obtenidos Correctamente.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo los barberos.";
                this.logger.LogError($"Ha ocurrido un EError obteniendo los barberos: {ex.Message}.");
            }

            return result;
        }

        public ServiceResult GetBarbero(string name, string password)
        {
            ServiceResult result = new ServiceResult();

            try
            {
                var getBarbero = this.BarberoRepository.GetBarberoName(name);

                if (!BarberoValidations.ValidationNameBarberoModel(getBarbero))
                {
                    result.Success = false;
                    result.Message = "Nombre de Barbero Incorrecto.";
                    return result;
                }

                if (!AuthenticationValidations.AuthenticationValidationPassword(password, getBarbero.Password))
                {
                    result.Success = false;
                    result.Message = "Contraseña incorrecta.";
                    return result;
                }

                result.Data = getBarbero;
                result.Message = "Se ha encontrado correctamente el barbero";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo el barbero.";
                this.logger.LogError($"Ha ocurrido un error obteniendo el barbero: {ex.Message}.");
            }

            return result;
        }

        public ServiceResult GetBarberosbyBarberiaId(int barberiaId)
        {
            ServiceResult result = new ServiceResult();


            var BarberoByAdminId = this.barberiaRepository.GetBarberiaByAdminId(barberiaId);


            try
            {
                var barberos = this.BarberoRepository.GetBarberosByBarberiaId(BarberoByAdminId.BarberiasId);
                result.Data = barberos;
                result.Message = "Barberos Obtenidos Correctamente.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo los barberos.";
                this.logger.LogError($"Ha ocurrido un error obteniendo los barberos: {ex.Message}.");
            }

            return result;
        }

        public ServiceResult GetBarberosbyCliente(int clienteBarberiaId)
        {
            ServiceResult result = new ServiceResult();

            try
            {
                var barberos = this.BarberoRepository.GetBarberosByBarberiaId(clienteBarberiaId);
                result.Data = barberos;
                result.Message = "Barberos Obtenidos Correctamente.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo los barberos.";
                this.logger.LogError($"Ha ocurrido un error obteniendo los barberos: {ex.Message}.");
            }

            return result;
        }

        public ServiceResult GetBarberobyId(int id)
        {

            ServiceResult result = new ServiceResult();

            try
            {

                var BarberoId = this.BarberoRepository.GetById(id);

                if (!BarberoValidations.ValidationIdBarbero(BarberoId))
                {

                    result.Success = false;
                    result.Message = "No se pudo obtener el barbero seleccionado.";
                    return result;

                }

                result.Data = BarberoId;
                result.Message = "Barbero Obtenido Correctamente.";
            }

            catch (Exception ex)
            {

                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo el barbero.";
                this.logger.LogError($"Ha ocurrido un error obteniendo el barbero: {ex.Message}.");

            }

            return result;
        }

        public ServiceResult Add(BarberoAddDto modelDto)
        {
            ServiceResult result = new ServiceResult();

            try
            {
                if (!BarberoValidations.ValidationAddBarbero(modelDto))
                {
                    result.Success = false;
                    result.Message = "Los campos para agregar un barbero NO cumplen con las validaciones establecidas.";
                    return result;
                }

                if (this.BarberoRepository.VerifyNameBarbero(modelDto.Nombre))
                {
                    result.Success = false;
                    result.Message = "Ya existe un barbero con ese nombre.";
                    return result;
                }

                string PasswordHashed = BCrypt.Net.BCrypt.HashPassword(modelDto.Password);

                var barberiaId = this.barberiaRepository.GetBarberiaByAdminId(modelDto.ChangeUser);

                var BarberiaId = this.barberiaServices.GetBarberiaById(barberiaId.BarberiasId);



                this.BarberoRepository.Add(new Barberos()
                {
                    BarberoId = modelDto.BarberoId,
                    BarberiaId = barberiaId.BarberiasId,
                    Nombre = modelDto.Nombre,
                    Telefono = modelDto.Telefono,
                    Email = modelDto.Email,
                    Password = PasswordHashed,
                    Imgbarbero = modelDto.Imgbarbero,
                    CreationDate = DateTime.Now,
                    CreationUser = modelDto.ChangeUser,
                });

                var emailModel = this.emailServices.GenerateEmailModelBarbero(modelDto.Nombre, modelDto.Password, BarberiaId.Data.NombreBarberia);
                var emailBody = this.emailServices.RenderTemplateBarbero("EmailTemplateNewBarber", emailModel);
                this.emailServices.SendEmail(modelDto.Email, "¡Bienvenido Estimado Barbero!", emailBody, true);

                this.BarberoRepository.SaveChanged();
                result.Message = "Se ha Agregado Correctamente el Barbero.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error guardando el barbero.";
                this.logger.LogError($"Ha ocurrido un error guardando el barbero en barberoService + {ex.Message}.");
            }

            return result;
        }

        public ServiceResult Update(BarberoUpdateDto modelDto)
        {

            ServiceResult result = new ServiceResult();

            try
            {

                var BarberoExistente = this.BarberoRepository.GetById(modelDto.BarberoId);

                if (!BarberoValidations.ValidationUpdateBarbero(modelDto))
                {
                    result.Success = false;
                    result.Message = "Los campos para actualizar un barbero NO cumplen con las validaciones establecidas.";
                    return result;
                }

                string PasswordHashed = BCrypt.Net.BCrypt.HashPassword(modelDto.Password);

                BarberoExistente.BarberoId = modelDto.BarberoId;
                BarberoExistente.Nombre = modelDto.Nombre;
                BarberoExistente.Telefono = modelDto.Telefono;
                BarberoExistente.Email = modelDto.Email;
                BarberoExistente.Password = PasswordHashed;
                BarberoExistente.Imgbarbero = modelDto.Imgbarbero;
                BarberoExistente.ModifyDate = DateTime.Now;
                BarberoExistente.UserMod = modelDto.ChangeUser;

                this.BarberoRepository.Update(BarberoExistente);
                this.BarberoRepository.SaveChanged();
                result.Message = "Barbero Actualizado Correctamente.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error actualizando el Barbero.";
                this.logger.LogError($"Ha ocurrido un error actualizando el Barbero: {ex.Message}.");
            }

            return result;
        }

        public ServiceResult Remove(BarberoRemoveDto modelDto)
        {

            ServiceResult result = new ServiceResult();

            try
            {

                var BarberoId = this.BarberoRepository.GetById(modelDto.BarberoId); 


                if (!BarberoValidations.ValidationRemoveBarbero(BarberoId))
                {
                    result.Success = false;
                    result.Message = "Ha ocurrido un error obtiendo el barbero.";
                    return result;
                }

                BarberoId.UserDeleted = modelDto.ChangeUser;

                this.BarberoRepository.Remove(BarberoId);
                this.BarberoRepository.SaveChanged();
                result.Message = "Barbero Removido Correctamente.";
            }
            catch (Exception ex)
            {

                result.Success = false;
                result.Message = "Ha ocurrido un error eliminando el barbero.";
                this.logger.LogError($"Ha ocurrido un error eliminando el barbero: {ex.Message}.");

            }

            return result;
        }

        public ServiceResult RemoveBarberoByAdmin(int barberoId, int adminId)
        {
            ServiceResult result = new ServiceResult();

            try
            {
               var RemoveBarbero = this.BarberoRepository.RemoveBarberoByAdmin(barberoId, adminId);

                if (!RemoveBarbero)
                {

                    result.Success = false;
                    result.Message = "No tienes permisos para remover este barbero.";
                    return result;

                }


                result.Message = "Barbero Removido Correctamente.";
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
