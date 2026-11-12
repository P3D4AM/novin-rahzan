using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

[ComVisible(false)]
public class SqlVariableDeclarationCollection : SqlCollection<SqlVariableDeclaration>
{
	internal SqlVariableDeclarationCollection(List<SqlVariableDeclaration> items)
		: base(items)
	{
	}

	internal SqlVariableDeclarationCollection(List<SqlParameterDeclaration> items)
		: base(AsVariables(items))
	{
	}

	private static IEnumerable<SqlVariableDeclaration> AsVariables(List<SqlParameterDeclaration> paramters)
	{
		foreach (SqlParameterDeclaration paramter in paramters)
		{
			yield return paramter;
		}
	}
}
