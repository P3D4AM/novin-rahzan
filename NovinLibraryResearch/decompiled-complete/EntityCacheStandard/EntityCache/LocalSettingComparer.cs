using System.Collections.Generic;
using EntityCache.Business;

namespace EntityCache;

internal class LocalSettingComparer : IEqualityComparer<LocalSettingBusiness>
{
	public bool Equals(LocalSettingBusiness x, LocalSettingBusiness y)
	{
		return x.Key == y.Key;
	}

	public int GetHashCode(LocalSettingBusiness obj)
	{
		return obj.Key.GetHashCode();
	}
}
