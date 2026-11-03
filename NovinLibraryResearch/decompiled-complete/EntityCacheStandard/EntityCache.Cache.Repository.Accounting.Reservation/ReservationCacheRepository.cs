using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Reservation;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Reservation;
using EntityCache.Mapping.BusinessMappers.Accounting.Reservation;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Filters.Hotel;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting.Reservation;

public class ReservationCacheRepository : CacheRepository<ReservationBusiness>, IReservationRepository, IRepository<ReservationBusiness>, INoGuidRepository<ReservationBusiness>, IDisposable
{
	private IReservationRepository _dbRepository;

	public override IItemAsyncMapper<ReservationBusiness, ReservationBusiness> Mapper => ReservationMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	protected override TimingCache<Guid, ReservationBusiness> TimingCache { get; set; }

	public ReservationCacheRepository(IReservationRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((IRepository<ReservationBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, ReservationBusiness>(base.ContextGuid, Mapper, CachingTime);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Reservation\\ReservationCacheRepository.cs", 41);
		}
	}

	public Task<IEnumerable<ReservationReportBusiness>> GetAllModelAsync(ReservationFilter filter, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllModelAsync(filter, token, tranName);
	}

	public Task<int> GetNextNumberAsync(CancellationToken token, string tranName)
	{
		return _dbRepository.GetNextNumberAsync(token, tranName);
	}

	public Task<ReservationBusiness> GetAsync(int number, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAsync(number, token, tranName);
	}
}
