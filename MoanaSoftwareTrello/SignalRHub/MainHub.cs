using Microsoft.AspNetCore.SignalR;

namespace MoanaSoftwareTrello.SignalRHub
{
    public class MainHub :Hub
    {
        public async IAsyncEnumerable<DateTime> Streaming(CancellationToken cancellationToken)
        {
            while (true)
            {
                yield return DateTime.UtcNow;
                await Task.Delay(1000, cancellationToken);
            }
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public async void SendUpdate(string name, string message, CancellationToken cancellationToken)
        {
            await Clients.All.SendCoreAsync(name, new object?[] { message }, cancellationToken);
        }
    }
}
