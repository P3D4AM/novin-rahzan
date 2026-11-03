using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Engine;

namespace Stimulsoft.Report.Dictionary;

internal class StiDataSort : IComparer<DataRow>
{
	private string[] sortColumns;

	private Hashtable rowToConditions;

	private object[,,] conditions;

	private Hashtable[] hashValues;

	private StiDataSource dataSource;

	private StiText textComp;

	private StiParserParameters parserParameters = new StiParserParameters();

	private static readonly object nullObject = new object();

	public int Compare(DataRow x, DataRow y)
	{
		return CompareRows(x, y);
	}

	private int CompareRows(object x, object y)
	{
		try
		{
			if (x == y)
			{
				return 0;
			}
			DataRow key = x as DataRow;
			DataRow key2 = y as DataRow;
			if (rowToConditions != null && conditions != null)
			{
				int num = (int)rowToConditions[key];
				int num2 = (int)rowToConditions[key2];
				int length = conditions.GetLength(1);
				for (int i = 0; i < length; i++)
				{
					object value = conditions[num, i, 0];
					object value2 = conditions[num2, i, 0];
					StiGroupSortDirection stiGroupSortDirection = (StiGroupSortDirection)conditions[num, i, 1];
					int num3 = CompareValues(value, value2, stiGroupSortDirection == StiGroupSortDirection.Ascending);
					if (num3 != 0)
					{
						return num3;
					}
				}
			}
			if (sortColumns == null || sortColumns.Length == 0)
			{
				return 0;
			}
			int num4 = 0;
			int j = 0;
			while (j < sortColumns.Length)
			{
				bool ascendary = sortColumns[j++] == "ASC";
				int num5 = j;
				for (; j < sortColumns.Length && sortColumns[j] != "ASC" && sortColumns[j] != "DESC"; j++)
				{
				}
				object obj = hashValues[num4][x];
				object obj2 = hashValues[num4][y];
				if (obj == null || obj2 == null)
				{
					StiDataSource parentDataSource = dataSource;
					DataRow dataRow = x as DataRow;
					DataRow dataRow2 = y as DataRow;
					string text = sortColumns[num5];
					if (text.StartsWith("{", StringComparison.InvariantCulture) && text.EndsWith("}", StringComparison.InvariantCulture))
					{
						string name = text.Substring(1, text.Length - 2);
						MethodInfo method = parentDataSource.Dictionary.Report.GetType().GetMethod(name);
						bool flag = parentDataSource.Dictionary.Report.CalculationMode == StiCalculationMode.Compilation;
						int position = (int)rowToConditions[dataRow];
						int position2 = (int)rowToConditions[dataRow2];
						int position3 = parentDataSource.Position;
						parentDataSource.Position = position;
						obj = (flag ? method.Invoke(parentDataSource.Dictionary.Report, new object[0]) : StiParser.ParseTextValue(text, textComp, parserParameters));
						parentDataSource.Position = position2;
						obj2 = (flag ? method.Invoke(parentDataSource.Dictionary.Report, new object[0]) : StiParser.ParseTextValue(text, textComp, parserParameters));
						parentDataSource.Position = position3;
					}
					else
					{
						if (j - num5 > 1)
						{
							for (int k = num5; k < j - 1; k++)
							{
								parentDataSource = parentDataSource.GetParentDataSource(sortColumns[k]);
								if (parentDataSource == null)
								{
									return 0;
								}
								if (obj == null)
								{
									DataRow[] array = dataRow?.GetParentRows(sortColumns[k]);
									dataRow = ((array == null || array.Length == 0) ? null : array[0]);
								}
								if (obj2 == null)
								{
									DataRow[] array2 = dataRow2?.GetParentRows(sortColumns[k]);
									dataRow2 = ((array2 == null || array2.Length == 0) ? null : array2[0]);
								}
							}
						}
						int columnIndex = parentDataSource.GetColumnIndex(sortColumns[j - 1]);
						if (columnIndex != -1)
						{
							if (obj == null)
							{
								obj = ((dataRow == null) ? nullObject : dataRow.ItemArray[columnIndex]);
								if (hashValues[num4][x] == null)
								{
									hashValues[num4].Add(x, obj);
								}
							}
							if (obj2 == null)
							{
								obj2 = ((dataRow2 == null) ? nullObject : dataRow2.ItemArray[columnIndex]);
								if (hashValues[num4][y] == null)
								{
									hashValues[num4].Add(y, obj2);
								}
							}
						}
						else
						{
							StiDataColumn stiDataColumn = parentDataSource.Columns[sortColumns[j - 1]];
							if (stiDataColumn is StiCalcDataColumn stiCalcDataColumn && rowToConditions != null)
							{
								string name2 = "Get" + parentDataSource.Name + "_" + stiCalcDataColumn.Name;
								MethodInfo method2 = parentDataSource.Dictionary.Report.GetType().GetMethod(name2);
								bool flag2 = parentDataSource.Dictionary.Report.CalculationMode == StiCalculationMode.Compilation;
								if (obj == null)
								{
									if (dataRow != null)
									{
										int position4 = (int)rowToConditions[dataRow];
										int position5 = parentDataSource.Position;
										parentDataSource.Position = position4;
										obj = (flag2 ? method2.Invoke(parentDataSource.Dictionary.Report, new object[0]) : StiParser.ParseTextValue("{" + stiCalcDataColumn.Expression + "}", textComp, parserParameters));
										parentDataSource.Position = position5;
									}
									else
									{
										obj = nullObject;
									}
									if (!hashValues[num4].ContainsKey(x))
									{
										hashValues[num4].Add(x, obj);
									}
								}
								if (obj2 == null)
								{
									if (dataRow2 != null)
									{
										int position6 = (int)rowToConditions[dataRow2];
										int position7 = parentDataSource.Position;
										parentDataSource.Position = position6;
										obj2 = (flag2 ? method2.Invoke(parentDataSource.Dictionary.Report, new object[0]) : StiParser.ParseTextValue("{" + stiCalcDataColumn.Expression + "}", textComp, parserParameters));
										parentDataSource.Position = position7;
									}
									else
									{
										obj2 = nullObject;
									}
									if (!hashValues[num4].ContainsKey(y))
									{
										hashValues[num4].Add(y, obj2);
									}
								}
							}
						}
					}
				}
				num4++;
				int num6 = CompareValues(obj, obj2, ascendary);
				if (num6 != 0)
				{
					return num6;
				}
			}
			return 0;
		}
		catch
		{
			return 0;
		}
	}

	internal static int CompareValues(object value1, object value2)
	{
		return CompareValues(value1, value2, ascendary: true);
	}

	internal static int CompareValues(object value1, object value2, bool ascendary)
	{
		int num = 1;
		if (!ascendary)
		{
			num = -1;
		}
		if (value1 == DBNull.Value && value2 == DBNull.Value)
		{
			return 0;
		}
		if (value1 == DBNull.Value)
		{
			return -1 * num;
		}
		if (value2 == DBNull.Value)
		{
			return num;
		}
		IComparable comparable = value1 as IComparable;
		IComparable comparable2 = value2 as IComparable;
		if (comparable == null && comparable2 == null)
		{
			return 0;
		}
		if (comparable == null)
		{
			return -1 * num;
		}
		if (comparable2 == null)
		{
			return num;
		}
		if (comparable != comparable2)
		{
			int num2 = ((!StiOptions.Engine.OrdinalStringComparison || !(value1 is string) || !(value2 is string)) ? comparable.CompareTo(comparable2) : StringComparer.Ordinal.Compare(value1, value2));
			if (!ascendary)
			{
				num2 = -num2;
			}
			if (num2 != 0)
			{
				return num2;
			}
		}
		return 0;
	}

	internal void Clear()
	{
		sortColumns = null;
		if (rowToConditions != null)
		{
			rowToConditions.Clear();
			rowToConditions = null;
		}
		conditions = null;
		hashValues = null;
		dataSource = null;
		textComp = null;
	}

	public StiDataSort(Hashtable rowToConditions, object[,,] conditions, string[] sortColumns, StiDataSource dataSource)
	{
		this.dataSource = dataSource;
		this.sortColumns = sortColumns;
		this.conditions = conditions;
		this.rowToConditions = rowToConditions;
		if (sortColumns != null)
		{
			this.sortColumns = sortColumns.Clone() as string[];
			int i = 0;
			int num = 0;
			for (; i < sortColumns.Length; i++)
			{
				string text = sortColumns[i];
				if (text == "ASC")
				{
					num++;
				}
				if (text == "DESC")
				{
					num++;
				}
				if (text.StartsWith("{", StringComparison.InvariantCulture) && text.EndsWith("}", StringComparison.InvariantCulture))
				{
					string text2 = text.Substring(1, text.Length - 2).Trim();
					if (dataSource.Columns.Contains(text2))
					{
						this.sortColumns[i] = "{" + StiNameValidator.CorrectName(dataSource.Name) + "." + text2 + "}";
					}
				}
			}
			hashValues = new Hashtable[num];
			for (int j = 0; j < num; j++)
			{
				hashValues[j] = new Hashtable();
			}
		}
		if (dataSource.Dictionary == null)
		{
			return;
		}
		bool flag = false;
		foreach (object column in dataSource.Columns)
		{
			if (column is StiCalcDataColumn)
			{
				flag = true;
				break;
			}
		}
		StiReport report = dataSource.Dictionary.Report;
		if (report != null && (report.CalculationMode == StiCalculationMode.Interpretation || flag))
		{
			textComp = new StiText
			{
				Name = "**StiDataSort**",
				Page = report.Pages[0]
			};
		}
	}
}
