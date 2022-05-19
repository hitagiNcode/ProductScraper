using ProductScarper.DataAccess.Repository.IRepository;
using ProductScraper.DataAccess;
using ProductScraper.DataAccess.Repository;
using ProductScraper.DataAccess.Repository.IRepository;

namespace ProductScarper.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppDbContext _db;

        public UnitOfWork(AppDbContext db)
        {
            _db = db;
            Product = new ProductRepository(_db);
            TrackingUser = new TrackingUserRepository(_db);
        }

        public IProductRepository Product { get; private set; }
        public ITrackingUserRepository TrackingUser { get; private set; }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
