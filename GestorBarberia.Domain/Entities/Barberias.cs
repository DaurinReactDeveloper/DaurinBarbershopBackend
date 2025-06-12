using GestorBarberia.Domain.Core;
using System;
using System.Collections.Generic;

namespace GestorBarberia.Domain.Entities
{
    public partial class Barberias : BaseEntity
    {
        public Barberias()
        {

            Barberos = new HashSet<Barberos>();
            Clientes = new HashSet<Clientes>();
            Estilosdecortes = new HashSet<Estilosdecortes>();
            Cita = new HashSet<Citas>();

        }

        public int BarberiasId { get; set; }
        public string? NombreBarberia { get; set; }
        public int? Admin { get; set; }

        public virtual Administradores? AdminNavigation { get; set; }
        public virtual ICollection<Barberos> Barberos { get; set; }
        public virtual ICollection<Clientes> Clientes { get; set; }
        public virtual ICollection<Estilosdecortes> Estilosdecortes { get; set; }
        public virtual ICollection<Citas> Cita { get; set; }

    }
}
