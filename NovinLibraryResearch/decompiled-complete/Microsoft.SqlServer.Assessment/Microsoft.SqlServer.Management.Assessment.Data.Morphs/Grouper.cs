using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Assessment.Data.Morphs;

internal abstract class Grouper
{
	public static readonly SingularGrouper Default = new SingularGrouper();

	public abstract IEnumerable<DataRowGroup> Transform(IDataView source);
}
