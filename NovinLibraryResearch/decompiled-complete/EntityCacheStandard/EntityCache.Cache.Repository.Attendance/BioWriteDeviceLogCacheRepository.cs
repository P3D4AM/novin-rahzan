using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Attendance;
using EntityCache.Core;
using EntityCache.Core.Repository.Attendance;
using EntityCache.Mapping.BusinessMappers.Attendance;
using PacketParser.BusinessLayer.Models.Attendance;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Filters.Attendance;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Attendance;

public class BioWriteDeviceLogCacheRepository : CacheRepository<BioWriteDeviceLogBusiness>, IBioWriteDeviceLogRepository, IRepository<BioWriteDeviceLogBusiness>, INoGuidRepository<BioWriteDeviceLogBusiness>, IDisposable
{
	private IBioWriteDeviceLogRepository _dbRepository;

	public override IItemAsyncMapper<BioWriteDeviceLogBusiness, BioWriteDeviceLogBusiness> Mapper => BioWriteDeviceLogMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	protected override TimingCache<Guid, BioWriteDeviceLogBusiness> TimingCache { get; set; }

	public BioWriteDeviceLogCacheRepository(IBioWriteDeviceLogRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((IRepository<BioWriteDeviceLogBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, BioWriteDeviceLogBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				_dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Attendance\\Biometric\\BioWriteDeviceLogCacheRepository.cs", 41);
		}
	}

	public Task<IEnumerable<BioWriteDeviceLogModel>> GetAllAsync(BioWriteDeviceLogFilter filter, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllAsync(filter, token, tranName);
	}
}
