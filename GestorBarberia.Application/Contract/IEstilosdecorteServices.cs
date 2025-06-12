using GestorBarberia.Application.Core;
using GestorBarberia.Application.Dtos.EstilodecorteDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Application.Contract
{
    public interface IEstilosdecorteServices : IBaseServices<EstilosdecorteAddDto, EstilosdecorteRemoveDto, EstilosdecorteUpdateDto>
    {
        ServiceResult GetEstilos();
        ServiceResult GetEstilosById(int id);
        ServiceResult GetEstilosbyBarberiaId(int barberiaId);
        ServiceResult GetEstilosbyCliente(int clienteBarberiaId);
        ServiceResult RemoveEstilosByAdmin(int estilosId, int adminId);

    }
}
