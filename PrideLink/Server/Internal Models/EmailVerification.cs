namespace PrideLink.Server.Internal_Models
{
    public class EmailVerification
    {
        public string userID { get; set; }
        public int verificationCode { get; set; }
    }
}
