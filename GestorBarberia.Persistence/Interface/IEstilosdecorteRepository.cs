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
    public interface IEstilosdecorteRepository : IBaseRepository<Estilosdecortes>
    {

        List<EstilosdecorteModel> GetEstilosdecorte();
        List<EstilosdecorteModel> GetEstilosdecorteByBarberiaId(int barberiaId);
        bool VerifyPermissionsEstilos(int estiloId, int adminId);
        bool RemoveEstilosByAdmin(int estiloId, int adminId);
    }
}
