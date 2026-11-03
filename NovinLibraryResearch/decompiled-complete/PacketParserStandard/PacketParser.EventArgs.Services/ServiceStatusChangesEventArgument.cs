using System.Threading;
using PacketParser.Entities;

namespace PacketParser.EventArgs.Services;

public class ServiceStatusChangesEventArgument
{
	public ServiceStatus NewStatus { get; private set; }

	public ServiceStatus OldStatus { get; private set; }

	public CancellationToken Token { get; private set; }

	public ServiceStatusChangesEventArgument(ServiceStatus oldStatus, ServiceStatus newStatus, CancellationToken token)
	{
		OldStatus = oldStatus;
		NewStatus = newStatus;
		Token = token;
	}
}
