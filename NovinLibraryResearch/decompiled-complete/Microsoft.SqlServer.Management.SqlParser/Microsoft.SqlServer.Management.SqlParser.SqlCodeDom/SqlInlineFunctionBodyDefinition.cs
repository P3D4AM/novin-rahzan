using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlInlineFunctionBodyDefinition : SqlFunctionBodyDefinition
{
	private readonly SqlQueryWithClause queryWithClause;

	private readonly SqlQueryExpression queryExpression;

	internal override bool IsVisitable => true;

	internal sealed override SqlClrFunctionBodyDefinition AsSqlClrFunctionBodyDefinition => null;

	internal sealed override SqlInlineFunctionBodyDefinition AsSqlInlineFunctionBodyDefinition => this;

	internal sealed override SqlMultistatementFunctionBodyDefinition AsSqlMultistatementFunctionBodyDefinition => null;

	public SqlQueryWithClause QueryWithClause => queryWithClause;

	public SqlQueryExpression QueryExpression => queryExpression;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			if (queryWithClause != null)
			{
				yield return queryWithClause;
			}
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

	internal override void Bind(BatchBindingContext bindingContext, FunctionUnion<IMutableScalarValuedFunction, IMutableTableValuedFunction, IMutableUserDefinedFunction> functionUnion)
	{
		BatchBindingContext bindingContext2 = bindingContext;
		if (queryWithClause != null)
		{
			queryWithClause.Bind(bindingContext2);
			base.BindingState |= queryWithClause.BindingState;
			if (queryWithClause.CTEBoundContext != null)
			{
				bindingContext2 = queryWithClause.CTEBoundContext;
			}
		}
		queryExpression.Bind(bindingContext2);
		base.BindingState = queryExpression.BindingState;
	}

	internal SqlInlineFunctionBodyDefinition(SqlQueryWithClause queryWithClause, SqlQueryExpression queryExpression)
	{
		this.queryWithClause = queryWithClause;
		this.queryExpression = queryExpression;
	}
}
