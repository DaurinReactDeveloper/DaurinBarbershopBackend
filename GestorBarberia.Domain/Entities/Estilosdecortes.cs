using GestorBarberia.Domain.Core;
using System;
using System.Collections.Generic;

namespace GestorBarberia.Domain.Entities
{
    public partial class Estilosdecortes : BaseEntity
    {
        public Estilosdecortes()
        {
            Cita = new HashSet<Citas>();
            Comentarios = new HashSet<Comentarios>();
        }

        public int EstiloId { get; set; }
        public int BarberiaId { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public decimal? Precio { get; set; }
        public string? Imgestilo { get; set; }

        public virtual Barberias? Barberia { get; set; }
        public virtual ICollection<Citas> Cita { get; set; }
        public virtual ICollection<Comentarios> Comentarios { get; set; }
    }
}
