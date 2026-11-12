using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Formatting;

namespace Serilog.Sinks.File;

internal sealed class RollingFileSink : ILogEventSink, IFlushableFileSink, IDisposable
{
	private readonly PathRoller _roller;

	private readonly ITextFormatter _textFormatter;

	private readonly long? _fileSizeLimitBytes;

	private readonly int? _retainedFileCountLimit;

	private readonly TimeSpan? _retainedFileTimeLimit;

	private readonly Encoding? _encoding;

	private readonly bool _buffered;

	private readonly bool _shared;

	private readonly bool _rollOnFileSizeLimit;

	private readonly FileLifecycleHooks? _hooks;

	private readonly object _syncRoot = new object();

	private bool _isDisposed;

	private DateTime? _nextCheckpoint;

	private IFileSink? _currentFile;

	private int? _currentFileSequence;

	public RollingFileSink(string path, ITextFormatter textFormatter, long? fileSizeLimitBytes, int? retainedFileCountLimit, Encoding? encoding, bool buffered, bool shared, RollingInterval rollingInterval, bool rollOnFileSizeLimit, FileLifecycleHooks? hooks, TimeSpan? retainedFileTimeLimit)
	{
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (fileSizeLimitBytes.HasValue && fileSizeLimitBytes < 1)
		{
			throw new ArgumentException("Invalid value provided; file size limit must be at least 1 byte, or null.");
		}
		if (retainedFileCountLimit.HasValue && retainedFileCountLimit < 1)
		{
			throw new ArgumentException("Zero or negative value provided; retained file count limit must be at least 1");
		}
		if (retainedFileTimeLimit.HasValue && retainedFileTimeLimit < TimeSpan.Zero)
		{
			throw new ArgumentException("Negative value provided; retained file time limit must be non-negative.", "retainedFileTimeLimit");
		}
		_roller = new PathRoller(path, rollingInterval);
		_textFormatter = textFormatter;
		_fileSizeLimitBytes = fileSizeLimitBytes;
		_retainedFileCountLimit = retainedFileCountLimit;
		_retainedFileTimeLimit = retainedFileTimeLimit;
		_encoding = encoding;
		_buffered = buffered;
		_shared = shared;
		_rollOnFileSizeLimit = rollOnFileSizeLimit;
		_hooks = hooks;
	}

	public void Emit(LogEvent logEvent)
	{
		if (logEvent == null)
		{
			throw new ArgumentNullException("logEvent");
		}
		lock (_syncRoot)
		{
			if (_isDisposed)
			{
				throw new ObjectDisposedException("The log file has been disposed.");
			}
			DateTime dateTimeNow = Clock.DateTimeNow;
			AlignCurrentFileTo(dateTimeNow);
			while (true)
			{
				IFileSink? currentFile = _currentFile;
				if (currentFile != null && !currentFile.EmitOrOverflow(logEvent) && _rollOnFileSizeLimit)
				{
					AlignCurrentFileTo(dateTimeNow, nextSequence: true);
					continue;
				}
				break;
			}
		}
	}

	private void AlignCurrentFileTo(DateTime now, bool nextSequence = false)
	{
		if (!_nextCheckpoint.HasValue)
		{
			OpenFile(now);
		}
		else if (nextSequence || now >= _nextCheckpoint.Value)
		{
			int? minSequence = null;
			if (nextSequence)
			{
				minSequence = (_currentFileSequence.HasValue ? new int?(_currentFileSequence.Value + 1) : new int?(1));
			}
			CloseFile();
			OpenFile(now, minSequence);
		}
	}

	private void OpenFile(DateTime now, int? minSequence = null)
	{
		DateTime? currentCheckpoint = _roller.GetCurrentCheckpoint(now);
		_nextCheckpoint = _roller.GetNextCheckpoint(now) ?? now.AddMinutes(30.0);
		IEnumerable<string> filenames = Enumerable.Empty<string>();
		try
		{
			if (Directory.Exists(_roller.LogFileDirectory))
			{
				filenames = from f in Directory.GetFiles(_roller.LogFileDirectory, _roller.DirectorySearchPattern)
					select Path.GetFileName(f);
			}
		}
		catch (DirectoryNotFoundException)
		{
		}
		int? num = (from m in _roller.SelectMatches(filenames).Where(delegate(RollingLogFile m)
			{
				DateTime? dateTime = m.DateTime;
				DateTime? dateTime2 = currentCheckpoint;
				if (dateTime.HasValue != dateTime2.HasValue)
				{
					return false;
				}
				return !dateTime.HasValue || dateTime.GetValueOrDefault() == dateTime2.GetValueOrDefault();
			})
			orderby m.SequenceNumber descending
			select m).FirstOrDefault()?.SequenceNumber;
		if (minSequence.HasValue && (!num.HasValue || num.Value < minSequence.Value))
		{
			num = minSequence;
		}
		for (int i = 0; i < 3; i++)
		{
			_roller.GetLogFilePath(now, num, out string path);
			try
			{
				IFileSink currentFile;
				if (!_shared)
				{
					IFileSink fileSink = new FileSink(path, _textFormatter, _fileSizeLimitBytes, _encoding, _buffered, _hooks);
					currentFile = fileSink;
				}
				else
				{
					IFileSink fileSink = new SharedFileSink(path, _textFormatter, _fileSizeLimitBytes, _encoding);
					currentFile = fileSink;
				}
				_currentFile = currentFile;
				_currentFileSequence = num;
			}
			catch (IOException ex2)
			{
				if (IOErrors.IsLockedFile(ex2))
				{
					SelfLog.WriteLine("File target {0} was locked, attempting to open next in sequence (attempt {1})", path, i + 1);
					num = num.GetValueOrDefault() + 1;
					continue;
				}
				throw;
			}
			ApplyRetentionPolicy(path, now);
			break;
		}
	}

	private void ApplyRetentionPolicy(string currentFilePath, DateTime now)
	{
		if (!_retainedFileCountLimit.HasValue && !_retainedFileTimeLimit.HasValue)
		{
			return;
		}
		string currentFileName = Path.GetFileName(currentFilePath);
		IEnumerable<string> filenames = (from f in Directory.GetFiles(_roller.LogFileDirectory, _roller.DirectorySearchPattern)
			select Path.GetFileName(f)).Union<string>(new string[1] { currentFileName });
		foreach (string item in (from x in (from m in _roller.SelectMatches(filenames)
				orderby m.DateTime descending, m.SequenceNumber descending
				select m into n
				where StringComparer.OrdinalIgnoreCase.Compare(currentFileName, n.Filename) != 0
				select n).SkipWhile((RollingLogFile f, int i) => ShouldRetainFile(f, i, now))
			select x.Filename).ToList())
		{
			string text = Path.Combine(_roller.LogFileDirectory, item);
			try
			{
				_hooks?.OnFileDeleting(text);
				System.IO.File.Delete(text);
			}
			catch (Exception arg)
			{
				SelfLog.WriteLine("Error {0} while processing obsolete log file {1}", arg, text);
			}
		}
	}

	private bool ShouldRetainFile(RollingLogFile file, int index, DateTime now)
	{
		if (_retainedFileCountLimit.HasValue && index >= _retainedFileCountLimit.Value - 1)
		{
			return false;
		}
		if (_retainedFileTimeLimit.HasValue && file.DateTime.HasValue && file.DateTime.Value < now.Subtract(_retainedFileTimeLimit.Value))
		{
			return false;
		}
		return true;
	}

	public void Dispose()
	{
		lock (_syncRoot)
		{
			if (_currentFile != null)
			{
				CloseFile();
				_isDisposed = true;
			}
		}
	}

	private void CloseFile()
	{
		if (_currentFile != null)
		{
			(_currentFile as IDisposable)?.Dispose();
			_currentFile = null;
		}
		_nextCheckpoint = null;
	}

	public void FlushToDisk()
	{
		lock (_syncRoot)
		{
			_currentFile?.FlushToDisk();
		}
	}
}
