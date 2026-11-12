using System;
using System.IO;
using System.Threading;
using Microsoft.Extensions.FileProviders.Internal;
using Microsoft.Extensions.FileProviders.Physical;
using Microsoft.Extensions.FileProviders.Physical.Internal;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.FileProviders;

public class PhysicalFileProvider : IFileProvider, IDisposable
{
	private const string PollingEnvironmentKey = "DOTNET_USE_POLLING_FILE_WATCHER";

	private static readonly char[] _pathSeparators = new char[2]
	{
		Path.DirectorySeparatorChar,
		Path.AltDirectorySeparatorChar
	};

	private readonly ExclusionFilters _filters;

	private readonly Func<PhysicalFilesWatcher> _fileWatcherFactory;

	private PhysicalFilesWatcher _fileWatcher;

	private bool _fileWatcherInitialized;

	private object _fileWatcherLock = new object();

	private bool? _usePollingFileWatcher;

	private bool? _useActivePolling;

	private bool _disposed;

	public bool UsePollingFileWatcher
	{
		get
		{
			if (_fileWatcher != null)
			{
				return false;
			}
			if (!_usePollingFileWatcher.HasValue)
			{
				ReadPollingEnvironmentVariables();
			}
			return _usePollingFileWatcher.GetValueOrDefault();
		}
		set
		{
			if (_fileWatcher != null)
			{
				throw new InvalidOperationException(System.SR.Format(System.SR.CannotModifyWhenFileWatcherInitialized, "UsePollingFileWatcher"));
			}
			_usePollingFileWatcher = value;
		}
	}

	public bool UseActivePolling
	{
		get
		{
			if (!_useActivePolling.HasValue)
			{
				ReadPollingEnvironmentVariables();
			}
			return _useActivePolling.Value;
		}
		set
		{
			_useActivePolling = value;
		}
	}

	internal PhysicalFilesWatcher FileWatcher
	{
		get
		{
			return LazyInitializer.EnsureInitialized(ref _fileWatcher, ref _fileWatcherInitialized, ref _fileWatcherLock, _fileWatcherFactory);
		}
		set
		{
			_fileWatcherInitialized = true;
			_fileWatcher = value;
		}
	}

	public string Root { get; }

	public PhysicalFileProvider(string root)
		: this(root, ExclusionFilters.Sensitive)
	{
	}

	public PhysicalFileProvider(string root, ExclusionFilters filters)
	{
		if (!Path.IsPathRooted(root))
		{
			throw new ArgumentException("The path must be absolute.", "root");
		}
		string fullPath = Path.GetFullPath(root);
		Root = PathUtils.EnsureTrailingSlash(fullPath);
		if (!Directory.Exists(Root))
		{
			throw new DirectoryNotFoundException(Root);
		}
		_filters = filters;
		_fileWatcherFactory = () => CreateFileWatcher();
	}

	internal PhysicalFilesWatcher CreateFileWatcher()
	{
		string text = PathUtils.EnsureTrailingSlash(Path.GetFullPath(Root));
		FileSystemWatcher fileSystemWatcher = ((UsePollingFileWatcher && UseActivePolling) ? null : new FileSystemWatcher(text));
		return new PhysicalFilesWatcher(text, fileSystemWatcher, UsePollingFileWatcher, _filters)
		{
			UseActivePolling = UseActivePolling
		};
	}

	private void ReadPollingEnvironmentVariables()
	{
		string environmentVariable = Environment.GetEnvironmentVariable("DOTNET_USE_POLLING_FILE_WATCHER");
		bool value = string.Equals(environmentVariable, "1", StringComparison.Ordinal) || string.Equals(environmentVariable, "true", StringComparison.OrdinalIgnoreCase);
		_usePollingFileWatcher = value;
		_useActivePolling = value;
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
			}
			_disposed = true;
		}
	}

	private string GetFullPath(string path)
	{
		if (PathUtils.PathNavigatesAboveRoot(path))
		{
			return null;
		}
		string fullPath;
		try
		{
			fullPath = Path.GetFullPath(Path.Combine(Root, path));
		}
		catch
		{
			return null;
		}
		if (!IsUnderneathRoot(fullPath))
		{
			return null;
		}
		return fullPath;
	}

	private bool IsUnderneathRoot(string fullPath)
	{
		return fullPath.StartsWith(Root, StringComparison.OrdinalIgnoreCase);
	}

	public IFileInfo GetFileInfo(string subpath)
	{
		if (string.IsNullOrEmpty(subpath) || PathUtils.HasInvalidPathChars(subpath))
		{
			return new NotFoundFileInfo(subpath);
		}
		subpath = subpath.TrimStart(_pathSeparators);
		if (Path.IsPathRooted(subpath))
		{
			return new NotFoundFileInfo(subpath);
		}
		string fullPath = GetFullPath(subpath);
		if (fullPath == null)
		{
			return new NotFoundFileInfo(subpath);
		}
		FileInfo fileInfo = new FileInfo(fullPath);
		if (FileSystemInfoHelper.IsExcluded(fileInfo, _filters))
		{
			return new NotFoundFileInfo(subpath);
		}
		return new PhysicalFileInfo(fileInfo);
	}

	public IDirectoryContents GetDirectoryContents(string subpath)
	{
		try
		{
			if (subpath == null || PathUtils.HasInvalidPathChars(subpath))
			{
				return NotFoundDirectoryContents.Singleton;
			}
			subpath = subpath.TrimStart(_pathSeparators);
			if (Path.IsPathRooted(subpath))
			{
				return NotFoundDirectoryContents.Singleton;
			}
			string fullPath = GetFullPath(subpath);
			if (fullPath == null || !Directory.Exists(fullPath))
			{
				return NotFoundDirectoryContents.Singleton;
			}
			return new PhysicalDirectoryContents(fullPath, _filters);
		}
		catch (DirectoryNotFoundException)
		{
		}
		catch (IOException)
		{
		}
		return NotFoundDirectoryContents.Singleton;
	}

	public IChangeToken Watch(string filter)
	{
		if (filter == null || PathUtils.HasInvalidFilterChars(filter))
		{
			return NullChangeToken.Singleton;
		}
		filter = filter.TrimStart(_pathSeparators);
		return FileWatcher.CreateFileChangeToken(filter);
	}
}
