using System;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers.Customer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class CustomerAddressCacheRepository : CacheRepository<CustomerAddressBusiness>, ICustomerAddressRepository, IRepository<CustomerAddressBusiness>, INoGuidRepository<CustomerAddressBusiness>, IDisposable
{
	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, CustomerAddressBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<CustomerAddressBusiness, CustomerAddressBusiness> Mapper => CustomerAddressMapper.Instance;

	public CustomerAddressCacheRepository(IRepository<CustomerAddressBusiness> dbRepository, int CachingTime)
		: base(dbRepository, CachingTime, cachingEnable: true)
	{
		TimingCache = new TimingCache<Guid, CustomerAddressBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Customer\\CustomerAddressCacheRepository.cs", 34);
		}
	}
}
