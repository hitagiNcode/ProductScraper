using ProductScarper.DataAccess.Repository.IRepository;
using ProductScraper.Domain;

namespace ProductScraper.DataAccess.Repository.IRepository
{
    public interface ITrackingUserRepository : IRepository<TrackingUser>
    {
        void Update(TrackingUser obj);
    }
}
