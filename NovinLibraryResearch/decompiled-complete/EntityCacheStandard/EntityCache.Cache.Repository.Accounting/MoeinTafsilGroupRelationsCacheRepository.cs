using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using EntityCache.Mapping.BusinessMappers.Accounting;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting;

public class MoeinTafsilGroupRelationsCacheRepository : CacheRepositoryNoGuiId<MoeinTafsilGroupRelationsBusiness, string>, IMoeinTafsilGroupRelationsRepository, INoGuidRepository<MoeinTafsilGroupRelationsBusiness>, IDisposable
{
	private IMoeinTafsilGroupRelationsRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<MoeinTafsilGroupRelationsBusiness, MoeinTafsilGroupRelationsBusiness> Mapper => MoeinTafsilGroupRelationMapper.Instance;

	protected override TimingCache<string, MoeinTafsilGroupRelationsBusiness> TimingCache { get; set; }

	public MoeinTafsilGroupRelationsCacheRepository(IMoeinTafsilGroupRelationsRepository dbRepository, int CachingTime)
		: base((INoGuidRepository<MoeinTafsilGroupRelationsBusiness>)dbRepository, CachingTime)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<string, MoeinTafsilGroupRelationsBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<IEnumerable<MoeinTafsilGroupRelationsBusiness>> GetAllAsync(Guid? moeinGuid, Guid? tafsilGui, CancellationToken token, string tranName)
	{
		return dbRepository.GetAllAsync(moeinGuid, tafsilGui, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> RemoveAsync(Guid? moeinGuid, Guid? tafsilGroupGuid, short tryCount, CancellationToken token, string tranName)
	{
		return dbRepository.RemoveAsync(moeinGuid, tafsilGroupGuid, tryCount, token, tranName);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Coding\\MoeinTafsilGroupRelationsCacheRepository.cs", 44);
		}
	}

	public Task<ReturnedSaveFuncInfo> GetAsync(Guid MoeinGuid, short tryCount, string tranName)
	{
		return GetAsync(MoeinGuid, tryCount, tranName);
	}
}
