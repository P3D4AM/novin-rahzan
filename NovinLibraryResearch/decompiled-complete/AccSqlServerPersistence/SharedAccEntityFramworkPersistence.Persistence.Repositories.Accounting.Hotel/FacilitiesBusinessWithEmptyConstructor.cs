using System;
using EntityCache.Business.Accounting.Bill;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.Hotel;

internal class FacilitiesBusinessWithEmptyConstructor : FacilitiesBusiness
{
	public FacilitiesBusinessWithEmptyConstructor()
		: base(Guid.Empty.ToString())
	{
	}
}
