using PrideLink.Server.Internal_Models;
using PrideLink.Shared.LoginDetails;
using System.Numerics;

namespace PrideLink.Server.Interfaces
{
    public interface ILoginInterface
    {
        public string? CheckLoginCred(string userName, string password);
        public int CreateLogin(string userName, string password);
        public List<UserRoles> GetRoles(string userID);
    }
}
