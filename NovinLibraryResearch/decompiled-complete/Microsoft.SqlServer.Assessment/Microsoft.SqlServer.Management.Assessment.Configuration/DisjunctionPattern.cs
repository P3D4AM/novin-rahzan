using System.Collections.Generic;
using System.Linq;

namespace Microsoft.SqlServer.Management.Assessment.Configuration;

internal class DisjunctionPattern<T> : CompositePattern<T>
{
	public DisjunctionPattern()
	{
	}

	public DisjunctionPattern(IEnumerable<IPattern<T>> arguments)
		: base(arguments)
	{
	}

	public override bool IsMatch(T value)
	{
		return base.Arguments.Any((IPattern<T> item) => item.IsMatch(value));
	}

	protected override CompositePattern<T> Copy()
	{
		return new DisjunctionPattern<T>();
	}
}
