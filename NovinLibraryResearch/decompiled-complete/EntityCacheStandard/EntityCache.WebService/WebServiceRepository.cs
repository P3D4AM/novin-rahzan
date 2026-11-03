using System;
using EntityCache.Core;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.WebService;

public class WebServiceRepository<T> : IDisposable where T : class, IHasJustGuid, IHasContext
{
	protected ClsCache Cache;

	protected INoGuidRepository<T> CacheRepository { get; set; }

	public WebContextHolder Context => Cache.Context;

	public string ContextGuid => Context.Guid;

	public bool IsDisposed { get; set; }

	public WebServiceRepository(INoGuidRepository<T> cacheRepository, ClsCache cache)
	{
		Cache = cache ?? throw new ArgumentNullException("cache");
		CacheRepository = cacheRepository ?? throw new ArgumentNullException("cacheRepository");
	}

	public virtual void Dispose()
	{
		Dispose(disposing: true);
	}

	public virtual void Dispose(bool disposing)
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				CacheRepository?.Dispose();
				CacheRepository = null;
				Cache = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\WebServiceRepository.cs", 34);
		}
	}
}
