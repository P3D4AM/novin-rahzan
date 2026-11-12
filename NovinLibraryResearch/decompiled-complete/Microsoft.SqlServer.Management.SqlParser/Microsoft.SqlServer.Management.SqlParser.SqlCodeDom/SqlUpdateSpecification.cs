using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlUpdateSpecification : SqlUpdateDeleteSpecificationBase
{
	private class UpdateSpecificationBindingContext : UpdateDeleteSpecificationBindingContext
	{
		protected override bool HasDeletedTable => true;

		protected override bool HasInsertedTable => true;

		public UpdateSpecificationBindingContext(LocalBindingContext baseBindingContext)
			: base(baseBindingContext)
		{
		}
	}

	private readonly SqlSetClause setClause;

	internal override bool IsVisitable => true;

	public SqlSetClause SetClause => setClause;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			if (base.TopSpecification != null)
			{
				yield return base.TopSpecification;
			}
			yield return base.Target;
			yield return setClause;
			if (base.OutputIntoClause != null)
			{
				yield return base.OutputIntoClause;
			}
			if (base.OutputClause != null)
			{
				yield return base.OutputClause;
			}
			if (base.FromClause != null)
			{
				yield return base.FromClause;
			}
			if (base.WhereClause != null)
			{
				yield return base.WhereClause;
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

	internal override DmlSpecificationBindingContext CreateBindingContext(LocalBindingContext localBindingContext)
	{
		return new UpdateSpecificationBindingContext(localBindingContext);
	}

	internal override void Bind(IBindingContext bindingContext)
	{
		base.Bind(bindingContext);
		setClause.Bind(BindingContext);
		base.BindingState |= setClause.BindingState;
	}

	internal SqlUpdateSpecification(SqlTopSpecification topSpecification, DmlTarget dmlTarget, SqlSetClause setClause, List<SqlOutputClause> outputClauses, SqlFromClause fromClause, SqlWhereClause whereClause)
		: base(topSpecification, dmlTarget, outputClauses, fromClause, whereClause)
	{
		this.setClause = setClause;
	}
}
