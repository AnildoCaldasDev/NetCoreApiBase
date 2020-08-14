using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreApiBase.Api.Hubs
{
    public class RealtimeBrokerHub : Hub
    {

        public Task ConnectToStock(string symbol)
        {
            Groups.AddToGroupAsync(Context.ConnectionId, symbol);
            return Task.CompletedTask;
        }

    }
}
