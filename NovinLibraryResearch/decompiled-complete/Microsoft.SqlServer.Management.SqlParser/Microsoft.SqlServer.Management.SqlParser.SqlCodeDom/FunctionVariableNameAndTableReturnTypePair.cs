namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class FunctionVariableNameAndTableReturnTypePair
{
	private readonly SqlLiteralExpression variable;

	private readonly SqlTableFunctionReturnType returnType;

	public SqlLiteralExpression Variable => variable;

	public SqlTableFunctionReturnType ReturnType => returnType;

	public FunctionVariableNameAndTableReturnTypePair(SqlLiteralExpression variable, SqlTableFunctionReturnType returnType)
	{
		this.variable = variable;
		this.returnType = returnType;
	}
}
