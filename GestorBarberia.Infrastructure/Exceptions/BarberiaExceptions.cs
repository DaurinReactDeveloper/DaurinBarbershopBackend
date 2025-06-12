using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Infrastructure.Exceptions
{
    public class BarberiaExceptions : Exception
    {

        public BarberiaExceptions(string message) : base(message)
        {
            

        }

    }
}
