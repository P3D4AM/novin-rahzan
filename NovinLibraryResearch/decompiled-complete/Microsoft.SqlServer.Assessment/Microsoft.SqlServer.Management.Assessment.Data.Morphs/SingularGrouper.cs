using System.Collections.Generic;
using System.Linq;

namespace Microsoft.SqlServer.Management.Assessment.Data.Morphs;

internal class SingularGrouper : Grouper
{
	public override IEnumerable<DataRowGroup> Transform(IDataView source)
	{
		if (source.Count == 0)
		{
			return Enumerable.Empty<DataRowGroup>();
		}
		return new DataRowGroup[1]
		{
			new DataRowGroup(new Measurement(), source)
		};
	}
}
