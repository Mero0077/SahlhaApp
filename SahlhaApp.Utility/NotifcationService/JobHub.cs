using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Utility.NotifcationService
{
    [Authorize]
    public class JobHub: Hub
    {
        public override async Task OnConnectedAsync() // upon client connecting to our Hub Log or set up session
        {
            var userId = Context.UserIdentifier; // This gets the logged-in user's ID
            Console.WriteLine($"User connected: {userId}");

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.UserIdentifier;
            Console.WriteLine($"User disconnected: {userId}");

            await base.OnDisconnectedAsync(exception);
        }

        // Optional: Clients can call this to join a group group targeting
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        // Optional: Clients can call this to leave a group
        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
