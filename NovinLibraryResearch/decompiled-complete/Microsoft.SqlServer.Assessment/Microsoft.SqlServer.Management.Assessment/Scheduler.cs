using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.SqlServer.Management.Assessment;

internal class Scheduler
{
	protected ConcurrentDictionary<ISqlObjectLocator, SemaphoreSlim> DispatchPost { get; } = new ConcurrentDictionary<ISqlObjectLocator, SemaphoreSlim>(new SqlLocatorAccessPointComparer());


	public async Task<TResult> Run<TRequest, TResult>(TRequest request, Func<TRequest, Task<TResult>> procedure) where TRequest : ITargetedObject
	{
		if (request == null)
		{
			throw new ArgumentNullException("request");
		}
		if (procedure == null)
		{
			throw new ArgumentNullException("procedure");
		}
		SemaphoreSlim semaphore = DispatchPost.GetOrAdd(request.Target, (ISqlObjectLocator s) => new SemaphoreSlim(1, 1));
		await semaphore.WaitAsync().ConfigureAwait(continueOnCapturedContext: false);
		try
		{
			return await procedure(request).ConfigureAwait(continueOnCapturedContext: false);
		}
		finally
		{
			semaphore.Release();
		}
	}
}
