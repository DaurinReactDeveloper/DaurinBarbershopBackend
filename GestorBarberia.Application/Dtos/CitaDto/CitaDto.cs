using GestorBarberia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Application.Dtos.CitaDto
{
    public abstract class CitaDto : DtoBase
    {
        public int CitaId { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public int BarberoId { get; set; }
        public int BarberiaId { get; set; }
        public int ClienteId { get; set; }
        public int EstiloId { get; set; }
        public string Estado { get; set; } = null!;
    }
}
