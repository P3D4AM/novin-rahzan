using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlScalarSubQueryExpression : SqlScalarExpression
{
	private IScalar boundScalar;

	private SqlQueryExpression queryExpression;

	internal override bool IsVisitable => true;

	internal sealed override bool IsValidFilterExpressionConstant => false;

	public override IScalar BoundScalar => boundScalar;

	internal override ScalarExpressionType ExpressionType => ScalarExpressionType.SubQuery;

	internal override bool IsConstantExpression => false;

	public SqlQueryExpression QueryExpression => queryExpression;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return queryExpression;
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
	}

	internal override void Unbind()
	{
		boundScalar = null;
		base.Unbind();
	}

	internal SqlScalarSubQueryExpression(SqlQueryExpression expression)
	{
		queryExpression = expression;
	}
}
