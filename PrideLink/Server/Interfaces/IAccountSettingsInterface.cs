using PrideLink.Shared.AccountSettings;

namespace PrideLink.Server.Interfaces
{
    public interface IAccountSettingsInterface
    {
        public string? CheckLoginCredWithUserNo(int userID, string password);
        public bool UpdatePassword(string password, int userNo);
        public bool UpdateEmail(string email, int userNo);
        public bool IsEmailCorrect(int userNo, string email);
        public List<NotificationSettings> GetNotificationSettings(int userNo);
        public bool GetTwoStepAuthenticationSettings(int userNo);
        public void UpdateNotificationSettings(int userNo, List<NotificationSettings> notificationSettings);
        public void UpdateTwoStepAuthenticationSettings(int userNo, bool isEnabled);
    }
}
