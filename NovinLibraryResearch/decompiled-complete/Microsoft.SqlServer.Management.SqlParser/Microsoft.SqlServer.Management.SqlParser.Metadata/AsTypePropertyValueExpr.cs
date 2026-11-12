using System;

namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public class AsTypePropertyValueExpr : PropertyValueExpr
{
	private readonly Type type;

	public Type Type => type;

	public AsTypePropertyValueExpr(Type type)
		: base(PropertValueExprType.AsType)
	{
		this.type = type;
	}
}
