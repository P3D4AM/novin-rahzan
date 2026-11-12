using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Protocols;

public class ConfigurationManager<T> : IConfigurationManager<T> where T : class
{
	public static readonly TimeSpan DefaultAutomaticRefreshInterval;

	public static readonly TimeSpan DefaultRefreshInterval;

	public static readonly TimeSpan MinimumAutomaticRefreshInterval;

	public static readonly TimeSpan MinimumRefreshInterval;

	private TimeSpan _automaticRefreshInterval = DefaultAutomaticRefreshInterval;

	private TimeSpan _refreshInterval = DefaultRefreshInterval;

	private DateTimeOffset _syncAfter = DateTimeOffset.MinValue;

	private DateTimeOffset _lastRefresh = DateTimeOffset.MinValue;

	private readonly SemaphoreSlim _refreshLock;

	private readonly string _metadataAddress;

	private readonly IDocumentRetriever _docRetriever;

	private readonly IConfigurationRetriever<T> _configRetriever;

	private T _currentConfiguration;

	public TimeSpan AutomaticRefreshInterval
	{
		get
		{
			return _automaticRefreshInterval;
		}
		set
		{
			if (value < MinimumAutomaticRefreshInterval)
			{
				throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException("value", LogHelper.FormatInvariant("IDX20107: When setting AutomaticRefreshInterval, the value must be greater than MinimumAutomaticRefreshInterval: '{0}'. value: '{1}'.", MinimumAutomaticRefreshInterval, value)));
			}
			_automaticRefreshInterval = value;
		}
	}

	public TimeSpan RefreshInterval
	{
		get
		{
			return _refreshInterval;
		}
		set
		{
			if (value < MinimumRefreshInterval)
			{
				throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException("value", LogHelper.FormatInvariant("IDX20106: When setting RefreshInterval, the value must be greater than MinimumRefreshInterval: '{0}'. value: '{1}'.", MinimumRefreshInterval, value)));
			}
			_refreshInterval = value;
		}
	}

	static ConfigurationManager()
	{
		DefaultAutomaticRefreshInterval = new TimeSpan(1, 0, 0, 0);
		DefaultRefreshInterval = new TimeSpan(0, 0, 0, 30);
		MinimumAutomaticRefreshInterval = new TimeSpan(0, 0, 5, 0);
		MinimumRefreshInterval = new TimeSpan(0, 0, 0, 1);
		LogHelper.LogVerbose("Assembly version info: " + typeof(ConfigurationManager<T>).AssemblyQualifiedName);
	}

	public ConfigurationManager(string metadataAddress, IConfigurationRetriever<T> configRetriever)
		: this(metadataAddress, configRetriever, (IDocumentRetriever)new HttpDocumentRetriever())
	{
	}

	public ConfigurationManager(string metadataAddress, IConfigurationRetriever<T> configRetriever, HttpClient httpClient)
		: this(metadataAddress, configRetriever, (IDocumentRetriever)new HttpDocumentRetriever(httpClient))
	{
	}

	public ConfigurationManager(string metadataAddress, IConfigurationRetriever<T> configRetriever, IDocumentRetriever docRetriever)
	{
		if (string.IsNullOrWhiteSpace(metadataAddress))
		{
			throw LogHelper.LogArgumentNullException("metadataAddress");
		}
		if (configRetriever == null)
		{
			throw LogHelper.LogArgumentNullException("configRetriever");
		}
		if (docRetriever == null)
		{
			throw LogHelper.LogArgumentNullException("docRetriever");
		}
		_metadataAddress = metadataAddress;
		_docRetriever = docRetriever;
		_configRetriever = configRetriever;
		_refreshLock = new SemaphoreSlim(1);
	}

	public async Task<T> GetConfigurationAsync()
	{
		return await GetConfigurationAsync(CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<T> GetConfigurationAsync(CancellationToken cancel)
	{
		DateTimeOffset now = DateTimeOffset.UtcNow;
		if (_currentConfiguration != null && _syncAfter > now)
		{
			return _currentConfiguration;
		}
		await _refreshLock.WaitAsync(cancel).ConfigureAwait(continueOnCapturedContext: false);
		try
		{
			if (_syncAfter <= now)
			{
				try
				{
					_currentConfiguration = await _configRetriever.GetConfigurationAsync(_metadataAddress, _docRetriever, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
					_lastRefresh = now;
					_syncAfter = DateTimeUtil.Add(now.UtcDateTime, _automaticRefreshInterval);
				}
				catch (Exception innerException)
				{
					_syncAfter = DateTimeUtil.Add(now.UtcDateTime, (_automaticRefreshInterval < _refreshInterval) ? _automaticRefreshInterval : _refreshInterval);
					if (_currentConfiguration == null)
					{
						throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant("IDX20803: Unable to obtain configuration from: '{0}'.", _metadataAddress ?? "null"), innerException));
					}
					LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant("IDX20806: Unable to obtain an updated configuration from: '{0}'. Returning the current configuration.", _metadataAddress ?? "null"), innerException));
				}
			}
			if (_currentConfiguration != null)
			{
				return _currentConfiguration;
			}
			throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant("IDX20803: Unable to obtain configuration from: '{0}'.", _metadataAddress ?? "null")));
		}
		finally
		{
			_refreshLock.Release();
		}
	}

	public void RequestRefresh()
	{
		DateTimeOffset utcNow = DateTimeOffset.UtcNow;
		if (utcNow >= DateTimeUtil.Add(_lastRefresh.UtcDateTime, RefreshInterval))
		{
			_syncAfter = utcNow;
		}
	}
}
