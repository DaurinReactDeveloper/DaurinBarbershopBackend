using GestorBarberia.Infrastructure.Models;
using GestorBarberia.Persistence.Context;
using GestorBarberia.Persistence.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Persistence.Repositories
{
    public class ReportesRepository : IReportesRepository
    {

        private readonly DbContextBarberia dbContextBarberia;
        private readonly ILogger<ReportesRepository> logger;

        public ReportesRepository(DbContextBarberia dbContext, ILogger<ReportesRepository> logger)
        {
            this.dbContextBarberia = dbContext;
            this.logger = logger;
        }

        public List<ReportesModel> GenerarTablaIngresos(int barberiaId, DateTime fechaInicio, DateTime fechaFin)
        {

            // Obtener citas realizadas en el rango de fechas para la barbería específica
            var citas = dbContextBarberia.Citas
                .Where(c => c.Fecha >= fechaInicio && c.Fecha <= fechaFin
                            && c.Estado == "Realizada"
                            && c.BarberiaId == barberiaId)
                .Include(c => c.Barbero)
                .Include(c => c.Estilo)
                .ToList();

            // Agrupar por fecha y luego por barbero, calculando métricas para cada grupo
            var resultado = citas
                .GroupBy(c => c.Fecha)
                .Select(g => new ReportesModel
                {
                    Fecha = g.Key.ToString("yyyy-MM-dd"),
                    CitasRealizadas = g.Count(),
                    IngresoCitaRealizada = g.Sum(c => c.Estilo?.Precio ?? 0),

                    Barbero = g.First().Barbero.Nombre,
                    CortesRealizados = g.Count(),
                    IngresosDelBarbero = g.Sum(c => c.Estilo?.Precio ?? 0),
                    Comision = g.Sum(c => (c.Estilo?.Precio ?? 0) * 0.3m)
                })
                .ToList();

            return resultado;

        }

        public decimal? ObtenerTotalIngresos(int barberiaId)
        {

            var totalIngreso = (from c in dbContextBarberia.Citas
                                where c.BarberiaId.Equals(barberiaId) && c.Estado == "Realizada"
                                select c.Estilo != null ? c.Estilo.Precio : 0
                               ).Sum();

            return totalIngreso;
        }

        public int ObtenerTotalClientes(int barberiaId)
        {
            var totalClientes = (from c in dbContextBarberia.Clientes
                                 where c.BarberiaId.Equals(barberiaId)
                                 select c.ClienteId).Distinct().Count();

            return totalClientes;
        }

        public int ObtenerTotalBarberos(int barberiaId)
        {

            var totalBarberos = (from c in dbContextBarberia.Barberos
                                 where c.BarberiaId.Equals(barberiaId)
                                 select c.BarberoId).Distinct().Count();

            return totalBarberos;
        }

    }
}
