using GestorBarberia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Application.Dtos.EstilodecorteDto
{
    public abstract class EstilosdecorteDto : DtoBase
    {

        public int EstiloId { get; set; }
        public int BarberiaId { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public decimal? Precio { get; set; }
        public string Imgestilo { get; set; }

    }
}
