using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Assessment.Data;

internal interface IMultidimensionalMorph
{
	IEnumerable<IDataRow> Transform(IDataView source);
}
