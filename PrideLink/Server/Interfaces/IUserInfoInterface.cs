using PrideLink.Server.TransLinkDataBase;
using PrideLink.Shared.FreindFinderDetails;
using PrideLink.Shared.General;
using PrideLink.Shared.UserInfo;

namespace PrideLink.Server.Interfaces
{
    public interface IUserInfoInterface
    {
        public List<TblGeneralConfiguration> GetUserGeneralConfiguration(List<int> userNos);
        public List<TblUser> GetTblUsers(List<int> userNos);
        public List<TblHobbyUserMappingTable> GetUserHobbyUserMappingTable(List<int> userNos);
        public List<TblHobby> GetUserHobbies();
        public List<TblRelationshipStatusType> GetUserRelationshipStatusTypes();





        public List<UserAccountGeneralInfo> GetUserAccountGeneralInfo(List<int> userNos);
        public List<UserAccountRelashionshipStatus> GetUserAccountRelashionshipStatus(List<int> userNos);





        public bool AddRemoveEmailFromUser(Email email, int userNo);

        public string GetUserVerificationStatus(int userNo);

        public bool AddUserPicture(UserPicture picture, int userNo); 
        public UserPicture GetUserPicture(UserPicture userPicture, int userNo);

        public bool AddRelationshipStatus(UserRelationshipStatusData relationshipStatus, int userNo);
        public UserRelationshipStatusData? GetUserRelationshipStatus(int userNo);
        public List<UserRelationshipStatusData?> GetRelationshipStatuses();

        public bool AddUserBioDescription(UserBioDescriptionData bioDescription, int userNo);
        public UserBioDescriptionData GetUserBioDescription(int userNo);

        public bool AddUserAge(Age age, int userNo);
        public Age? GetUserAge(int userNo);

        public bool AddUserHobbies(List<Hobbys> hobbys, int userNo);
        public List<Hobbys?> GetHobbys(int userNo);

        public bool AddUserName(DisplayName display, int userNo);
        public DisplayName? GetDisplayName(int userNo);

        public bool AddUpdateUserSocial(UserSocial userSocial, int userNo);
        public List<UserSocial?> GetUserSocial(int userNo);
    }
}
