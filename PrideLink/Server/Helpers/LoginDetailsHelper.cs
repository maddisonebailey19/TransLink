using PrideLink.Server.Controllers;
using PrideLink.Server.Interfaces;
using PrideLink.Server.Internal_Models;
using PrideLink.Server.TransLinkDataBase;
using PrideLink.Shared.LoginDetails;

namespace PrideLink.Server.Helpers
{
    public class LoginDetailsHelper : ILoginInterface
    {
        public string? CheckLoginCred(string userName, string password)
        {
            using(var context = new MasContext())
            {
                var entity = context.TblUsers.FirstOrDefault(e => e.Login == userName && e.Password == password);
                if(entity != null)
                {
                    return entity.UserId;
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
                        Login = userName,
                        Password = password,
                        UserType = 3,
                        Active = true,
                    };
                    context.Add(tblUser);
                    context.SaveChanges();

                    int newUserID = tblUser.UserNo;

                    TblUserRoleMappingTable tblUserRoleMappingTable = new TblUserRoleMappingTable
                    {

                        UserRoleTypeNo = 2,
                        UserNo = newUserID,
                        RoleAddedByUserNo = 8
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
