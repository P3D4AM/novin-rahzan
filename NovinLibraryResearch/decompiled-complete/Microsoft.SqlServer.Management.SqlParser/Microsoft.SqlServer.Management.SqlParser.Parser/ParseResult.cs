using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

namespace Microsoft.SqlServer.Management.SqlParser.Parser;

public class ParseResult
{
	private readonly SqlScript sqlScript;

	public int BatchCount => sqlScript.Batches.Count;

	public IEnumerable<Error> Errors => sqlScript.Errors;

	public SqlScript Script => sqlScript;

	internal ParseResult(SqlScript sqlScript)
	{
		this.sqlScript = sqlScript;
	}

	public int GetTokenNumber(int line, int col)
	{
		return sqlScript.GetTokenNumber(line, col);
	}
}
