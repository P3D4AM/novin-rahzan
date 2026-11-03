using System;
using EntityCache.Business.Accounting;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class KolBusinessWithEmptyConstructor : KolBusiness
{
	public KolBusinessWithEmptyConstructor()
		: base(Guid.Empty.ToString())
	{
	}
}
