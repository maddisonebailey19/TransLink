using PrideLink.Server.Controllers;
using PrideLink.Shared.FreindFinderDetails;
using PrideLink.Shared.Location;

namespace PrideLink.Server.Interfaces
{
    public interface IFreindFinderUserInfoInterface
    {
        public List<UserFreindFinderAccount> GetAllUserFreindFinderAccounts(UserLocationData location);
    }
}
