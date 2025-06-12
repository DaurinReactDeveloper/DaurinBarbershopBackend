using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Application.Dtos.AdminDto
{
    public abstract class AdminDto : DtoBase
    {

        public int AdministradoresId { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string Tipo { get; set; } = null!;

    }
}
