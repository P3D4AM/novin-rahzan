using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using EntityCache.Core;
using EntityCache.Core.Repository.Advertise;
using EntityCache.Mapping.BusinessMappers.Advertise;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces.Advertise;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Advertise;

public class SimCardCacheRepository : CacheRepository<SimCardBusiness>, ISimCardRepository, IRepository<SimCardBusiness>, INoGuidRepository<SimCardBusiness>, IDisposable
{
	private ISimCardRepository _dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, SimCardBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<SimCardBusiness, SimCardBusiness> Mapper => SimcardMapper.Instance;

	public SimCardCacheRepository(ISimCardRepository dbRepository, int CachingTime)
		: base((IRepository<SimCardBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, SimCardBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<long> GetNextNumberAsync(short advType, short maxDailyCountDivar, short maxDailyCountSheypoor, string IP, short maxMountlyCountDivar, short maxMountlyCountSheypoor, int maxAdvCountInIpInDayDivar, int maxAdvCountInIpInDaySheypoor, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return _dbRepository.GetNextNumberAsync(advType, maxDailyCountDivar, maxDailyCountSheypoor, IP, maxMountlyCountDivar, maxMountlyCountSheypoor, maxAdvCountInIpInDayDivar, maxAdvCountInIpInDaySheypoor, tranName);
	}

	public Task<List<long>> GetSimCardsForSendAdvAsync(string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return _dbRepository.GetSimCardsForSendAdvAsync(tranName);
	}

	public Task<string> GetOwnerNameAsync(long simCardNumber, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return _dbRepository.GetOwnerNameAsync(simCardNumber, tranName);
	}

	public Task<ReturnedSaveFuncInfo> SaveAsync(List<SimCardBusiness> lst, short tryCount, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return _dbRepository.SaveAsync(lst, tryCount, tranName);
	}

	public Task<SimCardBusiness> GetAsync(long number, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return _dbRepository.GetAsync(number, tranName);
	}

	public Task<bool> CheckNumberAsync(long number, Guid guid, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return _dbRepository.CheckNumberAsync(number, guid, tranName);
	}

	public Task<bool> CheckUserNameAsync(string username, Guid guid, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return _dbRepository.CheckUserNameAsync(username, guid, tranName);
	}

	public Task<List<SimCardBusiness>> GetAllSpecialAsync(Expression<Func<ISimCard, bool>> where, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return _dbRepository.GetAllSpecialAsync(where, tranName);
	}

	public Task<IEnumerable<SimCardBusiness>> GetAllAsync(Guid? ownerGuid, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return _dbRepository.GetAllAsync(ownerGuid, tranName);
	}

	public Task<int> DeleteCountAsync(string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return _dbRepository.DeleteCountAsync(tranName);
	}

	public async Task<ReturnedSaveFuncInfo> UpdateAllSheypoorBlocks(bool state, short tryCount, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return await _dbRepository.UpdateAllSheypoorBlocks(state, tryCount, tranName);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Advertise\\SimCardCacheRepository.cs", 106);
		}
	}
}
