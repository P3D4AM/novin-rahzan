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
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class GpsCacheRepository : SyncCacheRepository<GPSBusiness>, IGpsRepository, ISyncRepository<GPSBusiness>, IEditableDelivery, IRepository<GPSBusiness>, INoGuidRepository<GPSBusiness>, IDisposable
{
	private IGpsRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, GPSBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<GPSBusiness, GPSBusiness> Mapper => GpsMapper.Instance;

	public GpsCacheRepository(IGpsRepository dbRepository, int CachingTimeSeconds)
		: base((ISyncRepository<GPSBusiness>)dbRepository, CachingTimeSeconds, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, GPSBusiness>(base.ContextGuid, Mapper, base.CachingTime);
	}

	public async Task<List<GPSBusiness>> GetAllNotSentAsync(int count, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\GpsCacheRepository.cs"))
		{
			try
			{
				return await dbRepository.GetAllNotSentAsync(count, token, tranName);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\GpsCacheRepository.cs", 39);
				throw;
			}
		}
	}

	public Task<ReturnedSaveFuncInfo> RemoveAllAsync(IEnumerable<Guid> res, short MaxTryCount, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.RemoveAllAsync(res, MaxTryCount, token, tranName);
	}

	public override void Dispose()
	{
		try
		{
			dbRepository = null;
			base.Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\GpsCacheRepository.cs", 60);
		}
	}
}
