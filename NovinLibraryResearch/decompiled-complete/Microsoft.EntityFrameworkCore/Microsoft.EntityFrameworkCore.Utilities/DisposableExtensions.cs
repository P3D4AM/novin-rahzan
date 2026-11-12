using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Utilities;

internal static class DisposableExtensions
{
	public static ValueTask DisposeAsyncIfAvailable([JetBrains.Annotations.CanBeNull] this IDisposable disposable)
	{
		if (disposable != null)
		{
			if (disposable is IAsyncDisposable asyncDisposable)
			{
				return asyncDisposable.DisposeAsync();
			}
			disposable.Dispose();
		}
		return default(ValueTask);
	}
}
