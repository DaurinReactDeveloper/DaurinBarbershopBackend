using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Domain.Repository
{
    public interface IBaseRepository<Entity> where Entity : class
    {
        List<Entity> GetEntities();
        Entity GetById(int? id);
        void Add(Entity entity);
        void Update(Entity entity);
        void Remove(Entity entity);
        void SaveChanged();
    }
}
