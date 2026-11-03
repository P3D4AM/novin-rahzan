using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace EntityCache.Cache.Repository;

internal class OrderDetailCacheRepository : MasterCacheRepository<OrderDetailBusiness>, IOrderDetailRepository, IPersistenceMasterRepository<OrderDetailBusiness>, IDisposable
{
	private IOrderDetailRepository dbRepository;

	public OrderDetailCacheRepository(IOrderDetailRepository dbRepository, int CachingTime)
		: base((IPersistenceMasterRepository<OrderDetailBusiness>)dbRepository, CachingTime)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
	}

	[Obsolete("please use another overLoad and pass orderType", true)]
	public override Task<ReturnedSaveFuncInfo> RemoveAsync(Guid MasterGuid, short trycount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException("please use anotheroverLoads and pass orderType");
	}

	public virtual Task<ReturnedSaveFuncInfo> RemoveAsync(Guid MasterGuid, Document factorType, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.RemoveAsync(MasterGuid, factorType, token, tranName);
	}

	public Task GetAllAsync(OrderBusiness order, CancellationToken token, string tranName = "")
	{
		return dbRepository.GetAllAsync(order, token, tranName);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Order\\OrderDetailCacheRepository.cs", 39);
		}
	}

	public Task<Guid> GetMasterGuidAsync(Guid guid, CancellationToken token, string tranName)
	{
		return dbRepository.GetMasterGuidAsync(guid, token, tranName);
	}
}
