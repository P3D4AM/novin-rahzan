using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Attendance;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.Attandance;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters.Attendance;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Attendance;

public class DeviceInfoCacheRepository : SyncCacheRepository<DeviceInfoBusiness>, IDeviceInfoRepository, ISyncRepository<DeviceInfoBusiness>, IEditableDelivery, IRepository<DeviceInfoBusiness>, INoGuidRepository<DeviceInfoBusiness>, IDisposable
{
	private IDeviceInfoRepository _dbRepository;

	public override IItemAsyncMapper<DeviceInfoBusiness, DeviceInfoBusiness> Mapper => DeviceInfoMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, DeviceInfoBusiness> TimingCache { get; set; }

	public DeviceInfoCacheRepository(IDeviceInfoRepository dbRepo, int CachingTime, bool cachingEnable = true)
		: base((ISyncRepository<DeviceInfoBusiness>)dbRepo, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepo;
		TimingCache = new TimingCache<Guid, DeviceInfoBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public async Task<DeviceInfoBusiness> GetBySerialAsync(string serial, string tranName, CancellationToken token)
	{
		try
		{
			DeviceInfoBusiness device2 = TimingCache.GetAll().SingleOrDefault((DeviceInfoBusiness p) => p.SerialNumberStr == serial);
			if (device2 != null)
			{
				return device2;
			}
			device2 = await _dbRepository.GetBySerialAsync(serial, tranName, token);
			await TimingCache.UpdateTimeAsync(device2.Guid, device2, tranName);
			return device2;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetBySerialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Attendance\\DeviceInfoCacheRepository.cs", 44);
			throw;
		}
	}

	public async Task<DeviceInfoBusiness> GetByNumberAsync(short number, string tranName, CancellationToken token)
	{
		try
		{
			DeviceInfoBusiness device2 = TimingCache.GetAll().SingleOrDefault((DeviceInfoBusiness p) => p.DeviceNo == number);
			if (device2 != null)
			{
				return device2;
			}
			device2 = await _dbRepository.GetByNumberAsync(number, tranName, token);
			await TimingCache.UpdateTimeAsync(device2.Guid, device2, tranName);
			return device2;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetByNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Attendance\\DeviceInfoCacheRepository.cs", 61);
			throw;
		}
	}

	public Task<List<DeviceInfoBusiness>> GetAllAsync(DeviceInfoFilter filter, string tranName, CancellationToken token)
	{
		return _dbRepository.GetAllAsync(filter, tranName, token);
	}
}
