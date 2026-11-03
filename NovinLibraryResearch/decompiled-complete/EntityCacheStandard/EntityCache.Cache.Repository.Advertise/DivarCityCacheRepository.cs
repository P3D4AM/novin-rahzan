using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using EntityCache.Core;
using EntityCache.Core.Repository.Advertise;
using EntityCache.Mapping.BusinessMappers.Advertise;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Advertise;

public class DivarCityCacheRepository : CacheRepository<DivarCityBusiness>, IDivarCityRepository, IRepository<DivarCityBusiness>, INoGuidRepository<DivarCityBusiness>, IDisposable
{
	private IDivarCityRepository _dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, DivarCityBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<DivarCityBusiness, DivarCityBusiness> Mapper => DivarCityMapper.Instance;

	public DivarCityCacheRepository(IDivarCityRepository dbRepository, int CachingTime)
		: base((IRepository<DivarCityBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, DivarCityBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<List<DivarCityBusiness>> GetAllAsync(string search, CancellationToken token, bool? status, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return _dbRepository.GetAllAsync(search, token, status, tranName);
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Advertise\\DivarCityCacheRepository.cs", 46);
		}
	}
}
