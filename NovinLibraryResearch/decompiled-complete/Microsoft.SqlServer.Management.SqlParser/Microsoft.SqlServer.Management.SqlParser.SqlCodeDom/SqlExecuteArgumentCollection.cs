using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

[ComVisible(false)]
public class SqlExecuteArgumentCollection : SqlCollection<SqlExecuteArgument>
{
	internal SqlExecuteArgumentCollection(List<SqlExecuteArgument> items)
		: base(items)
	{
	}
}
