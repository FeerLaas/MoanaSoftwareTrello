using Microsoft.AspNetCore.SignalR;
using MoanaSoftwareTrello.Services;

namespace MoanaSoftwareTrello.SignalRHub
{
    public class MainHub : Hub
    {
        public MainHub()
        {
        }
        public async IAsyncEnumerable<DateTime> Streaming(CancellationToken cancellationToken)
        {

            while (true)
            {
                yield return DateTime.UtcNow;
                await Task.Delay(1000, cancellationToken);
            }
        }
        public async void UpdateCardPos(string id, string from, string to)
        {
            await Clients.All.SendAsync("ReceiveCardPos", id, from, to);
        }
    }
}
