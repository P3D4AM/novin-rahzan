using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace System.Text.Json;

internal static class ThrowHelper
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	[DoesNotReturn]
	public static void ThrowOutOfMemoryException_BufferMaximumSizeExceeded(uint capacity)
	{
		throw new OutOfMemoryException(System.SR.Format(System.SR.BufferMaximumSizeExceeded, capacity));
	}
}
