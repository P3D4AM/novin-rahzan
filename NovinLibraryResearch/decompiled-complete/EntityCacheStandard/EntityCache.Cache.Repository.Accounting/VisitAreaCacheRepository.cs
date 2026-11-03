using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting;

public class VisitAreaCacheRepository : SyncCacheRepository<VisitorAreaBusiness>, IVistorAreaRepository, ISyncRepository<VisitorAreaBusiness>, IEditableDelivery, IRepository<VisitorAreaBusiness>, INoGuidRepository<VisitorAreaBusiness>, IDisposable
{
	private IVistorAreaRepository _dbRepository;

	public override IItemAsyncMapper<VisitorAreaBusiness, VisitorAreaBusiness> Mapper => VisitorAreaMapper.Instance;

	public override bool HasStatus => false;

	public override bool HasModified => true;

	protected override TimingCache<Guid, VisitorAreaBusiness> TimingCache { get; set; }

	public VisitAreaCacheRepository(IVistorAreaRepository dbRepository, int CachingTime)
		: base((ISyncRepository<VisitorAreaBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		TimingCache = new TimingCache<Guid, VisitorAreaBusiness>(base.ContextGuid, Mapper, CachingTime);
		_dbRepository = dbRepository;
	}

	public Task<IEnumerable<VisitorAreaBusiness>> GetAllAsync(Guid? parentGuid, bool? status, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllAsync(parentGuid, status, token, tranName);
	}

	public Task<VisitorAreaBusiness> GetAsync(long code, Guid? parentGuid, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAsync(code, parentGuid, token, tranName);
	}

	public async Task<IEnumerable<VisitorAreaBusiness>> GetChildrenAsync(Guid parentGuid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetChildrenAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\VisitAreaCacheRepository.cs"))
		{
			try
			{
				if (parentGuid == Guid.Empty)
				{
					return null;
				}
				return (await GetAllAsync(true, token, tranName))?.Where((VisitorAreaBusiness w) => w.ParentGuid == parentGuid);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetChildrenAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\VisitAreaCacheRepository.cs", 45);
				throw;
			}
		}
	}

	public Task<long> GetNextCodeAsync(short? level, CancellationToken token, string tranName)
	{
		return _dbRepository.GetNextCodeAsync(level, token, tranName);
	}
}
