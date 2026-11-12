using System;
using System.IO;
using System.Security.AccessControl;
using System.Text;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;

namespace Serilog.Sinks.File;

[Obsolete("This type will be removed from the public API in a future version; use `WriteTo.File(shared: true)` instead.")]
public sealed class SharedFileSink : IFileSink, ILogEventSink, IFlushableFileSink, IDisposable
{
	private readonly MemoryStream _writeBuffer;

	private readonly string _path;

	private readonly TextWriter _output;

	private readonly ITextFormatter _textFormatter;

	private readonly long? _fileSizeLimitBytes;

	private readonly object _syncRoot = new object();

	private FileStream _fileOutput;

	private int _fileStreamBufferLength = 4096;

	private const int DefaultFileStreamBufferLength = 4096;

	public SharedFileSink(string path, ITextFormatter textFormatter, long? fileSizeLimitBytes, Encoding? encoding = null)
	{
		if (fileSizeLimitBytes.HasValue && fileSizeLimitBytes < 1)
		{
			throw new ArgumentException("Invalid value provided; file size limit must be at least 1 byte, or null");
		}
		_path = path ?? throw new ArgumentNullException("path");
		_textFormatter = textFormatter ?? throw new ArgumentNullException("textFormatter");
		_fileSizeLimitBytes = fileSizeLimitBytes;
		string directoryName = Path.GetDirectoryName(path);
		if (!string.IsNullOrWhiteSpace(directoryName) && !Directory.Exists(directoryName))
		{
			Directory.CreateDirectory(directoryName);
		}
		_fileOutput = new FileStream(path, FileMode.Append, FileSystemRights.AppendData, FileShare.ReadWrite, _fileStreamBufferLength, FileOptions.None);
		_writeBuffer = new MemoryStream();
		_output = new StreamWriter(_writeBuffer, encoding ?? new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
	}

	bool IFileSink.EmitOrOverflow(LogEvent logEvent)
	{
		if (logEvent == null)
		{
			throw new ArgumentNullException("logEvent");
		}
		lock (_syncRoot)
		{
			try
			{
				_textFormatter.Format(logEvent, _output);
				_output.Flush();
				byte[] buffer = _writeBuffer.GetBuffer();
				int num = (int)_writeBuffer.Length;
				if (num > _fileStreamBufferLength)
				{
					FileStream fileOutput = _fileOutput;
					_fileOutput = new FileStream(_path, FileMode.Append, FileSystemRights.AppendData, FileShare.ReadWrite, num, FileOptions.None);
					_fileStreamBufferLength = num;
					fileOutput.Dispose();
				}
				if (_fileSizeLimitBytes.HasValue)
				{
					try
					{
						if (_fileOutput.Length >= _fileSizeLimitBytes.Value)
						{
							return false;
						}
					}
					catch (FileNotFoundException)
					{
					}
				}
				_fileOutput.Write(buffer, 0, num);
				_fileOutput.Flush();
				return true;
			}
			catch
			{
				_output.Flush();
				throw;
			}
			finally
			{
				_writeBuffer.Position = 0L;
				_writeBuffer.SetLength(0L);
			}
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
			_fileOutput.Dispose();
		}
	}

	public void FlushToDisk()
	{
		lock (_syncRoot)
		{
			_output.Flush();
			_fileOutput.Flush(flushToDisk: true);
		}
	}
}
