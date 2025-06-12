using GestorBarberia.Application.Contract;
using GestorBarberia.Application.Core;
using GestorBarberia.Application.Dtos.ReporteriaDto;
using GestorBarberia.Application.Validations;
using GestorBarberia.Persistence.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Application.Services
{
    public class ReportesServices : IReporteriaServices
    {

        private readonly IReportesRepository reportesRepository;
        private readonly ILogger<ReportesServices> logger;
        private readonly IBarberiasRepository barberiasRepository;
        public ReportesServices(IReportesRepository reportesRepository, ILogger<ReportesServices> logger, IBarberiasRepository barberiasRepository)
        {

            this.reportesRepository = reportesRepository;
            this.logger = logger;
            this.barberiasRepository = barberiasRepository;
        }

        public ServiceResult GenerarTablaIngresos(int barberiaId, DateTime fechaInicio, DateTime fechaFin)
        {
            ServiceResult result = new ServiceResult();


            var idBarberia = this.barberiasRepository.GetBarberiaByAdminId(barberiaId);

            try
            {

                if (!ReportesValidations.ValidationsBarberiaIdReportes(idBarberia.BarberiasId))
                {

                    result.Success = false;
                    result.Message = "La barbería especificada no es válida o no existe. Por favor, verifique el ID de la barbería.";
                    return result;

                }

                var Reportes = this.reportesRepository.GenerarTablaIngresos(idBarberia.BarberiasId, fechaInicio, fechaFin);


                if (!ReportesValidations.ValidationsCountReportes(Reportes))
                {

                    result.Success = false;
                    result.Message = "No se encontraron citas realizadas en el rango de fechas especificado para la barbería.";
                    return result;

                }

                result.Data = Reportes;
                result.Message = "Los reportes de ingresos fueron generados y obtenidos correctamente.";

            }

            catch (Exception ex)
            {

                result.Success = false;
                result.Message = "Ha ocurrido un error obtenidos los reportes.";
                this.logger.LogError($"Ha ocurrido un error obtenidos los reportes: {ex.Message}.");

            }

            return result;
        }

        public ServiceResult ObtenerTotalIngresos(int barberiaId)
        {
            ServiceResult result = new ServiceResult();

            try
            {

                if (!ReportesValidations.ValidationsBarberiaIdReportes(barberiaId))
                {

                    result.Success = false;
                    result.Message = "La barbería especificada no es válida o no existe. Por favor, verifique el ID de la barbería.";
                    return result;

                }

                var BarberoByAdminId = this.barberiasRepository.GetBarberiaByAdminId(barberiaId);

                var Reportes = this.reportesRepository.ObtenerTotalIngresos(BarberoByAdminId.BarberiasId);

                result.Data = Reportes;
                result.Message = "El reporte de total de ingresos fue generado y obtenido correctamente.";

            }

            catch (Exception ex)
            {

                result.Success = false;
                result.Message = "Ha ocurrido un error obtenidos el total de ingresos.";
                this.logger.LogError($"Ha ocurrido un error obtenidos el total de ingresos: {ex.Message}.");

            }

            return result;
        }

        public ServiceResult ObtenerTotalBarberos(int barberiaId)
        {
            ServiceResult result = new ServiceResult();

            try
            {

                if (!ReportesValidations.ValidationsBarberiaIdReportes(barberiaId))
                {

                    result.Success = false;
                    result.Message = "La barbería especificada no es válida o no existe. Por favor, verifique el ID de la barbería.";
                    return result;

                }

                var BarberoByAdminId = this.barberiasRepository.GetBarberiaByAdminId(barberiaId);

                var Reportes = this.reportesRepository.ObtenerTotalBarberos(BarberoByAdminId.BarberiasId);

                result.Data = Reportes;
                result.Message = "El reporte de total de barbero fue generado y obtenido correctamente.";

            }

            catch (Exception ex)
            {

                result.Success = false;
                result.Message = "Ha ocurrido un error obtenidos el total de barberos.";
                this.logger.LogError($"Ha ocurrido un error obtenidos el total de barberos: {ex.Message}.");

            }

            return result;
        }

        public ServiceResult ObtenerTotalClientes(int barberiaId)
        {
            ServiceResult result = new ServiceResult();

            try
            {

                if (!ReportesValidations.ValidationsBarberiaIdReportes(barberiaId))
                {

                    result.Success = false;
                    result.Message = "La barbería especificada no es válida o no existe. Por favor, verifique el ID de la barbería.";
                    return result;

                }

                var BarberoByAdminId = this.barberiasRepository.GetBarberiaByAdminId(barberiaId);

                var Reportes = this.reportesRepository.ObtenerTotalClientes(BarberoByAdminId.BarberiasId);

                result.Data = Reportes;
                result.Message = "El reporte de total de clientes fue generado y obtenido correctamente.";

            }

            catch (Exception ex)
            {

                result.Success = false;
                result.Message = "Ha ocurrido un error obtenidos el total de clientes.";
                this.logger.LogError($"Ha ocurrido un error obtenidos el total de clientes: {ex.Message}.");

            }

            return result;
        }


    }
}
