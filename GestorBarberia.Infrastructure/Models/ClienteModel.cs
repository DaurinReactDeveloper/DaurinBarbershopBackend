using System;
using System.Collections.Generic;

namespace GestorBarberia.Infrastructure.Models
{
    public partial class ClienteModel
    {

        public int ClienteId { get; set; }
        public int? BarberiaId { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Imgcliente { get; set; }

    }
}
