using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Protocol;

namespace Microsoft.AspNetCore.SignalR.Internal;

public abstract class HubDispatcher<THub> : IInvocationBinder where THub : Hub
{
	public abstract Task OnConnectedAsync(HubConnectionContext connection);

	public abstract Task OnDisconnectedAsync(HubConnectionContext connection, Exception exception);

	public abstract Task DispatchMessageAsync(HubConnectionContext connection, HubMessage hubMessage);

	public abstract IReadOnlyList<Type> GetParameterTypes(string methodName);

	public abstract Type GetReturnType(string invocationId);
}
