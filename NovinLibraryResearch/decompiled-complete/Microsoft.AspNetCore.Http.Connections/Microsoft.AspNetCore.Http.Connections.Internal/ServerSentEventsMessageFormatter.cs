using System;
using System.Buffers;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Http.Connections.Internal;

public static class ServerSentEventsMessageFormatter
{
	private static readonly byte[] DataPrefix = new byte[6] { 100, 97, 116, 97, 58, 32 };

	private static readonly byte[] Newline = new byte[2] { 13, 10 };

	private const byte LineFeed = 10;

	public static async Task WriteMessageAsync(ReadOnlySequence<byte> payload, Stream output)
	{
		if (!payload.PositionOf<byte>(10).HasValue)
		{
			if (payload.Length > 0)
			{
				await output.WriteAsync(DataPrefix, 0, DataPrefix.Length);
				await output.WriteAsync(payload);
				await output.WriteAsync(Newline, 0, Newline.Length);
			}
			await output.WriteAsync(Newline, 0, Newline.Length);
		}
		else
		{
			MemoryStream ms = new MemoryStream();
			await WriteMessageToMemory(ms, payload);
			ms.Position = 0L;
			await ms.CopyToAsync(output);
		}
	}

	private static ReadOnlyMemory<byte> GetLastSegment(in ReadOnlySequence<byte> source, out long offset)
	{
		offset = 0L;
		long length = source.Length;
		SequencePosition position = source.Start;
		ReadOnlyMemory<byte> memory;
		while (source.TryGet(ref position, out memory))
		{
			if (offset + memory.Length >= length)
			{
				return memory;
			}
			offset += memory.Length;
		}
		throw new InvalidOperationException("Could not get last segment from sequence.");
	}

	private static async Task WriteMessageToMemory(Stream output, ReadOnlySequence<byte> payload)
	{
		bool keepWriting = true;
		while (keepWriting)
		{
			SequencePosition? sequencePosition = payload.PositionOf<byte>(10);
			ReadOnlySequence<byte> lineSegment;
			if (!sequencePosition.HasValue)
			{
				lineSegment = payload;
				payload = ReadOnlySequence<byte>.Empty;
				keepWriting = false;
			}
			else
			{
				lineSegment = payload.Slice(payload.Start, sequencePosition.Value);
				if (lineSegment.Length > 1)
				{
					long offset;
					ReadOnlyMemory<byte> lastSegment = GetLastSegment(in lineSegment, out offset);
					if (lastSegment.Span[lastSegment.Length - 1] == 13)
					{
						lineSegment = lineSegment.Slice(lineSegment.Start, offset + lastSegment.Length - 1);
					}
				}
				payload = payload.Slice(payload.GetPosition(1L, sequencePosition.Value));
			}
			await output.WriteAsync(DataPrefix, 0, DataPrefix.Length);
			await output.WriteAsync(lineSegment);
			await output.WriteAsync(Newline, 0, Newline.Length);
		}
		await output.WriteAsync(Newline, 0, Newline.Length);
	}
}
