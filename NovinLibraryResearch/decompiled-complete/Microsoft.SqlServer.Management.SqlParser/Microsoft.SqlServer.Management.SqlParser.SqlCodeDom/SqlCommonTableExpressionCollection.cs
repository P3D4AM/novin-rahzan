using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

[ComVisible(false)]
public class SqlCommonTableExpressionCollection : SqlCollection<SqlCommonTableExpression>
{
	internal SqlCommonTableExpressionCollection(List<SqlCommonTableExpression> items)
		: base(items)
	{
	}
}
