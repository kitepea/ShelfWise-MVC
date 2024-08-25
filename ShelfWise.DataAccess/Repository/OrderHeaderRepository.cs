using ShelfWise.DataAccess.Data;
using ShelfWise.DataAccess.Repository.IRepository;
using ShelfWise.Models;

namespace ShelfWise.DataAccess.Repository
{
	public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
	{
		public ApplicationDbContext _db { get; }

		public OrderHeaderRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Update(OrderHeader obj)
		{
			_db.OrderHeaders.Update(obj);
		}
	}
}
