namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlProcedureDefinitionForCreate : SqlProcedureDefinition
{
	private readonly bool isOrAlterStatement;

	public bool IsOrAlterStatement => isOrAlterStatement;

	internal SqlProcedureDefinitionForCreate(SqlProcedureDefinition sqlProcedureDefinition, bool isOrAlterStatement)
		: base(sqlProcedureDefinition)
	{
		this.isOrAlterStatement = isOrAlterStatement;
	}
}
