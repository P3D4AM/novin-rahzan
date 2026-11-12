using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Azure.Core.Pipeline;

internal class ReadTimeoutStream : ReadOnlyStream
{
	private readonly Stream _stream;

	private TimeSpan _readTimeout;

	private CancellationTokenSource _cancellationTokenSource;

	public override bool CanRead => _stream.CanRead;

	public override bool CanSeek => _stream.CanSeek;

	public override long Length => _stream.Length;

	public override long Position
	{
		get
		{
			return _stream.Position;
		}
		set
		{
			_stream.Position = value;
		}
	}

	public override int ReadTimeout
	{
		get
		{
			return (int)_readTimeout.TotalMilliseconds;
		}
		set
		{
			_readTimeout = TimeSpan.FromMilliseconds(value);
			UpdateReadTimeout();
		}
	}

	public ReadTimeoutStream(Stream stream, TimeSpan readTimeout)
	{
		_stream = stream;
		_readTimeout = readTimeout;
		UpdateReadTimeout();
		InitializeTokenSource();
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		bool dispose;
		CancellationTokenSource cancellationTokenSource = StartTimeout(default(CancellationToken), out dispose);
		try
		{
			return _stream.Read(buffer, offset, count);
		}
		catch (ObjectDisposedException)
		{
			cancellationTokenSource.Token.ThrowIfCancellationRequested();
			throw;
		}
		finally
		{
			StopTimeout(cancellationTokenSource, dispose);
		}
	}

	public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	{
		bool dispose;
		CancellationTokenSource source = StartTimeout(cancellationToken, out dispose);
		try
		{
			return await _stream.ReadAsync(buffer, offset, count, source.Token).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (ObjectDisposedException)
		{
			source.Token.ThrowIfCancellationRequested();
			throw;
		}
		finally
		{
			StopTimeout(source, dispose);
		}
	}

	private CancellationTokenSource StartTimeout(CancellationToken additionalToken, out bool dispose)
	{
		if (_cancellationTokenSource.IsCancellationRequested)
		{
			InitializeTokenSource();
		}
		CancellationTokenSource result;
		if (additionalToken.CanBeCanceled)
		{
			result = CancellationTokenSource.CreateLinkedTokenSource(additionalToken, _cancellationTokenSource.Token);
			dispose = true;
		}
		else
		{
			result = _cancellationTokenSource;
			dispose = false;
		}
		_cancellationTokenSource.CancelAfter(_readTimeout);
		return result;
	}

	private void InitializeTokenSource()
	{
		_cancellationTokenSource = new CancellationTokenSource();
		_cancellationTokenSource.Token.Register(delegate
		{
			DisposeStream();
		});
	}

	private void DisposeStream()
	{
		_stream.Dispose();
	}

	private void StopTimeout(CancellationTokenSource source, bool dispose)
	{
		_cancellationTokenSource.CancelAfter(Timeout.InfiniteTimeSpan);
		if (dispose)
		{
			source.Dispose();
		}
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		return _stream.Seek(offset, origin);
	}

	private void UpdateReadTimeout()
	{
		try
		{
			_stream.ReadTimeout = (int)_readTimeout.TotalMilliseconds;
		}
		catch
		{
		}
	}

	public override void Close()
	{
		_stream.Close();
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		_stream.Dispose();
		_cancellationTokenSource.Dispose();
	}
}
