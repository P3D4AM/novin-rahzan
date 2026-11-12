using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Stimulsoft.Data.Comparers;

public class StiArrayEqualityComparer : IEqualityComparer<object[]>
{
	public bool Equals(object[] x, object[] y)
	{
		return x.SequenceEqual(y, StiObjectComparer.Default);
	}

	public int GetHashCode(object[] x)
	{
		return ((IStructuralEquatable)x).GetHashCode((IEqualityComparer)EqualityComparer<object>.Default);
	}
}
