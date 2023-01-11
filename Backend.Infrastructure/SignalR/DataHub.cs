using Microsoft.AspNetCore.SignalR;

namespace Backend.Infrastructure.SignalR
{
    public class DataHub:Hub
    {
        public async Task SendNotification(string name,string text)
        {
            await Clients.All.SendAsync("recaiveNotification",name,text);
        }
    }
}
