using System.Linq.Expressions;

namespace Booking.API.ViewModel.Interfaces
{
	public interface ISelection<TSource, TDestination>
	{
		Expression<Func<TSource, TDestination>> GetSelection();
	}
}
