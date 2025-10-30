using PrideLink.Server.Internal_Models;
using PrideLink.Shared.LoginDetails;
using System.Numerics;

namespace PrideLink.Server.Interfaces
{
    public interface ILoginInterface
    {
        public int CheckLoginCred(string userName, string password);
        public int CreateLogin(string userName, string password);
        public bool UpdatePassword(string password, int userNo);
        public List<UserRoles> GetRoles(int userNo);
    }
}
