using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

[ComVisible(false)]
public class SqlRowConstructorExpressionCollection : SqlCollection<SqlRowConstructorExpression>
{
	internal SqlRowConstructorExpressionCollection(IEnumerable<SqlRowConstructorExpression> expressions)
		: base(expressions)
	{
	}
}
