using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using EntityCache.Business.Advertise.NoBisunessModels;
using EntityCache.Core;
using EntityCache.Core.Repository.Advertise;
using EntityCache.Mapping.BusinessMappers.Advertise;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces.Advertise;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Advertise;

public class AdvertiseLogCacheRepository : CacheRepository<AdvertiseLogBusiness>, IAdvertiseLogRepository, IRepository<AdvertiseLogBusiness>, INoGuidRepository<AdvertiseLogBusiness>, IDisposable
{
	private IAdvertiseLogRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<AdvertiseLogBusiness, AdvertiseLogBusiness> Mapper => AdvertiseLogMapper.Instance;

	protected override TimingCache<Guid, AdvertiseLogBusiness> TimingCache { get; set; }

	public AdvertiseLogCacheRepository(IAdvertiseLogRepository dbRepository, int CachingTime)
		: base((IRepository<AdvertiseLogBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, AdvertiseLogBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<AdvertiseLogBusiness> GetLastAdvAsync(string advName, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetLastAdvAsync(advName, tranName);
	}

	public Task<List<AdvertiseLogBusiness>> GetAllSpecialAsync(Expression<Func<IAdvertiseLog, bool>> where, Func<IQueryable<IAdvertiseLog>, IOrderedQueryable<IAdvertiseLog>> orderby, string includes, int takeCount, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllSpecialAsync(where, orderby, includes, takeCount, tranName);
	}

	public Task<int> GetAllAdvInDayFromIP(string ip, AdvertiseType type, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllAdvInDayFromIP(ip, type, tranName);
	}

	public Task<IEnumerable<AdvertiseLogBusiness>> GetAllAsync(string search, short state, AdvertiseType type, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllAsync(search, state, type, token, tranName);
	}

	public Task<List<int>> GetAdvCountInSpecialMounthAsync(DateTime firstDate, DateTime secondDate, AdvertiseType type, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAdvCountInSpecialMounthAsync(firstDate, secondDate, type, tranName);
	}

	public Task<List<int>> GetPublishedAdvCountInSpecialMounthAsync(DateTime firstDate, DateTime secondDate, AdvertiseType type, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetPublishedAdvCountInSpecialMounthAsync(firstDate, secondDate, type, tranName);
	}

	public Task<AdvertiseLogBusiness> GetAsync(string url, string tranName)
	{
		return dbRepository.GetAsync(url, tranName);
	}

	public Task<ReturnedSaveFuncInfo> RemoveRangeByDateAsync(short tryCount, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.RemoveRangeByDateAsync(tryCount, tranName);
	}

	public Task<IEnumerable<AdvertiseLogViewModel>> GetAllAsync(long number, DateTime d1, DateTime d2, AdvStatusCode statusCode, AdvertiseType type, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllAsync(number, d1, d2, statusCode, type, tranName);
	}

	public Task<IEnumerable<AdvertiseLogBusiness>> GetAllNotFullAsync(DateTime date1, DateTime date2, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllNotFullAsync(date1, date2, tranName);
	}

	public Task<IEnumerable<AdvertiseLogBusiness>> GetAllEditNeedAsync(DateTime date1, DateTime date2, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllEditNeedAsync(date1, date2, tranName);
	}

	public Task<IEnumerable<AdvertiseLogBusiness>> GetAdvCountInMountAsync(DateTime date1, DateTime date2, long number, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAdvCountInMountAsync(date1, date2, number, tranName);
	}

	public Task<IEnumerable<AdvertiseLogBusiness>> GetPublishAdvCountInMountAsync(DateTime date1, DateTime date2, long number, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetPublishAdvCountInMountAsync(date1, date2, number, tranName);
	}

	public Task<IEnumerable<AdvCountReport>> GetAllAdvCountReportAsync(string monthStart, string monthEnd, string dateSh, string ip, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllAdvCountReportAsync(monthStart, monthEnd, dateSh, ip, tranName);
	}

	public Task<AdvertiseLogBusiness> GetLastSendAsync(long number, AdvertiseType type, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetLastSendAsync(number, type, tranName);
	}

	public Task<IEnumerable<AdvertiseLogBusiness>> GetAllByNextUseAsync(DateTime date1, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllByNextUseAsync(date1, tranName);
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Advertise\\AdvertiseLogCacheRepository.cs", 138);
		}
	}
}
