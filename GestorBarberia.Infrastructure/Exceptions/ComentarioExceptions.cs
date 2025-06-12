using GestorBarberia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Infrastructure.Exceptions
{
    public class ComentarioExceptions : Exception
    {

        public ComentarioExceptions(string message) : base(message)
        {

        }

    }
}
