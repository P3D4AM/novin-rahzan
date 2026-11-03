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
using PacketParser.Filters.Attendance;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Attendance;

public class BioDeviceRelationCacheRepository : CacheRepositoryNoGuiId<BioDeviceRelationBusiness, string>, IBioDeviceRelationRepository, INoGuidRepository<BioDeviceRelationBusiness>, IDisposable
{
	private IBioDeviceRelationRepository _dbRepository;

	public override IItemAsyncMapper<BioDeviceRelationBusiness, BioDeviceRelationBusiness> Mapper => BioDeviceRelationMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	protected override TimingCache<string, BioDeviceRelationBusiness> TimingCache { get; set; }

	public BioDeviceRelationCacheRepository(IBioDeviceRelationRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((INoGuidRepository<BioDeviceRelationBusiness>)dbRepository, CachingTime)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<string, BioDeviceRelationBusiness>(base.ContextGuid, Mapper, CachingTime);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Attendance\\Biometric\\BioDeviceRelationCacheRepository.cs", 42);
		}
	}

	public Task<BioDeviceRelationBusiness> GetAsync(Guid bioGuid, Guid deviceGuid, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAsync(bioGuid, deviceGuid, token, tranName);
	}

	public Task<BioDeviceRelationBusiness> GetAsync(Guid deviceGuid, int hardwareCode, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAsync(deviceGuid, hardwareCode, token, tranName);
	}

	public Task<IEnumerable<BioDeviceRelationBusiness>> GetAllAsync(BioDeviceRelationFilter filter, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllAsync(filter, token, tranName);
	}

	public Task<BioDeviceRelationBusiness> GetByPersonAsync(Guid deviceGuid, Guid personGuid, CancellationToken token, string tranName)
	{
		return _dbRepository.GetByPersonAsync(deviceGuid, personGuid, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> RemoveAsync(Guid bioGuid, Guid deviceGuid, CancellationToken token, string tranName)
	{
		return _dbRepository.RemoveAsync(bioGuid, deviceGuid, token, tranName);
	}

	public Task<IEnumerable<BioDeviceRelationModel>> GetAllAsync(Guid deviceGuid, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllAsync(deviceGuid, token, tranName);
	}
}
