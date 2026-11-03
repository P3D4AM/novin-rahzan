using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global;
using EntityCache.Core;
using EntityCache.Core.Repository.Global;
using EntityCache.Mapping.Global;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Filters;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Global;

public class SaveTempCacheRepository : CacheRepository<SaveTempBusiness>, ISaveTempRepository, IRepository<SaveTempBusiness>, INoGuidRepository<SaveTempBusiness>, IDisposable
{
	private ISaveTempRepository _dbRepository;

	public override IItemAsyncMapper<SaveTempBusiness, SaveTempBusiness> Mapper => SaveTempMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, SaveTempBusiness> TimingCache { get; set; }

	public SaveTempCacheRepository(ISaveTempRepository dbRepository, int CachingTime)
		: base((IRepository<SaveTempBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, SaveTempBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<IEnumerable<SaveTempViewModel>> GetAllAsync(SaveTempFilter filter, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllAsync(filter, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> RemoveRangeAsync(Guid asGuid, Document asType, CancellationToken token, string tranName)
	{
		return _dbRepository.RemoveRangeAsync(asGuid, asType, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> RemoveRangeByDateAsync(CancellationToken token, string tranName)
	{
		return _dbRepository.RemoveRangeByDateAsync(token, tranName);
	}
}
