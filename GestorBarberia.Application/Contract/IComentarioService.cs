using GestorBarberia.Application.Core;
using GestorBarberia.Application.Dtos.ComentarioDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Application.Contract
{
    public interface IComentarioService: IBaseServices<ComentarioaddDto,ComentarioRemoveDto,ComentarioUpdateDto>
    {

        ServiceResult GetComentarios();
        ServiceResult GetComentsByBarbero(int barberoId);
        ServiceResult GetComentsByCliente(int clienteId);

    }
}
