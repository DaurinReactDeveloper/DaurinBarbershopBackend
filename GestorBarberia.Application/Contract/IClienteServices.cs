using GestorBarberia.Application.Core;
using GestorBarberia.Application.Dtos.ClienteDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Application.Contract
{
    public interface IClienteServices : IBaseServices<ClienteAddDto, ClienteRemoveDto, ClienteUpdateDto>
    {

        ServiceResult GetClientes();
        ServiceResult GetCliente(string name, string password);
        ServiceResult GetClientesbyBarberiaId(int barberiaId);
        ServiceResult GetClienteById(int id);
        ServiceResult RemoveClienteByAdmin(int clienteId, int adminId);

    }
}
        