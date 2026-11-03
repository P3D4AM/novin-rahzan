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
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class PrdTechnicalCacheRepository : CacheRepository<PrdTechnicalBusiness>, IPrdTechnicalRepository, IRepository<PrdTechnicalBusiness>, INoGuidRepository<PrdTechnicalBusiness>, IDisposable
{
	private IPrdTechnicalRepository _dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, PrdTechnicalBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<PrdTechnicalBusiness, PrdTechnicalBusiness> Mapper => PrdTechnicalMapper.Instance;

	public PrdTechnicalCacheRepository(IPrdTechnicalRepository dbRepository, int CachingTime)
		: base((IRepository<PrdTechnicalBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		try
		{
			_dbRepository = dbRepository;
			TimingCache = new TimingCache<Guid, PrdTechnicalBusiness>(base.ContextGuid, Mapper, CachingTime);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\PrdTechnicalCacheRepository.cs", 32);
		}
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\PrdTechnicalCacheRepository.cs", 43);
		}
	}

	public Task<IEnumerable<PrdTechnicalBusiness>> GetAllAsync(short technicalType, bool? status, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllAsync(technicalType, status, token, tranName);
	}

	public Task<IEnumerable<PrdTechnicalBusiness>> GetAllAsync(Guid prdGuid, bool? status, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllAsync(prdGuid, status, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> RemoveByProductGuidAsync(Guid prdGuid, CancellationToken token, string tranName)
	{
		return _dbRepository.RemoveByProductGuidAsync(prdGuid, token, tranName);
	}

	public Task<PrdTechnicalBusiness> GetAsync(Guid guid, short technicalType, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAsync(guid, technicalType, token, tranName);
	}
}
