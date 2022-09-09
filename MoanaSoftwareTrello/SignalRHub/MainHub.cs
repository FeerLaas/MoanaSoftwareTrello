using Microsoft.AspNetCore.SignalR;
using MoanaSoftwareTrello.Services;

namespace MoanaSoftwareTrello.SignalRHub
{
    public class MainHub : Hub
    {
        public MainHub()
        {
        }
        public async void UpdateCardPos(string id, string from, string to)
        {
            await Clients.All.SendAsync("ReceiveCardPos", id, from, to);
        }
    }
}
