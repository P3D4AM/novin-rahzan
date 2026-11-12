using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlIndexedColumn : SqlCodeObject
{
	private readonly SqlIdentifier name;

	private readonly SqlSortOrder sortOrder;

	internal override bool IsVisitable => true;

	public IColumn BoundColumn => GetBoundObject<IColumn>(name);

	public override IMetadataObject BoundObject => BoundColumn;

	public SqlIdentifier Name => name;

	public SqlSortOrder SortOrder => sortOrder;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return name;
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
	}

	internal void Bind(BatchBindingContext bindingContext, IDatabaseTable boundTable, IMutableRelationalIndex boundIndex, SqlConstraintType constraintType)
	{
		string value = name.Value;
		IColumn column = boundTable.Columns[value];
		if (column != null)
		{
			bindingContext.SetBoundObject(name, column);
			if (boundIndex.IndexedColumns.Contains(value))
			{
				name.ReportBindError(SR.DuplicateColumnNamesInIndex(value));
				base.BindingState |= BindingState.BindFailed;
				return;
			}
			if (column.IsSparse && (constraintType == SqlConstraintType.Unique || constraintType == SqlConstraintType.PrimaryKey))
			{
				ReportBindError(SR.ColumnIsInvalidForUseAsKeyColumnInIndex(value, boundTable.Name));
				base.BindingState |= BindingState.BindFailed;
			}
			IMutableIndexedColumn mutableIndexedColumn = bindingContext.MetadataFactory.Index.CreateIndexedColumn(column);
			mutableIndexedColumn.SortOrder = ((sortOrder == SqlSortOrder.Descending) ? Microsoft.SqlServer.Management.SqlParser.Metadata.SortOrder.Descending : Microsoft.SqlServer.Management.SqlParser.Metadata.SortOrder.Ascending);
			boundIndex.IndexedColumns.Add(mutableIndexedColumn);
			base.BindingState |= BindingState.BindSucceeded;
		}
		else
		{
			name.ReportBindError(SR.ColumnNameNotInTargetTable(value), ErrorFlags.UnresolvedReference);
			base.BindingState |= BindingState.BindFailed;
		}
	}

	internal SqlIndexedColumn(SqlIdentifier name, SqlSortOrder sortOrder)
	{
		this.name = name;
		this.sortOrder = sortOrder;
	}
}
