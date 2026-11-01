using System.Buffers;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.WebSockets;

internal static class ManagedWebSocketExtensions
{
	internal unsafe static string GetString(this UTF8Encoding encoding, Span<byte> bytes)
	{
		fixed (byte* bytes2 = &MemoryMarshal.GetReference(bytes))
		{
			return encoding.GetString(bytes2, bytes.Length);
		}
	}

	internal static ValueTask<int> ReadAsync(this Stream stream, Memory<byte> destination, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (MemoryMarshal.TryGetArray((ReadOnlyMemory<byte>)destination, out ArraySegment<byte> segment))
		{
			return new ValueTask<int>(stream.ReadAsync(segment.Array, segment.Offset, segment.Count, cancellationToken));
		}
		byte[] array = ArrayPool<byte>.Shared.Rent(destination.Length);
		return new ValueTask<int>(FinishReadAsync(stream.ReadAsync(array, 0, destination.Length, cancellationToken), array, destination));
		static async Task<int> FinishReadAsync(Task<int> readTask, byte[] localBuffer, Memory<byte> localDestination)
		{
			try
			{
				int num = await readTask.ConfigureAwait(continueOnCapturedContext: false);
				new Span<byte>(localBuffer, 0, num).CopyTo(localDestination.Span);
				return num;
			}
			finally
			{
				ArrayPool<byte>.Shared.Return(localBuffer);
			}
		}
	}

	internal static ValueTask WriteAsync(this Stream stream, ReadOnlyMemory<byte> source, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (MemoryMarshal.TryGetArray(source, out var segment))
		{
			return new ValueTask(stream.WriteAsync(segment.Array, segment.Offset, segment.Count, cancellationToken));
		}
		byte[] array = ArrayPool<byte>.Shared.Rent(source.Length);
		source.Span.CopyTo(array);
		return new ValueTask(FinishWriteAsync(stream.WriteAsync(array, 0, source.Length, cancellationToken), array));
		static async Task FinishWriteAsync(Task writeTask, byte[] localBuffer)
		{
			try
			{
				await writeTask.ConfigureAwait(continueOnCapturedContext: false);
			}
			finally
			{
				ArrayPool<byte>.Shared.Return(localBuffer);
			}
		}
	}
}
