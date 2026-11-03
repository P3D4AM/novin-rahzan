using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using EntityCache.Core;
using EntityCache.Core.Repository.Advertise;
using EntityCache.Mapping.BusinessMappers.Advertise;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Advertise;

public class TitleBlackListCacheRepository : CacheRepository<TitleBlackListBusiness>, ITitleBlackListRepository, IRepository<TitleBlackListBusiness>, INoGuidRepository<TitleBlackListBusiness>, IDisposable
{
	private ITitleBlackListRepository _dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, TitleBlackListBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<TitleBlackListBusiness, TitleBlackListBusiness> Mapper => TitleBlackListMapper.Instance;

	public TitleBlackListCacheRepository(ITitleBlackListRepository dbRepository, int CachingTime)
		: base((IRepository<TitleBlackListBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, TitleBlackListBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<List<TitleBlackListBusiness>> GetAllAsync(string search, CancellationToken token, bool? status, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return _dbRepository.GetAllAsync(search, token, status, tranName);
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				_dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Advertise\\TitleBlackListCacheRepository.cs", 44);
		}
	}
}
