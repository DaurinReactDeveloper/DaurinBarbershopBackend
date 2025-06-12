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
    public interface IBarberiasRepository : IBaseRepository<Barberias>
    {

        List<BarberiaModel> GetBarberias();
        BarberiaModel GetBarberiaByAdminId(int adminId);
        BarberiaModel GetBarberiaById(int barberiaId);

    }
}
