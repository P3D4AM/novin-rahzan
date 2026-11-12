using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlDeleteSpecification : SqlUpdateDeleteSpecificationBase
{
	private class DeleteSpecificationBindingContext : UpdateDeleteSpecificationBindingContext
	{
		protected override bool HasDeletedTable => true;

		protected override bool HasInsertedTable => false;

		public DeleteSpecificationBindingContext(LocalBindingContext baseBindingContext)
			: base(baseBindingContext)
		{
		}
	}

	internal override bool IsVisitable => true;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			if (base.TopSpecification != null)
			{
				yield return base.TopSpecification;
			}
			yield return base.Target;
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
		return new DeleteSpecificationBindingContext(localBindingContext);
	}

	internal SqlDeleteSpecification(SqlTopSpecification topSpecification, DmlTarget dmlTarget, List<SqlOutputClause> outputClauses, SqlFromClause fromClause, SqlWhereClause whereClause)
		: base(topSpecification, dmlTarget, outputClauses, fromClause, whereClause)
	{
	}
}
