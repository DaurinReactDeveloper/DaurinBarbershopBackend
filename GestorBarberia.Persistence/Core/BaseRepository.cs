using GestorBarberia.Domain.Repository;
using GestorBarberia.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Persistence.Core
{
    public abstract class BaseRepository<Entity> : IBaseRepository<Entity> where Entity : class
    {
        //Base de Datos
        private readonly DbContextBarberia dbContextBarberia;
        
        //Dbset - te ayudara a realizar el CRUDBasico
        private readonly DbSet<Entity> Dbset;

        public BaseRepository(DbContextBarberia dbContextBarberia)
        {
            this.dbContextBarberia = dbContextBarberia;
            this.Dbset = this.dbContextBarberia.Set<Entity>();
        }

        public virtual void Add(Entity entity)
        {
            this.Dbset.Add(entity);
        }

        public virtual Entity GetById(int? id)
        {
            return this.Dbset.Find(id);
        }

        public virtual List<Entity> GetEntities()
        {
            return Dbset.ToList();
        }

        public virtual void Update(Entity entity)
        {
            this.Dbset.Update(entity);
        }

        public virtual void Remove(Entity entity)
        {
            this.Dbset.Remove(entity);
        }

        public virtual void SaveChanged()
        {
            this.dbContextBarberia.SaveChanges();
        }

    }
}
