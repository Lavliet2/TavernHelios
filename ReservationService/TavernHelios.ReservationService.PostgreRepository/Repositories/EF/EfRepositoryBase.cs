using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TavernHelios.ReservationService.ApiCore.Interfaces;
using TavernHelios.ReservationService.APICore.Entities;

namespace TavernHelios.ReservationService.PostgreRepository.Repositories.EF
{
    public abstract class EfRepositoryBase<T> : IRepository<T> where T : class, IEntity
    {
        protected readonly DbContext Context;
        protected readonly DbSet<T> _entitySet;

        protected EfRepositoryBase(DbContext context)
        {
            Context = context;
            _entitySet = Context.Set<T>();
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
           
            var addResult = await _entitySet.AddAsync(entity);
            T result = addResult.Entity;
            Context.SaveChanges();
            return result;
        }

        public virtual async Task<long> DeleteAsync(long entityId)
        {
            long result = -1;
            var existing = await _entitySet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == entityId);

            if (existing != null)
            {
                result = _entitySet.Remove(existing).Entity.Id;
            }
            else
            {
                //Минимальная валидация
                Console.WriteLine($"Error: (Delete) Entity with id {entityId} does not exist");
            }
            Context.SaveChanges();
            return result;
        }

        public virtual async Task<T> GetByIdAsync(long id)
        {
            return await _entitySet.FirstAsync(x => x.Id == id);
        }

        public virtual async Task<IEnumerable<T>> GetByQueryAsync(Func<T, bool> condition)
        {
            IEnumerable<T> query = GetQuerySet();
            if (condition != null)
                query = query.Where(condition);

            return await Task.FromResult(query.ToList());
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            T result = null;
            var existing = await _entitySet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == entity.Id);

            if (existing != null)
            {
                result = _entitySet.Update(entity).Entity;
                Context.SaveChanges();
            }

            else
            {
                //Минимальная валидация
                Console.WriteLine($"Error: (Update) Entity with id {entity.Id} does not exist");
            }
            return result;
        }

        protected virtual IEnumerable<T> GetQuerySet()
        {
            return _entitySet;
        }
    }
}
