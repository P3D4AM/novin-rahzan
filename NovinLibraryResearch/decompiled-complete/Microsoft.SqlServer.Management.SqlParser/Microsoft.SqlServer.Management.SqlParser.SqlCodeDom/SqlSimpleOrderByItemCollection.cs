using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

[ComVisible(false)]
public class SqlSimpleOrderByItemCollection : SqlCollection<SqlSimpleOrderByItem>
{
	internal SqlSimpleOrderByItemCollection()
	{
	}

	internal SqlSimpleOrderByItemCollection(List<SqlSimpleOrderByItem> items)
		: base(items)
	{
	}
}
