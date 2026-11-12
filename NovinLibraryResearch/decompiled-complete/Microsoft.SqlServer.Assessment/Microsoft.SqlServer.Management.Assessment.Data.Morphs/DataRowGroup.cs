using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Assessment.Data.Morphs;

internal struct DataRowGroup
{
	public Measurement Result;

	public IEnumerable<IDataRow> Data;

	public DataRowGroup(Measurement result, IEnumerable<IDataRow> data)
	{
		Result = result;
		Data = data;
	}
}
