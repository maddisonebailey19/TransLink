using PrideLink.Server.Controllers;
using PrideLink.Server.Interfaces;
using PrideLink.Shared.FreindFinderDetails;
using PrideLink.Shared.General;
using PrideLink.Shared.Location;
using PrideLink.Shared.UserInfo;

namespace PrideLink.Server.Helpers
{
    public class FreindFinderUserAccountHelper : IFreindFinderUserInfoInterface
    {
        private readonly ILocationInterface _locationInterface;
        private readonly IUserInfoInterface _userInfoInterface;
        private readonly IGeneralInterface _generalInterface;

        public FreindFinderUserAccountHelper(ILocationInterface locationInterface, IUserInfoInterface userInfoInterface, IGeneralInterface generalInterface)
        {
            _locationInterface = locationInterface;
            _userInfoInterface = userInfoInterface;
            _generalInterface = generalInterface;
        }

        public List<UserFreindFinderAccount> GetAllUserFreindFinderAccounts(UserLocationData userLocation)
        {
            List<UserFreindFinderAccount> userFreindFinderAccounts = new List<UserFreindFinderAccount>();
            List<int> userNos = GetAllLocationWithinRange(userLocation);
            foreach(int user in  userNos)
            {
                UserFreindFinderAccount userFreindFinderAccount = new UserFreindFinderAccount();
                userFreindFinderAccount.userNo = user;
                UserAccountGeneralInfo? userAccountGeneralInfo = GetUserAccountGeneralInfo(user);
                if(userAccountGeneralInfo != null)
                {
                    userFreindFinderAccount.UserAccountGeneralInfo = userAccountGeneralInfo;
                    userFreindFinderAccount.UserAccountRelashionshipStatus = GetUserRelashionshipStatus(user);
                    userFreindFinderAccount.UserAccountSocials = GetUserAccountSocials(user);
                    userFreindFinderAccount.UserAccountPictures = GetUserAccountPictures(user);
                    userFreindFinderAccount.UserAccountHobbies = GetUserAccountHobbies(user);
                    userFreindFinderAccounts.Add(userFreindFinderAccount);
                }   
            }

            return userFreindFinderAccounts;
        }
        private List<int> GetAllLocationWithinRange(UserLocationData userLocation)
        {
            return _locationInterface.GetLocation(userLocation);
        }
        private UserAccountGeneralInfo? GetUserAccountGeneralInfo(int userNo)
        {
            try
            {
                UserAccountGeneralInfo userAccountGeneralInfo = new UserAccountGeneralInfo();

                UserBioDescriptionData? userBioDescription = new UserBioDescriptionData();
                DisplayName? displayName = new DisplayName();
                Age? age = new Age();

                userBioDescription = _userInfoInterface.GetUserBioDescription(userNo);
                displayName = _userInfoInterface.GetDisplayName(userNo);
                age = _userInfoInterface.GetUserAge(userNo);
                if(displayName != null)
                {
                    userAccountGeneralInfo.BioDescription = userBioDescription.bioText;
                    userAccountGeneralInfo.DisplayName = displayName.userName;
                    userAccountGeneralInfo.Age = age.AgeValue.ToString();
                    return userAccountGeneralInfo;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }
        private UserAccountRelashionshipStatus GetUserRelashionshipStatus(int userNo)
        {
            UserAccountRelashionshipStatus userAccountRelashionshipStatus = new UserAccountRelashionshipStatus();
            UserRelationshipStatusData userRelationshipStatus = _userInfoInterface.GetUserRelationshipStatus(userNo);
            userAccountRelashionshipStatus.relashionshipStatusNo = userRelationshipStatus.relationshipStatusNo;

            List<UserRelationshipStatusData?> userRelationshipStatuses = _userInfoInterface.GetRelationshipStatuses();
            userAccountRelashionshipStatus.relashionshipStatus = userRelationshipStatuses.FirstOrDefault(e => e.relationshipStatusNo == userAccountRelashionshipStatus.relashionshipStatusNo).relationshipStatus;

            return userAccountRelashionshipStatus;
        }
        private List<UserAccountSocials?> GetUserAccountSocials(int userNo)
        {
            List<UserAccountSocials> userAccountSocials = new List<UserAccountSocials>();
            List<UserSocial?> userSocials = _userInfoInterface.GetUserSocial(userNo);
            if(userSocials != null)
            {
                foreach (UserSocial social in userSocials)
                {
                    UserAccountSocials socials = new UserAccountSocials();
                    socials.socialTypeNo = social.SocialTypeNo;
                    socials.socialValue = social.SocialValue;
                    userAccountSocials.Add(socials);
                }
            }
            
            return userAccountSocials;
        }
        private List<UserAccountPictures> GetUserAccountPictures(int userNo)
        {
            List<UserAccountPictures> userAccountPictures = new List<UserAccountPictures>();
            for (int i = 1; i < 4; i++)
            {
                UserAccountPictures accountPictures = new UserAccountPictures();    
                UserPicture picture = new UserPicture();
                picture.pictureTypeNo = i;
                UserPicture userPicture = _userInfoInterface.GetUserPicture(picture, userNo);
                accountPictures.pictureTypeNo = userPicture.pictureTypeNo;
                accountPictures.base64Image = userPicture.base64Picture;
                userAccountPictures.Add(accountPictures);
            }
            return userAccountPictures;
        }
        private List<UserAccountHobbies?> GetUserAccountHobbies(int userNo)
        {
            List<UserAccountHobbies> userAccountHobbies = new List<UserAccountHobbies>();
            List<Hobbys?> userHobbies = _userInfoInterface.GetHobbys(userNo);

            List<Hobbys> allHobbies = _generalInterface.GetHobbies();
            if(userHobbies != null)
            {
                foreach (Hobbys hobby in userHobbies)
                {
                    UserAccountHobbies accountHobbies = new UserAccountHobbies();
                    accountHobbies.HobbiesTypeNo = hobby.HobbyNo;
                    accountHobbies.HobbyName = allHobbies.FirstOrDefault(e => e.HobbyNo == hobby.HobbyNo).HobbyName;
                    userAccountHobbies.Add(accountHobbies);
                }
                return userAccountHobbies;
            }
            else
            {
                return null;
            }
        }
    }
}
