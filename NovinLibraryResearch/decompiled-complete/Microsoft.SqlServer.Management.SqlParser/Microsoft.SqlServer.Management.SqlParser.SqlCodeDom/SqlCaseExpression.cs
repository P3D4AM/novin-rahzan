using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlCaseExpression : SqlScalarExpression
{
	private IScalarExpression boundScalar;

	protected SqlScalarExpression elseExpression;

	internal sealed override bool IsValidFilterExpressionConstant => false;

	public override IScalar BoundScalar => boundScalar;

	internal override ScalarExpressionType ExpressionType => ScalarExpressionType.Case;

	internal override bool IsConstantExpression => false;

	public SqlScalarExpression ElseExpression => elseExpression;

	internal sealed override void Bind(IBindingContext bindingContext)
	{
		base.Bind(bindingContext);
		BatchBindingContext batch = bindingContext.Batch;
		IScalarDataType scalarDataType = null;
		foreach (SqlScalarExpression item in RetrieveResultScalarExpressions())
		{
			IScalar scalar = item.BoundScalar;
			if (scalar != null)
			{
				scalarDataType = ((scalarDataType == null) ? scalar.DataType : batch.EvalScalarExpressionDataType(scalarDataType, scalar.DataType));
			}
		}
		if (scalarDataType == null)
		{
			scalarDataType = batch.MetadataFactory.DataType.UnknownScalar;
		}
		boundScalar = batch.MetadataFactory.Scalar.CreateScalarExpression(scalarDataType, nullable: true);
		base.BindingState |= BindingState.BindSucceeded;
	}

	internal override void Unbind()
	{
		boundScalar = null;
		base.Unbind();
	}

	internal abstract IEnumerable<SqlScalarExpression> RetrieveResultScalarExpressions();

	internal SqlCaseExpression(SqlScalarExpression elseExpression)
	{
		this.elseExpression = elseExpression;
	}
}
