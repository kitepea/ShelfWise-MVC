using ShelfWise.DataAccess.Data;
using ShelfWise.DataAccess.Repository.IRepository;
using ShelfWise.Models;

namespace ShelfWise.DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationDbContext _db { get; }

        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
