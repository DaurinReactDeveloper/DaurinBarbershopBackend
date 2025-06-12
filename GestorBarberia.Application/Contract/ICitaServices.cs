using GestorBarberia.Application.Core;
using GestorBarberia.Application.Dtos.CitaDto;
using GestorBarberia.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Application.Contract
{
    public interface ICitaServices : IBaseServices<CitaAddDto, CitaRemoveDto, CitaUpdateDto>
    {
        ServiceResult GetCitaById(int citaId);
        ServiceResult GetCitaByBarberoId(int barberoId);
        ServiceResult GetCitaByClienteId(int clienteId);
        ServiceResult UpdateEstado(CitaUpdateDto updateEstado);

    }
}
