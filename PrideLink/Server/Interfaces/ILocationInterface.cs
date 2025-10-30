using PrideLink.Server.TransLinkDataBase;
using PrideLink.Shared.Location;

namespace PrideLink.Server.Interfaces
{
    public interface ILocationInterface
    {
        public bool AddUpdateUserLocation(UserLocationData location, int userNo);

        public List<int> GetLocation(UserLocationData userLocation);
    }
}
