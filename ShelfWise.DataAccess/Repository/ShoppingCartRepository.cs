using ShelfWise.DataAccess.Data;
using ShelfWise.DataAccess.Repository.IRepository;
using ShelfWise.Models;

namespace ShelfWise.DataAccess.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        public ApplicationDbContext _db { get; }

        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ShoppingCart obj)
        {
            _db.ShoppingCarts.Update(obj);
        }
    }
}
