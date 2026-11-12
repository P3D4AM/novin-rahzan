using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

[ComVisible(false)]
public class SqlOrderByItemCollection : SqlCollection<SqlOrderByItem>
{
	internal SqlOrderByItemCollection()
	{
	}

	internal SqlOrderByItemCollection(List<SqlOrderByItem> items)
		: base(items)
	{
	}
}
