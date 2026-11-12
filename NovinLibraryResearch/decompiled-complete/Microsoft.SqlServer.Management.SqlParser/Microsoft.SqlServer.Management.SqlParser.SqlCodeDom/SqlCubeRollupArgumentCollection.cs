using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

[ComVisible(false)]
public class SqlCubeRollupArgumentCollection : SqlCollection<SqlSimpleGroupByItem>
{
	internal SqlCubeRollupArgumentCollection()
	{
	}

	internal SqlCubeRollupArgumentCollection(IEnumerable<SqlSimpleGroupByItem> items)
		: base(items)
	{
	}
}
