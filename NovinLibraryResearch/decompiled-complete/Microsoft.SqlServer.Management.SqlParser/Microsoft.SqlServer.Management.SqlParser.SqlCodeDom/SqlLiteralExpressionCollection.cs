using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

[ComVisible(false)]
public sealed class SqlLiteralExpressionCollection : SqlCollection<SqlLiteralExpression>
{
	internal SqlLiteralExpressionCollection()
	{
	}

	internal SqlLiteralExpressionCollection(IEnumerable<SqlLiteralExpression> items)
		: base(items)
	{
	}
}
