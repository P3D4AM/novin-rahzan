using System;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlNullScalarExpression : SqlScalarExpression, INullCodeObject
{
	private IScalarExpression boundScalar;

	internal override bool IsVisitable => true;

	internal sealed override bool IsValidFilterExpressionConstant => false;

	public override IScalar BoundScalar => boundScalar;

	internal override ScalarExpressionType ExpressionType => ScalarExpressionType.Null;

	internal override bool IsConstantExpression => false;

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
		IMetadataFactory metadataFactory = bindingContext.Batch.MetadataFactory;
		boundScalar = metadataFactory.Scalar.CreateScalarExpression(metadataFactory.DataType.UnknownScalar, nullable: true);
		base.BindingState = BindingState.BindSkipped;
	}

	internal override void Unbind()
	{
		boundScalar = null;
		base.Unbind();
	}
}
