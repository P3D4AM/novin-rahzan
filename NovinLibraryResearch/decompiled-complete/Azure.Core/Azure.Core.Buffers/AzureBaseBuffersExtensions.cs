using System;
using System.Buffers;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Azure.Core.Buffers;

internal static class AzureBaseBuffersExtensions
{
	public static async Task WriteAsync(this Stream stream, ReadOnlyMemory<byte> buffer, CancellationToken cancellation = default(CancellationToken))
	{
		Argument.AssertNotNull(stream, "stream");
		if (buffer.Length == 0)
		{
			return;
		}
		byte[] array = null;
		try
		{
			if (MemoryMarshal.TryGetArray(buffer, out var segment))
			{
				await stream.WriteAsync(segment.Array, segment.Offset, segment.Count, cancellation).ConfigureAwait(continueOnCapturedContext: false);
				return;
			}
			array = ArrayPool<byte>.Shared.Rent(buffer.Length);
			if (!buffer.TryCopyTo(array))
			{
				throw new Exception("could not rent large enough buffer.");
			}
			await stream.WriteAsync(array, 0, buffer.Length, cancellation).ConfigureAwait(continueOnCapturedContext: false);
		}
		finally
		{
			if (array != null)
			{
				ArrayPool<byte>.Shared.Return(array);
			}
		}
	}

	public static async Task WriteAsync(this Stream stream, ReadOnlySequence<byte> buffer, CancellationToken cancellation = default(CancellationToken))
	{
		Argument.AssertNotNull(stream, "stream");
		if (buffer.Length == 0L)
		{
			return;
		}
		byte[] array = null;
		try
		{
			ReadOnlySequence<byte>.Enumerator enumerator = buffer.GetEnumerator();
			while (enumerator.MoveNext())
			{
				ReadOnlyMemory<byte> current = enumerator.Current;
				if (MemoryMarshal.TryGetArray(current, out var segment))
				{
					await stream.WriteAsync(segment.Array, segment.Offset, segment.Count, cancellation).ConfigureAwait(continueOnCapturedContext: false);
					continue;
				}
				if (array == null || array.Length < current.Length)
				{
					if (array != null)
					{
						ArrayPool<byte>.Shared.Return(array);
					}
					array = ArrayPool<byte>.Shared.Rent(current.Length);
				}
				if (!current.TryCopyTo(array))
				{
					throw new Exception("could not rent large enough buffer.");
				}
				await stream.WriteAsync(array, 0, current.Length, cancellation).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		finally
		{
			if (array != null)
			{
				ArrayPool<byte>.Shared.Return(array);
			}
		}
	}
}
