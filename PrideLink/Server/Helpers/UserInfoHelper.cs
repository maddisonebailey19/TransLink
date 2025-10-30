using Microsoft.AspNetCore.Http.HttpResults;
using PrideLink.Server.Interfaces;
using PrideLink.Server.TransLinkDataBase;
using PrideLink.Server.Internal_Models;
using PrideLink.Shared.General;
using PrideLink.Shared.UserInfo;
using System.Linq.Expressions;

namespace PrideLink.Server.Helpers
{
    public class UserInfoHelper : IUserInfoInterface
    {
        private readonly InsertIntoGenericReference _insertIntoGenericReference;
        public UserInfoHelper(InsertIntoGenericReference insertIntoGenericReference)
        {
            _insertIntoGenericReference = insertIntoGenericReference;   
        }


        public bool AddRemoveEmailFromUser(Email email, int userNo)
        {
            using(var context = new MasContext())
            {
                var entity = context.TblUsers.FirstOrDefault(e => e.UserNo == userNo);
                if(entity != null)
                {
                    if (email.add)
                    {
                        entity.Email = email.email;
                    }
                    else
                    {
                        entity.Email = null;
                    }

                    context.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public bool AddUserPicture(UserPicture picture, int userNo)
        {
            GeneralConfigurationValues generalConfigurationValues = new GeneralConfigurationValues();
            generalConfigurationValues.TypeNo = 1;
            switch (picture.pictureTypeNo)
            {
                case 1:
                    generalConfigurationValues.Ref1 = picture.base64Picture;
                    break;
                case 2:
                    generalConfigurationValues.Ref2 = picture.base64Picture;
                    break;
                case 3:
                    generalConfigurationValues.Ref3 = picture.base64Picture;
                    break;
            }

            return _insertIntoGenericReference.AddUpdateGeneralConfiguration(userNo, generalConfigurationValues);


            //using (var context = new MasContext())
            //{
            //    var entity = context.TblGeneralConfigurations.FirstOrDefault(e => e.UserNo == userNo & e.UserPictureTypeNo == picture.pictureTypeNo);
            //    if(entity != null)
            //    {
            //        entity.Image64String = picture.base64Picture;
                    
            //    }
            //    else
            //    {
            //        TblUserProfilePic tblUserProfilePic = new TblUserProfilePic
            //        {
            //            UserNo = userNo,
            //            UserPictureTypeNo = picture.pictureTypeNo,
            //            Image64String = picture.base64Picture
            //        };
            //        context.Add(tblUserProfilePic);
            //    }

            //    context.SaveChanges();
            //    return true;
            //}
            //return false;
        }
        public UserPicture GetUserPicture(UserPicture userPicture, int userNo)
        {
            UserPicture picture = new UserPicture();
            using (var context = new MasContext())
            {
                var response = context.TblGeneralConfigurations.FirstOrDefault(e => e.UserNo == userNo & e.TypeNo == 1);
                if(response == null)
                {
                    picture.base64Picture = null;
                }
                else
                {
                    switch (userPicture.pictureTypeNo)
                    {
                        case 1:
                            picture.base64Picture = response.Ref1;
                            break;
                        case 2:
                            picture.base64Picture = response.Ref2;
                            break;
                        case 3:
                            picture.base64Picture = response.Ref3;
                            break;
                    }
                }
                picture.pictureTypeNo = userPicture.pictureTypeNo;
                return picture;
            }
        }

        public UserRelationshipStatusData? GetUserRelationshipStatus(int userNo)
        {
            UserRelationshipStatusData? userRelationshipStatus = new UserRelationshipStatusData();
            using (var context = new MasContext())
            {
                var entity = context.TblGeneralConfigurations.FirstOrDefault(e => e.UserNo == userNo && e.TypeNo == 3);
                if(entity != null)
                {
                    var userRelashionType = context.TblRelationshipStatusTypes.FirstOrDefault(e => e.RelationshipStatusTypeNo == entity.Int1);
                    userRelationshipStatus.relationshipStatusNo = userRelashionType.RelationshipStatusTypeNo;
                    userRelationshipStatus.relationshipStatus = userRelashionType.RelationshipStatusTypeName;

                    return userRelationshipStatus;
                }
                else
                {
                    return null;
                }
                //var entity = context.TblUserRelationshipStatuses.FirstOrDefault(e => e.UserNo == userNo);
                //if (entity != null)
                //{
                //    var userType = context.TblRelationshipStatusTypes.FirstOrDefault(e => e.RelationshipStatusTypeNo == entity.RelationshipStatusTypeNo);
                //    userRelationshipStatus.relationshipStatusNo = userType.RelationshipStatusTypeNo;
                //    userRelationshipStatus.relationshipStatus = userType.RelationshipStatusTypeName;

                //    return userRelationshipStatus;
                //}
                //else
                //{
                //    return null;
                //}
            }
        }
        public List<UserRelationshipStatusData?> GetRelationshipStatuses()
        {
            List<UserRelationshipStatusData> statuses = new List<UserRelationshipStatusData>();
            using(var context = new MasContext())
            {
                List<TblRelationshipStatusType> entity = context.TblRelationshipStatusTypes.ToList(); 
                foreach (var relationshipType in entity)
                {
                    UserRelationshipStatusData relationshipStatus = new UserRelationshipStatusData();
                    relationshipStatus.relationshipStatusNo = relationshipType.RelationshipStatusTypeNo;
                    relationshipStatus.relationshipStatus = relationshipType.RelationshipStatusTypeName;

                    statuses.Add(relationshipStatus);
                }
                return statuses;
            }
            return null;
        }
        public bool AddRelationshipStatus(UserRelationshipStatusData relationshipStatus, int userNo)
        {
            GeneralConfigurationValues generalConfigurationValues = new GeneralConfigurationValues();
            generalConfigurationValues.TypeNo = 3;
            generalConfigurationValues.Int1 = relationshipStatus.relationshipStatusNo;

            return _insertIntoGenericReference.AddUpdateGeneralConfiguration(userNo, generalConfigurationValues);

            //using (var context = new MasContext())
            //{
            //    var entity = context.TblUserRelationshipStatuses.FirstOrDefault(e => e.UserNo == userNo);
            //    if (entity == null)
            //    {
            //        TblUserRelationshipStatus tblUserRelationshipStatus = new TblUserRelationshipStatus
            //        {
            //            UserNo = userNo,
            //            RelationshipStatusTypeNo = relationshipStatus.relationshipStatusNo,
            //        };
            //        context.Add(tblUserRelationshipStatus);
            //    }
            //    else
            //    {
            //        entity.RelationshipStatusTypeNo = relationshipStatus.relationshipStatusNo;
            //    }
            //    context.SaveChanges();
            //    return true;
            //}
            //return false;
        }

        public bool AddUserBioDescription(UserBioDescriptionData bioDescription, int userNo)
        {
            GeneralConfigurationValues generalConfigurationValues = new GeneralConfigurationValues();
            generalConfigurationValues.TypeNo = 3;
            generalConfigurationValues.Ref1 = bioDescription.bioText;

            return _insertIntoGenericReference.AddUpdateGeneralConfiguration(userNo, generalConfigurationValues);


            //if (bioDescription.bioText != null)
            //{
            //    using (var context = new MasContext())
            //    {
            //        var entity = context.TblUserBioDescriptions.FirstOrDefault(e => e.UserNo == userNo);
            //        if (entity == null)
            //        {
            //            TblUserBioDescription tblUserBioDescription = new TblUserBioDescription
            //            {
            //                UserNo = userNo,
            //                BioText = bioDescription.bioText
            //            };
            //            context.Add(tblUserBioDescription);
            //        }
            //        else
            //        {
            //            entity.BioText = bioDescription.bioText;
            //        }

            //        context.SaveChanges();
            //        return true;
            //    }
               
            //}
            //else
            //{
            //    return false;
            //}
            
        }
        public UserBioDescriptionData? GetUserBioDescription(int userNo)
        {
            UserBioDescriptionData bioDescription = new UserBioDescriptionData();
            using(var context = new MasContext())
            {
                var entity = context.TblGeneralConfigurations.FirstOrDefault(e => e.UserNo == userNo && e.TypeNo == 3);
                if (entity != null)
                {
                    bioDescription.bioText = entity.Ref1;
                    return bioDescription;
                }
                else
                {
                    return null;
                }
                
            }
        }

        public bool AddUserAge(Age age, int userNo)
        {
            GeneralConfigurationValues generalConfigurationValues = new GeneralConfigurationValues();
            generalConfigurationValues.TypeNo = 3;
            generalConfigurationValues.Ref3 = age.AgeValue.ToString();

            return _insertIntoGenericReference.AddUpdateGeneralConfiguration(userNo,generalConfigurationValues);

            //if (age.AgeValue != null)
            //{
            //    using (var context = new MasContext())
            //    {
            //        var entity = context.TblUsers.FirstOrDefault(e => e.UserNo == userNo);
            //        if (entity != null)
            //        {
            //            entity.Age = age.AgeValue;
            //            context.SaveChanges();
            //            return true;
            //        }
            //        else
            //        {
            //            return false;
            //        }
            //    }
            //}
            //return true;
        }
        public Age? GetUserAge(int userNo)
        {
            Age age = new Age();
            using(var context = new MasContext())
            {
                var entity = context.TblGeneralConfigurations.FirstOrDefault(e => e.UserNo == userNo && e.TypeNo == 3);
                if(entity != null)
                {
                    age.AgeValue = int.Parse(entity.Ref3);
                    return age;
                }
                else
                {
                    return null;
                }
            }
        }

        public bool AddUserHobbies(List<Hobbys> hobbys, int userNo)
        {
            bool successfulRemoval = RemoveAllHobbiesFromUser(hobbys, userNo);
            if (successfulRemoval)
            {
                using (var context = new MasContext())
                {
                    foreach (Hobbys hobby in hobbys)
                    {
                        var entity = context.TblHobbyUserMappingTables.FirstOrDefault(e => e.UserNo == userNo && e.HobbyNo == hobby.HobbyNo);
                        if (entity != null)
                        {
                            entity.Active = true;
                        }
                        else
                        {
                            TblHobbyUserMappingTable tblHobbyUserMappingTable = new TblHobbyUserMappingTable()
                            {
                                UserNo = userNo,
                                HobbyNo = hobby.HobbyNo,
                            };
                            context.Add(tblHobbyUserMappingTable);
                        }
                        context.SaveChanges();
                    }
                    return true;
                }
            }
            return false;
            
        }
        private bool RemoveAllHobbiesFromUser(List<Hobbys> hobbys, int userNo)
        {
            using(var context = new MasContext())
            {
                List<TblHobbyUserMappingTable> tblHobbyUserMappingTables = context.TblHobbyUserMappingTables.Where(e => e.UserNo == userNo).ToList();
                foreach(TblHobbyUserMappingTable hobbyMapped in tblHobbyUserMappingTables)
                {
                    hobbyMapped.Active = false;
                    context.Update(hobbyMapped);
                    context.SaveChanges();
                }
                return true;    
            }
        }
        public List<Hobbys?> GetHobbys(int userNo)
        {
            List<Hobbys> hobbies = new List<Hobbys>();
            using(var context =new MasContext())
            {
                List<TblHobbyUserMappingTable> enity = context.TblHobbyUserMappingTables.Where(e => e.UserNo == userNo && e.Active == true).ToList();
                foreach(TblHobbyUserMappingTable hobby in enity)
                {
                    Hobbys selectedHobby = new Hobbys();
                    selectedHobby.HobbyNo = hobby.HobbyNo;
                    hobbies.Add(selectedHobby);
                }
                if(hobbies.Count == 0)
                {
                    return null;
                }
                else
                {
                    return hobbies;
                }
                
            }
        }

        public bool AddUserName(DisplayName display, int userNo)
        {
            GeneralConfigurationValues generalConfigurationValues = new GeneralConfigurationValues();
            generalConfigurationValues.TypeNo = 3;
            generalConfigurationValues.Ref2 = display.userName;

            return _insertIntoGenericReference.AddUpdateGeneralConfiguration(userNo, generalConfigurationValues);

            //using (var context = new MasContext())
            //{
            //    if(display.userName != null)
            //    {
            //        var entity = context.TblUsers.FirstOrDefault(e => e.UserNo == userNo);
            //        if (entity != null)
            //        {
            //            entity.FirstName = display.userName;
            //            context.SaveChanges();
            //            return true;
            //        }
            //        else
            //        {
            //            return false;
            //        }
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
        }
        public DisplayName? GetDisplayName(int userNo)
        {
            DisplayName displayName = new DisplayName();
            using(var context = new MasContext())
            {
                var entity = context.TblGeneralConfigurations.FirstOrDefault(e => e.UserNo == userNo && e.TypeNo == 3);
                if(entity != null)
                {
                    displayName.userName = entity.Ref2;
                    return displayName;
                }
                else
                {
                    return null;
                }
            }
        }

        public bool AddUpdateUserSocial(UserSocial userSocial, int userNo)
        {
            GeneralConfigurationValues generalConfigurationValues = new GeneralConfigurationValues();
            
            switch(userSocial.SocialTypeNo)
            {
                case 1:
                    generalConfigurationValues.TypeNo = 4;
                    generalConfigurationValues.Ref1 = userSocial.SocialValue;
                    break;
                case 2:
                    generalConfigurationValues.TypeNo = 4;
                    generalConfigurationValues.Ref2 = userSocial.SocialValue;
                    break;
                case 3:
                    generalConfigurationValues.TypeNo = 4;
                    generalConfigurationValues.Ref3 = userSocial.SocialValue;
                    break;
                case 4:
                    generalConfigurationValues.TypeNo = 5;
                    generalConfigurationValues.Ref1 = userSocial.SocialValue;
                    break;
            }

            return _insertIntoGenericReference.AddUpdateGeneralConfiguration(userNo , generalConfigurationValues);
        }
        public List<UserSocial?> GetUserSocial(int userNo)
        {
            List<UserSocial> userSocials = new List<UserSocial>();
            using (var context = new MasContext())
            {
                List<TblGeneralConfiguration> tblGeneralConfigurations = context.TblGeneralConfigurations.Where(e => e.UserNo == userNo && e.TypeNo == 4).ToList();
                if (tblGeneralConfigurations.Count != 0)
                {
                    for (int i = 1; i < 4; i++)
                    {
                        UserSocial userSocial = new UserSocial();
                        switch (i)
                        {
                            case 1:
                                userSocial.SocialTypeNo = 1;
                                userSocial.SocialValue = tblGeneralConfigurations[0].Ref1;
                                break;
                            case 2:
                                userSocial.SocialTypeNo = 2;
                                userSocial.SocialValue = tblGeneralConfigurations[0].Ref2;
                                break;
                            case 3:
                                userSocial.SocialTypeNo = 3;
                                userSocial.SocialValue = tblGeneralConfigurations[0].Ref3;
                                break;
                            case 4:
                                userSocial.SocialTypeNo = 4;
                                userSocial.SocialValue = tblGeneralConfigurations[1].Ref1;
                                break;
                        }
                        userSocials.Add(userSocial);
                    }
                    tblGeneralConfigurations = context.TblGeneralConfigurations.Where(e => e.UserNo == userNo && e.TypeNo == 5).ToList();
                    if(tblGeneralConfigurations.Count != 0)
                    {
                        UserSocial userSocial = new UserSocial();
                        userSocial.SocialTypeNo = 4;
                        userSocial.SocialValue = tblGeneralConfigurations[0].Ref1;
                        userSocials.Add(userSocial);
                    }
                    return userSocials;
                }
                else
                {
                    return null;
                }

            }
        }


        //TODO move into its own helper
        //private bool AddUpdateGeneralConfiguration(int userNo, GeneralConfigurationValues generalConfigurationValues)
        //{
        //    using(var context = new MasContext())
        //    {
        //        TblGeneralConfiguration tblGeneralConfiguration = context.TblGeneralConfigurations.FirstOrDefault(e => e.UserNo == userNo && e.TypeNo == generalConfigurationValues.TypeNo);
        //        if(tblGeneralConfiguration != null)
        //        {
        //            tblGeneralConfiguration.Ref1 = IsStringValueNullOrEmpty(generalConfigurationValues.Ref1, tblGeneralConfiguration.Ref1);
        //            tblGeneralConfiguration.Ref2 = IsStringValueNullOrEmpty(generalConfigurationValues.Ref2, tblGeneralConfiguration.Ref2);
        //            tblGeneralConfiguration.Ref3 = IsStringValueNullOrEmpty(generalConfigurationValues.Ref3 , tblGeneralConfiguration.Ref3);
        //            tblGeneralConfiguration.Int1 = IsLongValueNullOrEmpty(generalConfigurationValues.Int1, tblGeneralConfiguration.Int1);
        //            tblGeneralConfiguration.Int2 = IsLongValueNullOrEmpty(generalConfigurationValues.Int2, tblGeneralConfiguration.Int2);
        //            tblGeneralConfiguration.Int3 = IsLongValueNullOrEmpty(generalConfigurationValues.Int3, tblGeneralConfiguration.Int3);
        //            tblGeneralConfiguration.Date1 = IsDateTimeValueNullOrEmpty(generalConfigurationValues.DateTime1, tblGeneralConfiguration.Date1);
        //            tblGeneralConfiguration.Date2 = IsDateTimeValueNullOrEmpty(generalConfigurationValues.DateTime2, tblGeneralConfiguration.Date2);
        //            tblGeneralConfiguration.Date3 = IsDateTimeValueNullOrEmpty(generalConfigurationValues.DateTime3, tblGeneralConfiguration.Date3);  
        //            tblGeneralConfiguration.Active = IsBoolValueNullOrEmpty(generalConfigurationValues.Active, tblGeneralConfiguration.Active);
        //        }
        //        else
        //        {
        //            TblGeneralConfiguration newTblGeneralConfiguration = new TblGeneralConfiguration
        //            {
        //                UserNo = userNo,
        //                TypeNo = generalConfigurationValues.TypeNo,
        //                Ref1 = generalConfigurationValues.Ref1,
        //                Ref2 = generalConfigurationValues.Ref2,
        //                Ref3 = generalConfigurationValues.Ref3,
        //                Int1 = generalConfigurationValues.Int1,
        //                Int2 = generalConfigurationValues.Int2,
        //                Int3 = generalConfigurationValues.Int3,
        //                Date1 = generalConfigurationValues.DateTime1,
        //                Date2 = generalConfigurationValues.DateTime2,
        //                Date3 = generalConfigurationValues.DateTime3,
        //                Active = true
        //            };
        //            context.Add(newTblGeneralConfiguration);
        //        }
        //        context.SaveChanges();
        //        return true;
        //    }
        //}
        //private string? IsStringValueNullOrEmpty(string? value, string? origonalValue)
        //{
        //    if (value == null || value == "")
        //    {
        //        return origonalValue;   
        //    }
        //    else
        //    {
        //        return value;
        //    }      
        //}

        //private long? IsLongValueNullOrEmpty(int? value, long? origonalValue)
        //{
        //    if (value == null)
        //    {
        //        return origonalValue;
        //    }
        //    else
        //    {
        //        return value;
        //    }
        //}

        //private DateTime? IsDateTimeValueNullOrEmpty(DateTime? value, DateTime? origonalValue)
        //{
        //    if (value == null)
        //    {
        //        return origonalValue;
        //    }
        //    else
        //    {
        //        return value;
        //    }
        //}
        //private bool? IsBoolValueNullOrEmpty(bool? value, bool? origonalValue)
        //{
        //    if (value == null)
        //    {
        //        return origonalValue;
        //    }
        //    else
        //    {
        //        return value;
        //    }
        //}

        
    }
}
