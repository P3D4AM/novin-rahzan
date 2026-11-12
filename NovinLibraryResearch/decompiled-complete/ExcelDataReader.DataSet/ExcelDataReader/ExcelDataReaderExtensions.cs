using System;
using System.Collections.Generic;
using System.Data;

namespace ExcelDataReader;

public static class ExcelDataReaderExtensions
{
	public static DataSet AsDataSet(this IExcelDataReader self, ExcelDataSetConfiguration configuration = null)
	{
		if (configuration == null)
		{
			configuration = new ExcelDataSetConfiguration();
		}
		self.Reset();
		int num = -1;
		DataSet dataSet = new DataSet();
		do
		{
			num++;
			if (configuration.FilterSheet == null || configuration.FilterSheet(self, num))
			{
				ExcelDataTableConfiguration excelDataTableConfiguration = ((configuration.ConfigureDataTable != null) ? configuration.ConfigureDataTable(self) : null);
				if (excelDataTableConfiguration == null)
				{
					excelDataTableConfiguration = new ExcelDataTableConfiguration();
				}
				DataTable table = AsDataTable(self, excelDataTableConfiguration);
				dataSet.Tables.Add(table);
			}
		}
		while (self.NextResult());
		dataSet.AcceptChanges();
		if (configuration.UseColumnDataType)
		{
			FixDataTypes(dataSet);
		}
		self.Reset();
		return dataSet;
	}

	private static string GetUniqueColumnName(DataTable table, string name)
	{
		string text = name;
		int num = 1;
		while (table.Columns[text] != null)
		{
			text = $"{name}_{num}";
			num++;
		}
		return text;
	}

	private static DataTable AsDataTable(IExcelDataReader self, ExcelDataTableConfiguration configuration)
	{
		DataTable dataTable = new DataTable
		{
			TableName = self.Name
		};
		dataTable.ExtendedProperties.Add("visiblestate", self.VisibleState);
		bool flag = true;
		int num = 0;
		List<int> list = new List<int>();
		while (self.Read())
		{
			if (flag)
			{
				if (configuration.UseHeaderRow && configuration.ReadHeaderRow != null)
				{
					configuration.ReadHeaderRow(self);
				}
				for (int i = 0; i < self.FieldCount; i++)
				{
					if (configuration.FilterColumn == null || configuration.FilterColumn(self, i))
					{
						string text = (configuration.UseHeaderRow ? Convert.ToString(self.GetValue(i)) : null);
						if (string.IsNullOrEmpty(text))
						{
							text = configuration.EmptyColumnNamePrefix + i;
						}
						DataColumn column = new DataColumn(GetUniqueColumnName(dataTable, text), typeof(object))
						{
							Caption = text
						};
						dataTable.Columns.Add(column);
						list.Add(i);
					}
				}
				dataTable.BeginLoadData();
				flag = false;
				if (configuration.UseHeaderRow)
				{
					continue;
				}
			}
			if (configuration.FilterRow != null && !configuration.FilterRow(self))
			{
				continue;
			}
			if (IsEmptyRow(self))
			{
				num++;
				continue;
			}
			for (int j = 0; j < num; j++)
			{
				dataTable.Rows.Add(dataTable.NewRow());
			}
			num = 0;
			DataRow dataRow = dataTable.NewRow();
			for (int k = 0; k < list.Count; k++)
			{
				int i2 = list[k];
				object value = self.GetValue(i2);
				dataRow[k] = value;
			}
			dataTable.Rows.Add(dataRow);
		}
		dataTable.EndLoadData();
		return dataTable;
	}

	private static bool IsEmptyRow(IExcelDataReader reader)
	{
		for (int i = 0; i < reader.FieldCount; i++)
		{
			if (reader.GetValue(i) != null)
			{
				return false;
			}
		}
		return true;
	}

	private static void FixDataTypes(DataSet dataset)
	{
		List<DataTable> list = new List<DataTable>(dataset.Tables.Count);
		bool flag = false;
		foreach (DataTable table in dataset.Tables)
		{
			if (table.Rows.Count == 0)
			{
				list.Add(table);
				continue;
			}
			DataTable dataTable2 = null;
			for (int i = 0; i < table.Columns.Count; i++)
			{
				Type type = null;
				foreach (DataRow row2 in table.Rows)
				{
					if (row2.IsNull(i))
					{
						continue;
					}
					Type type2 = row2[i].GetType();
					if ((object)type2 != type)
					{
						if ((object)type != null)
						{
							type = null;
							break;
						}
						type = type2;
					}
				}
				if ((object)type != null)
				{
					flag = true;
					if (dataTable2 == null)
					{
						dataTable2 = table.Clone();
					}
					dataTable2.Columns[i].DataType = type;
				}
			}
			if (dataTable2 != null)
			{
				dataTable2.BeginLoadData();
				foreach (DataRow row3 in table.Rows)
				{
					dataTable2.ImportRow(row3);
				}
				dataTable2.EndLoadData();
				list.Add(dataTable2);
			}
			else
			{
				list.Add(table);
			}
		}
		if (flag)
		{
			dataset.Tables.Clear();
			dataset.Tables.AddRange(list.ToArray());
		}
	}
}
