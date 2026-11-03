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

public class StateCacheRepository : CacheRepository<StateBusiness>, IStateRepository, IRepository<StateBusiness>, INoGuidRepository<StateBusiness>, IDisposable
{
	private IStateRepository _dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, StateBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<StateBusiness, StateBusiness> Mapper => StateMapper.Instance;

	public StateCacheRepository(IStateRepository dbRepository, int CachingTime)
		: base((IRepository<StateBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, StateBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<List<StateBusiness>> GetAllAsync(string search, CancellationToken token, bool? status, string tranName)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Advertise\\StateCacheRepository.cs", 45);
		}
	}
}
