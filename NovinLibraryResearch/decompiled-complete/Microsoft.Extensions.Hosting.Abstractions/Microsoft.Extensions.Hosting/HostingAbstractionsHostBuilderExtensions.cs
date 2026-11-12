using System.Threading;

namespace Microsoft.Extensions.Hosting;

public static class HostingAbstractionsHostBuilderExtensions
{
	public static IHost Start(this IHostBuilder hostBuilder)
	{
		IHost host = hostBuilder.Build();
		host.StartAsync(CancellationToken.None).GetAwaiter().GetResult();
		return host;
	}
}
