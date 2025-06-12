using GestorBarberia.Domain.Entities;
using GestorBarberia.Domain.Repository;
using GestorBarberia.Infrastructure.Models;
using GestorBarberia.Persistence.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Persistence.Interface
{
    public interface IBarberoRepository : IBaseRepository<Barberos>
    {

        List<BarberoModel> GetBarberos();
        List<BarberoModel> GetBarberosByBarberiaId(int barberiaId);
        BarberoModel GetBarberoName(string name);
        bool VerifyNameBarbero(string nameBarbero);
        bool VerifyPermissionsBarbero(int barberoId, int adminId);
        bool RemoveBarberoByAdmin(int barberoId, int adminId);

    }
}
