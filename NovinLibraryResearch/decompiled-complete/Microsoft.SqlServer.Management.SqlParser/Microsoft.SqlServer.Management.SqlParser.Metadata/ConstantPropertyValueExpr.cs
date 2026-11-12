namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public class ConstantPropertyValueExpr : PropertyValueExpr
{
	private readonly object value;

	public object Value => value;

	public ConstantPropertyValueExpr(object value)
		: base(PropertValueExprType.Constant)
	{
		this.value = value;
	}
}
