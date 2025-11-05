using PrideLink.Server.Controllers;
using PrideLink.Server.Interfaces;
using PrideLink.Server.Internal_Models;
using PrideLink.Server.TransLinkDataBase;
using PrideLink.Shared.LoginDetails;

namespace PrideLink.Server.Helpers
{
    public class LoginDetailsHelper : ILoginInterface
    {
        private readonly PasswordHelper _passwordHelper;
        public LoginDetailsHelper(PasswordHelper passwordHelper)
        {
            _passwordHelper = passwordHelper;
        }
        public string? CheckLoginCred(string userName, string password)
        {
            using(var context = new MasContext())
            {
                string cleanedUserName = userName.Split("@")[0];
                var entity = context.TblUsers.FirstOrDefault(e => e.Login == cleanedUserName);
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

        public int CreateLogin(string userName, string password)
        {
            using(var context = new MasContext())
            {
                var entity = context.TblUsers.FirstOrDefault(e => e.Login == userName);
                if(entity == null)
                {
                    TblUser tblUser = new TblUser
                    {
                        UserId = Guid.NewGuid().ToString(),
                        Login = userName.Split("@")[0],
                        Password = _passwordHelper.HashPassword(password),
                        UserType = 3,
                        Email = userName,
                        Active = true,
                    };
                    context.Add(tblUser);
                    context.SaveChanges();

                    int newUserID = tblUser.UserNo;

                    TblUserRoleMappingTable tblUserRoleMappingTable = new TblUserRoleMappingTable
                    {

                        UserRoleTypeNo = 2,
                        UserNo = newUserID,
                        RoleAddedByUserNo = 1
                    };

                    context.Add(tblUserRoleMappingTable); 
                    context.SaveChanges();
                    
                    return newUserID;
                }
                return -1;

            }
        }
        public List<UserRoles> GetRoles(string userID)
        {
            List<UserRoles> roles = new List<UserRoles>();
            using(var context = new MasContext())
            {

                List<TblUserRoleMappingTable> mappingRoles = context.TblUserRoleMappingTables
                                                                            .Where(r => r.UserNo == context.TblUsers
                                                                            .Where(u => u.UserId == userID)
                                                                            .Select(u => u.UserNo)
                                                                            .FirstOrDefault())
                                                                            .ToList();

                foreach (var mappepedRole in mappingRoles)
                {
                    TblUserRoleType roleType = context.TblUserRoleTypes.FirstOrDefault(e => e.UserRoleTypeNo == mappepedRole.UserRoleTypeNo);
                    UserRoles role = new UserRoles();
                    role.roleID = roleType.UserRoleTypeNo;
                    role.roleName = roleType.UserRoleName;
                    roles.Add(role);
                }
            }
            return roles;
        }
        
    }
}
