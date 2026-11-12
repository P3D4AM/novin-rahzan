namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public class PropertyValueExpr
{
	private readonly PropertValueExprType exprType;

	public PropertValueExprType ExprType => exprType;

	public PropertyValueExpr(PropertValueExprType exprType)
	{
		this.exprType = exprType;
	}
}
