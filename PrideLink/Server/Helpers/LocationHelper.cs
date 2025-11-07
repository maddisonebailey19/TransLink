using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PrideLink.Server.Controllers;
using PrideLink.Server.Interfaces;
using PrideLink.Server.TransLinkDataBase;
using PrideLink.Shared.General;
using PrideLink.Shared.Location;


namespace PrideLink.Server.Helpers
{
    public class LocationHelper : ILocationInterface
    {
        public bool AddTownAndCityToUser(int userNo, int cityNo)
        {
            try
            {
                using (var context = new MasContext())
                {
                    TblGeneralConfiguration tblGeneralConfiguration = context.TblGeneralConfigurations.FirstOrDefault(e => e.UserNo == userNo && e.TypeNo == 6);
                    if (tblGeneralConfiguration == null)
                    {
                        TblGeneralConfiguration tblGeneralConfigurationType6 = new TblGeneralConfiguration()
                        {
                            UserNo = userNo,
                            TypeNo = 6,
                            Int1 = cityNo,
                            Int2 = 0
                        };
                        context.Add(tblGeneralConfigurationType6);
                        
                    }
                    else
                    {
                        tblGeneralConfiguration.Int1 = cityNo;
                    }
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
            
        }
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
        public List<TownCityLocations> GetTownCityLocations()
        {
            List<TownCityLocations> townCityLocations = new List<TownCityLocations>();
            using (var context = new MasContext())
            {
                townCityLocations = context.TblCities
                    .Select(city => new TownCityLocations
                    {
                        cityNo = city.CityNo,
                        cityName = city.CityName
                    })
                    .ToList();
            }
            return townCityLocations;
        }
        public List<VWUserHobbies> GetUserFriendsHobbies(int userNo)
        {
            var userNoParam = new SqlParameter("@UserNo", userNo);
            using (var context = new MasContext())
            {
                List<VWUserHobbies> users = context.VWUserHobbies
                .FromSqlRaw("EXEC spGetAllUserFriendHobbies @UserNo", userNoParam)
                .ToList();
                return users;
            }
        }
        public List<VWUserFriendFinderProfile> GetUserFriendsProfiles(int userNo)
        {
            var userNoParam = new SqlParameter("@UserNo", userNo);
            using (var context = new MasContext())
            {
                List<VWUserFriendFinderProfile> users = context.VWUserFriendFinderProfile
                .FromSqlRaw("EXEC spGetAllUserFriendProfiles @UserNo", userNoParam)
                .ToList();
                return users;
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
        public UserLocationData GetUserLocationFromTownAndCity(int userNo)
        {
            UserLocationData userLocation = new UserLocationData();
            using (var context = new MasContext())
            {
                var UserSettings1 = context.TblGeneralConfigurations.FirstOrDefault(e => e.UserNo == userNo && e.TypeNo == 6);
                if(UserSettings1.Int1 != null)
                {
                    TblCity location = context.TblCities.FirstOrDefault(e => e.CityNo == UserSettings1.Int1);
                    userLocation.Longitude = (float)location.Longitude;
                    userLocation.Latitude = (float)location.Latitude;
                }
            }
            return userLocation;
        }
        public List<VWUserFriendFinderProfile> GetUsersFromLocation(int userNo, UserLocationData userLocation, List<string> roles)
        {
            int radisuInMeters = 500000000;

            bool adminFlag = roles != null && roles.Contains("Admin", StringComparer.OrdinalIgnoreCase);

            List<int> userNos = new List<int>();
            var userNoParam = new SqlParameter("@UserNo", userNo);
            var latitudeParam = new SqlParameter("@Latitude", userLocation.Latitude);
            var longitudeParam = new SqlParameter("@Longitude", userLocation.Longitude);
            var radiusParam = new SqlParameter("@RadiusInMeters", radisuInMeters);
            var adminFlagParam = new SqlParameter("@AdminFlag", adminFlag);
            using (var context = new MasContext())
            {
                List<VWUserFriendFinderProfile> users = context.VWUserFriendFinderProfile
                .FromSqlRaw("EXEC spGetAllUserProfilesWithinRange @UserNo, @Latitude, @Longitude, @RadiusInMeters, @AdminFlag", userNoParam, latitudeParam, longitudeParam, radiusParam, adminFlagParam)
                .ToList();

                return users;
            }
            
            
        }
    }
}
