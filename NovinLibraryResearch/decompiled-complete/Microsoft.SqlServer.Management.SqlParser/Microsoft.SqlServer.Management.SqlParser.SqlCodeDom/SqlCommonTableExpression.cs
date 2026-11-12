using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlCommonTableExpression : SqlTableExpression
{
	private CteBindingContext bindingContext;

	private readonly SqlIdentifier name;

	private readonly SqlIdentifierCollection columnList;

	private readonly SqlQueryExpression queryExpression;

	internal override bool IsVisitable => true;

	internal override IBindingContext BindingContext => bindingContext;

	internal override TableExpressionName ExpressionName => new TableExpressionName(name.Value);

	public SqlIdentifier Name => name;

	public SqlIdentifierCollection ColumnList => columnList;

	public SqlQueryExpression QueryExpression => queryExpression;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return name;
			if (columnList != null)
			{
				foreach (SqlIdentifier column in columnList)
				{
					yield return column;
				}
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

	internal sealed override void Bind(IBindingContext bindingContext)
	{
		queryExpression.Bind(bindingContext);
		base.BindingState |= queryExpression.BindingState;
	}

	internal void BindCte(CteBindingContext cteBindingContext)
	{
		bindingContext = cteBindingContext;
		IMetadataFactory metadataFactory = cteBindingContext.MetadataFactory;
		ITabularFactory tabular = metadataFactory.Tabular;
		IColumnFactory column = metadataFactory.Column;
		CollationInfo databaseCollation = cteBindingContext.DatabaseCollation;
		ITabular cte = tabular.CreateCommonTableExpression(name.Value, databaseCollation);
		cteBindingContext.EnterCteScope(cte, queryExpression.HasUnionAll);
		queryExpression.ShallowBind(cteBindingContext);
		IMetadataOrderedCollection<IColumn> localColumnExpressions = queryExpression.BindingContext.LocalColumnExpressions;
		IMutableTabular mutableTabular = tabular.CreateCommonTableExpression(name.Value, databaseCollation);
		BindUtils.Table.PopulateVirtualTableColumns(name, columnList, localColumnExpressions, mutableTabular, column, HasAnyBindSkipped);
		cteBindingContext.UpdateCteInScope(mutableTabular);
		queryExpression.DeepBind(cteBindingContext, name);
		base.BindingState |= queryExpression.BindingState;
		cteBindingContext.LeaveCteScope(mutableTabular);
		if (cteBindingContext.AddCommonTableExpression(mutableTabular, GetLexicalScope()))
		{
			SetBoundCte(mutableTabular);
			base.BindingState |= BindingState.BindSucceeded;
		}
		else
		{
			Name.ReportBindError(SR.DuplicateCteName(Name.Value));
			base.BindingState |= BindingState.BindFailed;
		}
	}

	internal override bool MayChildHaveCompletionList(SqlCodeObject child)
	{
		return child == queryExpression;
	}

	internal SqlCommonTableExpression(SqlIdentifier name, List<SqlIdentifier> columnList, SqlQueryExpression queryExpression)
	{
		this.name = name;
		if (columnList != null)
		{
			this.columnList = new SqlIdentifierCollection(columnList);
		}
		this.queryExpression = queryExpression;
	}
}
