using Backend.Application.Abstract;
using Backend.Infrastructure.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Infrastructure.Concrete
{
    public class DataHubContext : IDataHub
    {
        private readonly IHubContext<DataHub> _hub;
        public DataHubContext(IHubContext<DataHub> hub)
        {
            _hub = hub;
        }
        public async Task SendNotification(string text)
        {
            await _hub.Clients.All.SendAsync("ReceiveNote",text);
        }
    }
}
