using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using EntityCache.Core;
using EntityCache.Core.Repository.Advertise;
using EntityCache.Mapping.BusinessMappers.Advertise;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Advertise;

public class VisitorCityCacheRepository : CacheRepository<VisitorCityBusiness>, IVisitorCityRepository, IRepository<VisitorCityBusiness>, INoGuidRepository<VisitorCityBusiness>, IDisposable
{
	private IVisitorCityRepository _dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, VisitorCityBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<VisitorCityBusiness, VisitorCityBusiness> Mapper => VisitorCityMapper.Instance;

	public VisitorCityCacheRepository(IVisitorCityRepository dbRepository, int CachingTime)
		: base((IRepository<VisitorCityBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, VisitorCityBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<IEnumerable<VisitorCityBusiness>> GetAllAsync(Guid visitorGuid, string traName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return _dbRepository.GetAllAsync(visitorGuid, traName);
	}

	public Task<ReturnedSaveFuncInfo> RemoveByVisitorGuidAsync(Guid visitorGuid, short tryCount, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return _dbRepository.RemoveByVisitorGuidAsync(visitorGuid, tryCount, tranName);
	}

	public Task<IEnumerable<VisitorCityBusiness>> GetAllCityAsync(Guid cityGuid, string traName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return _dbRepository.GetAllCityAsync(cityGuid, traName);
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				_dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Advertise\\VisitorCityCacheRepository.cs", 54);
		}
	}
}
