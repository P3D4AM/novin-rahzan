using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Aghsat;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting.Aghsat;
using EntityCache.Mapping.BusinessMappers.Accounting.Aghsat;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;
using PacketParser.Filters.Khazane;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting.Aghsat;

public class AghsatShCacheRepository : SyncCacheRepository<AghsatShBusiness>, IAghsatShRepository, ISyncRepository<AghsatShBusiness>, IEditableDelivery, IRepository<AghsatShBusiness>, INoGuidRepository<AghsatShBusiness>, IDisposable
{
	private IAghsatShRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	protected override TimingCache<Guid, AghsatShBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<AghsatShBusiness, AghsatShBusiness> Mapper => AghsatShMapper.Instance;

	public AghsatShCacheRepository(IAghsatShRepository dbRepository, int CachingTime)
		: base((ISyncRepository<AghsatShBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, AghsatShBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<int> NextId2Async(CancellationToken token, string tranName)
	{
		return dbRepository.NextId2Async(token, tranName);
	}

	public Task<IEnumerable<AghsatShBusiness>> GetAllAsync(AghsatShFilter filter, CancellationToken token, string tranName)
	{
		return dbRepository.GetAllAsync(filter, token, tranName);
	}

	public Task<AghsatShBusiness> GetAsync(int id2, CancellationToken token, string tranName)
	{
		return dbRepository.GetAsync(id2, token, tranName);
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Aghsat\\AghsatShCacheRepository.cs", 50);
		}
	}

	public Task<IEnumerable<BedehiTalabModel>> GetAllReportAsync(BedehiTalabReportFilter filter, CancellationToken token, string tranName)
	{
		return dbRepository.GetAllReportAsync(filter, token, tranName);
	}

	public Task<List<AghsatSHSummaryViewModel>> GetSummaryAsync(DateTime? date1, DateTime? date2, CancellationToken token, string tranName)
	{
		return dbRepository.GetSummaryAsync(date1, date2, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> UpdatePardakhtInfoAsync(Guid? pardakhtGuid, CancellationToken token, string tranName)
	{
		return dbRepository.UpdatePardakhtInfoAsync(pardakhtGuid, token, tranName);
	}
}
