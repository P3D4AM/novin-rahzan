using System;
using System.IO;
using System.Text;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;

namespace Serilog.Sinks.File;

public sealed class FileSink : IFileSink, ILogEventSink, IFlushableFileSink, IDisposable
{
	private readonly TextWriter _output;

	private readonly FileStream _underlyingStream;

	private readonly ITextFormatter _textFormatter;

	private readonly long? _fileSizeLimitBytes;

	private readonly bool _buffered;

	private readonly object _syncRoot = new object();

	private readonly WriteCountingStream? _countingStreamWrapper;

	[Obsolete("This type and constructor will be removed from the public API in a future version; use `WriteTo.File()` instead.")]
	public FileSink(string path, ITextFormatter textFormatter, long? fileSizeLimitBytes, Encoding? encoding = null, bool buffered = false)
		: this(path, textFormatter, fileSizeLimitBytes, encoding, buffered, null)
	{
	}

	internal FileSink(string path, ITextFormatter textFormatter, long? fileSizeLimitBytes, Encoding? encoding, bool buffered, FileLifecycleHooks? hooks)
	{
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (fileSizeLimitBytes.HasValue && fileSizeLimitBytes < 1)
		{
			throw new ArgumentException("Invalid value provided; file size limit must be at least 1 byte, or null.");
		}
		_textFormatter = textFormatter ?? throw new ArgumentNullException("textFormatter");
		_fileSizeLimitBytes = fileSizeLimitBytes;
		_buffered = buffered;
		string directoryName = Path.GetDirectoryName(path);
		if (!string.IsNullOrWhiteSpace(directoryName) && !Directory.Exists(directoryName))
		{
			Directory.CreateDirectory(directoryName);
		}
		Stream stream = (_underlyingStream = System.IO.File.Open(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read));
		stream.Seek(0L, SeekOrigin.End);
		if (_fileSizeLimitBytes.HasValue)
		{
			stream = (_countingStreamWrapper = new WriteCountingStream(_underlyingStream));
		}
		encoding = encoding ?? new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
		if (hooks != null)
		{
			stream = hooks.OnFileOpened(path, stream, encoding) ?? throw new InvalidOperationException("The file lifecycle hook `OnFileOpened(...)` returned `null`.");
		}
		_output = new StreamWriter(stream, encoding);
	}

	bool IFileSink.EmitOrOverflow(LogEvent logEvent)
	{
		if (logEvent == null)
		{
			throw new ArgumentNullException("logEvent");
		}
		lock (_syncRoot)
		{
			if (_fileSizeLimitBytes.HasValue && _countingStreamWrapper.CountedLength >= _fileSizeLimitBytes.Value)
			{
				return false;
			}
			_textFormatter.Format(logEvent, _output);
			if (!_buffered)
			{
				_output.Flush();
			}
			return true;
		}
	}

	public void Emit(LogEvent logEvent)
	{
		((IFileSink)this).EmitOrOverflow(logEvent);
	}

	public void Dispose()
	{
		lock (_syncRoot)
		{
			_output.Dispose();
		}
	}

	public void FlushToDisk()
	{
		lock (_syncRoot)
		{
			_output.Flush();
			_underlyingStream.Flush(flushToDisk: true);
		}
	}
}
