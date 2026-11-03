using System;
using EntityCache.Business;

namespace EntityCache.Persistence.Repository;

public interface ICustomerAddressPersistenceRepository : IDisposable
{
	bool Equals(CustomerAddressBusiness item, string tranName);

	new int GetHashCode();

	new string ToString();
}
