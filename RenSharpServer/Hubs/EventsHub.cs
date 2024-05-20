using Microsoft.AspNetCore.SignalR;

namespace RenSharpServer.Hubs
{
	public class EventsHub : Hub
	{
		public async Task Delete(int id)
		{
			await Clients.All.SendAsync("Delte", id);
		}

		public async Task Create(int id)
		{
			await Clients.All.SendAsync("Create", id);
		}
	}
}
