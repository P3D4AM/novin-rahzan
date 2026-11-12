using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

[ComVisible(false)]
public class SqlBatchCollection : SqlCollection<SqlBatch>
{
	internal SqlBatchCollection(List<SqlBatch> batches)
		: base(batches)
	{
	}
}
