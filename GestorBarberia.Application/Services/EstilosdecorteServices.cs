using GestorBarberia.Application.Contract;
using GestorBarberia.Application.Core;
using GestorBarberia.Application.Dtos.EstilodecorteDto;
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
    public class EstilosdecorteServices : IEstilosdecorteServices
    {

        private readonly IEstilosdecorteRepository estilosRepository;
        private readonly IBarberiasRepository barberiaRepository;
        private readonly ILogger<EstilosdecorteServices> logger;

        public EstilosdecorteServices(IEstilosdecorteRepository estilosRepository, ILogger<EstilosdecorteServices> logger, IBarberiasRepository barberiaRepository)
        {
            this.estilosRepository = estilosRepository;
            this.logger = logger;
            this.barberiaRepository = barberiaRepository;
        }

        public ServiceResult GetEstilos()
        {
            ServiceResult result = new ServiceResult();

            try
            {
                var estilosdecortes = this.estilosRepository.GetEstilosdecorte();

                if (!EstilosdecorteValidations.ValidationsCountCortes(estilosdecortes)) {

                    result.Success = false;
                    result.Message = "No hay estilos de cortes";
                    return result;
                
                }

                result.Data = estilosdecortes;
                result.Message = "Estilos de Corte Obtenidos Correctamente.";

            }
            catch (Exception ex)
            {

                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo los estilos de corte.";
                this.logger.LogError($"Ha ocurrido un error obteniendo los estilos de corte: {ex.Message}.");

            }

            return result;
        }

        public ServiceResult GetEstilosById(int id)
        {
            ServiceResult result = new ServiceResult();

            try
            {

                var estiloid = this.estilosRepository.GetById(id);

                if (!EstilosdecorteValidations.ValidationidCortes(estiloid))
                {

                    result.Success = false;
                    result.Message = "No se pudo obtener el id del estilo de corte.";
                    return result;

                }

                result.Data = estiloid;
                result.Message = "Se ha obtenido correctamente el estilo de corte.";

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo el estilo de corte.";
                this.logger.LogError($"Ha Ocurrido un error obteniendo el estilo de corte: {ex.Message}.");
            }

            return result;

        }

        public ServiceResult GetEstilosbyBarberiaId(int barberiaId)
        {
            ServiceResult result = new ServiceResult();

            var BarberoByAdminId = this.barberiaRepository.GetBarberiaByAdminId(barberiaId);

            try
            {
                var estilos = this.estilosRepository.GetEstilosdecorteByBarberiaId(BarberoByAdminId.BarberiasId);
                result.Data = estilos;
                result.Message = "Estilos Obtenidos Correctamente.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo los estilos.";
                this.logger.LogError($"Ha ocurrido un error obteniendo los estilos: {ex.Message}.");
            }

            return result;
        }

        public ServiceResult GetEstilosbyCliente(int clienteBarberiaId)
        {
            ServiceResult result = new ServiceResult();


            try
            {
                var estilos = this.estilosRepository.GetEstilosdecorteByBarberiaId(clienteBarberiaId);
                result.Data = estilos;
                result.Message = "Estilos Obtenidos Correctamente.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo los estilos.";
                this.logger.LogError($"Ha ocurrido un error obteniendo los estilos: {ex.Message}.");
            }

            return result;
        }

        public ServiceResult Add(EstilosdecorteAddDto modelDto)
        {
            ServiceResult result = new ServiceResult();

            try
            {

                if (!EstilosdecorteValidations.ValidationAddCortes(modelDto))
                {
                    result.Success = false;
                    result.Message = "Los campos para agregar un estilo de corte NO cumplen con las validaciones establecidas.";
                    return result;

                }

                var barberiaId = this.barberiaRepository.GetBarberiaByAdminId(modelDto.ChangeUser);


                this.estilosRepository.Add(new Estilosdecortes()
                {

                    EstiloId = modelDto.EstiloId,
                    BarberiaId = barberiaId.BarberiasId,
                    Descripcion = modelDto.Descripcion,
                    Nombre = modelDto.Nombre,
                    Precio = modelDto.Precio,
                    Imgestilo = modelDto.Imgestilo,
                    CreationDate = DateTime.Now,
                    CreationUser = modelDto.ChangeUser,

                });

                this.estilosRepository.SaveChanged();
                result.Message = "Estilo de Corte Guardado Correctamente.";
            }

            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error guardando el estilo de corte.";
                this.logger.LogError($"Ha ocurrido un error guardando el estilo de corte: {ex.Message}.");
            }

            return result;
        }

        public ServiceResult Update(EstilosdecorteUpdateDto modelDto)
        {
            ServiceResult result = new ServiceResult();

            try
            {
                var estiloUpdate = this.estilosRepository.GetById(modelDto.EstiloId);


                if (!EstilosdecorteValidations.ValidationUpdateCortes(modelDto))
                {
                    result.Success = false;
                    result.Message = "Los campos para actualizar un estilo de corte NO cumplen con las validaciones establecidas.";
                    return result;

                }

                estiloUpdate.Nombre = modelDto.Nombre;
                estiloUpdate.Descripcion = modelDto.Descripcion;
                estiloUpdate.Precio = modelDto.Precio;
                estiloUpdate.EstiloId = modelDto.EstiloId;
                estiloUpdate.Imgestilo = modelDto.Imgestilo;
                estiloUpdate.ModifyDate = DateTime.Now;
                estiloUpdate.UserMod = modelDto.ChangeUser;

                this.estilosRepository.Update(estiloUpdate);
                this.estilosRepository.SaveChanged();
                result.Message = "Estilo de Corte Actualizado Correctamente."; 
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error actualizando el estilo de corte.";
                this.logger.LogError($"Ha ocurrido un error actualizando el estilo de corte: {ex.Message}.");
            }

            return result;

        }

        public ServiceResult Remove(EstilosdecorteRemoveDto modelDto)
        {
            ServiceResult result = new ServiceResult();

            try
            {

                var estiloRemove = this.estilosRepository.GetById(modelDto.EstiloId);

                if (!EstilosdecorteValidations.ValidationRemoveCortes(estiloRemove))
                {

                    result.Success = false;
                    result.Message = "Ha ocurrido un error obteniendo el id del estilo de corte.";
                    return result;

                }

                estiloRemove.UserDeleted = modelDto.ChangeUser;

                this.estilosRepository.Remove(estiloRemove);
                this.estilosRepository.SaveChanged();
                result.Message = "Estilo de Corte Removido Correctamente.";

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error eliminando el estilo de corte";
                this.logger.LogError($"Ha ocurrido un error eliminando el estilo de corte: {ex.Message}.");
            }

            return result;

        }

        public ServiceResult RemoveEstilosByAdmin(int estilosId, int adminId)
        {
            ServiceResult result = new ServiceResult();

            try
            {
                var RemoveEstilo = this.estilosRepository.RemoveEstilosByAdmin(estilosId, adminId);

                if (!RemoveEstilo)
                {
                    result.Success = false;
                    result.Message = "No tienes permisos para remover este estilo.";
                    return result;

                }

                result.Message = "Estilo Removido Correctamente.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error eliminando el estilo.";
                this.logger.LogError($"Ha ocurrido un error eliminando el estilo: {ex.Message}.");
            }

            return result;
        }

    }
}
