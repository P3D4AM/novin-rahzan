using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlUdtStaticMemberExpression : SqlUdtMemberExpression
{
	private readonly SqlDataTypeSpecification type;

	public SqlDataTypeSpecification Type => type;

	internal IClrDataType BindType(IBindingContext bindingContext)
	{
		IClrDataType clrDataType = null;
		type.Bind(bindingContext);
		base.BindingState |= type.BindingState;
		IDataType boundType = type.BoundType;
		if (boundType != null)
		{
			IScalarDataType scalarDataType = BindUtils.DataType.AsScalarDataType(boundType);
			clrDataType = ((scalarDataType != null) ? BindUtils.ScalarDataType.AsClrDataType(scalarDataType) : null);
			if (clrDataType == null)
			{
				type.ReportBindError(SR.CannotCallMethodsOnType(Type.DataType.ObjectIdentifier.ObjectName.Value), ErrorFlags.UnresolvedReference);
				base.BindingState |= BindingState.BindFailed;
			}
		}
		return clrDataType;
	}

	internal SqlUdtStaticMemberExpression(SqlDataTypeSpecification type, SqlIdentifier memberName)
		: base(memberName)
	{
		this.type = type;
	}
}
