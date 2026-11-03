using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Aghsat;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting.Aghsat;
using EntityCache.Mapping.BusinessMappers.Accounting.Aghsat;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting.Aghsat;

public class AghsatM_GroupCacheRepository : SyncCacheRepository<AghsatM_GroupBusiness>, IAghsatM_GroupRepository, ISyncRepository<AghsatM_GroupBusiness>, IEditableDelivery, IRepository<AghsatM_GroupBusiness>, INoGuidRepository<AghsatM_GroupBusiness>, IDisposable
{
	private IAghsatM_GroupRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<AghsatM_GroupBusiness, AghsatM_GroupBusiness> Mapper => AghsatMGroupMapper.Instance;

	public new bool IsDisposed { get; private set; }

	protected override TimingCache<Guid, AghsatM_GroupBusiness> TimingCache { get; set; }

	public AghsatM_GroupCacheRepository(IAghsatM_GroupRepository dbRepository, int CachingTime)
		: base((ISyncRepository<AghsatM_GroupBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, AghsatM_GroupBusiness>(base.Context.Guid, Mapper, CachingTime);
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Aghsat\\AghsatM_GroupCacheRepository.cs", 42);
		}
	}

	public Task<ReturnedSaveFuncInfo> UpdateSumAsync(Guid guid, CancellationToken token, string tranName)
	{
		return dbRepository.UpdateSumAsync(guid, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> SetSanadNoAsync(Guid guid, int sanadNo, CancellationToken token, string tranName)
	{
		return dbRepository.SetSanadNoAsync(guid, sanadNo, token, tranName);
	}
}
