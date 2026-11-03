using System;
using EntityCache.Business;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class ProductCompactBusinessWithEmptyConstructor : ProductCompactBusiness
{
	public ProductCompactBusinessWithEmptyConstructor()
		: base(Guid.Empty.ToString())
	{
	}
}
