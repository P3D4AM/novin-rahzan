using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlAssignmentCollection : SqlCollection<SqlAssignment>
{
	internal SqlAssignmentCollection(IEnumerable<SqlAssignment> expressions)
		: base(expressions)
	{
	}
}
