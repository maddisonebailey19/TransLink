namespace PrideLink.Client.Helpers
{
    public class LoginStatus
    {
        public event Action? OnChange;
        public string userNo { set; get; }
        public void NotifyStateChanged() => OnChange?.Invoke();
    }
}
