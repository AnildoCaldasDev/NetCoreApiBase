using Microsoft.AspNetCore.SignalR;
using NetCoreApiBase.Domain.Models;
using System.Threading.Tasks;

namespace NetCoreApiBase.Api.Hubs
{
    public class ChatMessageHub : Hub
    {
        public async Task NewMessage(Message msg)
        {
            await Clients.All.SendAsync("MessageReceived", msg);
        }
    }
}
