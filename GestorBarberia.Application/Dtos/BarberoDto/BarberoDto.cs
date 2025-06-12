using GestorBarberia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Application.Dtos.BarberoDto
{
    public abstract class BarberoDto : DtoBase
    {

        public int BarberoId { get; set; }
        public int BarberiaId { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }
        public string Imgbarbero { get; set; }

    }
}
