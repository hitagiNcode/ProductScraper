using ProductScarper.DataAccess.Repository;
using ProductScraper.DataAccess.Repository.IRepository;
using ProductScraper.Domain;

namespace ProductScraper.DataAccess.Repository
{
    public class TrackingUserRepository : Repository<TrackingUser>, ITrackingUserRepository
    {
        private AppDbContext _db;

        public TrackingUserRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(TrackingUser obj)
        {
            _db.TrackingUsers.Update(obj);
        }

    }
}
