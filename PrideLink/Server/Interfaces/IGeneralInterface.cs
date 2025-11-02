using PrideLink.Shared.General;

namespace PrideLink.Server.Interfaces
{
    public interface IGeneralInterface
    {
        public List<Hobbys> GetHobbies();
        public string UserVerificationStatus(int userNo);
        public void UpdateVerificationStatus(int userNo, string UserTypeName);
    }
}
