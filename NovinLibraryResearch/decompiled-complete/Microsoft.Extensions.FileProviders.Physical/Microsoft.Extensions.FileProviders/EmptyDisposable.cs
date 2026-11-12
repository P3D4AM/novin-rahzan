using System;

namespace Microsoft.Extensions.FileProviders;

internal sealed class EmptyDisposable : IDisposable
{
	public static Microsoft.Extensions.FileProviders.EmptyDisposable Instance { get; } = new Microsoft.Extensions.FileProviders.EmptyDisposable();


	private EmptyDisposable()
	{
	}

	public void Dispose()
	{
	}
}
