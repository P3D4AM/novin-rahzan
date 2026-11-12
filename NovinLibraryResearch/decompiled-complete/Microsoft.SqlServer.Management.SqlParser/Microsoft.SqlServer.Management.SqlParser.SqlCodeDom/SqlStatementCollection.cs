using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

[ComVisible(false)]
public class SqlStatementCollection : SqlCollection<SqlStatement>
{
	internal SqlStatementCollection()
	{
	}

	internal SqlStatementCollection(IEnumerable<SqlStatement> items)
		: base(items)
	{
	}
}
