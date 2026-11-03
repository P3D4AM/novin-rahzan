using System;
using System.Data;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dictionary;

public class StiDataTableAdapterService : StiDataStoreAdapterService
{
	public override string ServiceName => StiLocalization.Get("Adapters", "AdapterDataTables");

	public override bool IsObjectAdapter => true;

	public override string GetDataCategoryName(StiData data)
	{
		string text = data.Name;
		DataTable dataTable = data.ViewData as DataTable;
		if (dataTable == null && data.ViewData is DataView)
		{
			dataTable = (data.ViewData as DataView).Table;
		}
		if (dataTable != null)
		{
			if (data.Name != null && data.Data is DataTable)
			{
				int num = text.LastIndexOf(".", StringComparison.InvariantCulture);
				int num2 = dataTable.TableName.IndexOf(".");
				if (num2 != -1)
				{
					if (text.Length > dataTable.TableName.Length)
					{
						text = text.Substring(0, text.Length - dataTable.TableName.Length - 1);
					}
				}
				else if (num != -1 && text.IndexOf(dataTable.TableName) != -1)
				{
					text = text.Substring(0, num);
				}
			}
			else if (dataTable.DataSet != null)
			{
				return dataTable.DataSet.DataSetName;
			}
		}
		if (data.Alias != text && !string.IsNullOrEmpty(data.Alias))
		{
			if (StiOptions.Dictionary.ShowOnlyAliasForData && !string.IsNullOrWhiteSpace(data.Alias))
			{
				return data.Alias;
			}
			text = text + " [" + data.Alias + "]";
		}
		return text;
	}

	public override StiDataColumnsCollection GetColumnsFromData(StiData data, StiDataSource dataSource)
	{
		return GetColumnsFromData(data, dataSource, CommandBehavior.SchemaOnly);
	}

	public override StiDataColumnsCollection GetColumnsFromData(StiData data, StiDataSource dataSource, CommandBehavior retrieveMode)
	{
		StiDataColumnsCollection stiDataColumnsCollection = new StiDataColumnsCollection();
		DataTable dataTable = data.ViewData as DataTable;
		if (dataTable == null && data.ViewData is DataView)
		{
			dataTable = (data.ViewData as DataView).Table;
		}
		if (dataTable == null)
		{
			return null;
		}
		foreach (DataColumn column in dataTable.Columns)
		{
			Type type = column.DataType;
			if (column.AllowDBNull)
			{
				if (StiOptions.Dictionary.UseAllowDBNullProperty)
				{
					try
					{
						if (type.IsValueType && (!type.IsGenericType || !(type.GetGenericTypeDefinition() == typeof(Nullable<>))))
						{
							type = typeof(Nullable<>).MakeGenericType(type);
						}
					}
					catch
					{
					}
				}
				if (type == typeof(DateTime) && StiOptions.Dictionary.UseNullableDateTime)
				{
					type = typeof(DateTime?);
				}
				if (type == typeof(TimeSpan) && StiOptions.Dictionary.UseNullableTimeSpan)
				{
					type = typeof(TimeSpan?);
				}
			}
			stiDataColumnsCollection.Add(new StiDataColumn(column.ColumnName, column.Caption, type));
		}
		foreach (StiDataColumn item in stiDataColumnsCollection)
		{
			string name = item.Name;
			string alias = item.Alias;
			string name2 = item.Name;
			if (StiOptions.Designer.AutoCorrectDataColumnName)
			{
				item.Name = StiNameValidator.CorrectName(item.Name, dataSource?.Dictionary?.Report);
			}
			item.NameInSource = name2;
			item.Alias = name2;
			if (alias != name)
			{
				item.Alias = alias;
			}
		}
		return stiDataColumnsCollection;
	}

	public override StiDataParametersCollection GetParametersFromData(StiData data, StiDataSource dataSource)
	{
		return new StiDataParametersCollection();
	}

	public override void SetDataSourceNames(StiData data, StiDataSource dataSource)
	{
		base.SetDataSourceNames(data, dataSource);
		DataTable dataTable = data.ViewData as DataTable;
		if (dataTable == null && data.ViewData is DataView)
		{
			dataTable = (data.ViewData as DataView).Table;
		}
		if (dataTable == null)
		{
			return;
		}
		dataSource.Name = dataTable.TableName;
		dataSource.Alias = dataTable.TableName;
		string displayExpression = dataTable.DisplayExpression;
		if (!string.IsNullOrEmpty(displayExpression))
		{
			displayExpression = displayExpression.Trim();
			if (displayExpression.StartsWith("'") && displayExpression.EndsWith("'"))
			{
				dataSource.Alias = displayExpression.Substring(1, displayExpression.Length - 2);
			}
		}
	}

	public override Type GetDataSourceType()
	{
		return typeof(StiDataTableSource);
	}

	public override Type[] GetDataTypes()
	{
		return new Type[1] { typeof(DataTable) };
	}

	public StiData GetDataFromDataSource(StiDictionary dictionary, StiDataSource dataSource)
	{
		if (dataSource.Dictionary == null)
		{
			return null;
		}
		StiDataStoreSource stiDataStoreSource = dataSource as StiDataStoreSource;
		string text = stiDataStoreSource.NameInSource.ToLowerInvariant();
		foreach (StiData item in dataSource.Dictionary.DataStore)
		{
			if (item != null && item.Name.ToLowerInvariant() == text && (item.ViewData is DataTable || item.ViewData is DataView))
			{
				return item;
			}
		}
		if (!StiOptions.Dictionary.UseAdvancedDataSearch)
		{
			return null;
		}
		if (dataSource.Dictionary != null && dataSource.Dictionary.Report != null && dataSource.Dictionary.Report.IsDesigning)
		{
			return null;
		}
		string text2 = null;
		if (text.IndexOf(".", StringComparison.InvariantCulture) != -1)
		{
			text2 = text.Substring(text.IndexOf(".", StringComparison.InvariantCulture) + 1);
		}
		foreach (StiData item2 in dataSource.Dictionary.DataStore)
		{
			if (item2.ViewData is DataTable || item2.ViewData is DataView)
			{
				string text3 = item2.Name.ToLowerInvariant();
				if (text3 == text || text3 == text2)
				{
					return item2;
				}
				if (text3.IndexOf(".", StringComparison.InvariantCulture) != -1)
				{
					text3 = text3.Substring(text3.IndexOf(".", StringComparison.InvariantCulture) + 1);
				}
				if (text3 == text || text3 == text2)
				{
					return item2;
				}
			}
		}
		if (!StiOptions.Dictionary.AllowConnectToFirstTableForEmptyDataSource)
		{
			return null;
		}
		int num = 0;
		StiData result = null;
		foreach (StiData item3 in dataSource.Dictionary.DataStore)
		{
			if (item3.ViewData is DataTable || item3.ViewData is DataView)
			{
				num++;
				if (num > 1)
				{
					return null;
				}
				result = item3;
			}
		}
		if (num == 1 && dataSource.Dictionary.DataSources.Count > 1)
		{
			int num2 = 0;
			foreach (StiDataSource dataSource2 in dataSource.Dictionary.DataSources)
			{
				if (dataSource2.GetDataAdapter() is StiDataTableAdapterService)
				{
					num2++;
				}
			}
			if (num2 > 1)
			{
				return null;
			}
		}
		return result;
	}

	public override void ConnectDataSourceToData(StiDictionary dictionary, StiDataSource dataSource, bool loadData)
	{
		StiDataLeader.Disconnect(dataSource);
		StiData dataFromDataSource = GetDataFromDataSource(dictionary, dataSource);
		if (dataFromDataSource == null)
		{
			return;
		}
		DataTable dataTable = dataFromDataSource.ViewData as DataTable;
		if (dataTable == null && dataFromDataSource.ViewData is DataView)
		{
			dataTable = (dataFromDataSource.ViewData as DataView).Table;
		}
		if (dataTable != null)
		{
			if (dataSource.Dictionary.Report.CacheAllData && loadData)
			{
				dataSource.DataTable = dataSource.GetDataTable(dataTable);
			}
			else if (dataSource.Dictionary.Report.CacheAllData)
			{
				dataSource.Dictionary.Report.CacheAllData = false;
				dataSource.DataTable = dataTable;
				dataSource.Dictionary.Report.CacheAllData = true;
			}
			else
			{
				dataSource.DataTable = dataTable;
			}
		}
	}
}
