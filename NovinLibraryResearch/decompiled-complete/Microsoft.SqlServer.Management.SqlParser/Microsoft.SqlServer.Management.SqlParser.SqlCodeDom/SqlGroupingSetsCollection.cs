using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

[ComVisible(false)]
public class SqlGroupingSetsCollection : SqlCollection<SqlGroupingSet>
{
	internal SqlGroupingSetsCollection()
	{
	}

	internal SqlGroupingSetsCollection(IEnumerable<SqlGroupingSet> sets)
		: base(sets)
	{
	}
}
