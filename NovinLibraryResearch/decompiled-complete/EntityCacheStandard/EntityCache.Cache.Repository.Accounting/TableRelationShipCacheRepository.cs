using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using EntityCache.Mapping.BusinessMappers;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting;

public class TableRelationShipCacheRepository : CacheRepository<TableRelationShipBusiness>, ITableRelationShipRepository, IRepository<TableRelationShipBusiness>, INoGuidRepository<TableRelationShipBusiness>, IDisposable
{
	private ITableRelationShipRepository dbRepository;

	public override bool HasStatus => false;

	public override bool HasModified => false;

	public override IItemAsyncMapper<TableRelationShipBusiness, TableRelationShipBusiness> Mapper => TableRelationShipMapper.Instance;

	protected override TimingCache<Guid, TableRelationShipBusiness> TimingCache { get; set; }

	public TableRelationShipCacheRepository(ITableRelationShipRepository dbRepository, int CachingTime)
		: base((IRepository<TableRelationShipBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, TableRelationShipBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<IEnumerable<TableRelationShipBusiness>> GetAllRelatedSanadAsync(Guid sanadGuid, Document sanadType, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllRelatedSanadAsync(sanadGuid, sanadType, token, tranName);
	}

	public Task<IEnumerable<TableRelationShipBusiness>> GetAllInRelationSanadAsync(Guid sanadGuid, Document sanadType, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllInRelationSanadAsync(sanadGuid, sanadType, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> RemoveRangeAsync(Guid sanadGuid, Document sanadType, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.RemoveRangeAsync(sanadGuid, sanadType, token, tranName);
	}
}
