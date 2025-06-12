using GestorBarberia.Application.Contract;
using GestorBarberia.Application.Core;
using GestorBarberia.Application.Dtos.AdminDto;
using GestorBarberia.Application.Validations;
using GestorBarberia.Domain.Entities;
using GestorBarberia.Persistence.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GestorBarberia.Application.Services
{
    public class AdministradorServices : IAdministradorServices
    {

        private readonly IAdministradorRepository administradorRepository;
        private readonly ILogger<AdministradorServices> logger;

        public AdministradorServices(IAdministradorRepository administradorRepository, ILogger<AdministradorServices> logger)
        {
            this.administradorRepository = administradorRepository;
            this.logger = logger;
        }

        public ServiceResult GetAllAdministradores()
        {
            ServiceResult result = new ServiceResult();

            try
            {

                var getAdministradores = this.administradorRepository.GetAdministradores();

                if (!AdministradorValidations.AdministradorCountValidations(getAdministradores))
                {

                    result.Success = false;
                    result.Message = "No hay Administradores";
                    return result;
                }

                result.Data = getAdministradores;
                result.Message = "Se ha encontrado correctamente los administradores.";
            }
            catch (Exception ex)
            {

                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo los administradores.";
                this.logger.LogError($"Ha ocurrido un error obteniendo los administradores: {ex.Message}.");
            
            }

            return result;


        }

        public ServiceResult GetAdministradorPropietarioBarberia(string nombre, string password)
        {

            ServiceResult result = new ServiceResult();

            try
            {

                var getAdministrador = this.administradorRepository.GetAdministrador(nombre);

                if (!AdministradorValidations.AdministradorNameValidations(getAdministrador))
                {
                    result.Success = false;
                    result.Message = "Nombre del Administrador Incorrecto.";
                    return result;
                }

                if (!AuthenticationValidations.AuthenticationValidationPassword(password, getAdministrador.Password))
                {
                    result.Success = false;
                    result.Message = "Contraseña incorrecta.";
                    return result;
                }

                if (!AdministradorValidations.AdministradorPropietarioBarberiaValidations(getAdministrador.Tipo))
                {
                    result.Success = false;
                    result.Message = "No tiene los permisos necesarios para acceder a esta área.";
                    return result;

                }

                result.Data = getAdministrador;
                result.Message = "Se ha encontrado correctamente el administrador.";

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo el administrador.";
                this.logger.LogError($"Ha ocurrido un error obteniendo el administrador: {ex.Message}.");
            }

            return result;

        }

        public ServiceResult GetAdministradorPropietarioApp(string nombre, string password)
        {

            ServiceResult result = new ServiceResult();

            try
            {

                var getAdministrador = this.administradorRepository.GetAdministrador(nombre);

                if (!AdministradorValidations.AdministradorNameValidations(getAdministrador))
                {
                    result.Success = false;
                    result.Message = "Nombre del Administrador Incorrecto.";
                    return result;
                }

                if (!AuthenticationValidations.AuthenticationValidationPassword(password, getAdministrador.Password))
                {
                    result.Success = false;
                    result.Message = "Contraseña incorrecta.";
                    return result;
                }

                if (!AdministradorValidations.AdministradorPropietarioAppValidations(getAdministrador.Tipo))
                {

                    result.Success = false;
                    result.Message = "No tiene los permisos necesarios para acceder a esta área.";
                    return result;

                }

                result.Data = getAdministrador;
                result.Message = "Se ha encontrado correctamente el administrador.";

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo el administrador.";
                this.logger.LogError($"Ha ocurrido un error obteniendo el administrador: {ex.Message}.");
            }

            return result;

        }

        public ServiceResult GetAdministradorById(int id)
        {
            ServiceResult result = new ServiceResult();

            try
            {

                var getAdministrador = this.administradorRepository.GetAdministradorById(id);

                if (!AdministradorValidations.AdministradorIdValidations(getAdministrador))
                {

                    result.Success = false;
                    result.Message = "No se pudo obtener el administrador seleccionado.";
                    return result;

                }

                result.Data = getAdministrador;
                result.Message = "Se ha encontrado correctamente el administrador.";

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo el administrador.";
                this.logger.LogError($"Ha ocurrido un error obteniendo el administrador: {ex.Message}.");
            }

            return result;
        }

        public ServiceResult Add(AdminAddDto modelDto)
        {
            ServiceResult result = new ServiceResult();

            try
            {

                if (!AdministradorValidations.AdministradorAddValidations(modelDto))
                {
                    result.Success = false;
                    result.Message = "Los campos para agregar un administrador NO cumplen con las validaciones establecidas.";
                    return result;
                }

                var NombreAdmin = this.administradorRepository.VerifyNameAdmin(modelDto.Nombre);

                if (!NombreAdmin)
                {
                    result.Success = false;
                    result.Message = "Ya existe un administrador con ese nombre.";
                    return result;
                }

                string PasswordHashed = BCrypt.Net.BCrypt.HashPassword(modelDto.Password);

                this.administradorRepository.Add(new Administradores()
                {

                    Nombre = modelDto.Nombre,
                    Email = modelDto.Email,
                    Telefono = modelDto.Telefono,
                    Tipo = "PropietarioBarberia",
                    Password = PasswordHashed,
                    CreationDate = DateTime.Now,
                    CreationUser = modelDto.ChangeUser,

                });

                this.administradorRepository.SaveChanged();
                result.Message = "Se ha Agregado Correctamente el Administrador.";

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error guardando el administrador.";
                this.logger.LogError($"Ha ocurrido un error guardando el administrador + {ex.Message}.");
            }

            return result;
        }

        public ServiceResult Update(AdminUpdateDto modelDto)
        {
            ServiceResult result = new ServiceResult();

            try
            {

                var AdminExistente = this.administradorRepository.GetById(modelDto.AdministradoresId);

                var AdminName = this.administradorRepository.VerifyNameAdmin(modelDto.Nombre);

                if (!AdminName)
                {
                    result.Success = false;
                    result.Message = "Ya existe un administrador con este nombre.";
                    return result;
                }

                if (!AdministradorValidations.AdministradorUpdateValidations(modelDto))
                {
                    result.Success = false;
                    result.Message = "Los campos para actualizar el administrador NO cumplen con las validaciones establecidas.";
                    return result;
                }

                string PasswordHashed = BCrypt.Net.BCrypt.HashPassword(modelDto.Password);

                AdminExistente.AdministradoresId = modelDto.AdministradoresId;
                AdminExistente.Nombre = modelDto.Nombre;
                AdminExistente.Telefono = modelDto.Telefono;
                AdminExistente.Email = modelDto.Email;
                AdminExistente.Password = PasswordHashed;
                AdminExistente.ModifyDate = DateTime.Now;
                AdminExistente.UserMod = modelDto.ChangeUser;

                this.administradorRepository.Update(AdminExistente);
                this.administradorRepository.SaveChanged();
                result.Message = "Administrador Actualizado Correctamente.";
            }

            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error actualizando el Admin.";
                this.logger.LogError($"Ha ocurrido un error actualizando el Admin: {ex.Message}.");
            }

            return result;
        }

        public ServiceResult Remove(AdminRemoveDto modelDto)
        {
            ServiceResult result = new ServiceResult();

            try
            {

                var adminRemove = this.administradorRepository.GetById(modelDto.AdministradoresId);


                if (!AdministradorValidations.AdministradorRemoveValidations(adminRemove))
                {
                    result.Success = false;
                    result.Message = "Ha ocurrido un error obteniendo el id del administrador.";
                    return result;
                }      
                
                if (!AdministradorValidations.AdministradorRemoveTipoValidations(adminRemove))
                {
                    result.Success = false;
                    result.Message = "No se puede eliminar este tipo de administrador.";
                    return result;
                }

                adminRemove.UserDeleted = modelDto.ChangeUser;

                this.administradorRepository.Remove(adminRemove);
                this.administradorRepository.SaveChanged();
                result.Message = "Administrador Removido Correctamente.";
            }
            catch (Exception ex)
            {

                result.Success = false;
                result.Message = "Ha ocurrido un error eliminando el administrador.";
                this.logger.LogError($"Ha ocurrido un error eliminando el administrador: {ex.Message}.");

            }

            return result;
        }

    }
}
