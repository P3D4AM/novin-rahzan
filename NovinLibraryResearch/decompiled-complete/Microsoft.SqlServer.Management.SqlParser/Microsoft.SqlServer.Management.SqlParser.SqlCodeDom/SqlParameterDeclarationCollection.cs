using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

[ComVisible(false)]
public class SqlParameterDeclarationCollection : SqlCollection<SqlParameterDeclaration>
{
	internal SqlParameterDeclarationCollection()
	{
	}

	internal SqlParameterDeclarationCollection(IEnumerable<SqlParameterDeclaration> items)
		: base(items)
	{
	}
}
