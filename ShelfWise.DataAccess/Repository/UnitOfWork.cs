using ShelfWise.DataAccess.Data;
using ShelfWise.DataAccess.Repository.IRepository;

namespace ShelfWise.DataAccess.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		public ApplicationDbContext _db { get; }
		public ICategoryRepository Category { get; private set; }
		public IProductRepository Product { get; private set; }
		public ICompanyRepository Company { get; private set; }
		public IShoppingCartRepository ShoppingCart { get; private set; }
		public IApplicationUserRepository ApplicationUser { get; private set; }
		public IOrderHeaderRepository OrderHeader { get; }
		public IOrderDetailRepository OrderDetails { get; }

		public UnitOfWork(ApplicationDbContext db)
		{
			_db = db;
			Category = new CategoryRepository(_db);
			Product = new ProductRepository(_db);
			Company = new CompanyRepository(_db);
			ShoppingCart = new ShoppingCartRepository(_db);
			ApplicationUser = new ApplicationUserRepository(_db);
			OrderHeader = new OrderHeaderRepository(_db);
			OrderDetails = new OrderDetailRepository(_db);
		}

		public void Save()
		{
			_db.SaveChanges();
		}
	}
}
