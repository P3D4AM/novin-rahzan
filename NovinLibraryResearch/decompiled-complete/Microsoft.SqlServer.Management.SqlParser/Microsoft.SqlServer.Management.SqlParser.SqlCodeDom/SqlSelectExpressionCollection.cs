using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

[ComVisible(false)]
public sealed class SqlSelectExpressionCollection : SqlCollection<SqlSelectExpression>
{
	internal SqlSelectExpressionCollection(IEnumerable<SqlSelectExpression> expressions)
		: base(expressions)
	{
	}
}
