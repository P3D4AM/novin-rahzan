using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionHostedServiceExtensions
{
	public static IServiceCollection AddHostedService<THostedService>(this IServiceCollection services) where THostedService : class, IHostedService
	{
		return services.AddTransient<IHostedService, THostedService>();
	}
}
