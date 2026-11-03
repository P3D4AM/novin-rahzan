using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.Coding.ReportsT;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using EntityCache.Mapping.BusinessMappers.Accounting;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Filters.Hotel;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class HesabGroupCacheRepository : CacheRepository<HesabGroupBusiness>, IHesabGroupRepository, IRepository<HesabGroupBusiness>, INoGuidRepository<HesabGroupBusiness>, IDisposable
{
	private IHesabGroupRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, HesabGroupBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<HesabGroupBusiness, HesabGroupBusiness> Mapper => HesabGroupMapper.Instance;

	public HesabGroupCacheRepository(IHesabGroupRepository dbRepository, int CachingTime)
		: base((IRepository<HesabGroupBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, HesabGroupBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Coding\\HesabGroupCacheRepository.cs", 39);
		}
	}

	public Task<IEnumerable<HesabGroupBusiness>> GetAllAsync(Guid? hesabTypeGuid, bool? status, CancellationToken token, string tranName)
	{
		return dbRepository.GetAllAsync(hesabTypeGuid, status, token, tranName);
	}

	public Task<int> GetNextCodeHesabGroupAsync(Guid typeGuid, CancellationToken token, string tranName)
	{
		return dbRepository.GetNextCodeHesabGroupAsync(typeGuid, token, tranName);
	}

	public Task<IEnumerable<HesabGroupReportBusiness>> GetAllAsync(HesabGroupFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return dbRepository.GetAllAsync(filter, token, tranName);
	}
}
