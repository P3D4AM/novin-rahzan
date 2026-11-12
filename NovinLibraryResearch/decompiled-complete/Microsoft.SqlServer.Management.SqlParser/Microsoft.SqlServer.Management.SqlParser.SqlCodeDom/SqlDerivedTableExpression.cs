using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlDerivedTableExpression : SqlTableExpression
{
	private SqlQueryExpression queryExpression;

	private SqlIdentifier alias;

	private SqlIdentifierCollection columnList;

	internal override bool IsVisitable => true;

	internal override TableExpressionName ExpressionName => new TableExpressionName(alias.Value);

	public SqlQueryExpression QueryExpression => queryExpression;

	public SqlIdentifier Alias => alias;

	public SqlIdentifierCollection ColumnList => columnList;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return queryExpression;
			yield return alias;
			if (ColumnList == null)
			{
				yield break;
			}
			foreach (SqlIdentifier column in ColumnList)
			{
				yield return column;
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
		base.BindingState |= queryExpression.BindingState;
		if (!SqlIdentifier.IsNullOrEmpty(alias))
		{
			IMetadataFactory metadataFactory = bindingContext.MetadataFactory;
			CollationInfo databaseCollation = bindingContext.DatabaseCollation;
			IMetadataOrderedCollection<IColumn> localColumnExpressions = queryExpression.BindingContext.LocalColumnExpressions;
			IMutableTabular table = metadataFactory.Tabular.CreateDerivedTable(alias.Value ?? string.Empty, databaseCollation);
			BindUtils.Table.PopulateVirtualTableColumns(alias, columnList, localColumnExpressions, table, metadataFactory.Column, HasAnyBindSkipped);
			SetBoundTable(bindingContext, table, alias);
			base.BindingState |= BindingState.BindSucceeded;
		}
		else
		{
			base.BindingState |= BindingState.BindFailed;
		}
	}

	internal override bool MayChildHaveCompletionList(SqlCodeObject child)
	{
		return child == queryExpression;
	}

	internal SqlDerivedTableExpression(SqlQueryExpression queryExpression, SqlIdentifier alias, List<SqlIdentifier> columnList)
	{
		this.queryExpression = queryExpression;
		this.alias = alias;
		if (columnList != null)
		{
			this.columnList = new SqlIdentifierCollection(columnList);
		}
	}
}
