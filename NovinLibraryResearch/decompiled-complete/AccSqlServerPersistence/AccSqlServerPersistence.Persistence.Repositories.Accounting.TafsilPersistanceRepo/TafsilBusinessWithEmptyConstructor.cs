using System;
using EntityCache.Business.Accounting;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting.TafsilPersistanceRepo;

internal class TafsilBusinessWithEmptyConstructor : TafsilBusiness
{
	public TafsilBusinessWithEmptyConstructor()
		: base(Guid.Empty.ToString())
	{
	}
}
