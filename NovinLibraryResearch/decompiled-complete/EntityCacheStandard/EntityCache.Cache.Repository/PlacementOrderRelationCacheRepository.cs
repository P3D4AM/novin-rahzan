using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.OrderDepot.OpenInvoices;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Order.OpenInvoices;
using EntityCache.Mapping.BusinessMappers.OrderDepot;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class PlacementOrderRelationCacheRepository : CacheRepositoryNoGuiId<PlacementOrderRelationBusiness, string>, IPlacementOrderRelationRepository, INoGuidRepository<PlacementOrderRelationBusiness>, IDisposable
{
	private IPlacementOrderRelationRepository _dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<string, PlacementOrderRelationBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<PlacementOrderRelationBusiness, PlacementOrderRelationBusiness> Mapper => PlacementOrderRelationMapper.Instance;

	public PlacementOrderRelationCacheRepository(IPlacementOrderRelationRepository dbRepository, int CachingTime)
		: base((INoGuidRepository<PlacementOrderRelationBusiness>)dbRepository, CachingTime)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<string, PlacementOrderRelationBusiness>(base.ContextGuid, Mapper, CachingTime);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Order\\OpenInvoices\\PlacementOrderRelationCacheRepository.cs", 39);
		}
	}

	public Task<IEnumerable<PlacementOrderRelationReportBusiness>> GetAllModelAsync(Guid? placementGuid, Guid? orderGuid, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllModelAsync(placementGuid, orderGuid, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> RemoveRangeAsync(List<PlacementOrderRelationBusiness> list, CancellationToken token, string tranName)
	{
		return _dbRepository.RemoveRangeAsync(list, token, tranName);
	}
}
