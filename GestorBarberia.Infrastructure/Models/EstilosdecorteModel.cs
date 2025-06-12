using System;
using System.Collections.Generic;

namespace GestorBarberia.Infrastructure.Models
{
    public partial class EstilosdecorteModel
    {
        public int EstiloId { get; set; }
        public int BarberiaId { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public decimal? Precio { get; set; }
        public string? Imgestilo { get; set; }

    }
}
