using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Attendance.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.Attandance;
using PacketParser.Caching;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class CalenderCacheRepository : CacheRepository<CalenderBusiness>, ICalenderRepository, IRepository<CalenderBusiness>, INoGuidRepository<CalenderBusiness>, IDisposable
{
	private ICalenderRepository _dbRepository;

	protected override TimingCache<Guid, CalenderBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<CalenderBusiness, CalenderBusiness> Mapper => CalenderMapper.Instance;

	public override bool HasStatus => false;

	public override bool HasModified => false;

	public CalenderCacheRepository(IRepository<CalenderBusiness> dbRepository, int CachingTime)
		: base(dbRepository, CachingTime, cachingEnable: true)
	{
		TimingCache = new TimingCache<Guid, CalenderBusiness>(base.ContextGuid, Mapper, CachingTime);
		_dbRepository = (ICalenderRepository)dbRepository;
	}

	public Task<IEnumerable<CalenderBusiness>> GetAllAsync(DateTime? d1, DateTime? d2, bool? status, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllAsync(d1, d2, status, token, tranName);
	}

	public Task<CalenderBusiness> GetAsync(Guid guid, DateTime dateM, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAsync(guid, dateM, token, tranName);
	}

	public Task<CalenderBusiness> GetAsync(DateTime dateM, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAsync(dateM, token, tranName);
	}
}
