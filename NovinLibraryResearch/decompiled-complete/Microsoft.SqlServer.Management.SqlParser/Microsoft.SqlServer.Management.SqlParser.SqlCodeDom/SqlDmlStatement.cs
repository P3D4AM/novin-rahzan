using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlDmlStatement : SqlStatement
{
	private readonly SqlQueryWithClause queryWithClause;

	public SqlQueryWithClause QueryWithClause => queryWithClause;

	internal abstract SqlDmlSpecification DmlSpecification { get; }

	public sealed override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			if (queryWithClause != null)
			{
				yield return queryWithClause;
			}
			yield return DmlSpecification;
		}
	}

	internal override void BindStatement(BatchBindingContext bindingContext, StatementBindMode bindMode)
	{
		BindStatement(bindingContext);
	}

	private void BindStatement(BatchBindingContext bindingContext)
	{
		IBindingContext bindingContext2 = bindingContext;
		if (queryWithClause != null)
		{
			queryWithClause.Bind(bindingContext2);
			base.BindingState |= queryWithClause.BindingState;
			if (queryWithClause.CTEBoundContext != null)
			{
				bindingContext2 = queryWithClause.CTEBoundContext;
			}
		}
		SqlDmlSpecification dmlSpecification = DmlSpecification;
		dmlSpecification.Bind(bindingContext2);
		base.BindingState |= dmlSpecification.BindingState;
	}

	internal SqlDmlStatement(SqlQueryWithClause queryWithClause)
	{
		this.queryWithClause = queryWithClause;
	}
}
