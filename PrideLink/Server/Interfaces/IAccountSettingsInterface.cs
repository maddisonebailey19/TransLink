namespace PrideLink.Server.Interfaces
{
    public interface IAccountSettingsInterface
    {
        public string? CheckLoginCredWithUserNo(int userID, string password);
        public bool UpdatePassword(string password, int userNo);
        public bool UpdateEmail(string email, int userNo);
        public bool IsEmailCorrect(int userNo, string email);
    }
}
