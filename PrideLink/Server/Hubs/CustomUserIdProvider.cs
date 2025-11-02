using Microsoft.AspNetCore.SignalR;

public class CustomUserIdProvider : IUserIdProvider
{
    public string GetUserId(HubConnectionContext connection)
    {
        // Read the userId from the query string
        return connection.GetHttpContext()?.Request.Query["userId"];
    }
}
