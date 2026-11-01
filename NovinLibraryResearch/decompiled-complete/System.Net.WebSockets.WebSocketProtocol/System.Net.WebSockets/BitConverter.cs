using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Net.WebSockets;

internal static class BitConverter
{
	internal static int ToInt32(ReadOnlySpan<byte> value)
	{
		return Unsafe.ReadUnaligned<int>(ref MemoryMarshal.GetReference(value));
	}
}
