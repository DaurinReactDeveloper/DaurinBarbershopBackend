using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Infrastructure.Models
{
    public class EmailModel
    {
        public string NombreCliente { get; set; }
        public string Password { get; set; }
        public string NombreBarbero { get; set; }
        public string NombreBarberia { get; set; }
        public string NombreAdminSucursal { get; set; }
        public string Estado { get; set; }
        public string FechaCita { get; set; }
        public string HoraCita { get; set; }
    }
}
