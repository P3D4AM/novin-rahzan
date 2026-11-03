using System;
using EntityCache.Business.Accounting;

namespace AccSqlServerPersistence.Persistence.Repository.Accounting;

internal class MoeinBusinessWithEmptyConstructor : MoeinBusiness
{
	public MoeinBusinessWithEmptyConstructor()
		: base(Guid.Empty.ToString())
	{
	}
}
