using System;
using System.Data;
using System.Globalization;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dictionary;

public class StiDataViewAdapterService : StiDataStoreAdapterService
{
	public override string ServiceName => StiLocalization.Get("Adapters", "AdapterDataViews");

	public override bool IsObjectAdapter => true;

	public override string GetDataCategoryName(StiData data)
	{
		string text = data.Name;
		if (data.ViewData is DataTable dataTable)
		{
			if (data.Name != null && data.Data is DataTable)
			{
				int num = text.LastIndexOf(".", StringComparison.InvariantCulture);
				if (num != -1)
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
			if (StiOptions.Dictionary.ShowOnlyAliasForData)
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
		DataView dataView = data.Data as DataView;
		if (dataView.Table == null)
		{
			return stiDataColumnsCollection;
		}
		foreach (DataColumn column in dataView.Table.Columns)
		{
			stiDataColumnsCollection.Add(new StiDataColumn(column.ColumnName, column.Caption, column.DataType));
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
		DataView dataView = data.Data as DataView;
		dataSource.Name = "view" + dataView.Table.TableName;
		dataSource.Alias = "view" + dataView.Table.TableName;
	}

	public override Type GetDataSourceType()
	{
		return typeof(StiDataViewSource);
	}

	public override Type[] GetDataTypes()
	{
		return new Type[1] { typeof(DataView) };
	}

	public override void ConnectDataSourceToData(StiDictionary dictionary, StiDataSource dataSource, bool loadData)
	{
		StiDataLeader.Disconnect(dataSource);
		StiDataStoreSource stiDataStoreSource = dataSource as StiDataStoreSource;
		DataView dataView = null;
		string text = stiDataStoreSource.NameInSource.ToLower(CultureInfo.InvariantCulture);
		foreach (StiData item in dataSource.Dictionary.DataStore)
		{
			if (item.Name.ToLower(CultureInfo.InvariantCulture) == text && item.Data is DataView)
			{
				dataView = item.Data as DataView;
				break;
			}
		}
		if (dataView == null)
		{
			string text2 = null;
			if (text.StartsWith("view", StringComparison.InvariantCulture))
			{
				text2 = text.Substring(4);
			}
			foreach (StiData item2 in dataSource.Dictionary.DataStore)
			{
				if (item2.Data is DataView)
				{
					string text3 = item2.Name.ToLower(CultureInfo.InvariantCulture);
					string text4 = null;
					if (text3.StartsWith("view", StringComparison.InvariantCulture))
					{
						text4 = text3.Substring(4);
					}
					if (text3 == text || text3 == text2 || text4 == text || text4 == text2)
					{
						dataView = item2.Data as DataView;
						break;
					}
				}
			}
		}
		if (dataView != null)
		{
			stiDataStoreSource.DataTable = GetDataTableFromData(dataView, loadData);
		}
	}

	private DataTable GetDataTableFromData(DataView view, bool loadData)
	{
		DataTable dataTable = view.Table.Clone();
		dataTable.Rows.Clear();
		if (!loadData)
		{
			return dataTable;
		}
		for (int i = 0; i < view.Count; i++)
		{
			DataRow dataRow = dataTable.NewRow();
			foreach (DataColumn column in view.Table.Columns)
			{
				try
				{
					dataRow[column.ColumnName] = view[i][column.ColumnName];
				}
				catch (Exception e)
				{
					StiLogService.Write(((object)this).GetType(), e);
				}
			}
			dataTable.Rows.Add(dataRow);
		}
		return dataTable;
	}
}
