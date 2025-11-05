using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PrideLink.Server.Interfaces;
using PrideLink.Server.TransLinkDataBase;
using PrideLink.Shared.General;
using PrideLink.Shared.Location;


namespace PrideLink.Server.Helpers
{
    public class LocationHelper : ILocationInterface
    {
        public bool AddUpdateUserLocation(UserLocationData location, int userNo)
        {
            using(var context = new MasContext())
            {
                var userNoParam = new SqlParameter("@userNo", userNo);
                var LatitudeParam = new SqlParameter("@Latitude", location.Latitude);
                var LongitudeParam = new SqlParameter("@Longitude", location.Longitude);
                context.Database.ExecuteSqlRaw("EXEC spImportLocationData @UserNo, @Latitude, @Longitude",
                userNoParam, LatitudeParam, LongitudeParam);

                return true;
            }
        }

        public List<VWUserHobbies> GetUserHobbies(UserLocationData userLocation, List<string> roles)
        {
            int radisuInMeters = 500000000;

            bool adminFlag = roles != null && roles.Contains("Admin", StringComparer.OrdinalIgnoreCase);

            List<int> userNos = new List<int>();
            var latitudeParam = new SqlParameter("@Latitude", userLocation.Latitude);
            var longitudeParam = new SqlParameter("@Longitude", userLocation.Longitude);
            var radiusParam = new SqlParameter("@RadiusInMeters", radisuInMeters);
            var adminFlagParam = new SqlParameter("@AdminFlag", adminFlag);
            using (var context = new MasContext())
            {
                List<VWUserHobbies> users = context.VWUserHobbies
                .FromSqlRaw("EXEC spGetAllUserHobbiesWithinRange @Latitude, @Longitude, @RadiusInMeters, @AdminFlag", latitudeParam, longitudeParam, radiusParam, adminFlagParam)
                .ToList();

                return users;
            }
        }

        public List<VWUserFriendFinderProfile> GetUsersFromLocation(UserLocationData userLocation, List<string> roles)
        {
            int radisuInMeters = 500000000;

            bool adminFlag = roles != null && roles.Contains("Admin", StringComparer.OrdinalIgnoreCase);

            List<int> userNos = new List<int>();    
            var latitudeParam = new SqlParameter("@Latitude", userLocation.Latitude);
            var longitudeParam = new SqlParameter("@Longitude", userLocation.Longitude);
            var radiusParam = new SqlParameter("@RadiusInMeters", radisuInMeters);
            var adminFlagParam = new SqlParameter("@AdminFlag", adminFlag);
            using (var context = new MasContext())
            {
                List<VWUserFriendFinderProfile> users = context.VWUserFriendFinderProfile
                .FromSqlRaw("EXEC spGetAllUserProfilesWithinRange @Latitude, @Longitude, @RadiusInMeters, @AdminFlag", latitudeParam, longitudeParam, radiusParam, adminFlagParam)
                .ToList();

                return users;
            }
            
            
        }
    }
}
