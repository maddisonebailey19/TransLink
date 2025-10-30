using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrideLink.Shared.LoginDetails
{
    public class jwtToken
    {
        public string token { get; set; }
        public DateTime expireDate { get; set; }
    }
}
