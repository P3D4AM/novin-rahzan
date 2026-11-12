using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Assessment.Data;

internal interface IAggregateFunc
{
	object GetAggregate(IEnumerable<object> sequence);
}
