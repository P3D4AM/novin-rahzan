using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.Management.Assessment.Data.AggregateFuncs;

[StructLayout(LayoutKind.Sequential, Size = 1)]
internal struct ArrayAggregateFunc : IAggregateFunc
{
	public object GetAggregate(IEnumerable<object> sequence)
	{
		return sequence.ToArray();
	}
}
