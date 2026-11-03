using System;
using EntityCache.Business.Accounting.Aghsat;

internal class AghsatMBusinessWithEmptyConstructor : AghsatMBusiness
{
	public AghsatMBusinessWithEmptyConstructor()
		: base(Guid.Empty.ToString())
	{
	}
}
