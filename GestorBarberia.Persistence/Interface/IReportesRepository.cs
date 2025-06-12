using GestorBarberia.Domain.Repository;
using GestorBarberia.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Persistence.Interface
{
    public interface IReportesRepository
    {

        List<ReportesModel> GenerarTablaIngresos(int barberiaId,DateTime fechaInicio, DateTime fechaFin);
        decimal? ObtenerTotalIngresos(int barberiaId);
        int ObtenerTotalClientes(int barberiaId);
        int ObtenerTotalBarberos(int barberiaId);

    }
}
