using GestorBarberia.Domain.Core;
using System;
using System.Collections.Generic;

namespace GestorBarberia.Domain.Entities
{
    public partial class Administradores : BaseEntity
    {
        public Administradores()
        {
            Barberia = new HashSet<Barberias>();
        }

        public int AdministradoresId { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Tipo { get; set; }

        public virtual ICollection<Barberias> Barberia { get; set; }
    }
}
