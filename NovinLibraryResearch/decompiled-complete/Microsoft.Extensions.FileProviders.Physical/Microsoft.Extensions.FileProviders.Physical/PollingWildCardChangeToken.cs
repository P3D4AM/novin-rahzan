using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.FileProviders.Physical;

public class PollingWildCardChangeToken : IPollingChangeToken, IChangeToken
{
	private static readonly byte[] Separator = Encoding.Unicode.GetBytes("|");

	private readonly object _enumerationLock = new object();

	private readonly DirectoryInfoBase _directoryInfo;

	private readonly Matcher _matcher;

	private bool _changed;

	private DateTime? _lastScanTimeUtc;

	private byte[] _byteBuffer;

	private byte[] _previousHash;

	private CancellationTokenSource _tokenSource;

	private CancellationChangeToken _changeToken;

	public bool ActiveChangeCallbacks { get; internal set; }

	internal TimeSpan PollingInterval { get; set; } = PhysicalFilesWatcher.DefaultPollingInterval;


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

	private IClock Clock { get; }

	public bool HasChanged
	{
		get
		{
			if (_changed)
			{
				return _changed;
			}
			DateTime utcNow = Clock.UtcNow;
			DateTime? lastScanTimeUtc = _lastScanTimeUtc;
			if (utcNow - lastScanTimeUtc >= PollingInterval)
			{
				lock (_enumerationLock)
				{
					_changed = CalculateChanges();
				}
			}
			return _changed;
		}
	}

	public PollingWildCardChangeToken(string root, string pattern)
		: this(new DirectoryInfoWrapper(new DirectoryInfo(root)), pattern, Microsoft.Extensions.FileProviders.Physical.Clock.Instance)
	{
	}

	internal PollingWildCardChangeToken(DirectoryInfoBase directoryInfo, string pattern, IClock clock)
	{
		_directoryInfo = directoryInfo;
		Clock = clock;
		_matcher = new Matcher(StringComparison.OrdinalIgnoreCase);
		_matcher.AddInclude(pattern);
		CalculateChanges();
	}

	private bool CalculateChanges()
	{
		PatternMatchingResult patternMatchingResult = _matcher.Execute(_directoryInfo);
		IOrderedEnumerable<FilePatternMatch> orderedEnumerable = patternMatchingResult.Files.OrderBy<FilePatternMatch, string>((FilePatternMatch f) => f.Path, StringComparer.Ordinal);
		using (IncrementalHash incrementalHash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256))
		{
			foreach (FilePatternMatch item in orderedEnumerable)
			{
				DateTime lastWriteUtc = GetLastWriteUtc(item.Path);
				if (_lastScanTimeUtc.HasValue && _lastScanTimeUtc < lastWriteUtc)
				{
					return true;
				}
				ComputeHash(incrementalHash, item.Path, lastWriteUtc);
			}
			byte[] hashAndReset = incrementalHash.GetHashAndReset();
			if (!ArrayEquals(_previousHash, hashAndReset))
			{
				return true;
			}
			_previousHash = hashAndReset;
			_lastScanTimeUtc = Clock.UtcNow;
		}
		return false;
	}

	protected virtual DateTime GetLastWriteUtc(string path)
	{
		string text = Path.Combine(_directoryInfo.FullName, path);
		return FileSystemInfoHelper.GetFileLinkTargetLastWriteTimeUtc(text) ?? File.GetLastWriteTimeUtc(text);
	}

	private static bool ArrayEquals(byte[] previousHash, byte[] currentHash)
	{
		if (previousHash == null)
		{
			return true;
		}
		for (int i = 0; i < previousHash.Length; i++)
		{
			if (previousHash[i] != currentHash[i])
			{
				return false;
			}
		}
		return true;
	}

	private unsafe void ComputeHash(IncrementalHash sha256, string path, DateTime lastChangedUtc)
	{
		int byteCount = Encoding.Unicode.GetByteCount(path);
		if (_byteBuffer == null || byteCount > _byteBuffer.Length)
		{
			_byteBuffer = new byte[Math.Max(byteCount, 256)];
		}
		int bytes = Encoding.Unicode.GetBytes(path, 0, path.Length, _byteBuffer, 0);
		sha256.AppendData(_byteBuffer, 0, bytes);
		sha256.AppendData(Separator, 0, Separator.Length);
		fixed (byte* ptr = _byteBuffer)
		{
			*(long*)ptr = lastChangedUtc.Ticks;
		}
		sha256.AppendData(_byteBuffer, 0, 8);
		sha256.AppendData(Separator, 0, Separator.Length);
	}

	IDisposable IChangeToken.RegisterChangeCallback(Action<object> callback, object state)
	{
		if (!ActiveChangeCallbacks)
		{
			return Microsoft.Extensions.FileProviders.EmptyDisposable.Instance;
		}
		return _changeToken.RegisterChangeCallback(callback, state);
	}
}
