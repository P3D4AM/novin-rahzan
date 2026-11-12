using System;
using System.IO;
using System.Threading;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.FileProviders.Physical;

public class PollingFileChangeToken : IPollingChangeToken, IChangeToken
{
	private readonly FileInfo _fileInfo;

	private DateTime _previousWriteTimeUtc;

	private DateTime _lastCheckedTimeUtc;

	private bool _hasChanged;

	private CancellationTokenSource _tokenSource;

	private CancellationChangeToken _changeToken;

	internal static TimeSpan PollingInterval { get; set; } = PhysicalFilesWatcher.DefaultPollingInterval;


	public bool ActiveChangeCallbacks { get; internal set; }

	internal CancellationTokenSource CancellationTokenSource
	{
		get
		{
			return _tokenSource;
		}
		set
		{
			_tokenSource = value;
			_changeToken = new CancellationChangeToken(_tokenSource.Token);
		}
	}

	CancellationTokenSource IPollingChangeToken.CancellationTokenSource => CancellationTokenSource;

	public bool HasChanged
	{
		get
		{
			if (_hasChanged)
			{
				return _hasChanged;
			}
			DateTime utcNow = DateTime.UtcNow;
			if (utcNow - _lastCheckedTimeUtc < PollingInterval)
			{
				return _hasChanged;
			}
			DateTime lastWriteTimeUtc = GetLastWriteTimeUtc();
			if (_previousWriteTimeUtc != lastWriteTimeUtc)
			{
				_previousWriteTimeUtc = lastWriteTimeUtc;
				_hasChanged = true;
			}
			_lastCheckedTimeUtc = utcNow;
			return _hasChanged;
		}
	}

	public PollingFileChangeToken(FileInfo fileInfo)
	{
		_fileInfo = fileInfo;
		_previousWriteTimeUtc = GetLastWriteTimeUtc();
	}

	private DateTime GetLastWriteTimeUtc()
	{
		_fileInfo.Refresh();
		if (!_fileInfo.Exists)
		{
			return DateTime.MinValue;
		}
		return FileSystemInfoHelper.GetFileLinkTargetLastWriteTimeUtc(_fileInfo) ?? _fileInfo.LastWriteTimeUtc;
	}

	public IDisposable RegisterChangeCallback(Action<object> callback, object state)
	{
		if (!ActiveChangeCallbacks)
		{
			return Microsoft.Extensions.FileProviders.EmptyDisposable.Instance;
		}
		return _changeToken.RegisterChangeCallback(callback, state);
	}
}
