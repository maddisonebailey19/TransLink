using PrideLink.Server.TransLinkDataBase;
using PrideLink.Shared.Location;

namespace PrideLink.Server.Interfaces
{
    public interface ILocationInterface
    {
        public bool AddUpdateUserLocation(UserLocationData location, int userNo);
        public List<VWUserFriendFinderProfile> GetUsersFromLocation(int userNo, UserLocationData userLocation, List<string> roles);
        public List<VWUserFriendFinderProfile> GetUserFriendsProfiles(int userNo);
        public List<VWUserHobbies> GetUserFriendsHobbies(int userNo);
        public List<TownCityLocations> GetTownCityLocations();
        public List<VWUserHobbies> GetUserHobbies(UserLocationData userLocation, List<string> roles);
        public bool AddTownAndCityToUser(int userNo, int cityNo);
        public UserLocationData GetUserLocationFromTownAndCity(int userNo);
    }
}
