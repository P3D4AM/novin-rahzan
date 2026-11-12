using System;
using System.Globalization;
using System.Text;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.SqlScriptPublish;

internal class TableSelectStatement
{
	private Table table;

	private bool hasWritableColumns;

	private bool hasUserDefinedType;

	private string columnNames;

	private string tableName;

	internal bool HasUserDefinedType => hasUserDefinedType;

	internal bool HasWritableColumns => hasWritableColumns;

	internal string TableName => tableName;

	public TableSelectStatement(Table table)
	{
		this.table = table;
		tableName = table.FullQualifiedName;
		hasWritableColumns = false;
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = true;
		foreach (Column column in this.table.Columns)
		{
			if (column.DataType.SqlDataType != SqlDataType.Timestamp && !column.Computed)
			{
				if (!flag)
				{
					stringBuilder.Append(", ");
				}
				flag = false;
				if (column.DataType.SqlDataType == SqlDataType.UserDefinedType)
				{
					hasUserDefinedType = true;
				}
				stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "[{0}]", column.Name));
			}
		}
		hasWritableColumns = stringBuilder.Length > 0;
		if (hasWritableColumns)
		{
			columnNames = stringBuilder.ToString();
		}
	}

	internal string GetSelectStatement()
	{
		if (!HasWritableColumns)
		{
			throw new InvalidOperationException();
		}
		return string.Format(CultureInfo.InvariantCulture, "SELECT {0} FROM {1}", columnNames, tableName);
	}
}
