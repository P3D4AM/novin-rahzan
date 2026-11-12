using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class DmlTarget
{
	private readonly SqlTableExpression tableExpression;

	private readonly List<SqlColumnRefExpression> targetColumns;

	public SqlTableExpression TableExpression => tableExpression;

	public List<SqlColumnRefExpression> TargetColumns => targetColumns;

	internal DmlTarget(SqlTableExpression tableExpression, List<SqlColumnRefExpression> targetColumns)
	{
		this.tableExpression = tableExpression;
		this.targetColumns = targetColumns;
	}
}
