using Microsoft.EntityFrameworkCore;
using ShelfWise.DataAccess.Data;
using ShelfWise.DataAccess.Repository.IRepository;
using System.Linq.Expressions;

namespace ShelfWise.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
            // _db.Categories == dbSet
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            IQueryable<T> query;
            query = tracked ? dbSet : dbSet.AsNoTracking();

            query = query.Where(filter);
            if (!String.IsNullOrEmpty(includeProperties))
            {
                foreach (var incluseProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluseProp);
                }
            }
            return query.FirstOrDefault();
        }

        // Category, OtherType
        public IEnumerable<T> GetAll(string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (!String.IsNullOrEmpty(includeProperties))
            {
                foreach (var incluseProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluseProp);
                }
            }
            return [.. query];
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }
}
