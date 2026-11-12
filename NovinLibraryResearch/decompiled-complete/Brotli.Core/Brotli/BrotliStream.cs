using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Brotli;

public class BrotliStream : Stream
{
	private const int BufferSize = 65536;

	protected Stream _stream;

	protected MemoryStream _intermediateStream = new MemoryStream();

	protected CompressionMode _mode = CompressionMode.Compress;

	protected IntPtr _state = IntPtr.Zero;

	protected IntPtr _ptrInputBuffer = IntPtr.Zero;

	protected IntPtr _ptrOutputBuffer = IntPtr.Zero;

	protected IntPtr _ptrNextInput = IntPtr.Zero;

	protected IntPtr _ptrNextOutput = IntPtr.Zero;

	protected uint _availableIn;

	protected uint _availableOut = 65536u;

	protected byte[] _managedBuffer;

	protected bool _endOfStream;

	protected int _readOffset;

	protected BrotliDecoderResult _lastDecodeResult = BrotliDecoderResult.NeedsMoreInput;

	protected bool _leaveOpen;

	private static int totalWrote;

	public override bool CanRead
	{
		get
		{
			if (_stream == null)
			{
				return false;
			}
			if (_mode == CompressionMode.Decompress)
			{
				return _stream.CanRead;
			}
			return false;
		}
	}

	public override bool CanSeek => false;

	public override bool CanWrite
	{
		get
		{
			if (_stream == null)
			{
				return false;
			}
			if (_mode == CompressionMode.Compress)
			{
				return _stream.CanWrite;
			}
			return false;
		}
	}

	public override long Length
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override long Position
	{
		get
		{
			throw new NotImplementedException();
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	public BrotliStream(Stream baseStream, CompressionMode mode, bool leaveOpen)
	{
		if (baseStream == null)
		{
			throw new ArgumentNullException("baseStream");
		}
		_mode = mode;
		_stream = baseStream;
		_leaveOpen = leaveOpen;
		if (_mode == CompressionMode.Compress)
		{
			_state = Brolib.BrotliEncoderCreateInstance();
			if (_state == IntPtr.Zero)
			{
				throw new BrotliException("Unable to create brotli encoder instance");
			}
			Brolib.BrotliEncoderSetParameter(_state, BrotliEncoderParameter.Quality, 5u);
			Brolib.BrotliEncoderSetParameter(_state, BrotliEncoderParameter.LGWin, 22u);
		}
		else
		{
			_state = Brolib.BrotliDecoderCreateInstance();
			if (_state == IntPtr.Zero)
			{
				throw new BrotliException("Unable to create brotli decoder instance");
			}
			if (!Brolib.BrotliDecoderSetParameter(_state, BrotliDecoderParameter.LargeWindow, 1u))
			{
				throw new BrotliException("failed to set decoder parameter to large window");
			}
		}
		_ptrInputBuffer = Marshal.AllocHGlobal(65536);
		_ptrOutputBuffer = Marshal.AllocHGlobal(65536);
		_ptrNextInput = _ptrInputBuffer;
		_ptrNextOutput = _ptrOutputBuffer;
		_managedBuffer = new byte[65536];
	}

	public BrotliStream(Stream baseStream, CompressionMode mode)
		: this(baseStream, mode, leaveOpen: false)
	{
	}

	public void SetQuality(uint quality)
	{
		if (quality < 0 || quality > 11)
		{
			throw new ArgumentException("quality", "the range of quality is 0~11");
		}
		Brolib.BrotliEncoderSetParameter(_state, BrotliEncoderParameter.Quality, quality);
	}

	public void SetWindow(uint window)
	{
		if (window < 10 || window > 24)
		{
			throw new ArgumentException("window", "the range of window is 10~24");
		}
		Brolib.BrotliEncoderSetParameter(_state, BrotliEncoderParameter.LGWin, window);
	}

	public override async Task FlushAsync(CancellationToken cancellationToken)
	{
		if (_stream == null)
		{
			throw new ObjectDisposedException(null, "Underlying stream is disposed");
		}
		if (_mode == CompressionMode.Compress)
		{
			await FlushBrotliStreamAsync(finished: false).ConfigureAwait(continueOnCapturedContext: false);
		}
	}

	public override void Flush()
	{
		AsyncHelper.RunSync(() => FlushAsync());
	}

	protected virtual async Task FlushBrotliStreamAsync(bool finished)
	{
		if (_state == IntPtr.Zero || Brolib.BrotliEncoderIsFinished(_state))
		{
			return;
		}
		BrotliEncoderOperation op = ((!finished) ? BrotliEncoderOperation.Flush : BrotliEncoderOperation.Finish);
		uint totalOut = 0u;
		bool extraData;
		do
		{
			if (!Brolib.BrotliEncoderCompressStream(_state, op, ref _availableIn, ref _ptrNextInput, ref _availableOut, ref _ptrNextOutput, out totalOut))
			{
				throw new BrotliException("Unable to finish encode stream");
			}
			extraData = _availableOut != 65536;
			if (extraData)
			{
				int num = (int)(65536 - _availableOut);
				Marshal.Copy(_ptrOutputBuffer, _managedBuffer, 0, num);
				await _stream.WriteAsync(_managedBuffer, 0, num).ConfigureAwait(continueOnCapturedContext: false);
				_availableOut = 65536u;
				_ptrNextOutput = _ptrOutputBuffer;
			}
		}
		while (!Brolib.BrotliEncoderIsFinished(_state) && extraData);
	}

	protected virtual void FlushBrotliStream(bool finished)
	{
		AsyncHelper.RunSync(() => FlushBrotliStreamAsync(finished));
	}

	protected override void Dispose(bool disposing)
	{
		if (_mode == CompressionMode.Compress)
		{
			FlushBrotliStream(finished: true);
		}
		base.Dispose(disposing);
		if (!_leaveOpen)
		{
			_stream.Dispose();
		}
		_intermediateStream.Dispose();
		if (_ptrInputBuffer != IntPtr.Zero)
		{
			Marshal.FreeHGlobal(_ptrInputBuffer);
		}
		if (_ptrOutputBuffer != IntPtr.Zero)
		{
			Marshal.FreeHGlobal(_ptrOutputBuffer);
		}
		_managedBuffer = null;
		_ptrInputBuffer = IntPtr.Zero;
		_ptrOutputBuffer = IntPtr.Zero;
		if (_state != IntPtr.Zero)
		{
			if (_mode == CompressionMode.Compress)
			{
				Brolib.BrotliEncoderDestroyInstance(_state);
			}
			else
			{
				Brolib.BrotliDecoderDestroyInstance(_state);
			}
			_state = IntPtr.Zero;
		}
	}

	public void TruncateBeginning(MemoryStream ms, int numberOfBytesToRemove)
	{
		byte[] buffer = ms.GetBuffer();
		Buffer.BlockCopy(buffer, numberOfBytesToRemove, buffer, 0, (int)ms.Length - numberOfBytesToRemove);
		ms.SetLength(ms.Length - numberOfBytesToRemove);
	}

	public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	{
		if (_mode != 0)
		{
			throw new BrotliException("Can't read on this stream");
		}
		int bytesRead = (int)(_intermediateStream.Length - _readOffset);
		uint totalOut = 0u;
		bool endOfStream = false;
		bool errorDetected = false;
		while (bytesRead < count)
		{
			do
			{
				if (_lastDecodeResult == BrotliDecoderResult.NeedsMoreInput)
				{
					_availableIn = (uint)(await _stream.ReadAsync(_managedBuffer, 0, 65536).ConfigureAwait(continueOnCapturedContext: false));
					_ptrNextInput = _ptrInputBuffer;
					if (_availableIn == 0)
					{
						endOfStream = true;
						break;
					}
					Marshal.Copy(_managedBuffer, 0, _ptrInputBuffer, (int)_availableIn);
				}
				else
				{
					if (_lastDecodeResult != BrotliDecoderResult.NeedsMoreOutput)
					{
						endOfStream = true;
						break;
					}
					Marshal.Copy(_ptrOutputBuffer, _managedBuffer, 0, 65536);
					await _intermediateStream.WriteAsync(_managedBuffer, 0, 65536).ConfigureAwait(continueOnCapturedContext: false);
					bytesRead += 65536;
					_availableOut = 65536u;
					_ptrNextOutput = _ptrOutputBuffer;
				}
				_lastDecodeResult = Brolib.BrotliDecoderDecompressStream(_state, ref _availableIn, ref _ptrNextInput, ref _availableOut, ref _ptrNextOutput, out totalOut);
			}
			while (bytesRead < count);
			if (endOfStream && !Brolib.BrotliDecoderIsFinished(_state))
			{
				errorDetected = true;
			}
			if (_lastDecodeResult == BrotliDecoderResult.Error || errorDetected)
			{
				int num = Brolib.BrotliDecoderGetErrorCode(_state);
				string text = Brolib.BrotliDecoderErrorString(num);
				throw new BrotliDecodeException($"Unable to decode stream,possibly corrupt data.Code={num}({text})", num, text);
			}
			if (endOfStream && !Brolib.BrotliDecoderIsFinished(_state) && _lastDecodeResult == BrotliDecoderResult.NeedsMoreInput)
			{
				throw new BrotliException("Unable to decode stream,unexpected EOF");
			}
			if (endOfStream && _ptrNextOutput != _ptrOutputBuffer)
			{
				int num2 = (int)(_ptrNextOutput.ToInt64() - _ptrOutputBuffer.ToInt64());
				bytesRead += num2;
				Marshal.Copy(_ptrOutputBuffer, _managedBuffer, 0, num2);
				await _intermediateStream.WriteAsync(_managedBuffer, 0, num2).ConfigureAwait(continueOnCapturedContext: false);
				_ptrNextOutput = _ptrOutputBuffer;
			}
			if (endOfStream)
			{
				break;
			}
		}
		if (_intermediateStream.Length - _readOffset >= count || endOfStream)
		{
			_intermediateStream.Seek(_readOffset, SeekOrigin.Begin);
			int bytesToRead = (int)(_intermediateStream.Length - _readOffset);
			if (bytesToRead > count)
			{
				bytesToRead = count;
			}
			await _intermediateStream.ReadAsync(buffer, offset, bytesToRead).ConfigureAwait(continueOnCapturedContext: false);
			TruncateBeginning(_intermediateStream, _readOffset + bytesToRead);
			_readOffset = 0;
			return bytesToRead;
		}
		return 0;
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		return AsyncHelper.RunSync(task);
		async Task<int> task()
		{
			return await ReadAsync(buffer, offset, count).ConfigureAwait(continueOnCapturedContext: false);
		}
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw new NotImplementedException();
	}

	public override void SetLength(long value)
	{
		throw new NotImplementedException();
	}

	public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	{
		if (_mode != CompressionMode.Compress)
		{
			throw new BrotliException("Can't write on this stream");
		}
		totalWrote += count;
		uint totalOut = 0u;
		int bytesRemain = count;
		int currentOffset = offset;
		while (bytesRemain > 0)
		{
			int num = ((bytesRemain > 65536) ? 65536 : bytesRemain);
			Marshal.Copy(buffer, currentOffset, _ptrInputBuffer, num);
			bytesRemain -= num;
			currentOffset += num;
			_availableIn = (uint)num;
			_ptrNextInput = _ptrInputBuffer;
			while (_availableIn != 0)
			{
				if (!Brolib.BrotliEncoderCompressStream(_state, BrotliEncoderOperation.Process, ref _availableIn, ref _ptrNextInput, ref _availableOut, ref _ptrNextOutput, out totalOut))
				{
					throw new BrotliException("Unable to compress stream");
				}
				if (_availableOut != 65536)
				{
					int num2 = (int)(65536 - _availableOut);
					Marshal.Copy(_ptrOutputBuffer, _managedBuffer, 0, num2);
					await _stream.WriteAsync(_managedBuffer, 0, num2).ConfigureAwait(continueOnCapturedContext: false);
					_availableOut = 65536u;
					_ptrNextOutput = _ptrOutputBuffer;
				}
			}
			if (Brolib.BrotliEncoderIsFinished(_state))
			{
				break;
			}
		}
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		AsyncHelper.RunSync(task);
		async Task task()
		{
			await WriteAsync(buffer, offset, count).ConfigureAwait(continueOnCapturedContext: false);
		}
	}
}
