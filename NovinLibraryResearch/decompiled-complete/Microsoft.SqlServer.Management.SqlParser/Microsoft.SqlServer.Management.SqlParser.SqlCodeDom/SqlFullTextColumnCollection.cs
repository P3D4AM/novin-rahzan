using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

[ComVisible(false)]
public class SqlFullTextColumnCollection : SqlCollection<SqlFullTextColumn>
{
	internal SqlFullTextColumnCollection()
	{
	}

	internal SqlFullTextColumnCollection(List<SqlFullTextColumn> items)
		: base(items)
	{
	}
}
