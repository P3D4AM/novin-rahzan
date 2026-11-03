using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Attendance;
using EntityCache.Core;
using EntityCache.Core.Repository.Attendance;
using EntityCache.Mapping.BusinessMappers.Attendance;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Attendance;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.CompositeKeys;
using PacketParser.EventArgs;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Attendance;

public class BioWriteDeviceCacheRepository : CacheRepositoryNoGuiId<BioWriteDeviceBusiness, string>, IBioWriteDeviceRepository, INoGuidRepository<BioWriteDeviceBusiness>, IDisposable
{
	private IBioWriteDeviceRepository _dbRepository;

	public override IItemAsyncMapper<BioWriteDeviceBusiness, BioWriteDeviceBusiness> Mapper => BioWriteDeviceMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	protected override TimingCache<string, BioWriteDeviceBusiness> TimingCache { get; set; }

	public BioWriteDeviceCacheRepository(IBioWriteDeviceRepository dbRepository, int CachingTime)
		: base((INoGuidRepository<BioWriteDeviceBusiness>)dbRepository, CachingTime)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<string, BioWriteDeviceBusiness>(base.ContextGuid, Mapper, CachingTime);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Attendance\\Biometric\\BioWriteDeviceCacheRepository.cs", 43);
		}
	}

	public Task<BioWriteDeviceBusiness> FetchNextForSendAsync(Guid deviceGuid, CancellationToken token, string tranName)
	{
		return _dbRepository.FetchNextForSendAsync(deviceGuid, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> SetSaveResult(SaveResponse<BioWriteDeviceCompositeKey> item, CancellationToken token, string tranName)
	{
		return _dbRepository.SetSaveResult(item, token, tranName);
	}

	public Task<IEnumerable<BioWriteDeviceModel>> GetAllAync(string contextGuid, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllAync(contextGuid, token, tranName);
	}
}
