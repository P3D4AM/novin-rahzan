using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Customers;
using EntityCache.Models.Accounting;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Core.Repository.Customer;

public interface ICustomerRoleRepository : ISyncRepository<CustomerRoleBusiness>, IEditableDelivery, IRepository<CustomerRoleBusiness>, INoGuidRepository<CustomerRoleBusiness>, IDisposable
{
	Task<CustomerRoleBusiness> GetAsync(string name, CancellationToken token, string tranName);

	Task<IEnumerable<CustomerRoleModel>> GetAllModelAsync(bool? status, CancellationToken token, string tranName);
}
