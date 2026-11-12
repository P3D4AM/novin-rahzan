using System;

namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public class IsTypePropertyValueExpr : PropertyValueExpr
{
	private readonly Type type;

	public Type Type => type;

	public IsTypePropertyValueExpr(Type type)
		: base(PropertValueExprType.IsType)
	{
		this.type = type;
	}
}
