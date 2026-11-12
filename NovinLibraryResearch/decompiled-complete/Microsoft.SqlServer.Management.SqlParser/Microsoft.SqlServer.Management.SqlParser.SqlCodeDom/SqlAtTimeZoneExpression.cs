using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlAtTimeZoneExpression : SqlScalarExpression
{
	private IScalarExpression boundScalar;

	private SqlScalarExpression dateValue;

	private SqlScalarExpression timeZone;

	internal override bool IsVisitable => true;

	internal sealed override bool IsValidFilterExpressionConstant => false;

	public override IScalar BoundScalar => boundScalar;

	internal override ScalarExpressionType ExpressionType => ScalarExpressionType.Binary;

	internal override bool IsConstantExpression
	{
		get
		{
			if (dateValue.IsConstantExpression)
			{
				return timeZone.IsConstantExpression;
			}
			return false;
		}
	}

	public SqlScalarExpression DateValue => dateValue;

	public SqlScalarExpression TimeZone => timeZone;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return DateValue;
			yield return TimeZone;
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

	internal sealed override void Bind(IBindingContext bindingContext)
	{
		base.Bind(bindingContext);
		BatchBindingContext batch = bindingContext.Batch;
		boundScalar = batch.MetadataFactory.Scalar.CreateScalarExpression(batch.MetadataFactory.DataType.UnknownScalar, nullable: true);
		base.BindingState = BindingState.BindSucceeded;
	}

	internal override void Unbind()
	{
		boundScalar = null;
		base.Unbind();
	}

	internal SqlAtTimeZoneExpression(SqlScalarExpression dateValue, SqlScalarExpression timeZone)
	{
		this.dateValue = dateValue;
		this.timeZone = timeZone;
	}
}
