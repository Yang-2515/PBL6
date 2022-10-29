using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
		//Async
		Task InsertAsync(T entity);

		Task InsertRangeAsync(IEnumerable<T> entities);

		Task RemoveAsync(T entity);

		Task RemoveRangeAsync(IEnumerable<T> entities);

		Task UpdateAsync(T entity);

		Task UpdateRangeAsync(IEnumerable<T> entities);

		Task<T> GetAsync(Expression<Func<T, bool>> expression);

		Task<List<T>> GetListAsync(Expression<Func<T, bool>> expression);

		Task<List<T>> GetAllAsync();

		Task<bool> AnyAsync(Expression<Func<T, bool>> expression);

		//Sync
		void Insert(T entity);

		void InsertRange(IEnumerable<T> entities);

		void Remove(T entity);

		void RemoveRange(IEnumerable<T> entities);

		void Update(T entity);

		void UpdateRange(IEnumerable<T> entities);

		T Get(Expression<Func<T, bool>> expression);

		List<T> GetList(Expression<Func<T, bool>> expression);

		List<T> GetAll();

		bool Any(Expression<Func<T, bool>> expression);

		IQueryable<T> GetQuery(Expression<Func<T, bool>> expression);

		IQueryable<T> GetQuery();
	}
}
