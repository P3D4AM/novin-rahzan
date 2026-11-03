using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using EntityCache.Core;
using EntityCache.Core.Repository.Advertise;
using EntityCache.Mapping.BusinessMappers.Advertise;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Advertise;

public class OwnerCacheRepository : CacheRepository<OwnerBusiness>, IOwnerRepository, IRepository<OwnerBusiness>, INoGuidRepository<OwnerBusiness>, IDisposable
{
	private IOwnerRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, OwnerBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<OwnerBusiness, OwnerBusiness> Mapper => OwnerMapper.Instance;

	public OwnerCacheRepository(IOwnerRepository dbRepository, int CachingTime)
		: base((IRepository<OwnerBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, OwnerBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<IEnumerable<OwnerBusiness>> GetAllAsync(string search, CancellationToken token, bool? status, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllAsync(search, token, status, tranName);
	}

	public Task<int> DeleteCountAsync(string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.DeleteCountAsync(tranName);
	}

	public Task<bool> CheckNameAsync(string name, Guid guid, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.CheckNameAsync(name, guid, tranName);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Advertise\\OwnerCacheRepository.cs", 57);
		}
	}
}
