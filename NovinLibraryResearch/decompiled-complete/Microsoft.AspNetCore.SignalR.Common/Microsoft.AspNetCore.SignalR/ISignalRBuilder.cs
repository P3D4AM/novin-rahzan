using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.SignalR;

public interface ISignalRBuilder
{
	IServiceCollection Services { get; }
}
