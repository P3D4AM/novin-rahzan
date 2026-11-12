using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Versioning;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders.Physical.Internal;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.FileProviders.Physical;

public class PhysicalFilesWatcher : IDisposable
{
	private readonly struct ChangeTokenInfo
	{
		public CancellationTokenSource TokenSource { get; }

		public CancellationChangeToken ChangeToken { get; }

		public Matcher Matcher { get; }

		public ChangeTokenInfo(CancellationTokenSource tokenSource, CancellationChangeToken changeToken)
			: this(tokenSource, changeToken, null)
		{
		}

		public ChangeTokenInfo(CancellationTokenSource tokenSource, CancellationChangeToken changeToken, Matcher matcher)
		{
			TokenSource = tokenSource;
			ChangeToken = changeToken;
			Matcher = matcher;
		}
	}

	private static readonly Action<object> _cancelTokenSource = delegate(object state)
	{
		((CancellationTokenSource)state).Cancel();
	};

	internal static TimeSpan DefaultPollingInterval = TimeSpan.FromSeconds(4.0);

	private readonly ConcurrentDictionary<string, ChangeTokenInfo> _filePathTokenLookup = new ConcurrentDictionary<string, ChangeTokenInfo>(StringComparer.OrdinalIgnoreCase);

	private readonly ConcurrentDictionary<string, ChangeTokenInfo> _wildcardTokenLookup = new ConcurrentDictionary<string, ChangeTokenInfo>(StringComparer.OrdinalIgnoreCase);

	private readonly FileSystemWatcher _fileWatcher;

	private readonly object _fileWatcherLock = new object();

	private readonly string _root;

	private readonly ExclusionFilters _filters;

	private Timer _timer;

	private bool _timerInitialzed;

	private object _timerLock = new object();

	private Func<Timer> _timerFactory;

	private bool _disposed;

	internal bool PollForChanges { get; }

	internal bool UseActivePolling { get; set; }

	internal ConcurrentDictionary<IPollingChangeToken, IPollingChangeToken> PollingChangeTokens { get; }

	public PhysicalFilesWatcher(string root, FileSystemWatcher fileSystemWatcher, bool pollForChanges)
		: this(root, fileSystemWatcher, pollForChanges, ExclusionFilters.Sensitive)
	{
	}

	public PhysicalFilesWatcher(string root, FileSystemWatcher fileSystemWatcher, bool pollForChanges, ExclusionFilters filters)
	{
		if (fileSystemWatcher == null && !pollForChanges)
		{
			throw new ArgumentNullException("fileSystemWatcher", System.SR.Error_FileSystemWatcherRequiredWithoutPolling);
		}
		_root = root;
		if (fileSystemWatcher != null)
		{
			_fileWatcher = fileSystemWatcher;
			_fileWatcher.IncludeSubdirectories = true;
			_fileWatcher.Created += OnChanged;
			_fileWatcher.Changed += OnChanged;
			_fileWatcher.Renamed += OnRenamed;
			_fileWatcher.Deleted += OnChanged;
			_fileWatcher.Error += OnError;
		}
		PollForChanges = pollForChanges;
		_filters = filters;
		PollingChangeTokens = new ConcurrentDictionary<IPollingChangeToken, IPollingChangeToken>();
		_timerFactory = () => NonCapturingTimer.Create(RaiseChangeEvents, PollingChangeTokens, TimeSpan.Zero, DefaultPollingInterval);
	}

	public IChangeToken CreateFileChangeToken(string filter)
	{
		if (filter == null)
		{
			throw new ArgumentNullException("filter");
		}
		filter = NormalizePath(filter);
		if (Path.IsPathRooted(filter) || PathUtils.PathNavigatesAboveRoot(filter))
		{
			return NullChangeToken.Singleton;
		}
		IChangeToken orAddChangeToken = GetOrAddChangeToken(filter);
		TryEnableFileSystemWatcher();
		return orAddChangeToken;
	}

	private IChangeToken GetOrAddChangeToken(string pattern)
	{
		if (UseActivePolling)
		{
			LazyInitializer.EnsureInitialized(ref _timer, ref _timerInitialzed, ref _timerLock, _timerFactory);
		}
		if (pattern.IndexOf('*') != -1 || IsDirectoryPath(pattern))
		{
			return GetOrAddWildcardChangeToken(pattern);
		}
		return GetOrAddFilePathChangeToken(pattern);
	}

	internal IChangeToken GetOrAddFilePathChangeToken(string filePath)
	{
		if (!_filePathTokenLookup.TryGetValue(filePath, out var value))
		{
			CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
			CancellationChangeToken changeToken = new CancellationChangeToken(cancellationTokenSource.Token);
			value = new ChangeTokenInfo(cancellationTokenSource, changeToken);
			value = _filePathTokenLookup.GetOrAdd(filePath, value);
		}
		IChangeToken changeToken2 = value.ChangeToken;
		if (PollForChanges)
		{
			PollingFileChangeToken pollingFileChangeToken = new PollingFileChangeToken(new FileInfo(Path.Combine(_root, filePath)));
			if (UseActivePolling)
			{
				pollingFileChangeToken.ActiveChangeCallbacks = true;
				pollingFileChangeToken.CancellationTokenSource = new CancellationTokenSource();
				PollingChangeTokens.TryAdd(pollingFileChangeToken, pollingFileChangeToken);
			}
			changeToken2 = new CompositeChangeToken(new IChangeToken[2] { changeToken2, pollingFileChangeToken });
		}
		return changeToken2;
	}

	internal IChangeToken GetOrAddWildcardChangeToken(string pattern)
	{
		if (!_wildcardTokenLookup.TryGetValue(pattern, out var value))
		{
			CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
			CancellationChangeToken changeToken = new CancellationChangeToken(cancellationTokenSource.Token);
			Matcher matcher = new Matcher(StringComparison.OrdinalIgnoreCase);
			matcher.AddInclude(pattern);
			value = new ChangeTokenInfo(cancellationTokenSource, changeToken, matcher);
			value = _wildcardTokenLookup.GetOrAdd(pattern, value);
		}
		IChangeToken changeToken2 = value.ChangeToken;
		if (PollForChanges)
		{
			PollingWildCardChangeToken pollingWildCardChangeToken = new PollingWildCardChangeToken(_root, pattern);
			if (UseActivePolling)
			{
				pollingWildCardChangeToken.ActiveChangeCallbacks = true;
				pollingWildCardChangeToken.CancellationTokenSource = new CancellationTokenSource();
				PollingChangeTokens.TryAdd(pollingWildCardChangeToken, pollingWildCardChangeToken);
			}
			changeToken2 = new CompositeChangeToken(new IChangeToken[2] { changeToken2, pollingWildCardChangeToken });
		}
		return changeToken2;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposed)
		{
			if (disposing)
			{
				_fileWatcher?.Dispose();
				_timer?.Dispose();
			}
			_disposed = true;
		}
	}

	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[SupportedOSPlatform("maccatalyst")]
	private void OnRenamed(object sender, RenamedEventArgs e)
	{
		OnFileSystemEntryChange(e.OldFullPath);
		OnFileSystemEntryChange(e.FullPath);
		if (!Directory.Exists(e.FullPath))
		{
			return;
		}
		try
		{
			foreach (string item in Directory.EnumerateFileSystemEntries(e.FullPath, "*", SearchOption.AllDirectories))
			{
				string fullPath = Path.Combine(e.OldFullPath, item.Substring(e.FullPath.Length + 1));
				OnFileSystemEntryChange(fullPath);
				OnFileSystemEntryChange(item);
			}
		}
		catch (Exception ex) when (ex is IOException || ex is SecurityException || ex is DirectoryNotFoundException || ex is UnauthorizedAccessException)
		{
		}
	}

	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[SupportedOSPlatform("maccatalyst")]
	private void OnChanged(object sender, FileSystemEventArgs e)
	{
		OnFileSystemEntryChange(e.FullPath);
	}

	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[SupportedOSPlatform("maccatalyst")]
	private void OnError(object sender, ErrorEventArgs e)
	{
		foreach (string key in _filePathTokenLookup.Keys)
		{
			ReportChangeForMatchedEntries(key);
		}
	}

	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[SupportedOSPlatform("maccatalyst")]
	private void OnFileSystemEntryChange(string fullPath)
	{
		try
		{
			FileInfo fileSystemInfo = new FileInfo(fullPath);
			if (!FileSystemInfoHelper.IsExcluded(fileSystemInfo, _filters))
			{
				string path = fullPath.Substring(_root.Length);
				ReportChangeForMatchedEntries(path);
			}
		}
		catch (Exception ex) when (ex is IOException || ex is SecurityException || ex is UnauthorizedAccessException)
		{
		}
	}

	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[SupportedOSPlatform("maccatalyst")]
	private void ReportChangeForMatchedEntries(string path)
	{
		if (string.IsNullOrEmpty(path))
		{
			return;
		}
		path = NormalizePath(path);
		bool flag = false;
		if (_filePathTokenLookup.TryRemove(path, out var value))
		{
			CancelToken(value);
			flag = true;
		}
		foreach (KeyValuePair<string, ChangeTokenInfo> item in _wildcardTokenLookup)
		{
			PatternMatchingResult patternMatchingResult = item.Value.Matcher.Match(path);
			if (patternMatchingResult.HasMatches && _wildcardTokenLookup.TryRemove(item.Key, out value))
			{
				CancelToken(value);
				flag = true;
			}
		}
		if (flag)
		{
			TryDisableFileSystemWatcher();
		}
	}

	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[SupportedOSPlatform("maccatalyst")]
	private void TryDisableFileSystemWatcher()
	{
		if (_fileWatcher == null)
		{
			return;
		}
		lock (_fileWatcherLock)
		{
			if (_filePathTokenLookup.IsEmpty && _wildcardTokenLookup.IsEmpty && _fileWatcher.EnableRaisingEvents)
			{
				_fileWatcher.EnableRaisingEvents = false;
			}
		}
	}

	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	[SupportedOSPlatform("maccatalyst")]
	private void TryEnableFileSystemWatcher()
	{
		if (_fileWatcher == null)
		{
			return;
		}
		lock (_fileWatcherLock)
		{
			if ((!_filePathTokenLookup.IsEmpty || !_wildcardTokenLookup.IsEmpty) && !_fileWatcher.EnableRaisingEvents)
			{
				_fileWatcher.EnableRaisingEvents = true;
			}
		}
	}

	private static string NormalizePath(string filter)
	{
		return filter = filter.Replace('\\', '/');
	}

	private static bool IsDirectoryPath(string path)
	{
		if (path.Length > 0)
		{
			if (path[path.Length - 1] != Path.DirectorySeparatorChar)
			{
				return path[path.Length - 1] == Path.AltDirectorySeparatorChar;
			}
			return true;
		}
		return false;
	}

	private static void CancelToken(ChangeTokenInfo matchInfo)
	{
		if (!matchInfo.TokenSource.IsCancellationRequested)
		{
			Task.Factory.StartNew(_cancelTokenSource, matchInfo.TokenSource, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
		}
	}

	internal static void RaiseChangeEvents(object state)
	{
		ConcurrentDictionary<IPollingChangeToken, IPollingChangeToken> concurrentDictionary = (ConcurrentDictionary<IPollingChangeToken, IPollingChangeToken>)state;
		foreach (KeyValuePair<IPollingChangeToken, IPollingChangeToken> item in concurrentDictionary)
		{
			IPollingChangeToken key = item.Key;
			if (key.HasChanged && concurrentDictionary.TryRemove(key, out var _))
			{
				try
				{
					key.CancellationTokenSource.Cancel();
				}
				catch
				{
				}
			}
		}
	}
}
