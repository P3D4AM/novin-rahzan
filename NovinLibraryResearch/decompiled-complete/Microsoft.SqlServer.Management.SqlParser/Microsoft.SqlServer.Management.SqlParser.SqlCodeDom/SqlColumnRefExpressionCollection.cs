using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlColumnRefExpressionCollection : SqlCollection<SqlColumnRefExpression>
{
	internal SqlColumnRefExpressionCollection(List<SqlColumnRefExpression> items)
		: base(items)
	{
	}
}
