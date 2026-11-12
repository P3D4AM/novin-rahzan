using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlBinaryScalarExpression : SqlScalarExpression
{
	private IScalarExpression boundScalar;

	private readonly SqlBinaryScalarOperatorType operatorType;

	private readonly SqlScalarExpression left;

	private readonly SqlScalarExpression right;

	internal override bool IsVisitable => true;

	internal sealed override bool IsValidFilterExpressionConstant => false;

	public override IScalar BoundScalar => boundScalar;

	internal override ScalarExpressionType ExpressionType => ScalarExpressionType.Binary;

	internal override bool IsConstantExpression
	{
		get
		{
			if (left.IsConstantExpression)
			{
				return right.IsConstantExpression;
			}
			return false;
		}
	}

	public SqlBinaryScalarOperatorType Operator => operatorType;

	public SqlScalarExpression Left => left;

	public SqlScalarExpression Right => right;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return left;
			yield return right;
		}
	}

	public override void Accept(ISqlCodeObjectVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override T Accept<T>(ISqlCodeObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}

	public override void Accept<C>(ISqlCodeObjectContextVisitor<C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this, context);
	}

	public override T Accept<T, C>(ISqlCodeObjectContextVisitor<T, C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this, context);
	}

	internal override void Bind(IBindingContext bindingContext)
	{
		base.Bind(bindingContext);
		BatchBindingContext batch = bindingContext.Batch;
		IScalar scalar = left.BoundScalar;
		IScalar scalar2 = right.BoundScalar;
		IScalarDataType dataType = ((scalar != null) ? ((scalar2 == null) ? scalar.DataType : bindingContext.Batch.EvalScalarExpressionDataType(scalar.DataType, scalar2.DataType)) : ((scalar2 == null) ? batch.MetadataFactory.DataType.UnknownScalar : scalar2.DataType));
		boundScalar = batch.MetadataFactory.Scalar.CreateScalarExpression(dataType, nullable: true);
		base.BindingState = BindingState.BindSucceeded;
	}

	internal override void Unbind()
	{
		boundScalar = null;
		base.Unbind();
	}

	internal SqlBinaryScalarExpression(SqlScalarExpression left, SqlBinaryScalarOperatorType type, SqlScalarExpression right)
	{
		this.left = left;
		operatorType = type;
		this.right = right;
	}
}
