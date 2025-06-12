using GestorBarberia.Application.Core;
using GestorBarberia.Application.Dtos.BarberiaDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Application.Contract
{
    public interface IBarberiaServices : IBaseServices<BarberiaAddDto, BarberiaRemoveDto,BarberiaUpdateDto>
    {

        ServiceResult GetBarberias();
        ServiceResult GetBarberiaById(int barberiaId);

    }
}
