using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlPivotTableExpression : SqlTableExpression
{
	private readonly SqlTableExpression source;

	private readonly SqlPivotClause pivotClause;

	private readonly SqlIdentifier alias;

	internal override bool IsVisitable => true;

	internal override TableExpressionName ExpressionName => new TableExpressionName(alias.Value);

	public SqlTableExpression Source => source;

	public SqlPivotClause PivotClause => pivotClause;

	public SqlIdentifier Alias => alias;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return source;
			yield return pivotClause;
			yield return alias;
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
		LocalBindingContext bindingContext2 = bindingContext.CreateLocalBindingContext();
		source.Bind(bindingContext2);
		base.BindingState |= source.BindingState;
		pivotClause.Bind(bindingContext2);
		base.BindingState |= source.BindingState;
		IMetadataFactory metadataFactory = bindingContext.MetadataFactory;
		CollationInfo databaseCollation = bindingContext.DatabaseCollation;
		IMutableTabular mutableTabular = metadataFactory.Tabular.CreatePivotTable(alias.Value, databaseCollation);
		ITabular tabular = source.BoundTable;
		if (tabular != null)
		{
			ColumnOrderedCollection columnOrderedCollection = new ColumnOrderedCollection(pivotClause.AggregatedColumns.Count + 1, databaseCollation);
			foreach (SqlColumnRefExpression aggregatedColumn in pivotClause.AggregatedColumns)
			{
				IColumn boundColumn = aggregatedColumn.BoundColumn;
				if (boundColumn != null && !columnOrderedCollection.Contains(boundColumn.Name))
				{
					columnOrderedCollection.Add(boundColumn);
				}
			}
			IColumn boundColumn2 = pivotClause.PivotedColumn.BoundColumn;
			if (boundColumn2 != null && !columnOrderedCollection.Contains(boundColumn2.Name))
			{
				columnOrderedCollection.Add(boundColumn2);
			}
			foreach (IColumn column in tabular.Columns)
			{
				if (!columnOrderedCollection.Contains(column.Name))
				{
					mutableTabular.Columns.Add(column);
				}
			}
			foreach (SqlColumnRefExpression column2 in pivotClause.ColumnList)
			{
				string value = column2.ColumnName.Value;
				if (column2.ColumnName.IsEmpty)
				{
					continue;
				}
				if (column2.IsPrefixed)
				{
					column2.ReportBindError(SR.PrefixedColumnsNotAllowedInPivot);
					continue;
				}
				bool flag = true;
				if (tabular.Columns.Contains(column2.ColumnName.Value))
				{
					column2.ReportBindError(SR.ColumnNameConflictsInPivot(value));
					flag = false;
				}
				if (mutableTabular.Columns.Contains(value))
				{
					column2.ReportBindError(SR.ColumnSpecifiedMultipleTimes(value, alias.Value));
					flag = false;
				}
				if (flag)
				{
					mutableTabular.Columns.Add(metadataFactory.Column.CreateSimpleColumn(mutableTabular, value, metadataFactory.DataType.UnknownScalar, nullable: true));
				}
			}
		}
		SetBoundTable(bindingContext, mutableTabular, alias);
	}

	internal SqlPivotTableExpression(SqlTableExpression source, SqlPivotClause pivotClause, SqlIdentifier alias)
	{
		this.source = source;
		this.pivotClause = pivotClause;
		this.alias = alias;
	}
}
