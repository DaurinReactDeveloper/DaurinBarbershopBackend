using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Application.Core
{
    public interface IBaseAdministrador
    {
        ServiceResult GetAdministrador(string nombre, string password);
    }
}
