using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

[ComVisible(false)]
public class SqlObjectIdentifierCollection : SqlCollection<SqlObjectIdentifier>
{
	internal SqlObjectIdentifierCollection(IEnumerable<SqlObjectIdentifier> objects)
		: base(objects)
	{
	}

	internal SqlObjectIdentifierCollection()
	{
	}
}
