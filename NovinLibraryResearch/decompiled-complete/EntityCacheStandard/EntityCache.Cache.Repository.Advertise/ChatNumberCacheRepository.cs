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

public class ChatNumberCacheRepository : CacheRepository<ChatNumberBusiness>, IChatNumberRepository, IRepository<ChatNumberBusiness>, INoGuidRepository<ChatNumberBusiness>, IDisposable
{
	private IChatNumberRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<ChatNumberBusiness, ChatNumberBusiness> Mapper => ChatNumbersMapper.Instance;

	protected override TimingCache<Guid, ChatNumberBusiness> TimingCache { get; set; }

	public ChatNumberCacheRepository(IChatNumberRepository dbRepository, int CachingTime)
		: base((IRepository<ChatNumberBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, ChatNumberBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public async Task<bool> CheckNumber(string number, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return await dbRepository.CheckNumber(number, tranName);
	}

	public async Task<IEnumerable<ChatNumberBusiness>> GetAllAsyncBySp(bool? status, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return await dbRepository.GetAllAsync(status, token, tranName);
	}

	public async Task<List<string>> GetAllCities(string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return await dbRepository.GetAllCities(tranName);
	}

	public async Task<List<string>> GetAllCategories(string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return await dbRepository.GetAllCategories(tranName);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Advertise\\ChatNumberCacheRepository.cs", 63);
		}
	}
}
