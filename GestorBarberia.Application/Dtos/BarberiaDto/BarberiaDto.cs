using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Application.Dtos.BarberiaDto
{
    public abstract class BarberiaDto : DtoBase
    {
        public int BarberiasId { get; set; }
        public string? NombreBarberia { get; set; }
        public int Admin { get; set; }

    }
}
