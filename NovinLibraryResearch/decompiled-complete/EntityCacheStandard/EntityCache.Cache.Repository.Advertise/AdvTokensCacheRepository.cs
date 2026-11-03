using System;
using System.Collections.Generic;
using System.Linq;
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

public class AdvTokensCacheRepository : CacheRepository<AdvTokensBusiness>, IAdvTokensRepository, IRepository<AdvTokensBusiness>, INoGuidRepository<AdvTokensBusiness>, IDisposable
{
	private IAdvTokensRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<AdvTokensBusiness, AdvTokensBusiness> Mapper => AdvTokensMapper.Instance;

	protected override TimingCache<Guid, AdvTokensBusiness> TimingCache { get; set; }

	public AdvTokensCacheRepository(IAdvTokensRepository dbRepository, int CachingTime)
		: base((IRepository<AdvTokensBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, AdvTokensBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public async Task<AdvTokensBusiness> GetBySimCardAsync(long simCardNumber, AdvertiseType type, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return (await GetAllAsync(default(CancellationToken), tranName)).FirstOrDefault((AdvTokensBusiness q) => q.Number == simCardNumber && q.Type == type);
	}

	public Task<IEnumerable<AdvTokensBusiness>> GetAllAsync(long simcardNumber, bool? status, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllAsync(simcardNumber, status, tranName);
	}

	public async Task<IEnumerable<AdvTokensBusiness>> GetAllAsync(long simcardNumber, AdvertiseType type, bool? status, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return await dbRepository.GetAllAsync(simcardNumber, type, status, tranName);
	}

	public async Task<IEnumerable<AdvTokensBusiness>> GetAllAsync(AdvertiseType type, bool? status, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return await dbRepository.GetAllAsync(type, status, tranName);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Advertise\\AdvTokensCacheRepository.cs", 63);
		}
	}
}
