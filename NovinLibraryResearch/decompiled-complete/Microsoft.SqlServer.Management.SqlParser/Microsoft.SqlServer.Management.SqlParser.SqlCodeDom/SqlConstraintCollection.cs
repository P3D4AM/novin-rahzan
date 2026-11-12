using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

[ComVisible(false)]
public class SqlConstraintCollection : SqlCollection<SqlConstraint>
{
	internal SqlConstraintCollection(IEnumerable<SqlConstraint> constraints)
		: base(constraints)
	{
	}
}
