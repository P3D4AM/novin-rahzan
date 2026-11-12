using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

[ComVisible(false)]
public class SqlTableExpressionCollection : SqlCollection<SqlTableExpression>
{
	internal SqlTableExpressionCollection(List<SqlTableExpression> items)
		: base(items)
	{
	}
}
