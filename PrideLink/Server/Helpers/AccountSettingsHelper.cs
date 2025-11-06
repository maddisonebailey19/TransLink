using PrideLink.Server.Interfaces;
using PrideLink.Server.TransLinkDataBase;
using PrideLink.Shared.AccountSettings;

namespace PrideLink.Server.Helpers
{
    public class AccountSettingsHelper : IAccountSettingsInterface
    {
        private readonly PasswordHelper _passwordHelper;
        public AccountSettingsHelper(PasswordHelper passwordHelper)
        {
            _passwordHelper = passwordHelper;
        }
        public string? CheckLoginCredWithUserNo(int userNo, string password)
        {
            using (var context = new MasContext())
            {
                var entity = context.TblUsers.FirstOrDefault(e => e.UserNo == userNo);
                if (entity != null)
                {
                    return _passwordHelper.VerifyPassword(password, entity.Password) ? entity.UserId : null;
                }
                else
                {
                    return null;
                }
            }
        }

        public List<NotificationSettings> GetNotificationSettings(int userNo)
        {
            List<NotificationSettings> notificationSettings = new List<NotificationSettings>();
            using (var context = new MasContext())
            {
                TblGeneralConfiguration notificationSetting1 = context.TblGeneralConfigurations.FirstOrDefault(e => e.UserNo == userNo && e.TypeNo == 7);
                if(notificationSetting1 != null)
                {
                    notificationSettings.Add(new NotificationSettings
                    {
                        NotificationSettingsNo = 1,
                        Value = notificationSetting1.Int1 == 1 ? true : false
                    });
                    notificationSettings.Add(new NotificationSettings
                    {
                        NotificationSettingsNo = 2,
                        Value = notificationSetting1.Int2 == 1 ? true : false
                    });
                }
                else
                {
                    context.Add(new TblGeneralConfiguration
                    {
                        UserNo = userNo,
                        TypeNo = 7,
                        Int1 = 1,
                        Int2 = 1,
                    });
                    context.SaveChanges();
                }
                
            }
            return notificationSettings;
        }

        public bool GetTwoStepAuthenticationSettings(int userNo)
        {
            using(var context = new MasContext())
            {
                TblGeneralConfiguration twoStepAuthenticationSetting = context.TblGeneralConfigurations.FirstOrDefault(e => e.UserNo == userNo && e.TypeNo == 6);
                if(twoStepAuthenticationSetting != null)
                {
                    return twoStepAuthenticationSetting.Int2 == 1 ? true : false;
                }
                else
                {
                    context.Add(new TblGeneralConfiguration
                    {
                        UserNo = userNo,
                        TypeNo = 6,
                        Int2 = 0
                    });
                    context.SaveChanges();
                    return false;
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

        public void UpdateNotificationSettings(int userNo, List<NotificationSettings> notificationSettings)
        {
            using(var context = new MasContext())
            {
                TblGeneralConfiguration notificationSetting1 = context.TblGeneralConfigurations.FirstOrDefault(e => e.UserNo == userNo && e.TypeNo == 7);
                if(notificationSetting1 != null)
                {
                    foreach(var setting in notificationSettings)
                    {
                        switch(setting.NotificationSettingsNo)
                        {
                            case 1:
                                notificationSetting1.Int1 = setting.Value ? 1 : 0;
                                break;
                            case 2:
                                notificationSetting1.Int2 = setting.Value ? 1 : 0;
                                break;
                        }
                    }
                    context.SaveChanges();
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
                    entity.Password = _passwordHelper.HashPassword(password);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void UpdateTwoStepAuthenticationSettings(int userNo, bool isEnabled)
        {
            using(var context = new MasContext())
            {
                TblGeneralConfiguration twoStepAuthenticationSetting = context.TblGeneralConfigurations.FirstOrDefault(e => e.UserNo == userNo && e.TypeNo == 6);
                if(twoStepAuthenticationSetting != null)
                {
                    twoStepAuthenticationSetting.Int2 = isEnabled ? 1 : 0;
                    context.SaveChanges();
                }

            }
        }
    }


}
