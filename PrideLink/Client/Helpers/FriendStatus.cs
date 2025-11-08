namespace PrideLink.Client.Helpers
{
    public class FriendStatus
    {
        public event Action? OnChange;
        public int userNo { get; set; }
        public void NotifyStateChanged() => OnChange?.Invoke();
    }
}
