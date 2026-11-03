using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using EntityCache.Mapping.BusinessMappers.Accounting;
using PacketParser.BusinessLayer.Models.Customer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting;

public class TafsilSelectedListCacheRepository : CacheRepository<TafsilSelectedBusiness>, ITafsilSelectedListRepository, IRepository<TafsilSelectedBusiness>, INoGuidRepository<TafsilSelectedBusiness>, IDisposable
{
	private ITafsilSelectedListRepository _dbRepository;

	public override IItemAsyncMapper<TafsilSelectedBusiness, TafsilSelectedBusiness> Mapper => TafsilSelectedMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, TafsilSelectedBusiness> TimingCache { get; set; }

	public TafsilSelectedListCacheRepository(ITafsilSelectedListRepository dbRepository, int cachingTime)
		: base((IRepository<TafsilSelectedBusiness>)dbRepository, cachingTime, cachingEnable: true)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, TafsilSelectedBusiness>(base.ContextGuid, Mapper, base.CachingTime);
	}

	public async Task<IEnumerable<TafsilSelectedBusiness>> GetAllAsync(TafsilSelectedFilter filter, CancellationToken token, string tranName)
	{
		try
		{
			if (base.IsDisposed)
			{
				throw new ObjectDisposedException(GetType().Name);
			}
			return await _dbRepository.GetAllAsync(filter, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Selection\\TafsilSelectedListCacheRepository.cs", 39);
			throw;
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				_dbRepository?.Dispose();
				_dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Selection\\TafsilSelectedListCacheRepository.cs", 52);
		}
	}
}
