using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Attendance;
using EntityCache.Core;
using EntityCache.Core.Repository.Attendance;
using EntityCache.Mapping.BusinessMappers.Attendance;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Attendance.Biometric;

public class BiometricCacheRepository : CacheRepository<BiometricBusiness>, IBiometricRepository, IRepository<BiometricBusiness>, INoGuidRepository<BiometricBusiness>, IDisposable
{
	private IBiometricRepository _dbRepository;

	public override IItemAsyncMapper<BiometricBusiness, BiometricBusiness> Mapper => BiometricMapper.Instance;

	public override bool HasStatus => false;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	protected override TimingCache<Guid, BiometricBusiness> TimingCache { get; set; }

	public BiometricCacheRepository(IBiometricRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((IRepository<BiometricBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, BiometricBusiness>(base.ContextGuid, Mapper, CachingTime);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Attendance\\Biometric\\BiometricCacheRepository.cs", 38);
		}
	}

	public Task<BiometricBusiness> GetAsync(Guid personGuid, EnDeviceGroup deviceGroup, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAsync(personGuid, deviceGroup, token, tranName);
	}
}
