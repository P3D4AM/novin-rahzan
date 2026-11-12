using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlMergeActionClauseCollection : SqlCollection<SqlMergeActionClause>
{
	internal SqlMergeActionClauseCollection(IEnumerable<SqlMergeActionClause> actionClauses)
		: base(actionClauses)
	{
	}
}
