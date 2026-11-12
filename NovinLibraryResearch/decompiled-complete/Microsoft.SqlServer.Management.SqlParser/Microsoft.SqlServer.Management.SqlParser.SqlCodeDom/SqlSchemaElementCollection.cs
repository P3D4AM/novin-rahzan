using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlSchemaElementCollection : SqlCollection<SqlStatement>
{
	internal SqlSchemaElementCollection(IEnumerable<SqlStatement> schemaElements)
		: base(schemaElements)
	{
	}
}
