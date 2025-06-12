using GestorBarberia.Domain.Core;
using System;
using System.Collections.Generic;

namespace GestorBarberia.Domain.Entities
{
    public partial class Barberos : BaseEntity
    {
        public Barberos()
        {
            Cita = new HashSet<Citas>();
            Comentarios = new HashSet<Comentarios>();
        }

        public int BarberoId { get; set; }
        public int? BarberiaId { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Imgbarbero { get; set; }

        public virtual Barberias? Barberia { get; set; }
        public virtual ICollection<Citas> Cita { get; set; }
        public virtual ICollection<Comentarios> Comentarios { get; set; }
    }
}
