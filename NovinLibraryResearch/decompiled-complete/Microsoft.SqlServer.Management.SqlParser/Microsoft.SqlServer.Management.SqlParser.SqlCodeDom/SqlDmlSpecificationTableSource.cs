using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlDmlSpecificationTableSource : SqlTableExpression
{
	private readonly SqlDmlSpecification dmlSpecification;

	private readonly SqlIdentifier alias;

	private readonly SqlIdentifierCollection columnList;

	internal override bool IsVisitable => true;

	internal override TableExpressionName ExpressionName => new TableExpressionName(alias.Value);

	public SqlDmlSpecification DmlSpecification => dmlSpecification;

	public SqlIdentifier Alias => alias;

	public SqlIdentifierCollection ColumnList => columnList;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return dmlSpecification;
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
		dmlSpecification.Bind(bindingContext);
		SqlOutputClause outputClause = dmlSpecification.OutputClause;
		IMetadataOrderedCollection<IColumn> queryColumns;
		if (outputClause != null)
		{
			queryColumns = outputClause.BindingContext.LocalColumnExpressions;
		}
		else
		{
			dmlSpecification.ReportBindError(SR.NestedDmlMustHaveOutputClause);
			queryColumns = Collection<IColumn>.EmptyOrdered;
		}
		IMetadataFactory metadataFactory = bindingContext.MetadataFactory;
		IColumnFactory column = metadataFactory.Column;
		ITabularFactory tabular = metadataFactory.Tabular;
		CollationInfo databaseCollation = bindingContext.DatabaseCollation;
		if (!SqlIdentifier.IsNullOrEmpty(alias))
		{
			IMutableTabular table = tabular.CreateDmlTableSource(alias.Value, databaseCollation);
			BindUtils.Table.PopulateVirtualTableColumns(alias, columnList, queryColumns, table, column, HasAnyBindSkipped);
			SetBoundTable(bindingContext, table, alias);
			base.BindingState |= BindingState.BindSucceeded;
		}
		else
		{
			base.BindingState |= BindingState.BindFailed;
		}
	}

	internal SqlDmlSpecificationTableSource(SqlDmlSpecification dmlSpecification, SqlIdentifier alias, List<SqlIdentifier> columnList)
	{
		this.dmlSpecification = dmlSpecification;
		this.alias = alias;
		if (columnList != null)
		{
			this.columnList = new SqlIdentifierCollection(columnList);
		}
	}
}
