using System;
using Microsoft.SqlServer.Management.SqlParser.Binder;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlNullQueryExpression : SqlQueryExpression, INullCodeObject
{
	private IBindingContext bindingContext;

	internal override bool IsVisitable => true;

	internal override IBindingContext BindingContext => bindingContext;

	internal override LocalBindingContext LocalBindingContext => bindingContext.AsLocalBindingContext;

	internal override bool HasUnionAll => false;

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
		this.bindingContext = bindingContext;
		base.BindingState = BindingState.BindSkipped;
	}

	internal override void ShallowBind(CteBindingContext bindingContext)
	{
		this.bindingContext = bindingContext;
		base.BindingState = BindingState.BindSkipped;
	}

	internal override void DeepBind(CteBindingContext bindingContext, SqlIdentifier cteName)
	{
	}

	internal override void Unbind()
	{
		base.Unbind();
		bindingContext = null;
	}

	internal override SqlSelectIntoClause GetIntoClause()
	{
		return null;
	}

	internal override void Validate(QueryExpressionValidateOption option)
	{
	}
}
