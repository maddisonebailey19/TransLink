namespace PrideLink.Client.Helpers
{
    public class NotificationStatus
    {
        public event Action<string> OnNotify;

        public void Notify(string message)
        {
            OnNotify?.Invoke(message);
        }
    }
}
