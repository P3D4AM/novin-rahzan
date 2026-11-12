using System.Collections.Generic;
using System.Linq;

namespace Microsoft.SqlServer.Management.Assessment.Data.AggregateFuncs;

internal class MinimumFunc : IAggregateFunc
{
	public object GetAggregate(IEnumerable<object> sequence)
	{
		return sequence.Select(Utils.Normalize<decimal>).Min();
	}
}
