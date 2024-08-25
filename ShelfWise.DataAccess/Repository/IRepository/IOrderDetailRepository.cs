using ShelfWise.Models;

namespace ShelfWise.DataAccess.Repository.IRepository
{
	public interface IOrderDetailRepository : IRepository<OrderDetail>
	{
		void Update(OrderDetail obj);
	}
}
