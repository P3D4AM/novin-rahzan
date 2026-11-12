using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup;

internal static class ThrowHelper
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	[DoesNotReturn]
	internal static void ThrowObjectDisposedException()
	{
		throw new ObjectDisposedException("IServiceProvider");
	}
}
