using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CourseStudio.Doamin.Models;
using CourseStudio.Domain.Persistence;

namespace CourseStudio.Domain.Repositories
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }

        Task<bool> SaveAsync();

        // TODO: ALL TEntity should be the same as T
        Task CreateAsync<TEntity>(TEntity entity) where TEntity : class;
        Task CreateRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
		void Remove<TEntity>(TEntity entity) where TEntity : class;
		void RemoveRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        void Update<TEntity>(TEntity original, TEntity modified) where TEntity : class;
    }
}
