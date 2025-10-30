using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrideLink.Shared.FreindFinderDetails
{
    public class UserFreindFinderAccount
    {
        public int userNo { get; set; }
        public UserAccountGeneralInfo? UserAccountGeneralInfo { get; set; }
        public UserAccountRelashionshipStatus? UserAccountRelashionshipStatus { get; set; }
        public List<UserAccountSocials?> UserAccountSocials { get; set; }
        public List<UserAccountPictures?> UserAccountPictures { get; set; }
        public List<UserAccountHobbies?> UserAccountHobbies { get; set; }
    }
}
