using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Infrastructure.Exceptions
{
    public class BarberoExceptions : Exception
    {
        public BarberoExceptions(string message) : base(message)
        {
            
        }
    }
}
