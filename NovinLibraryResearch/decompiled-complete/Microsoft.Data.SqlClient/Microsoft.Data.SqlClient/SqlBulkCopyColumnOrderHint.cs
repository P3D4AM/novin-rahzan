using System;

namespace Microsoft.Data.SqlClient;

public sealed class SqlBulkCopyColumnOrderHint
{
	private string _columnName;

	private SortOrder _sortOrder;

	public string Column
	{
		get
		{
			return _columnName ?? string.Empty;
		}
		set
		{
			if (string.IsNullOrEmpty(value))
			{
				throw SQL.BulkLoadNullEmptyColumnName("Column");
			}
			if (_columnName != value)
			{
				OnNameChanging(value);
				_columnName = value;
			}
		}
	}

	public SortOrder SortOrder
	{
		get
		{
			return _sortOrder;
		}
		set
		{
			if (value != SortOrder.Unspecified)
			{
				_sortOrder = value;
				return;
			}
			throw SQL.BulkLoadUnspecifiedSortOrder();
		}
	}

	internal event EventHandler<string> NameChanging;

	public SqlBulkCopyColumnOrderHint(string column, SortOrder sortOrder)
	{
		Column = column;
		SortOrder = sortOrder;
	}

	private void OnNameChanging(string newName)
	{
		this.NameChanging?.Invoke(this, newName);
	}
}
