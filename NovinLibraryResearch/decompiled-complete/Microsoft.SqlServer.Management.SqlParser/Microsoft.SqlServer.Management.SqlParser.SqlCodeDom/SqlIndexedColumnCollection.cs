using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

[ComVisible(false)]
public class SqlIndexedColumnCollection : SqlCollection<SqlIndexedColumn>
{
	internal SqlIndexedColumnCollection(IEnumerable<SqlIndexedColumn> items)
		: base(items)
	{
	}
}
