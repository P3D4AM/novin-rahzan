using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.SignalR.Internal;

internal class SignalRServerBuilder : ISignalRServerBuilder, ISignalRBuilder
{
	public IServiceCollection Services { get; }

	public SignalRServerBuilder(IServiceCollection services)
	{
		Services = services;
	}
}
