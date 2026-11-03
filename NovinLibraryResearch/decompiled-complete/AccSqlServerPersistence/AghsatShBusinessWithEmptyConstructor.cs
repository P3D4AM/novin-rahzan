using System;
using EntityCache.Business.Accounting.Aghsat;

internal class AghsatShBusinessWithEmptyConstructor : AghsatShBusiness
{
	public AghsatShBusinessWithEmptyConstructor()
		: base(Guid.Empty.ToString())
	{
	}
}
