using System;
using System.Collections.Generic;
using System.IO;
using PacketParser.Entities;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace PacketParser;

public class LogCache
{
	private Dictionary<string, ILogger> _Cache = new Dictionary<string, ILogger>();

	private ENSource _Source;

	private string _Path;

	private string _Version;

	private string _Lock;

	private LogEventLevel _RestrictedToMinimumLevel;

	private bool _Buffered;

	private bool _Shared;

	private bool _RollOnFileSizeLimit;

	private bool _IsInitialized;

	private int? _RetainedFileCountLimit;

	private RollingInterval _RollingInterval;

	private long? _FileSizeLimitBytes;

	public static Lazy<LogCache> Instance = new Lazy<LogCache>(() => new LogCache());

	public ILogger Get(string loggerName)
	{
		if (_Cache.ContainsKey(loggerName))
		{
			return _Cache[loggerName];
		}
		return null;
	}

	public ILogger GetConnectionLogger()
	{
		return Get("ConnectionLogger");
	}

	public ILogger GetLkLogger()
	{
		return Get("LkLogger");
	}

	public ILogger GetHesabYarLogger()
	{
		return Get("HesabYarLogger");
	}

	public ILogger GetContextLogger()
	{
		return Get("ContextLogger");
	}

	public ILogger GetOrCreate(string loggerName)
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		if (string.IsNullOrEmpty(_Path))
		{
			throw new ArgumentException("not initialized");
		}
		if (!Directory.Exists(_Path))
		{
			throw new Exception("directory does not exists (" + _Path + ")");
		}
		if (!_IsInitialized)
		{
			throw new InvalidOperationException("Cache is not inititialized, therefore it can not create new loggers");
		}
		if (_Cache.ContainsKey(loggerName))
		{
			return _Cache[loggerName];
		}
		LoggerConfiguration val = new LoggerConfiguration().Enrich.WithProperty("Source", (object)_Source.ToString(), false).Enrich.WithProperty("Version", (object)_Version, false);
		if (!string.IsNullOrEmpty(_Lock))
		{
			val.Enrich.WithProperty("Lk", (object)_Lock, false);
		}
		Logger val2 = val.CreateLogger();
		_Cache.Add(loggerName, (ILogger)(object)val2);
		return (ILogger)(object)val2;
	}

	public void Set(string loggerName, ILogger logger)
	{
		if (_Cache.ContainsKey(loggerName))
		{
			_Cache[loggerName] = logger;
		}
		else
		{
			_Cache.Add(loggerName, logger);
		}
	}

	public void Init(ENSource source, string version, string path, string lk = "", LogEventLevel restrictedToMinimumLevel = 2, long? fileSizeLimitBytes = 1073741824L, bool buffered = false, bool shared = false, RollingInterval rollingInterval = 0, bool rollOnFileSizeLimit = false, int? retainedFileCountLimit = 31)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		_Source = source;
		_Lock = lk;
		_Version = version;
		_Path = path;
		_RestrictedToMinimumLevel = restrictedToMinimumLevel;
		_FileSizeLimitBytes = fileSizeLimitBytes;
		_Buffered = buffered;
		_Shared = shared;
		_RollingInterval = rollingInterval;
		_RollOnFileSizeLimit = rollOnFileSizeLimit;
		_RetainedFileCountLimit = retainedFileCountLimit;
		_IsInitialized = true;
	}
}
