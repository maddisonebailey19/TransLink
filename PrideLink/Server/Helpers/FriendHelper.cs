using Microsoft.AspNetCore.Components.Routing;
using PrideLink.Server.Controllers;
using PrideLink.Server.Interfaces;
using PrideLink.Server.TransLinkDataBase;
using PrideLink.Shared.AccountSettings;
using PrideLink.Shared.FreindFinderDetails;
using PrideLink.Shared.Notification;
using System.Data;

namespace PrideLink.Server.Helpers
{
    public class FriendHelper : IFriendInterface
    {
        private readonly ILocationInterface _locationInterface;
        private readonly IUserInfoInterface _userInfoInterface;
        public FriendHelper(ILocationInterface locationInterface, IUserInfoInterface userInfoInterface)
        {
            _locationInterface = locationInterface;
            _userInfoInterface = userInfoInterface;
        }
        public NotificationContent? AcceptFriendRequest(int userNo, int friendUserNo)
        {
            using (var context = new MasContext())
            {
                try
                {
                    TblFriendMappingTable friendRequest = context.TblFriendMappingTables.FirstOrDefault(e => e.UserNo == userNo && e.FriendUserNo == friendUserNo);
                    if (friendRequest == null)
                    {
                        return null;
                    }
                    friendRequest.FriendStatusNo = 1;
                    context.SaveChanges();

                    TblFriendMappingTable newFriendRequest = context.TblFriendMappingTables.FirstOrDefault(e => e.UserNo == friendUserNo && e.FriendUserNo == userNo);
                    if (newFriendRequest != null)
                    {
                        newFriendRequest.FriendStatusNo = 1;
                        context.SaveChanges();
                        return FriendRequestAcceptedEmail(userNo, friendUserNo);
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
        }
        public NotificationContent? AddFriend(int userNo, int friendUserNo)
        {
            using (var context = new MasContext())
            {
                try
                {
                    TblFriendMappingTable userFriendRequest = context.TblFriendMappingTables.FirstOrDefault(e => e.UserNo == userNo && e.FriendUserNo == friendUserNo);
                    if (userFriendRequest == null)
                    {
                        context.Add(new TblFriendMappingTable
                        {
                            UserNo = userNo,
                            FriendUserNo = friendUserNo,
                            FriendStatusNo = 2
                        });
                    }
                    else
                    {
                        if (userFriendRequest.Active == false)
                        {
                            userFriendRequest.Active = true;
                            userFriendRequest.FriendStatusNo = 2;
                        }
                    }
                    TblFriendMappingTable newFriendRequest = context.TblFriendMappingTables.FirstOrDefault(e => e.UserNo == friendUserNo && e.FriendUserNo == userNo);
                    if (newFriendRequest == null)
                    {
                        context.Add(new TblFriendMappingTable
                        {
                            UserNo = friendUserNo,
                            FriendUserNo = userNo,
                            FriendStatusNo = 2
                        });
                    }
                    else
                    {
                        if (newFriendRequest.Active == false)
                        {
                            newFriendRequest.Active = true;
                            newFriendRequest.FriendStatusNo = 2;
                        }
                    }

                    context.SaveChanges();
                    return NewFriendEmail(userNo, friendUserNo);
                }
                catch (Exception ex)
                {
                    return null;
                }

            }
        }
        public bool BlockUser(int userNo, int blockedUserNo)
        {
            try
            {
                using (var context = new MasContext())
                {
                    TblFriendMappingTable friend = context.TblFriendMappingTables.FirstOrDefault(e => e.UserNo == userNo && e.FriendUserNo == blockedUserNo);
                    if (friend != null)
                    {
                        friend.FriendStatusNo = 3;
                        context.SaveChanges();
                    }
                    else
                    {
                        context.Add(new TblFriendMappingTable
                        {
                            UserNo = userNo,
                            FriendUserNo = blockedUserNo,
                            FriendStatusNo = 3
                        });
                        context.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public bool DeclineFriendRequest(int userNo, int friendUserNo)
        {
            try
            {
                using (var context = new MasContext())
                {
                    TblFriendMappingTable friendRequest = context.TblFriendMappingTables.FirstOrDefault(e => e.UserNo == friendUserNo && e.FriendUserNo == userNo);
                    if (friendRequest != null)
                    {
                        friendRequest.FriendStatusNo = 3;
                        context.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public List<UserFreindFinderAccount?> GetAllUserFriends(int userNo)
        {
            List<UserFreindFinderAccount> userAccounts = new List<UserFreindFinderAccount>();

            List<VWUserFriendFinderProfile> usersFromLocation = _locationInterface.GetUserFriendsProfiles(userNo);

            List<VWUserHobbies> userHobbies = _locationInterface.GetUserFriendsHobbies(userNo);

            foreach (VWUserFriendFinderProfile userFromLocation in usersFromLocation)
            {
                UserFreindFinderAccount userAccount = new UserFreindFinderAccount()
                {
                    userNo = userFromLocation.UserNo,
                    UserAccountGeneralInfo = new UserAccountGeneralInfo()
                    {
                        BioDescription = userFromLocation.BioDescription,
                        DisplayName = userFromLocation.DisplayName,
                        Age = userFromLocation.Age,
                        UserVerified = userFromLocation.IsUserVerified switch
                        {
                            2 => "Verified",
                            3 => "Unverified",
                            _ => "Unverified"
                        },
                        FriendStatus = userFromLocation.FriendStatus
                    },
                    UserAccountRelashionshipStatus = new UserAccountRelashionshipStatus()
                    {
                        relashionshipStatusNo = (int)(userFromLocation.RelationshipStatusNo ?? 0),
                        relashionshipStatus = userFromLocation.RelationshipStatusName
                    },
                    UserAccountPictures = new List<UserAccountPictures>()
                    {
                        new UserAccountPictures()
                        {
                            pictureTypeNo = 1,
                            base64Image = userFromLocation.Picture1
                        },
                        new UserAccountPictures()
                        {
                            pictureTypeNo = 2,
                            base64Image = userFromLocation.Picture2
                        },
                        new UserAccountPictures()
                        {
                            pictureTypeNo = 3,
                            base64Image = userFromLocation.Picture3
                        }
                    },
                    UserAccountSocials = new List<UserAccountSocials>()
                    {
                        new UserAccountSocials()
                        {
                            socialTypeNo = 1,
                            socialValue = userFromLocation.Instagram
                        },
                        new UserAccountSocials()
                        {
                            socialTypeNo = 2,
                            socialValue = userFromLocation.Snapchat
                        },
                        new UserAccountSocials()
                        {
                            socialTypeNo = 3,
                            socialValue = userFromLocation.WhatsApp
                        },
                        new UserAccountSocials()
                        {
                            socialTypeNo = 4,
                            socialValue = userFromLocation.Discord
                        }
                    },
                    UserAccountHobbies = userHobbies.Where(e => e.UserNo == userFromLocation.UserNo).Select(hobby => new UserAccountHobbies()
                    {
                        HobbiesTypeNo = hobby.HobbyNo,
                        HobbyName = hobby.HobbyName
                    }).ToList()
                };
                userAccounts.Add(userAccount);
            }

            return userAccounts;
        }
        public bool RemoveFriend(int userNo, int friendUserNo)
        {
            try
            {
                using (var context = new MasContext())
                {
                    TblFriendMappingTable user = context.TblFriendMappingTables.FirstOrDefault(e => e.UserNo == userNo && e.FriendUserNo == friendUserNo);
                    TblFriendMappingTable friend = context.TblFriendMappingTables.FirstOrDefault(e => e.UserNo == friendUserNo && e.FriendUserNo == userNo);
                    if (user != null)
                    {
                        user.Active = false;

                    }
                    if (friend != null)
                    {
                        friend.Active = false;
                    }
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private NotificationContent NewFriendEmail(int userNo, int friendUserNo)
        {
            using (var context = new MasContext())
            {
                TblUser toUser = context.TblUsers.FirstOrDefault(e => e.UserNo == friendUserNo);
                TblGeneralConfiguration newFriendDisplayName = context.TblGeneralConfigurations.FirstOrDefault(e => e.UserNo == userNo && e.TypeNo == 3);


                NotificationContent emailContent = new NotificationContent()
                {
                    ToEmail = toUser.Email,
                    EmailContentNo = 3,
                    Subject = "You have a new friend request on TransLink! 💜",
                    EmailContents = new Dictionary<string, string>()
                };
                emailContent.EmailContents.Add("@toUserName", toUser.Login);
                emailContent.EmailContents.Add("@newFriendUserName", newFriendDisplayName.Ref2);

                return emailContent;
            }
        }
        private NotificationContent FriendRequestAcceptedEmail(int userNo, int friendUserNo)
        {
            using (var context = new MasContext())
            {
                TblUser toUser = context.TblUsers.FirstOrDefault(e => e.UserNo == friendUserNo);
                TblGeneralConfiguration acceptedFriendDisplayName = context.TblGeneralConfigurations.FirstOrDefault(e => e.UserNo == userNo && e.TypeNo == 3);
                NotificationContent emailContent = new NotificationContent()
                {
                    ToEmail = toUser.Email,
                    EmailContentNo = 4,
                    Subject = "Your friend request has been accepted on TransLink! 💜",
                    EmailContents = new Dictionary<string, string>()
                };
                emailContent.EmailContents.Add("@toUserName", toUser.Login);
                emailContent.EmailContents.Add("@newFriendUserName", acceptedFriendDisplayName.Ref2);
                return emailContent;
            }
        }
    }
}
