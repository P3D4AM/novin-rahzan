using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

[ComVisible(false)]
public class SqlGroupingSetItemCollection : SqlCollection<SqlGroupingSetItem>
{
	internal SqlGroupingSetItemCollection()
	{
	}

	internal SqlGroupingSetItemCollection(IEnumerable<SqlGroupingSetItem> items)
		: base(items)
	{
	}

	internal SqlGroupingSetItemCollection(SqlGroupingSetItem item)
		: base(item)
	{
	}
}
