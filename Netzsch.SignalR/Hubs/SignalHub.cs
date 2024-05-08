using Microsoft.AspNetCore.SignalR;

namespace Netzsch.SignalR.Hubs
{
    public class SignalHub : Hub
    {
        public async Task SendWpfInput(string input)
        {
            await Clients.All.SendAsync("SendWpfInput", input);
        }

        public async Task SendWebInput(string input)
        {
            await Clients.All.SendAsync("SendWebInput", input);
        }
    }
}
