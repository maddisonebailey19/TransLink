using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrideLink.Shared.Notification
{
    public class NotificationContent
    {
        public string ToEmail { get; set; }
        public int EmailContentNo { get; set; }
        public string Subject { get; set; }
        public Dictionary<string, string> EmailContents { get; set; }
    }
}
