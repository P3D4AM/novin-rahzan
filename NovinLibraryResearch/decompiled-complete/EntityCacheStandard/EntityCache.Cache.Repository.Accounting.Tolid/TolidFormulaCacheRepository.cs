using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Tolid;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Tolid;
using EntityCache.Mapping.BusinessMappers.Accounting.Tolid;
using PacketParser.Caching;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting.Tolid;

public class TolidFormulaCacheRepository : CacheRepository<TolidFormulaBusiness>, ITolidFormulaRepository, IRepository<TolidFormulaBusiness>, INoGuidRepository<TolidFormulaBusiness>, IDisposable
{
	private ITolidFormulaRepository _dbRepository;

	public override IItemAsyncMapper<TolidFormulaBusiness, TolidFormulaBusiness> Mapper => TolidFormulaMapper.Instance;

	public override bool HasStatus => false;

	public override bool HasModified => true;

	protected override TimingCache<Guid, TolidFormulaBusiness> TimingCache { get; set; }

	public TolidFormulaCacheRepository(ITolidFormulaRepository dbRepository, int CachingTime)
		: base((IRepository<TolidFormulaBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		TimingCache = new TimingCache<Guid, TolidFormulaBusiness>(base.ContextGuid, Mapper, CachingTime);
		_dbRepository = dbRepository;
	}

	public Task<List<TolidFormulaBusiness>> GetAllByProductAsync(Guid prdGuid, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllByProductAsync(prdGuid, token, tranName);
	}

	public override Task<TolidFormulaBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAsync(guid, token, tranName);
	}
}
