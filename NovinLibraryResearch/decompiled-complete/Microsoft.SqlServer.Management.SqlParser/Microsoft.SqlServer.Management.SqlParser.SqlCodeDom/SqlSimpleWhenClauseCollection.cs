using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

[ComVisible(false)]
public class SqlSimpleWhenClauseCollection : SqlCollection<SqlSimpleWhenClause>
{
	internal SqlSimpleWhenClauseCollection(List<SqlSimpleWhenClause> whenClauses)
		: base(whenClauses)
	{
	}
}
