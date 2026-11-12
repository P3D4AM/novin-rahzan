using System.Threading;

namespace Microsoft.Extensions.Hosting;

public interface IApplicationLifetime
{
	CancellationToken ApplicationStarted { get; }

	CancellationToken ApplicationStopping { get; }

	CancellationToken ApplicationStopped { get; }

	void StopApplication();
}
