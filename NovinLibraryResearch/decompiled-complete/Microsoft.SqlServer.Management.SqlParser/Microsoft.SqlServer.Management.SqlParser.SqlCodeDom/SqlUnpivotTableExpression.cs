using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlUnpivotTableExpression : SqlTableExpression
{
	private readonly SqlTableExpression source;

	private readonly SqlUnpivotClause unpivotClause;

	private readonly SqlIdentifier alias;

	internal override bool IsVisitable => true;

	internal override TableExpressionName ExpressionName => new TableExpressionName(alias.Value);

	private IEnumerable<SqlColumnRefExpression> ValueAndPivotedColumns
	{
		get
		{
			yield return unpivotClause.ValueColumn;
			yield return unpivotClause.PivotedColumn;
		}
	}

	public SqlTableExpression Source => source;

	public SqlUnpivotClause UnpivotClause => unpivotClause;

	public SqlIdentifier Alias => alias;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return source;
			yield return unpivotClause;
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
		unpivotClause.Bind(bindingContext2);
		base.BindingState |= source.BindingState;
		CollationInfo databaseCollation = bindingContext.DatabaseCollation;
		IMutableTabular mutableTabular = bindingContext.MetadataFactory.Tabular.CreateUnpivotTable(alias.Value, databaseCollation);
		ITabular tabular = source.BoundTable;
		if (tabular != null)
		{
			IMetadataFactory metadataFactory = bindingContext.MetadataFactory;
			ColumnCollection columnCollection = new ColumnCollection(unpivotClause.ColumnList.Count, databaseCollation);
			foreach (SqlColumnRefExpression column in unpivotClause.ColumnList)
			{
				IColumn boundColumn = column.BoundColumn;
				if (boundColumn != null)
				{
					if (columnCollection.Contains(boundColumn.Name))
					{
						column.ReportBindError(SR.ColumnSpecifiedMultipleTimesInUnpivot(column.ColumnName.Value));
					}
					else
					{
						columnCollection.Add(boundColumn);
					}
				}
			}
			foreach (IColumn column2 in tabular.Columns)
			{
				if (!columnCollection.Contains(column2))
				{
					mutableTabular.Columns.Add(column2);
				}
			}
			foreach (SqlColumnRefExpression valueAndPivotedColumn in ValueAndPivotedColumns)
			{
				string value = valueAndPivotedColumn.ColumnName.Value;
				if (valueAndPivotedColumn.ColumnName.IsEmpty)
				{
					continue;
				}
				if (valueAndPivotedColumn.IsPrefixed)
				{
					valueAndPivotedColumn.ReportBindError(SR.PrefixedColumnsNotAllowedInUnpivot);
					continue;
				}
				bool flag = true;
				if (tabular.Columns.Contains(valueAndPivotedColumn.ColumnName.Value))
				{
					valueAndPivotedColumn.ReportBindError(SR.ColumnNameConflictsInUnpivot(value));
					flag = false;
				}
				if (mutableTabular.Columns.Contains(value))
				{
					valueAndPivotedColumn.ReportBindError(SR.ColumnSpecifiedMultipleTimes(value, alias.Value));
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

	internal SqlUnpivotTableExpression(SqlTableExpression source, SqlUnpivotClause unpivotClause, SqlIdentifier alias)
	{
		this.source = source;
		this.unpivotClause = unpivotClause;
		this.alias = alias;
	}
}
