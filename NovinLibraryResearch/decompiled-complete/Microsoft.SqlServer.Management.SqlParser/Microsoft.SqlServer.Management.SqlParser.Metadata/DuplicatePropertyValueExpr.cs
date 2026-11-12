namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public class DuplicatePropertyValueExpr : PropertyValueExpr
{
	private readonly string name;

	public string Name => name;

	public DuplicatePropertyValueExpr(string name)
		: base(PropertValueExprType.Duplicate)
	{
		this.name = name;
	}
}
