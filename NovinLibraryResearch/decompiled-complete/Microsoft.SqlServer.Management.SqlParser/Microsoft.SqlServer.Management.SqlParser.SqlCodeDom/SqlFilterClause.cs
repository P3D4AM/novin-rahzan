using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlFilterClause : SqlCodeObject
{
	private LocalBindingContext localBindingContext;

	private readonly SqlFilterExpression filterExpression;

	internal override bool IsVisitable => true;

	internal override IBindingContext BindingContext => localBindingContext;

	public SqlFilterExpression FilterExpression => filterExpression;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return filterExpression;
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
	}

	internal void Bind(BatchBindingContext bindingContext, ITableViewBase boundTable)
	{
		localBindingContext = bindingContext.CreateLocalBindingContext();
		localBindingContext.AddLocalTableExpression(boundTable);
		filterExpression.Bind(localBindingContext);
		base.BindingState |= filterExpression.BindingState;
	}

	internal override void Unbind()
	{
		base.Unbind();
		localBindingContext = null;
	}

	internal SqlFilterClause(SqlFilterExpression filterExpression)
	{
		this.filterExpression = filterExpression;
	}
}
