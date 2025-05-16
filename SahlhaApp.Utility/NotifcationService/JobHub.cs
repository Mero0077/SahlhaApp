using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Utility.NotifcationService
{
    //[Authorize]
    public class JobHub : Hub
    {

        public async Task SendNotification(string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", message);
        }


        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public override async Task OnConnectedAsync() // upon client connecting to our Hub Log or set up session
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine($"🟢 SignalR Connected: {userId}");

            foreach (var claim in Context.User?.Claims ?? Enumerable.Empty<Claim>())
            {
                Console.WriteLine($"CLAIM: {claim.Type} = {claim.Value}");
            }

            await base.OnConnectedAsync();
        }


        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.FindFirst("nameid")?.Value;
            Console.WriteLine($"🔴 SignalR Disconnected: {userId}");

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
