using System;

namespace Microsoft.AspNetCore.SignalR.Protocol;

public abstract class HubMethodInvocationMessage : HubInvocationMessage
{
	public string Target { get; }

	public object?[] Arguments { get; }

	public string[]? StreamIds { get; }

	protected HubMethodInvocationMessage(string? invocationId, string target, object?[] arguments, string[]? streamIds)
		: this(invocationId, target, arguments)
	{
		StreamIds = streamIds;
	}

	protected HubMethodInvocationMessage(string? invocationId, string target, object?[] arguments)
		: base(invocationId)
	{
		if (string.IsNullOrEmpty(target))
		{
			throw new ArgumentNullException("target");
		}
		Target = target;
		Arguments = arguments;
	}
}
