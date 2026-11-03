using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Tolid;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Tolid;
using EntityCache.Mapping.BusinessMappers.Accounting.Tolid;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting.Tolid;

public class TolidCacheRepository : CacheEntityRepository<TolidBusiness, TolidReportBusiness>, ITolidRepository, IEntityRepository<TolidBusiness, TolidReportBusiness>, IEditableDelivery, IDisposable
{
	private ITolidRepository _dbRepository;

	public override IItemAsyncMapper<TolidBusiness, TolidBusiness> Mapper => TolidMapper.Instance;

	public bool HasStatus => false;

	public bool HasModified => true;

	public TolidCacheRepository(ITolidRepository dbRepository, int cachingTime)
		: base((IEntityRepository<TolidBusiness, TolidReportBusiness>)dbRepository, cachingTime)
	{
		_dbRepository = dbRepository;
	}

	public Task<int> NextId2Async(CancellationToken token, string tranName)
	{
		return _dbRepository.NextId2Async(token, tranName);
	}

	public Task<IEnumerable<TolidReportBusiness>> GetAllReportAsync(TolidFilter filter, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllReportAsync(filter, token, tranName);
	}

	public Task<TolidBusiness> GetFromDbAsync(Guid guid, CancellationToken token, string tranName)
	{
		return _dbRepository.GetFromDbAsync(guid, token, tranName);
	}

	public override Task<TolidBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAsync(guid, token, tranName);
	}
}
