using System;
using System.Collections.Generic;
using System.Linq;
using Ninject.Components;

namespace Ninject.Planning.Bindings;

public class BindingPrecedenceComparer : NinjectComponent, IBindingPrecedenceComparer, INinjectComponent, IDisposable, IComparer<IBinding>
{
	public int Compare(IBinding x, IBinding y)
	{
		if (x == y)
		{
			return 0;
		}
		return (from func in new List<Func<IBinding, bool>>
			{
				(IBinding b) => b != null,
				(IBinding b) => b.IsConditional,
				(IBinding b) => !b.Service.ContainsGenericParameters,
				(IBinding b) => !b.IsImplicit
			}
			let xVal = func(x)
			where xVal != func(y)
			select xVal ? 1 : (-1)).FirstOrDefault();
	}
}
