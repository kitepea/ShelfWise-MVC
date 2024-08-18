using ShelfWise.DataAccess.Data;
using ShelfWise.DataAccess.Repository.IRepository;

namespace ShelfWise.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public ApplicationDbContext _db { get; }
        public ICategoryRepository CategoryRepository { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            CategoryRepository = new CategoryRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
