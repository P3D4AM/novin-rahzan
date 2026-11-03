using System;
using EntityCache.Core;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.WebService;

public class WebServiceDetailRepository<T> : INpDisposable, IDisposable where T : class, IHasGuid, IHasContext
{
	private bool _isDisposed;

	protected ClsCache Cache;

	protected IDetailRepository<T> CacheRepository { get; set; }

	public string ContextGuid => Cache.ContextGuid;

	public bool IsDisposed => _isDisposed;

	public WebServiceDetailRepository(IDetailRepository<T> cacheRepository, ClsCache cache)
	{
		Cache = cache ?? throw new ArgumentNullException("cache");
		CacheRepository = cacheRepository ?? throw new ArgumentNullException("cacheRepository");
	}

	public virtual void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				Cache = null;
				CacheRepository = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\WebServiceDetailRepository.cs", 33);
		}
	}
}
