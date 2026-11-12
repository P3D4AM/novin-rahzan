using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting;

public static class HostingAbstractionsHostExtensions
{
	public static void Start(this IHost host)
	{
		host.StartAsync().GetAwaiter().GetResult();
	}

	public static Task StopAsync(this IHost host, TimeSpan timeout)
	{
		return host.StopAsync(new CancellationTokenSource(timeout).Token);
	}

	public static void WaitForShutdown(this IHost host)
	{
		host.WaitForShutdownAsync().GetAwaiter().GetResult();
	}

	public static void Run(this IHost host)
	{
		host.RunAsync().GetAwaiter().GetResult();
	}

	public static async Task RunAsync(this IHost host, CancellationToken token = default(CancellationToken))
	{
		using (host)
		{
			await host.StartAsync(token);
			await host.WaitForShutdownAsync(token);
		}
	}

	public static async Task WaitForShutdownAsync(this IHost host, CancellationToken token = default(CancellationToken))
	{
		IApplicationLifetime service = host.Services.GetService<IApplicationLifetime>();
		token.Register(delegate(object state)
		{
			((IApplicationLifetime)state).StopApplication();
		}, service);
		TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);
		service.ApplicationStopping.Register(delegate(object obj)
		{
			((TaskCompletionSource<object>)obj).TrySetResult(null);
		}, taskCompletionSource);
		await taskCompletionSource.Task;
		await host.StopAsync();
	}
}
