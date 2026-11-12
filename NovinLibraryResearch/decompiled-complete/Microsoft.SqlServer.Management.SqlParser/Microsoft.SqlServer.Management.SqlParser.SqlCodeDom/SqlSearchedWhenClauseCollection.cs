using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

[ComVisible(false)]
public class SqlSearchedWhenClauseCollection : SqlCollection<SqlSearchedWhenClause>
{
	internal SqlSearchedWhenClauseCollection(List<SqlSearchedWhenClause> whenClauses)
		: base(whenClauses)
	{
	}
}
