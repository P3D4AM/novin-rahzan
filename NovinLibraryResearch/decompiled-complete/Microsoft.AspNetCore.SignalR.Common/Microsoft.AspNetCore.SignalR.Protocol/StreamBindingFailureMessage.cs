using System.Runtime.ExceptionServices;

namespace Microsoft.AspNetCore.SignalR.Protocol;

public class StreamBindingFailureMessage : HubMessage
{
	public string Id { get; }

	public ExceptionDispatchInfo BindingFailure { get; }

	public StreamBindingFailureMessage(string id, ExceptionDispatchInfo bindingFailure)
	{
		Id = id;
		BindingFailure = bindingFailure;
	}
}
