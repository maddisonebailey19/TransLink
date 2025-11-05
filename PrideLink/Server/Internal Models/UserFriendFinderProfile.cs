namespace PrideLink.Server.Internal_Models
{
    public class UserFriendFinderProfile
    {
        public int UserNo { get; set; }

        // General Info
        public string? BioDescription { get; set; }
        public string? DisplayName { get; set; }
        public string? Age { get; set; }
        public int? RelationshipStatusNo { get; set; }
        public string? RelationshipStatusName { get; set; }

        // Pictures
        public string? Picture1 { get; set; }
        public string? Picture2 { get; set; }
        public string? Picture3 { get; set; }

        // Socials
        public string? Instagram { get; set; }
        public string? Snapchat { get; set; }
        public string? WhatsApp { get; set; }
        public string? Discord { get; set; }
    }
}
