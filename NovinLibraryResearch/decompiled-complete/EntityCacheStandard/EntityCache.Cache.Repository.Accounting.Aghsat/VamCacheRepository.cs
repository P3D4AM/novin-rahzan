using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using EntityCache.Mapping.BusinessMappers.Accounting.Aghsat;
using PacketParser.BusinessLayer.Models.Aghsat;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting.Aghsat;

public class VamCacheRepository : CacheRepository<VamBusiness>, IVamRepository, IRepository<VamBusiness>, INoGuidRepository<VamBusiness>, IDisposable
{
	private IVamRepository dbRepository;

	public override IItemAsyncMapper<VamBusiness, VamBusiness> Mapper => VamMapper.Instance;

	protected override TimingCache<Guid, VamBusiness> TimingCache { get; set; }

	public new bool IsDisposed { get; private set; }

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public VamCacheRepository(IVamRepository dbRepository, int CachingTime)
		: base((IRepository<VamBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, VamBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<int> NextId2Async(CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.NextId2Async(token, tranName);
	}

	public Task<IEnumerable<VamModel>> GetAllModelAsync(bool? status, CancellationToken token, string traName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllModelAsync(status, token, traName);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\VamCacheRepository.cs", 51);
		}
	}
}
