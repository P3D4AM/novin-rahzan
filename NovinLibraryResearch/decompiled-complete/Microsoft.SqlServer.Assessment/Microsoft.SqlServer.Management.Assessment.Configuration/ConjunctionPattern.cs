using System.Collections.Generic;
using System.Linq;

namespace Microsoft.SqlServer.Management.Assessment.Configuration;

internal class ConjunctionPattern<T> : CompositePattern<T>
{
	public ConjunctionPattern()
	{
	}

	public ConjunctionPattern(IEnumerable<IPattern<T>> arguments)
		: base(arguments)
	{
	}

	public override bool IsMatch(T value)
	{
		return base.Arguments.All((IPattern<T> item) => item.IsMatch(value));
	}

	protected override CompositePattern<T> Copy()
	{
		return new ConjunctionPattern<T>();
	}
}
