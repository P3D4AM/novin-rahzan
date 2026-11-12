using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

[ComVisible(false)]
public class SqlGroupingSetCollection : SqlCollection<SqlGroupingSet>
{
	internal SqlGroupingSetCollection()
	{
	}

	internal SqlGroupingSetCollection(IEnumerable<SqlGroupingSet> items)
		: base(items)
	{
	}
}
