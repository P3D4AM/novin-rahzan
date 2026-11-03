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

public class CityCacheRepository : CacheRepository<CityBusiness>, ICityRepository, IRepository<CityBusiness>, INoGuidRepository<CityBusiness>, IDisposable
{
	private ICityRepository _dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<CityBusiness, CityBusiness> Mapper => CityMapper.Instance;

	protected override TimingCache<Guid, CityBusiness> TimingCache { get; set; }

	public CityCacheRepository(ICityRepository dbRepository, int CachingTime)
		: base((IRepository<CityBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, CityBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<IEnumerable<CityBusiness>> GetAllAsync(string search, Guid StateGuid, CancellationToken token, bool? status, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return _dbRepository.GetAllAsync(search, StateGuid, token, status, tranName);
	}

	public Task<CityBusiness> GetAsync(string Name, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return _dbRepository.GetAsync(Name, tranName);
	}

	public async Task UpdateAllCityChecked(AdvertiseType type, short tryCount, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		await _dbRepository.UpdateAllCityChecked(type, tryCount, tranName);
	}

	public Task<IEnumerable<CityBusiness>> GetAllAsync(Guid stateGuid, bool? status, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return _dbRepository.GetAllAsync(stateGuid, status, tranName);
	}

	public Task<bool> CheckNameAsync(string name, Guid guid, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return _dbRepository.CheckNameAsync(name, guid, tranName);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Advertise\\CityCacheRepository.cs", 63);
		}
	}
}
