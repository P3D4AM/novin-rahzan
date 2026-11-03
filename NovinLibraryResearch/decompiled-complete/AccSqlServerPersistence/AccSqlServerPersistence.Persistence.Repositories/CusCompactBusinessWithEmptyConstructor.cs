using System;
using EntityCache.Business;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class CusCompactBusinessWithEmptyConstructor : CusCompactBusiness
{
	public CusCompactBusinessWithEmptyConstructor()
		: base(Guid.Empty.ToString())
	{
	}
}
