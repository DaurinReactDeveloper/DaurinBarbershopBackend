using GestorBarberia.Domain.Entities;
using GestorBarberia.Domain.Repository;
using GestorBarberia.Infrastructure.Models;

namespace GestorBarberia.Persistence.Interface
{
    public interface ICitaRepository : IBaseRepository<Citas>
    {
        CitaModel GetCitaById(int citaId);
        List<CitaModel> GetCitaByBarberoId(int barberoId);
        List<CitaModel> GetCitaByClienteId(int clienteId);
        void UpdateEstado(Citas citaUpdate);
        bool VerifyCita(DateTime fecha, TimeSpan hora);

    }
}
