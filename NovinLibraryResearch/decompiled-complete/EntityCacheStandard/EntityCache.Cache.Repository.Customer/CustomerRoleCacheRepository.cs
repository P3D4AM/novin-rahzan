using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Customers;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Customer;
using EntityCache.Mapping.BusinessMappers.Customer;
using EntityCache.Models.Accounting;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Customer;

public class CustomerRoleCacheRepository : SyncCacheRepository<CustomerRoleBusiness>, ICustomerRoleRepository, ISyncRepository<CustomerRoleBusiness>, IEditableDelivery, IRepository<CustomerRoleBusiness>, INoGuidRepository<CustomerRoleBusiness>, IDisposable
{
	private ICustomerRoleRepository _dbRepository;

	public override IItemAsyncMapper<CustomerRoleBusiness, CustomerRoleBusiness> Mapper => CustomerRoleMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, CustomerRoleBusiness> TimingCache { get; set; }

	public CustomerRoleCacheRepository(ICustomerRoleRepository dbRepo, int CachingTime, bool cachingEnable = true)
		: base((ISyncRepository<CustomerRoleBusiness>)dbRepo, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepo;
		TimingCache = new TimingCache<Guid, CustomerRoleBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				_dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Customer\\CustomerRoleCacheRepository.cs", 39);
		}
	}

	public Task<CustomerRoleBusiness> GetAsync(string name, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAsync(name, token, tranName);
	}

	public Task<IEnumerable<CustomerRoleModel>> GetAllModelAsync(bool? status, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllModelAsync(status, token, tranName);
	}
}
