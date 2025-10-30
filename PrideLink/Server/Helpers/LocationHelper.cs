using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PrideLink.Server.Interfaces;
using PrideLink.Server.TransLinkDataBase;
using PrideLink.Shared.Location;
using Microsoft.EntityFrameworkCore;


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

        public List<int> GetLocation(UserLocationData userLocation)
        {
            List<int> userNos = new List<int>();    
            var latitudeParam = new SqlParameter("@Latitude", userLocation.Latitude);
            var longitudeParam = new SqlParameter("@Longitude", userLocation.Longitude);
            var radiusParam = new SqlParameter("@RadiusInMeters", 50000);
            using(var context = new MasContext())
            {
                List<TblUser> users = context.TblUsers
                .FromSqlRaw("EXEC spGetAllUsersWithinRange @Latitude, @Longitude, @RadiusInMeters", latitudeParam, longitudeParam, radiusParam)
                .ToList();

                foreach (TblUser user in users)
                {
                    userNos.Add(user.UserNo);
                }
                return userNos;
            }
            
            
        }
    }
}
