using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Data.SqlClient;

public sealed class SqlBulkCopyColumnOrderHintCollection : CollectionBase
{
	private readonly HashSet<string> _columnNames = new HashSet<string>();

	public SqlBulkCopyColumnOrderHint this[int index] => (SqlBulkCopyColumnOrderHint)base.List[index];

	public SqlBulkCopyColumnOrderHint Add(SqlBulkCopyColumnOrderHint columnOrderHint)
	{
		if (columnOrderHint == null)
		{
			throw new ArgumentNullException("columnOrderHint");
		}
		if (string.IsNullOrEmpty(columnOrderHint.Column) || columnOrderHint.SortOrder == SortOrder.Unspecified)
		{
			throw SQL.BulkLoadInvalidOrderHint();
		}
		RegisterColumnName(columnOrderHint, columnOrderHint.Column);
		base.InnerList.Add(columnOrderHint);
		return columnOrderHint;
	}

	public SqlBulkCopyColumnOrderHint Add(string column, SortOrder sortOrder)
	{
		return Add(new SqlBulkCopyColumnOrderHint(column, sortOrder));
	}

	protected override void OnClear()
	{
		foreach (SqlBulkCopyColumnOrderHint inner in base.InnerList)
		{
			UnregisterColumnName(inner, inner.Column);
		}
	}

	public bool Contains(SqlBulkCopyColumnOrderHint value)
	{
		return base.InnerList.Contains(value);
	}

	public void CopyTo(SqlBulkCopyColumnOrderHint[] array, int index)
	{
		base.InnerList.CopyTo(array, index);
	}

	public int IndexOf(SqlBulkCopyColumnOrderHint value)
	{
		return base.InnerList.IndexOf(value);
	}

	public void Insert(int index, SqlBulkCopyColumnOrderHint columnOrderHint)
	{
		if (index < 0 || index > base.InnerList.Count)
		{
			base.InnerList.Insert(index, columnOrderHint);
		}
		if (columnOrderHint == null)
		{
			throw new ArgumentNullException("columnOrderHint");
		}
		RegisterColumnName(columnOrderHint, columnOrderHint.Column);
		base.InnerList.Insert(index, columnOrderHint);
	}

	public void Remove(SqlBulkCopyColumnOrderHint columnOrderHint)
	{
		if (columnOrderHint == null)
		{
			throw new ArgumentNullException("columnOrderHint");
		}
		base.List.Remove(columnOrderHint);
	}

	protected override void OnRemove(int index, object value)
	{
		if (value is SqlBulkCopyColumnOrderHint sqlBulkCopyColumnOrderHint)
		{
			UnregisterColumnName(sqlBulkCopyColumnOrderHint, sqlBulkCopyColumnOrderHint.Column);
			return;
		}
		throw new ArgumentNullException("orderHint");
	}

	private void ColumnNameChanging(object sender, string newName)
	{
		if (sender is SqlBulkCopyColumnOrderHint sqlBulkCopyColumnOrderHint)
		{
			if (_columnNames.Contains(newName))
			{
				throw SQL.BulkLoadOrderHintDuplicateColumn(newName);
			}
			UnregisterColumnName(sqlBulkCopyColumnOrderHint, sqlBulkCopyColumnOrderHint.Column);
			RegisterColumnName(sqlBulkCopyColumnOrderHint, newName);
		}
	}

	private void RegisterColumnName(SqlBulkCopyColumnOrderHint orderHint, string columnName)
	{
		if (_columnNames.Contains(columnName))
		{
			throw SQL.BulkLoadOrderHintDuplicateColumn(orderHint.Column);
		}
		_columnNames.Add(columnName);
		orderHint.NameChanging += ColumnNameChanging;
	}

	private void UnregisterColumnName(SqlBulkCopyColumnOrderHint orderHint, string columnName)
	{
		if (Contains(orderHint))
		{
			_columnNames.Remove(columnName);
			orderHint.NameChanging -= ColumnNameChanging;
		}
	}
}
