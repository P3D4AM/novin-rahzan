using System;

namespace Microsoft.Extensions.Logging;

internal sealed class NullScope : IDisposable
{
	public static Microsoft.Extensions.Logging.NullScope Instance { get; } = new Microsoft.Extensions.Logging.NullScope();


	private NullScope()
	{
	}

	public void Dispose()
	{
	}
}
