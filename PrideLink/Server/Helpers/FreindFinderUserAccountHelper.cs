using PrideLink.Server.Controllers;
using PrideLink.Server.Interfaces;
using PrideLink.Server.TransLinkDataBase;
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

        public List<UserFreindFinderAccount> GetAllUserFreindFinderAccounts(UserLocationData userLocation, List<string> roles)
        {
            List<UserFreindFinderAccount> userFreindFinderAccounts = new List<UserFreindFinderAccount>();
            List<int> userNos = GetAllLocationWithinRange(userLocation, roles);

            //Need to get general info from tblGeneralConfiguration and TblUsers
            //Need to get relashionship status from tblGeneralConfiguration
            //Need to get socials from tblGeneralConfiguration type 4 and 5
            //Need to get pictures from tblGeneralConfiguration type 1
            List<TblGeneralConfiguration> tblGeneralConfigurations = _userInfoInterface.GetUserGeneralConfiguration(userNos);
            List<TblUser> tblUsers = _userInfoInterface.GetTblUsers(userNos);
            List<TblRelationshipStatusType> tblRelationshipStatusTypes = _userInfoInterface.GetUserRelationshipStatusTypes();

            //Need to get hobbies from TblHobbyUserMappingTable
            List<TblHobbyUserMappingTable> tblHobbyUserMappingTables = _userInfoInterface.GetUserHobbyUserMappingTable(userNos);
            List<TblHobby> tblHobbies = _userInfoInterface.GetUserHobbies();

            foreach(int userNo in userNos)
            {
                TblUser tblUser = tblUsers.FirstOrDefault(e => e.UserNo == userNo);
                List<TblGeneralConfiguration> generalConfiguration = tblGeneralConfigurations.Where(e => e.UserNo == userNo).ToList();
                List<TblHobbyUserMappingTable> hobbyUserMappingTable = tblHobbyUserMappingTables.Where(e => e.UserNo == userNo).ToList();

                
                if(generalConfiguration.FirstOrDefault(e => e.TypeNo == 3) != null)
                {
                    UserFreindFinderAccount userFreindFinderAccount = new UserFreindFinderAccount();
                    userFreindFinderAccount.userNo = tblUser.UserNo;
                    //User General info
                    userFreindFinderAccount.UserAccountGeneralInfo = new UserAccountGeneralInfo()
                    {
                        BioDescription = generalConfiguration.FirstOrDefault(e => e.TypeNo == 3).Ref1,
                        DisplayName = generalConfiguration.FirstOrDefault(e => e.TypeNo == 3).Ref2,
                        Age = generalConfiguration.FirstOrDefault(e => e.TypeNo == 3).Ref3,
                        UserVerified = tblUser.UserType switch
                        {
                            2 => "Verified",
                            3 => "Unverified",
                            _ => "Unverified"
                        }
                    };
                    //User Relationship Status
                    userFreindFinderAccount.UserAccountRelashionshipStatus = new UserAccountRelashionshipStatus()
                    {
                        relashionshipStatusNo = (int)generalConfiguration.FirstOrDefault(e => e.TypeNo == 3).Int1,
                        relashionshipStatus = tblRelationshipStatusTypes.FirstOrDefault(e => e.RelationshipStatusTypeNo == generalConfiguration.FirstOrDefault(e => e.TypeNo == 3).Int1).RelationshipStatusTypeName
                    };
                    //User Pictures
                    userFreindFinderAccount.UserAccountPictures = new List<UserAccountPictures>();
                    userFreindFinderAccount.UserAccountPictures.Add(new UserAccountPictures
                    {
                        pictureTypeNo = 1,
                        base64Image = generalConfiguration.FirstOrDefault(e => e.TypeNo == 1).Ref1
                    });
                    userFreindFinderAccount.UserAccountPictures.Add(new UserAccountPictures
                    {
                        pictureTypeNo = 2,
                        base64Image = generalConfiguration.FirstOrDefault(e => e.TypeNo == 1).Ref2
                    });
                    userFreindFinderAccount.UserAccountPictures.Add(new UserAccountPictures
                    {
                        pictureTypeNo = 3,
                        base64Image = generalConfiguration.FirstOrDefault(e => e.TypeNo == 1).Ref3
                    });
                    //User Socials
                    userFreindFinderAccount.UserAccountSocials = new List<UserAccountSocials>();
                    if(generalConfiguration.FirstOrDefault(e => e.TypeNo == 4) != null)
                    {
                        if (generalConfiguration.FirstOrDefault(e => e.TypeNo == 4).Ref1 != null)
                        {
                            userFreindFinderAccount.UserAccountSocials.Add(new UserAccountSocials
                            {
                                socialTypeNo = 1,
                                socialValue = generalConfiguration.FirstOrDefault(e => e.TypeNo == 4).Ref1
                            });
                        }
                        if (generalConfiguration.FirstOrDefault(e => e.TypeNo == 4).Ref1 != null)
                        {
                            userFreindFinderAccount.UserAccountSocials.Add(new UserAccountSocials
                            {
                                socialTypeNo = 2,
                                socialValue = generalConfiguration.FirstOrDefault(e => e.TypeNo == 4).Ref2
                            });
                        }
                        if (generalConfiguration.FirstOrDefault(e => e.TypeNo == 4).Ref1 != null)
                        {
                            userFreindFinderAccount.UserAccountSocials.Add(new UserAccountSocials
                            {
                                socialTypeNo = 3,
                                socialValue = generalConfiguration.FirstOrDefault(e => e.TypeNo == 4).Ref3
                            });
                        }
                    }
                    if (generalConfiguration.FirstOrDefault(e => e.TypeNo == 5) != null)
                    {
                        if (generalConfiguration.FirstOrDefault(e => e.TypeNo == 5).Ref1 != null)
                        {
                            userFreindFinderAccount.UserAccountSocials.Add(new UserAccountSocials
                            {
                                socialTypeNo = 4,
                                socialValue = generalConfiguration.FirstOrDefault(e => e.TypeNo == 5).Ref1
                            });
                        }
                    }
                    
                    //User Hobbies
                    userFreindFinderAccount.UserAccountHobbies = new List<UserAccountHobbies>();
                    foreach (var hobbyMapping in hobbyUserMappingTable)
                    {
                        TblHobby hobby = tblHobbies.FirstOrDefault(e => e.HobbyNo == hobbyMapping.HobbyNo);
                        userFreindFinderAccount.UserAccountHobbies.Add(new UserAccountHobbies
                        {
                            HobbiesTypeNo = hobby.HobbyNo,
                            HobbyName = hobby.HobbyName
                        });
                    }

                    userFreindFinderAccounts.Add(userFreindFinderAccount);
                }
                
            }






            //foreach (int user in  userNos)
            //{
            //    UserFreindFinderAccount userFreindFinderAccount = new UserFreindFinderAccount();
            //    userFreindFinderAccount.userNo = user;
            //    UserAccountGeneralInfo? userAccountGeneralInfo = GetUserAccountGeneralInfo(user);
            //    if(userAccountGeneralInfo != null)
            //    {
            //        userFreindFinderAccount.UserAccountGeneralInfo = userAccountGeneralInfo;
            //        userFreindFinderAccount.UserAccountRelashionshipStatus = GetUserRelashionshipStatus(user);
            //        userFreindFinderAccount.UserAccountSocials = GetUserAccountSocials(user);
            //        userFreindFinderAccount.UserAccountPictures = GetUserAccountPictures(user);
            //        userFreindFinderAccount.UserAccountHobbies = GetUserAccountHobbies(user);
            //        userFreindFinderAccounts.Add(userFreindFinderAccount);
            //    }   
            //}

            return userFreindFinderAccounts;
        }
        private List<int> GetAllLocationWithinRange(UserLocationData userLocation, List<string> roles)
        {
            return _locationInterface.GetLocation(userLocation, roles);
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
                if (displayName != null)
                {
                    userAccountGeneralInfo.BioDescription = userBioDescription.bioText;
                    userAccountGeneralInfo.DisplayName = displayName.userName;
                    userAccountGeneralInfo.Age = age.AgeValue.ToString();
                    userAccountGeneralInfo.UserVerified = _userInfoInterface.GetUserVerificationStatus(userNo);
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
