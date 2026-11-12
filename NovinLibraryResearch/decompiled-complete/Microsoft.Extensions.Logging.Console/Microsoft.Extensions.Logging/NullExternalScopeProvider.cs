using System;

namespace Microsoft.Extensions.Logging;

internal sealed class NullExternalScopeProvider : IExternalScopeProvider
{
	public static IExternalScopeProvider Instance { get; } = new Microsoft.Extensions.Logging.NullExternalScopeProvider();


	private NullExternalScopeProvider()
	{
	}

	void IExternalScopeProvider.ForEachScope<TState>(Action<object, TState> callback, TState state)
	{
	}

	IDisposable IExternalScopeProvider.Push(object state)
	{
		return Microsoft.Extensions.Logging.NullScope.Instance;
	}
}
