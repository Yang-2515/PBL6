using Booking.Domain.Base;
using Booking.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Infrastructure.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
		private readonly ApplicationDbContext _dbContext;
		private DbSet<T> _dbSet;

		protected DbSet<T> DbSet => _dbSet ?? (_dbSet = _dbContext.Set<T>());

		public GenericRepository(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		#region Get an item

		public virtual Task<T> GetAsync(Expression<Func<T, bool>> expression)
		{
			return DbSet.FirstOrDefaultAsync(expression);
		}

		public virtual T Get(Expression<Func<T, bool>> expression)
		{
			return DbSet.FirstOrDefault(expression);
		}

		public virtual Task<List<T>> GetListAsync(Expression<Func<T, bool>> expression)
		{
			return GetQuery(expression).ToListAsync();
		}

		public virtual IQueryable<T> GetQuery()
		{
			return DbSet;
		}

		public virtual Task<List<T>> GetAllAsync()
		{
			return GetQuery().ToListAsync();
		}

		public virtual List<T> GetList(Expression<Func<T, bool>> expression)
		{
			return GetQuery(expression).ToList();
		}

		public virtual List<T> GetAll()
		{
			return GetQuery().ToList();
		}

		#endregion Get an item

		#region Get list of items

		public virtual IQueryable<T> GetQuery(Expression<Func<T, bool>> expression)
		{
			return DbSet.Where(expression);
		}

		#endregion Get list of items

		#region Insert

		public virtual void Insert(T entity)
		{
			DbSet.Add(entity);
		}

		public virtual Task InsertAsync(T entity)
		{
			return DbSet.AddAsync(entity).AsTask();
		}

		public virtual async Task InsertRangeAsync(IEnumerable<T> entities)
		{
			if (entities?.Any() == true)
				await DbSet.AddRangeAsync(entities);
		}

		public virtual void InsertRange(IEnumerable<T> entities)
		{
			if (entities?.Any() == true)
				DbSet.AddRange(entities);
		}

		#endregion Insert

		#region Remove

		public virtual void Remove(T entity)
		{
			DbSet.Remove(entity);
		}

		public virtual void RemoveRange(IEnumerable<T> entities)
		{
			if (entities?.Any() == true)
				DbSet.RemoveRange(entities);
		}

		public virtual Task RemoveAsync(T entity)
		{
			Remove(entity);
			return Task.CompletedTask;
		}

		public virtual Task RemoveRangeAsync(IEnumerable<T> entities)
		{
			RemoveRange(entities);
			return Task.CompletedTask;
		}

		#endregion Remove

		#region Update

		public virtual void Update(T entity)
		{
			DbSet.Update(entity);
		}

		public virtual void UpdateRange(IEnumerable<T> entities)
		{
			if (entities?.Any() == true)
				DbSet.UpdateRange(entities);
		}

		public virtual Task UpdateAsync(T entity)
		{
			Update(entity);
			return Task.CompletedTask;
		}

		public virtual Task UpdateRangeAsync(IEnumerable<T> entities)
		{
			UpdateRange(entities);
			return Task.CompletedTask;
		}

		#endregion Update

		public virtual Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
		{
			return DbSet.AnyAsync(expression);
		}

		public virtual bool Any(Expression<Func<T, bool>> expression)
		{
			return DbSet.Any(expression);
		}
	}
}
