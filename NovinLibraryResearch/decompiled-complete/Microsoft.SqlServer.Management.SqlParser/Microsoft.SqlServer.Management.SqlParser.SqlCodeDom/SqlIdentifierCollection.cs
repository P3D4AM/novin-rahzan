using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

[ComVisible(false)]
public sealed class SqlIdentifierCollection : SqlCollection<SqlIdentifier>
{
	internal SqlIdentifierCollection(IEnumerable<SqlIdentifier> identifiers)
		: base(identifiers)
	{
	}

	internal SqlIdentifierCollection()
	{
	}
}
