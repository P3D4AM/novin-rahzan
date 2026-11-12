using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

[ComVisible(false)]
public sealed class SqlScalarExpressionCollection : SqlCollection<SqlScalarExpression>
{
	internal SqlScalarExpressionCollection()
	{
	}

	internal SqlScalarExpressionCollection(SqlScalarExpression expression)
		: base(expression)
	{
	}

	internal SqlScalarExpressionCollection(IEnumerable<SqlScalarExpression> expressions)
		: base(expressions)
	{
	}
}
