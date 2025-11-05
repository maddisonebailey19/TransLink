using PrideLink.Server.Interfaces;
using PrideLink.Server.TransLinkDataBase;

namespace PrideLink.Server.Helpers
{
    public class AccountSettingsHelper : IAccountSettingsInterface
    {
        public string? CheckLoginCredWithUserNo(int userNo, string password)
        {
            using (var context = new MasContext())
            {
                var entity = context.TblUsers.FirstOrDefault(e => e.UserNo == userNo && e.Password == password);
                if (entity != null)
                {
                    return entity.UserId;
                }
                else
                {
                    return null;
                }
            }
        }

        public bool IsEmailCorrect(int userNo, string email)
        {
            using (var context = new MasContext())
            {
                if(context.TblUsers.Any(e => e.UserNo == userNo && e.Email == email))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool UpdateEmail(string email, int userNo)
        {
            using(var context = new MasContext())
            {
                var entity = context.TblUsers.FirstOrDefault(e => e.UserNo == userNo);
                if (entity != null)
                {
                    entity.Email = email;
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool UpdatePassword(string password, int userNo)
        {
            using (var context = new MasContext())
            {
                var entity = context.TblUsers.FirstOrDefault(e => e.UserNo == userNo);
                if (entity != null)
                {
                    entity.Password = password;
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }


}
