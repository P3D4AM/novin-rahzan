using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlIndexOptionCollection : SqlCollection<SqlIndexOption>
{
	internal SqlIndexOptionCollection(IEnumerable<SqlIndexOption> indexOptions)
		: base(indexOptions)
	{
	}
}
