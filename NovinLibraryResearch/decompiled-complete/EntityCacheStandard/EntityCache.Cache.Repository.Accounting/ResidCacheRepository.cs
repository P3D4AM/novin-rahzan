using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using EntityCache.Mapping.BusinessMappers.Accounting;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Filters;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting;

public class ResidCacheRepository : CacheRepository<ResidBusiness>, IResidRepository, IRepository<ResidBusiness>, INoGuidRepository<ResidBusiness>, IDisposable
{
	private IResidRepository _dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<ResidBusiness, ResidBusiness> Mapper => ResidMapper.Instance;

	protected override TimingCache<Guid, ResidBusiness> TimingCache { get; set; }

	public ResidCacheRepository(IResidRepository dbRepository, int CachingTime)
		: base((IRepository<ResidBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, ResidBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<ResidBusiness> GetAsync(long Id2, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return _dbRepository.GetAsync(Id2, token, tranName);
	}

	public Task<IEnumerable<ResidCompactBusiness>> GetAllCompactAsync(ResidFilter filter, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllCompactAsync(filter, token, tranName);
	}

	public Task<long> GetNextId2Async(int? statusId, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return _dbRepository.GetNextId2Async(statusId, token, tranName);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\ResidCacheRepository.cs", 51);
		}
	}

	public Task<ResidBusiness> GetByOrderGuidAsync(Guid guid, CancellationToken token, string tranName)
	{
		return _dbRepository.GetByOrderGuidAsync(guid, token, tranName);
	}

	public Task<ResidBusiness> GetByReceptionGuidAsync(Guid guid, CancellationToken token, string tranName)
	{
		return _dbRepository.GetByReceptionGuidAsync(guid, token, tranName);
	}

	public Task<List<string>> GetSuggestionListAsync(CancellationToken token, string tranName)
	{
		return _dbRepository.GetSuggestionListAsync(token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> ChangeStatusIdAsync(Guid guid, int statusId, CancellationToken token, string tranName)
	{
		return _dbRepository.ChangeStatusIdAsync(guid, statusId, token, tranName);
	}
}
