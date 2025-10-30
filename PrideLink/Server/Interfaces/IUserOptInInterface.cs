using PrideLink.Shared.UserOptIn;

namespace PrideLink.Server.Interfaces
{
    public interface IUserOptInInterface
    {
        public bool UserOptIn(UserOptIn userOptIns, int userNo);
    }
}
