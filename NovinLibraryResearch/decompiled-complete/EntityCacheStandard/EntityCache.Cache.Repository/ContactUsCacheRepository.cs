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

namespace EntityCache.Cache.Repository;

public class ContactUsCacheRepository : SyncCacheRepository<ContactUsBusiness>, IContactUsRepository, ISyncRepository<ContactUsBusiness>, IEditableDelivery, IRepository<ContactUsBusiness>, INoGuidRepository<ContactUsBusiness>, IDisposable
{
	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, ContactUsBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<ContactUsBusiness, ContactUsBusiness> Mapper => ContactUsMapper.Instance;

	public ContactUsCacheRepository(IContactUsRepository dbRepository, int CachingTime)
		: base((ISyncRepository<ContactUsBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		TimingCache = new TimingCache<Guid, ContactUsBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public async Task<IEnumerable<ContactUsBusiness>> GetAllAsync(TypeContact type, bool? status, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\ContactUsCacheRepository.cs"))
		{
			_ = 1;
			try
			{
				await LoadCacheAsync(token, tranName);
				return (await GetAllAsync(status, token, tranName)).Where((ContactUsBusiness x) => x.Type == type)?.ToList();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\ContactUsCacheRepository.cs", 37);
				throw;
			}
		}
	}

	public override void Dispose()
	{
		try
		{
			base.Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\ContactUsCacheRepository.cs", 50);
		}
	}
}
