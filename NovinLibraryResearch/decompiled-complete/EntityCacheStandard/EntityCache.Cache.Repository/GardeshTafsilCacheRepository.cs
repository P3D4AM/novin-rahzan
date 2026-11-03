using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Filters;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class GardeshTafsilCacheRepository : CacheRepository<GardeshTafsilBusiness>, IGardeshTafsilRepository, IRepository<GardeshTafsilBusiness>, INoGuidRepository<GardeshTafsilBusiness>, IDisposable
{
	private IGardeshTafsilRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, GardeshTafsilBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<GardeshTafsilBusiness, GardeshTafsilBusiness> Mapper => GardeshTafsilMapper.Instance;

	public GardeshTafsilCacheRepository(IGardeshTafsilRepository dbRepository, int CachingTime)
		: base((IRepository<GardeshTafsilBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, GardeshTafsilBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<IEnumerable<GardeshTafsilBusiness>> GetAllAsync(GardeshTafsiliFilter filter, EnSystem system, string tranName, CancellationToken token)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllAsync(filter, system, tranName, token);
	}

	public Task<IEnumerable<GardeshTafsilBusiness>> GetAllFromVSanadDetAsync(List<Guid> listTafsilGuid, EnSystem system, int top, DateTime? Date1, DateTime? Date2, List<Guid> listMoeinGuid, Guid? kolGuid, bool? status, string tranName, CancellationToken token)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllFromVSanadDetAsync(listTafsilGuid, system, top, Date1, Date2, listMoeinGuid, kolGuid, status, tranName, token);
	}

	public Task<decimal> GardeshBallanceAsync(Guid? tafsilGuid, Guid? moeinGuid, DateTime? date1, DateTime? date2, string tranName, CancellationToken token)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GardeshBallanceAsync(tafsilGuid, moeinGuid, date1, date2, tranName, token);
	}

	public Task<GardeshTafsilBusiness> GetLastTransactionAsync(Guid tafsilGuid, EnSystem system, string tranName, CancellationToken token)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetLastTransactionAsync(tafsilGuid, system, tranName, token);
	}

	public new Task KeepRangeAsync(IEnumerable<Guid> guids, short tryCount, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.KeepRangeAsync(guids, tryCount, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> DeleteRangeAsync(IEnumerable<Guid> kolGuid, IEnumerable<Guid> moeinGuid, IEnumerable<Guid?> tafsilGuid, short tryCount, string tranName, CancellationToken token)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.DeleteRangeAsync(kolGuid, moeinGuid, tafsilGuid, tryCount, tranName, token);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\GardeshTafsilCacheRepository.cs", 75);
		}
	}
}
