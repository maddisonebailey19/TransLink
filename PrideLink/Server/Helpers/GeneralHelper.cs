using PrideLink.Server.Interfaces;
using PrideLink.Server.TransLinkDataBase;
using PrideLink.Shared.General;

namespace PrideLink.Server.Helpers
{
    public class GeneralHelper : IGeneralInterface
    {
        public List<Hobbys> GetHobbies()
        {
            List<Hobbys> hobbys = new List<Hobbys>();
            using(var context = new MasContext())
            {
                List<TblHobby> listOfHobby = context.TblHobbies.ToList();
                foreach(TblHobby hobby in listOfHobby)
                {
                    Hobbys newHobby = new Hobbys();
                    newHobby.HobbyNo = hobby.HobbyNo;
                    newHobby.HobbyName = hobby.HobbyName;
                    hobbys.Add(newHobby);
                }
                return hobbys;
            }
        }

        public void UpdateVerificationStatus(int userNo, string UserTypeName)
        {
            using (var context = new MasContext())
            {
                TblUser user = context.TblUsers.FirstOrDefault(e => e.UserNo == userNo);
                int UserTypeNo;
                switch (UserTypeName)
                {
                    case "Verified":
                        UserTypeNo = 2;
                        break;
                    case "Unverified":
                        UserTypeNo = 3;
                        break;
                    default:
                        UserTypeNo = 3;
                        break;
                }
                user.UserType = UserTypeNo;
                context.SaveChanges();
            }
        }

        public string? UserVerificationStatus(int userNo)
        {
            using (var context = new MasContext())
            {
                int? userType = context.TblUsers.FirstOrDefault(e => e.UserNo == userNo).UserType;
                if (userType != null)
                {
                    switch (userType)
                    {
                        case 1:
                            return "System";
                        case 2:
                            return "Verified";
                        case 3:
                            return "Unverified";
                    }
                }
            }
            return null;
        }
    }
}
