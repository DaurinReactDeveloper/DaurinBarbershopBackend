using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Application.Dtos.ComentarioDto
{
    public abstract class ComentarioDto : DtoBase
    {

        public int IdComentarios { get; set; }
        public int IdCita { get; set; }
        public int IdCliente { get; set; }
        public int? IdCorte { get; set; }
        public int? IdBarbero { get; set; }
        public int? Calificacion { get; set; }
        public string? Comentario { get; set; }

    }
}
