using ShelfWise.Models;

namespace ShelfWise.DataAccess.Repository.IRepository
{
	public interface IOrderHeaderRepository : IRepository<OrderHeader>
	{
		void Update(OrderHeader obj);
	}
}
