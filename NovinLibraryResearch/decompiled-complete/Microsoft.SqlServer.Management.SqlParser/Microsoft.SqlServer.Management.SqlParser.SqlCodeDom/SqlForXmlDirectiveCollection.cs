using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

[ComVisible(false)]
public sealed class SqlForXmlDirectiveCollection : SqlCollection<SqlForXmlDirective>
{
	internal SqlForXmlDirectiveCollection(IEnumerable<SqlForXmlDirective> directives)
		: base(directives)
	{
	}

	internal SqlForXmlDirectiveCollection()
	{
	}
}
