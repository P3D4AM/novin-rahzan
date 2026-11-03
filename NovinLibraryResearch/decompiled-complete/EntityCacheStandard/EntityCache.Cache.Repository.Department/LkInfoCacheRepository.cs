using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Department;
using EntityCache.Core;
using EntityCache.Core.Repository.Department;
using EntityCache.Mapping.BusinessMappers.Department;
using PacketParser.Caching;
using PacketParser.DepartmentApiModel;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Department;

public class LkInfoCacheRepository : CacheRepositoryNoGuiId<LkInfoBusiness, long>, ILkInfoRepository, INoGuidRepository<LkInfoBusiness>, IDisposable
{
	private ILkInfoRepository dbRepository;

	public override IItemAsyncMapper<LkInfoBusiness, LkInfoBusiness> Mapper => LkInfoMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	protected override TimingCache<long, LkInfoBusiness> TimingCache { get; set; }

	public LkInfoCacheRepository(ILkInfoRepository dbRepository, int CachingTime)
		: base((INoGuidRepository<LkInfoBusiness>)dbRepository, CachingTime)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<long, LkInfoBusiness>(base.ContextGuid, Mapper, CachingTime);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Department\\LkInfoCacheRepository.cs", 44);
		}
	}

	public Task<LkInfoBusiness> GetAsync(long lkSerial, CancellationToken token, string transaction)
	{
		return dbRepository.GetAsync(lkSerial, token, transaction);
	}

	public Task<List<LkInfoBusiness>> GetAllAsync(LkModel filters, CancellationToken token, string tranName)
	{
		return dbRepository.GetAllAsync(filters, token, tranName);
	}

	public async Task<int> GetAllGuidsDontLock()
	{
		return 1;
	}

	public async Task<int> GetAllGuidsHasLock()
	{
		return 1;
	}

	public Task<List<long>> GetAllNotSentAsync(string tranName)
	{
		return dbRepository.GetAllNotSentAsync(tranName);
	}
}
