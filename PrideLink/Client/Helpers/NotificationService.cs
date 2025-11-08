
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using PrideLink.Client.Helpers;

public class NotificationService
{
    private readonly NotificationStatus _notificationService;
    private readonly NavigationManager _nav;
    private readonly IToastService _toastService;
    private readonly LoginStatus _loginStatus;
    private readonly FriendStatus _friendStatus;
    public HubConnection HubConnection { get; private set; }
    public bool IsConnected => HubConnection?.State == HubConnectionState.Connected;

    public NotificationService(NotificationStatus notificationService, NavigationManager nav, IToastService toastService, LoginStatus loginStatus, FriendStatus friendStatus)
    {
        _loginStatus = loginStatus;
        _toastService = toastService;
        _notificationService = notificationService;
        _nav = nav;
        _friendStatus = friendStatus;
    }
    public async Task InitializeAsync(string userId, NavigationManager nav)
    {
        Console.WriteLine($"connecting to signalR for userID: {userId}");
        if (HubConnection != null) return; // already initialized

        HubConnection = new HubConnectionBuilder()
            .WithUrl(nav.ToAbsoluteUri($"/notificationhub?userId={userId}"))
            .WithAutomaticReconnect()
            .Build();

        HubConnection.On<string>("UserVerified", (message) =>
        {
            Console.WriteLine($"Notification: {message}");
            // You can also raise an event or invoke a callback here
            _toastService.ShowSuccess(message);
            _loginStatus.NotifyStateChanged();
        });

        HubConnection.On<Dictionary<int, string>>("FriendStatus", (message) =>
        {
            Console.WriteLine($"Notification: {message}");
            // You can also raise an event or invoke a callback here
            _toastService.ShowSuccess(message.First().Value);
            _friendStatus.userNo = message.First().Key;
            _friendStatus.NotifyStateChanged();
        });

        await HubConnection.StartAsync();
        Console.WriteLine("Connected to SignalR hub!");
    }
}
