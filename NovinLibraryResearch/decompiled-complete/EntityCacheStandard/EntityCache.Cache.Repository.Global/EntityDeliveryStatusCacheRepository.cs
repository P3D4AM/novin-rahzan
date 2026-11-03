using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global;
using EntityCache.Core;
using EntityCache.Core.Repository.Global;
using EntityCache.Mapping.Global;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Global;

public class EntityDeliveryStatusCacheRepository : CacheRepositoryNoGuiId<EntityDeliveryStatusBusiness, string>, IEntityDeliveryStatusRepository, INoGuidRepository<EntityDeliveryStatusBusiness>, IDisposable
{
	private IEntityDeliveryStatusRepository _dbRepository;

	public override IItemAsyncMapper<EntityDeliveryStatusBusiness, EntityDeliveryStatusBusiness> Mapper => EntityDeliveryStatusMapper.Instance;

	public override bool HasStatus => false;

	public override bool HasModified => false;

	protected override TimingCache<string, EntityDeliveryStatusBusiness> TimingCache { get; set; }

	public EntityDeliveryStatusCacheRepository(IEntityDeliveryStatusRepository dbRepository, int cachingTime)
		: base((INoGuidRepository<EntityDeliveryStatusBusiness>)dbRepository, cachingTime)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<string, EntityDeliveryStatusBusiness>(base.ContextGuid, Mapper, cachingTime);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Global\\EntityDeliveryStatusCacheRepository.cs", 39);
		}
	}

	public Task<ReturnedSaveFuncInfo> RemoveAsync(Guid entityGuid, EntityPart entityPart, bool isTemp, CancellationToken token, string tranName)
	{
		return _dbRepository.RemoveAsync(entityGuid, entityPart, isTemp, token, tranName);
	}

	public Task<IEnumerable<EntityDeliveryStatusBusiness>> GetAllAsync(EntityPart entityPart, bool isTemp, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllAsync(entityPart, isTemp, token, tranName);
	}
}
