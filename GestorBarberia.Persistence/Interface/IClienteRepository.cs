using GestorBarberia.Domain.Entities;
using GestorBarberia.Domain.Repository;
using GestorBarberia.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Persistence.Interface
{
    public interface IClienteRepository : IBaseRepository<Clientes>
    {
        List<ClienteModel> GetClientes();
        List<ClienteModel> GetClientesByBarberiaId(int barberiaId);
        ClienteModel GetClienteName(string name);
        bool VerifyNameCliente(string nameCliente);
        bool VerifyPermissionsCliente(int clienteId, int adminId);
        bool RemoveClienteByAdmin(int clienteId, int adminId);
    }
}
