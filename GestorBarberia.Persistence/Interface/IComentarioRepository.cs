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
    public interface IComentarioRepository : IBaseRepository<Comentarios>
    {

        List<ComentarioModel> GetComentarios();
        List<ComentarioModel> GetComentsByBarberoId(int id);
        List<ComentarioModel> GetComentsByClienteId(int id);

    }
}
