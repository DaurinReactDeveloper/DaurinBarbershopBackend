using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Infrastructure.Models
{
    public class ReportesModel
    {

        public string Fecha { get; set; } // Fecha en formato "yyyy-MM-dd"
        public int CitasRealizadas { get; set; } // Total de citas realizadas en ese día
        public decimal IngresoCitaRealizada { get; set; } // Ingreso total de las citas realizadas en ese día
        public string Barbero { get; set; }
        public int CortesRealizados { get; set; }
        public decimal IngresosDelBarbero { get; set; }
        public decimal Comision { get; set; }

    }
}
