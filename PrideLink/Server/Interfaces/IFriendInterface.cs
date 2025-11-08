using PrideLink.Shared.FreindFinderDetails;
using PrideLink.Shared.Notification;

namespace PrideLink.Server.Interfaces
{
    public interface IFriendInterface
    {
        public NotificationContent AddFriend(int userNo, int friendUserNo);
        public bool RemoveFriend(int userNo, int friendUserNo);
        public NotificationContent AcceptFriendRequest(int userNo, int friendUserNo);
        public bool DeclineFriendRequest(int userNo, int friendUserNo);
        public bool BlockUser(int userNo, int blockedUserNo);
        public List<UserFreindFinderAccount?> GetAllUserFriends(int userNo);
    }
}
