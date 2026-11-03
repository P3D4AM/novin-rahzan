using System;
using EntityCache.Business;

namespace EntityCache.Core.Repository;

public interface ICustomerAddressRepository : IRepository<CustomerAddressBusiness>, INoGuidRepository<CustomerAddressBusiness>, IDisposable
{
}
