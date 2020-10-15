using System.Collections.Generic;
using System.Threading.Tasks;
using CourseStudio.Domain.Persistence;
using CourseStudio.Doamin.Models;

namespace CourseStudio.Domain.Repositories
{
    public class RepositoryBase<T>: IRepository<T>
        where T: class, IAggregateRoot
    {
        protected CourseContext _context;

        public RepositoryBase(CourseContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _context;
            }
        }

        public async Task<bool> SaveAsync()
        {
            return await UnitOfWork.SaveEntitiesAsync();
        }

        public async Task CreateAsync<TEntity>(TEntity entity) where TEntity : class
        {
            await _context.AddAsync(entity);
        }

        public async Task CreateRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            await _context.AddRangeAsync(entities);
        }

        public void Remove<TEntity>(TEntity entity) where TEntity : class
        {
            _context.Set<TEntity>().Remove(entity);
        }

		public void RemoveRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
			_context.Set<TEntity>().RemoveRange(entities);
        }

        public void Update<TEntity>(TEntity original, TEntity modified) where TEntity : class
        {
            _context.Entry(original).CurrentValues.SetValues(modified);
        }
    }
}
