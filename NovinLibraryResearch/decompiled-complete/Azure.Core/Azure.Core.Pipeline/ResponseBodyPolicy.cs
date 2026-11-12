using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core.Buffers;

namespace Azure.Core.Pipeline;

internal class ResponseBodyPolicy : HttpPipelinePolicy
{
	private const int DefaultCopyBufferSize = 81920;

	private readonly TimeSpan _networkTimeout;

	public ResponseBodyPolicy(TimeSpan networkTimeout)
	{
		_networkTimeout = networkTimeout;
	}

	public override async ValueTask ProcessAsync(HttpMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline)
	{
		await ProcessAsync(message, pipeline, async: true).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override void Process(HttpMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline)
	{
		ProcessAsync(message, pipeline, async: false).EnsureCompleted();
	}

	private async ValueTask ProcessAsync(HttpMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline, bool async)
	{
		CancellationToken oldToken = message.CancellationToken;
		using CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(new CancellationToken[1] { oldToken });
		cts.CancelAfter(_networkTimeout);
		try
		{
			message.CancellationToken = cts.Token;
			if (async)
			{
				await HttpPipelinePolicy.ProcessNextAsync(message, pipeline).ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				HttpPipelinePolicy.ProcessNext(message, pipeline);
			}
		}
		finally
		{
			message.CancellationToken = oldToken;
			cts.CancelAfter(-1);
		}
		Stream responseContentStream = message.Response.ContentStream;
		if (responseContentStream == null || responseContentStream.CanSeek)
		{
			return;
		}
		if (message.BufferResponse)
		{
			if (_networkTimeout != Timeout.InfiniteTimeSpan)
			{
				cts.Token.Register(delegate(object state)
				{
					((Stream)state)?.Dispose();
				}, responseContentStream);
			}
			try
			{
				MemoryStream bufferedStream = new MemoryStream();
				if (async)
				{
					await CopyToAsync(responseContentStream, bufferedStream, cts).ConfigureAwait(continueOnCapturedContext: false);
				}
				else
				{
					CopyTo(responseContentStream, bufferedStream, cts);
				}
				responseContentStream.Dispose();
				bufferedStream.Position = 0L;
				message.Response.ContentStream = bufferedStream;
				return;
			}
			catch (ObjectDisposedException)
			{
				cts.Token.ThrowIfCancellationRequested();
				throw;
			}
		}
		if (_networkTimeout != Timeout.InfiniteTimeSpan)
		{
			message.Response.ContentStream = new ReadTimeoutStream(responseContentStream, _networkTimeout);
		}
	}

	private async Task CopyToAsync(Stream source, Stream destination, CancellationTokenSource cancellationTokenSource)
	{
		byte[] buffer = ArrayPool<byte>.Shared.Rent(81920);
		try
		{
			while (true)
			{
				cancellationTokenSource.CancelAfter(_networkTimeout);
				int num = await source.ReadAsync(buffer, 0, buffer.Length, cancellationTokenSource.Token).ConfigureAwait(continueOnCapturedContext: false);
				if (num == 0)
				{
					break;
				}
				await destination.WriteAsync(new ReadOnlyMemory<byte>(buffer, 0, num), cancellationTokenSource.Token).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		finally
		{
			cancellationTokenSource.CancelAfter(Timeout.InfiniteTimeSpan);
			ArrayPool<byte>.Shared.Return(buffer);
		}
	}

	private void CopyTo(Stream source, Stream destination, CancellationTokenSource cancellationTokenSource)
	{
		byte[] array = ArrayPool<byte>.Shared.Rent(81920);
		try
		{
			int count;
			while ((count = source.Read(array, 0, array.Length)) != 0)
			{
				cancellationTokenSource.Token.ThrowIfCancellationRequested();
				cancellationTokenSource.CancelAfter(_networkTimeout);
				destination.Write(array, 0, count);
			}
		}
		finally
		{
			cancellationTokenSource.CancelAfter(Timeout.InfiniteTimeSpan);
			ArrayPool<byte>.Shared.Return(array);
		}
	}
}
