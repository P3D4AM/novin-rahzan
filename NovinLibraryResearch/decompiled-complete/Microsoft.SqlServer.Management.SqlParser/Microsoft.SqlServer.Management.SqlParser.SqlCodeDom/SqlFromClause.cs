using System;
using System.Collections.Generic;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlFromClause : SqlCodeObject
{
	private class FromClauseBindingContext : LocalBindingContext
	{
		public override IMetadataOrderedCollection<IColumn> LocalColumnExpressions => Collection<IColumn>.EmptyOrdered;

		public override IMetadataCollection<ITabular> LocalTableExpressions => Collection<ITabular>.Empty;

		public FromClauseBindingContext(LocalBindingContext baseBindingContext)
			: base(baseBindingContext)
		{
		}
	}

	private FromClauseBindingContext bindingContext;

	private readonly SqlTableExpressionCollection tableExpressions;

	internal override bool IsVisitable => true;

	internal override IBindingContext BindingContext => bindingContext;

	public SqlTableExpressionCollection TableExpressions => tableExpressions;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			foreach (SqlTableExpression tableExpression in tableExpressions)
			{
				yield return tableExpression;
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
		base.Bind(this.bindingContext = new FromClauseBindingContext(bindingContext.AsLocalBindingContext));
	}

	internal override void Unbind()
	{
		bindingContext = null;
		base.Unbind();
	}

	internal override SqlTableExpression FindSiblingTableExpressionBoundTo(SqlTableExpression tableExpression, ITabular boundTable)
	{
		SqlTableExpression sqlTableExpression = null;
		for (int i = 0; i < tableExpressions.Count; i++)
		{
			SqlTableExpression sqlTableExpression2 = tableExpressions[i];
			if (sqlTableExpression2 == tableExpression)
			{
				break;
			}
			sqlTableExpression = sqlTableExpression2.FindTableExpressionBoundTo(boundTable);
			if (sqlTableExpression != null)
			{
				break;
			}
		}
		return sqlTableExpression;
	}

	internal override IMetadataCollection<IMetadataObject> GetCompletionList(LexLocation caretLocation, CompletionContext completionContext)
	{
		return base.GetCompletionList(caretLocation, CompletionContext.SelectTabular);
	}

	internal SqlFromClause(List<SqlTableExpression> tableExpressions)
	{
		this.tableExpressions = new SqlTableExpressionCollection(tableExpressions);
	}
}
