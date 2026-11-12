using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

[ComVisible(false)]
public class SqlColumnDefinitionCollection : SqlCollection<SqlColumnDefinition>
{
	internal SqlColumnDefinitionCollection(IEnumerable<SqlColumnDefinition> definitions)
		: base(definitions)
	{
	}
}
