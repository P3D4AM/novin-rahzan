using System;
using System.Linq;
using System.Linq.Expressions;

namespace Microsoft.EntityFrameworkCore.ChangeTracking;

public class ArrayStructuralComparer<TElement> : ValueComparer<TElement[]>
{
	public ArrayStructuralComparer()
		: base(ValueComparer<TElement[]>.CreateDefaultEqualsExpression(), ValueComparer<TElement[]>.CreateDefaultHashCodeExpression(favorStructuralComparisons: true), (Expression<Func<TElement[], TElement[]>>)((TElement[] v) => (v == null) ? null : v.ToArray()))
	{
	}
}
