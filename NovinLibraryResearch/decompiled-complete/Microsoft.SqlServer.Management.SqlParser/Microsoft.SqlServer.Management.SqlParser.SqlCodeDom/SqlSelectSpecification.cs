using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlSelectSpecification : SqlCodeObject
{
	private readonly SqlQueryExpression queryExpression;

	private readonly SqlOrderByClause orderByClause;

	private readonly SqlForClause forClause;

	internal override bool IsVisitable => true;

	internal override IBindingContext BindingContext => queryExpression.BindingContext;

	internal LocalBindingContext LocalBindingContext => queryExpression.LocalBindingContext;

	public SqlQueryExpression QueryExpression => queryExpression;

	public SqlOrderByClause OrderByClause => orderByClause;

	public SqlForClause ForClause => forClause;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return queryExpression;
			if (orderByClause != null)
			{
				yield return orderByClause;
			}
			if (forClause != null)
			{
				yield return forClause;
			}
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
		queryExpression.Bind(bindingContext);
		base.BindingState = queryExpression.BindingState;
		if (orderByClause != null && base.IsBound)
		{
			IBindingContext bindingContext2 = queryExpression.BindingContext;
			orderByClause.Bind(bindingContext2);
			base.BindingState |= orderByClause.BindingState;
		}
	}

	internal SqlSelectSpecification(SqlQueryExpression queryExpression, SqlOrderByClause orderByClause, SqlForClause forClause)
	{
		this.queryExpression = queryExpression;
		this.orderByClause = orderByClause;
		this.forClause = forClause;
	}
}
