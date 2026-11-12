using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

[ComVisible(false)]
public class SqlGroupByItemCollection : SqlCollection<SqlGroupByItem>
{
	internal SqlGroupByItemCollection()
	{
	}

	internal SqlGroupByItemCollection(IEnumerable<SqlGroupByItem> items)
		: base(items)
	{
	}
}
