using GestorBarberia.Domain.Core;
using System;
using System.Collections.Generic;

namespace GestorBarberia.Domain.Entities
{
    public partial class Comentarios : BaseEntity
    {
        public int IdComentarios { get; set; }
        public int? IdCita { get; set; }
        public int? IdCliente { get; set; }
        public int? IdCorte { get; set; }
        public int? IdBarbero { get; set; }
        public int? Calificacion { get; set; }
        public string? Comentario { get; set; }

        public virtual Barberos? IdBarberoNavigation { get; set; }
        public virtual Citas? IdCitaNavigation { get; set; }
        public virtual Clientes? IdClienteNavigation { get; set; }
        public virtual Estilosdecortes? IdCorteNavigation { get; set; }
    }
}
