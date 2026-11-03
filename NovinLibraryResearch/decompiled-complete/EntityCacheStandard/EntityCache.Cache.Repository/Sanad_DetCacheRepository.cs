using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers.Sanad;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class Sanad_DetCacheRepository : CacheRepository<Sanad_DetBusiness>, ISanad_DetRepository, IRepository<Sanad_DetBusiness>, INoGuidRepository<Sanad_DetBusiness>, IDisposable
{
	private ISanad_DetRepository dbRepository;

	protected override TimingCache<Guid, Sanad_DetBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<Sanad_DetBusiness, Sanad_DetBusiness> Mapper => SanadDetMapper.Instance;

	public override bool HasStatus { get; }

	public override bool HasModified { get; }

	public Sanad_DetCacheRepository(ISanad_DetRepository dbRepository, int CachingTime)
		: base((IRepository<Sanad_DetBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, Sanad_DetBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public override void Dispose()
	{
		try
		{
			dbRepository = null;
			base.Dispose(disposing: true);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Sanad\\Sanad_DetCacheRepository.cs", 38);
		}
	}

	public Task<List<Sanad_DetBusiness>> GetAllAsync(Guid SanadGuid, Document SanadType, Guid? MoeinGuid, Guid? TafsilGuid, CancellationToken token, string tranName = "")
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SaveListToSanadDetAsync(int SanadGuid, Document SanadType, List<Sanad_DetBusiness> lst, Guid SanadMoeinGuid, Guid SanadTafsilGuid, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> DeleteBySanadNoAsync(int SanadNo, Guid SanadGuid, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<List<Sanad_DetBusiness>> GetAllAsync(int sanadNo, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<Sanad_DetBusiness> items, SanadBusiness master, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.SaveRangeAsync(items, master, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> RemoveRangeAsync(Guid asGuid, Document asType, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.RemoveRangeAsync(asGuid, asType, token, tranName);
	}
}
