using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

[ComVisible(false)]
public sealed class SqlHintCollection : SqlCollection<SqlHint>
{
	internal SqlHintCollection()
	{
	}

	internal SqlHintCollection(IEnumerable<SqlHint> items)
		: base(items)
	{
	}
}
