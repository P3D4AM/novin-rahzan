using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using EntityCache.Core;
using EntityCache.Core.Repository.Advertise;
using EntityCache.Mapping.BusinessMappers.Advertise;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Advertise;

public class AdvGroupCacheRepository : CacheRepository<AdvGroupBusiness>, IAdvGroupRepository, IRepository<AdvGroupBusiness>, INoGuidRepository<AdvGroupBusiness>, IDisposable
{
	private IAdvGroupRepository _dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<AdvGroupBusiness, AdvGroupBusiness> Mapper => AdvGroupMapper.Instance;

	protected override TimingCache<Guid, AdvGroupBusiness> TimingCache { get; set; }

	public AdvGroupCacheRepository(IAdvGroupRepository dbRepository, int cachingTime)
		: base((IRepository<AdvGroupBusiness>)dbRepository, cachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, AdvGroupBusiness>(base.ContextGuid, Mapper, base.CachingTime);
	}

	public async Task<bool> CheckNameAsync(string name, Guid guid, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return await _dbRepository.CheckNameAsync(name, guid, tranName);
	}

	public async Task<AdvGroupBusiness> GetAsyncByParent(Guid parentGuid, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return await _dbRepository.GetAsyncByParent(parentGuid, tranName);
	}

	public async Task<List<AdvGroupBusiness>> GetAllAsync(Guid parentGuid, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return await _dbRepository.GetAllAsync(parentGuid, tranName);
	}

	public async Task<AdvGroupBusiness> GetAsync(string name, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return await _dbRepository.GetAsync(name, tranName);
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Advertise\\AdvGroupCacheRepository.cs", 60);
		}
	}
}
