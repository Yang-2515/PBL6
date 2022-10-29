using System.Linq.Expressions;

namespace Booking.API.ViewModel.Interfaces
{
	public interface IFilter<TEntity>
	{
		Expression<Func<TEntity, bool>> GetFilter();
	}
}
