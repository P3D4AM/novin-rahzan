using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlForeignKeyConstraint : SqlConstraint, ISqlTableElement
{
	private struct ColumnInfo
	{
		public readonly IColumn Column;

		public readonly SqlCodeObject ErrObject;

		public ColumnInfo(IColumn column, SqlCodeObject errObject)
		{
			Column = column;
			ErrObject = errObject;
		}
	}

	private LexicalScope lexicalScope;

	private readonly SqlIdentifierCollection columns;

	private readonly SqlObjectIdentifier referencedTable;

	private readonly SqlIdentifierCollection referencedColumns;

	private readonly SqlForeignKeyAction deleteAction;

	private readonly SqlForeignKeyAction updateAction;

	private readonly bool notForReplication;

	internal override bool IsVisitable => true;

	public SqlIdentifierCollection Columns => columns;

	public SqlObjectIdentifier ReferencedTable => referencedTable;

	public SqlIdentifierCollection ReferencedColumns => referencedColumns;

	public SqlForeignKeyAction DeleteAction => deleteAction;

	public SqlForeignKeyAction UpdateAction => updateAction;

	public bool NotForReplication => notForReplication;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			if (base.Name != null)
			{
				yield return base.Name;
			}
			if (columns != null)
			{
				foreach (SqlIdentifier column in columns)
				{
					yield return column;
				}
			}
			yield return referencedTable;
			if (referencedColumns == null)
			{
				yield break;
			}
			foreach (SqlIdentifier referencedColumn in referencedColumns)
			{
				yield return referencedColumn;
			}
		}
	}

	bool ISqlTableElement.IsConstraint => true;

	SqlConstraint ISqlTableElement.AsConstraint => this;

	bool ISqlTableElement.IsColumnDefinition => false;

	SqlColumnDefinition ISqlTableElement.AsColumnDefinition => null;

	SqlTemporalPeriodDefinition ISqlTableElement.AsTemporalPeriodDefinition => null;

	bool ISqlTableElement.IsTemporalPeriodDefinition => false;

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

	internal override LexicalScope GetLexicalScope()
	{
		return lexicalScope;
	}

	internal override void Bind(BatchBindingContext bindingContext, ConstraintBindingHelper bindingHelper, IMutableDatabaseTable boundTable)
	{
		Bind(bindingContext, bindingHelper, boundTable, null);
	}

	internal override void Bind(BatchBindingContext bindingContext, ConstraintBindingHelper bindingHelper, IMutableDatabaseTable boundTable, IMutableColumn boundColumn)
	{
		lexicalScope = CreateLexicalScope(isModule: false);
		if (boundTable is IMutableTable boundTable2)
		{
			Bind(bindingContext, boundTable2, boundColumn);
		}
		else
		{
			base.BindingState = BindingState.BindFailed;
		}
	}

	private void Bind(BatchBindingContext bindingContext, IMutableTable boundTable, IMutableColumn boundColumn)
	{
		IConstraintFactory constraint = bindingContext.MetadataFactory.Constraint;
		bool hasErrors = false;
		string text;
		bool isSystemNamed;
		if (SqlIdentifier.IsNullOrEmpty(base.Name))
		{
			List<string> list = new List<string>();
			if (columns == null)
			{
				list.Add((boundColumn != null) ? boundColumn.Name : boundTable.Name);
			}
			else
			{
				foreach (SqlIdentifier column in columns)
				{
					list.Add(column.Value);
				}
			}
			text = BindUtils.Constraint.GenerateForeignKeyName(boundTable, list);
			isSystemNamed = true;
		}
		else
		{
			text = base.Name.Value;
			isSystemNamed = false;
		}
		IMutableForeignKeyConstraint mutableForeignKeyConstraint = constraint.CreateForeignKeyConstraint(boundTable, text);
		mutableForeignKeyConstraint.IsSystemNamed = isSystemNamed;
		ColumnInfo[] array = BindReferencingColumns(bindingContext, text, boundTable, boundColumn, ref hasErrors);
		ITable table = BindUtils.Table.AsTable(bindingContext.FindTableOrView(referencedTable.DatabaseName.Value, referencedTable.SchemaName.Value, referencedTable.ObjectName.Value, lexicalScope));
		if (table != null)
		{
			bindingContext.SetBoundObject(referencedTable.ObjectName, table);
			ColumnInfo[] array2 = BindReferencedColumns(bindingContext, text, table, ref hasErrors);
			if (array2.Length != 0)
			{
				if (array2.Length == array.Length)
				{
					CheckColumnTypes(bindingContext, text, boundTable, array, table, array2, ref hasErrors);
				}
				else
				{
					ReportBindError(SR.ForeignKeyNumberOfRefColumnsDiffers(boundTable.Name));
					base.BindingState |= BindingState.BindFailed;
					hasErrors = true;
				}
			}
			if (!hasErrors)
			{
				for (int i = 0; i < array2.Length; i++)
				{
					mutableForeignKeyConstraint.Columns.Add(constraint.CreateForeignKeyColumn(array[i].Column, array2[i].Column));
				}
				mutableForeignKeyConstraint.ReferencedTable = table;
				mutableForeignKeyConstraint.DeleteAction = GetForeignKeyAction(deleteAction);
				mutableForeignKeyConstraint.UpdateAction = GetForeignKeyAction(updateAction);
				mutableForeignKeyConstraint.IsChecked = true;
				mutableForeignKeyConstraint.IsEnabled = true;
				mutableForeignKeyConstraint.NotForReplication = NotForReplication;
				boundTable.Constraints.Add(mutableForeignKeyConstraint);
				base.BindingState |= BindingState.BindSucceeded;
			}
		}
		else
		{
			referencedTable.ReportBindError(SR.ForeignKeyReferencesInvalidTable(text, referencedTable.ToString()), ErrorFlags.UnresolvedDatabaseObjectReference);
			base.BindingState |= BindingState.BindFailed;
		}
	}

	private ColumnInfo[] BindReferencingColumns(BatchBindingContext bindingContext, string constraintName, IMutableTable boundTable, IMutableColumn boundColumn, ref bool hasErrors)
	{
		ColumnInfo[] array;
		if (columns == null)
		{
			array = ((boundColumn == null) ? new ColumnInfo[0] : new ColumnInfo[1]
			{
				new ColumnInfo(boundColumn, this)
			});
		}
		else
		{
			array = new ColumnInfo[columns.Count];
			for (int i = 0; i < columns.Count; i++)
			{
				SqlIdentifier sqlIdentifier = columns[i];
				IColumn column = boundTable.Columns[sqlIdentifier.Value];
				if (column != null)
				{
					bindingContext.SetBoundObject(sqlIdentifier, column);
				}
				else
				{
					sqlIdentifier.ReportBindError(SR.ForeignKeyInvalidReferencingColumn(constraintName, boundTable.Name, sqlIdentifier.Value), ErrorFlags.UnresolvedReference);
					base.BindingState |= BindingState.BindFailed;
					hasErrors = true;
				}
				array[i] = new ColumnInfo(column, sqlIdentifier);
			}
		}
		return array;
	}

	private ColumnInfo[] BindReferencedColumns(BatchBindingContext bindingContext, string constraintName, ITable refBoundTable, ref bool hasErrors)
	{
		ColumnInfo[] array;
		if (referencedColumns != null)
		{
			array = new ColumnInfo[referencedColumns.Count];
			bool flag = false;
			for (int i = 0; i < referencedColumns.Count; i++)
			{
				SqlIdentifier sqlIdentifier = referencedColumns[i];
				IColumn column = refBoundTable.Columns[sqlIdentifier.Value];
				if (column != null)
				{
					bindingContext.SetBoundObject(sqlIdentifier, column);
				}
				else
				{
					sqlIdentifier.ReportBindError(SR.ForeignKeyInvalidReferencedColumn(constraintName, referencedTable.ToString(), sqlIdentifier.Value), ErrorFlags.UnresolvedReference);
					base.BindingState |= BindingState.BindFailed;
					flag = true;
				}
				array[i] = new ColumnInfo(column, sqlIdentifier);
			}
			if (!flag)
			{
				IRelationalIndex relationalIndex = FindReferencedKey(refBoundTable, array);
				if (relationalIndex == null)
				{
					referencedTable.ReportBindError(SR.NoPrimaryKeysInReferencedTable(constraintName, referencedTable.ToString()));
					base.BindingState |= BindingState.BindFailed;
					hasErrors = true;
				}
			}
			else
			{
				IRelationalIndex relationalIndex = null;
				hasErrors = true;
			}
		}
		else
		{
			IRelationalIndex relationalIndex = FindPrimaryKey(refBoundTable);
			if (relationalIndex != null)
			{
				IMetadataOrderedCollection<IIndexedColumn> indexedColumns = relationalIndex.IndexedColumns;
				array = new ColumnInfo[indexedColumns.Count];
				for (int j = 0; j < indexedColumns.Count; j++)
				{
					array[j] = new ColumnInfo(indexedColumns[j].ReferencedColumn, referencedTable);
				}
			}
			else
			{
				array = new ColumnInfo[0];
				referencedTable.ReportBindError(SR.ForeignKeyReferencesImplicitlyTableWithoutPrimaryKey(constraintName, referencedTable.ToString()));
				base.BindingState |= BindingState.BindFailed;
				hasErrors = true;
			}
		}
		return array;
	}

	internal override void Unbind()
	{
		base.Unbind();
		lexicalScope = null;
	}

	private void CheckColumnTypes(BatchBindingContext bindingContext, string constraintName, ITable boundTable, ColumnInfo[] referencingColumns, ITable refBoundTable, ColumnInfo[] referencedColumns, ref bool hasErrors)
	{
		for (int i = 0; i < referencingColumns.Length; i++)
		{
			IColumn column = referencingColumns[i].Column;
			IColumn column2 = referencedColumns[i].Column;
			if (column != null && column2 != null)
			{
				ISystemDataType baseSystemDataType = column.DataType.BaseSystemDataType;
				ISystemDataType baseSystemDataType2 = column2.DataType.BaseSystemDataType;
				if (baseSystemDataType != null && baseSystemDataType2 != null && baseSystemDataType != baseSystemDataType2)
				{
					hasErrors = true;
					referencingColumns[i].ErrObject.ReportBindError(SR.ColumnIsNotSameTypeAsRefColumn(constraintName, boundTable.Name, column.Name, referencedTable.ToString(), column2.Name));
					base.BindingState |= BindingState.BindFailed;
				}
			}
		}
	}

	private static IRelationalIndex FindPrimaryKey(ITable table)
	{
		IRelationalIndex result = null;
		foreach (IRelationalIndex index in table.Indexes)
		{
			if (index.IndexKey != null && index.IndexKey.Type == ConstraintType.PrimaryKey)
			{
				result = index;
				break;
			}
		}
		return result;
	}

	private static IRelationalIndex FindReferencedKey(ITable table, ColumnInfo[] columns)
	{
		IRelationalIndex result = null;
		foreach (IRelationalIndex index in table.Indexes)
		{
			if (!index.IsUnique)
			{
				continue;
			}
			int num = 0;
			foreach (IIndexedColumn indexedColumn2 in index.IndexedColumns)
			{
				if (!indexedColumn2.IsIncluded)
				{
					num++;
				}
			}
			if (num != columns.Length)
			{
				continue;
			}
			bool flag = true;
			for (int i = 0; i < columns.Length; i++)
			{
				ColumnInfo columnInfo = columns[i];
				IIndexedColumn indexedColumn = index.IndexedColumns[columnInfo.Column.Name];
				if (indexedColumn == null || indexedColumn.IsIncluded)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				result = index;
				break;
			}
		}
		return result;
	}

	private static ForeignKeyAction GetForeignKeyAction(SqlForeignKeyAction action)
	{
		switch (action)
		{
		case SqlForeignKeyAction.Cascade:
			return ForeignKeyAction.Cascade;
		case SqlForeignKeyAction.None:
		case SqlForeignKeyAction.NoAction:
			return ForeignKeyAction.NoAction;
		case SqlForeignKeyAction.SetDefault:
			return ForeignKeyAction.SetDefault;
		case SqlForeignKeyAction.SetNull:
			return ForeignKeyAction.SetNull;
		default:
			return ForeignKeyAction.NoAction;
		}
	}

	internal SqlForeignKeyConstraint(List<SqlIdentifier> columnList, SqlObjectIdentifier referencedTableName, List<SqlIdentifier> referencedColumnList, SqlForeignKeyAction deleteUpdateActions, bool notForReplication)
		: base(SqlConstraintType.ForeignKey)
	{
		columns = ((columnList != null) ? new SqlIdentifierCollection(columnList) : null);
		referencedTable = referencedTableName;
		referencedColumns = ((referencedColumnList != null) ? new SqlIdentifierCollection(referencedColumnList) : null);
		deleteAction = deleteUpdateActions & (SqlForeignKeyAction)255;
		updateAction = (SqlForeignKeyAction)((int)deleteUpdateActions >> 8);
		this.notForReplication = notForReplication;
	}
}
