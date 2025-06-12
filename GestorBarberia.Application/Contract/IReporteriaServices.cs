using GestorBarberia.Application.Core;
using GestorBarberia.Application.Dtos;
using GestorBarberia.Application.Dtos.ReporteriaDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Application.Contract
{
    public interface IReporteriaServices
    {

        ServiceResult GenerarTablaIngresos(int barberiaId, DateTime fechaInicio, DateTime fechaFin);
        ServiceResult ObtenerTotalIngresos(int barberiaId);
        ServiceResult ObtenerTotalBarberos(int barberiaId);
        ServiceResult ObtenerTotalClientes(int barberiaId);


    }
}
