using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Ninject.Infrastructure;

namespace Ninject.Activation.Caching;

public class WeakReferenceEqualityComparer : IEqualityComparer<object>
{
	public new bool Equals(object x, object y)
	{
		return x.Equals(y);
	}

	public int GetHashCode(object obj)
	{
		if (!(obj is ReferenceEqualWeakReference referenceEqualWeakReference))
		{
			return RuntimeHelpers.GetHashCode(obj);
		}
		return referenceEqualWeakReference.GetHashCode();
	}
}
