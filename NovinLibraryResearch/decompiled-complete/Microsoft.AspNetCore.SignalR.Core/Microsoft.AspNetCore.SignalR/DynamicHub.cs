namespace Microsoft.AspNetCore.SignalR;

public abstract class DynamicHub : Hub
{
	private DynamicHubClients _clients;

	public new DynamicHubClients Clients
	{
		get
		{
			if (_clients == null)
			{
				_clients = new DynamicHubClients(base.Clients);
			}
			return _clients;
		}
		set
		{
			_clients = value;
		}
	}
}
