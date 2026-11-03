using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using EntityCache.Core;
using EntityCache.Core.Repository.Advertise;
using EntityCache.Mapping.BusinessMappers.Advertise;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Advertise;

public class VisitorCacheRepository : CacheRepository<VisitorBusiness>, IVisitorRepository, IRepository<VisitorBusiness>, INoGuidRepository<VisitorBusiness>, IDisposable
{
	private IVisitorRepository _dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<VisitorBusiness, VisitorBusiness> Mapper => VisitorMapper.Instance;

	protected override TimingCache<Guid, VisitorBusiness> TimingCache { get; set; }

	public VisitorCacheRepository(IVisitorRepository dbRepository, int CachingTime)
		: base((IRepository<VisitorBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, VisitorBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<MasterSlaveAdvReplacementsModel> GetMasterSlaveAdvReplacementsAsync(long simcardNumber, string traName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return _dbRepository.GetMasterSlaveAdvReplacementsAsync(simcardNumber, traName);
	}

	public Task<List<VisitorBusiness>> GetAllAsync(string search, CancellationToken token, bool? status, string traName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return _dbRepository.GetAllAsync(search, token, status, traName);
	}

	public Task<ReturnedSaveFuncInfo> ChangeEnableStatusAsync(Guid guid, string status, short tryCount, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return _dbRepository.ChangeEnableStatusAsync(guid, status, tryCount, tranName);
	}

	public Task<int> DeleteCountAsync(short tryCount, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return _dbRepository.DeleteCountAsync(tryCount, tranName);
	}

	public Task<List<VisitorBusiness>> GetAllAsync(Guid depGuid, string traName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return _dbRepository.GetAllAsync(depGuid, traName);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Advertise\\VisitorCacheRepository.cs", 64);
		}
	}
}
