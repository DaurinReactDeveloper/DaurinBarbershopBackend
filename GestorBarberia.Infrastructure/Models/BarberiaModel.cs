using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Infrastructure.Models
{
    public class BarberiaModel
    {
        public int BarberiasId { get; set; }
        public string? NombreBarberia { get; set; }
        public int? Admin { get; set; }

    }
}