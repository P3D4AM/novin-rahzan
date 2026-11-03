using System;
using System.Data;
using System.Globalization;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;

namespace Stimulsoft.Report.Dictionary;

public class StiUserAdapterService : StiDataStoreAdapterService
{
	public override string ServiceName => StiLocalization.Get("Adapters", "AdapterUserSources");

	public override bool IsObjectAdapter => true;

	public override void SetDataSourceNames(StiData data, StiDataSource dataSource)
	{
		base.SetDataSourceNames(data, dataSource);
		if (data.ViewData is StiUserData stiUserData)
		{
			dataSource.Name = stiUserData.Name;
			dataSource.Alias = stiUserData.Alias;
		}
	}

	public override string GetDataCategoryName(StiData data)
	{
		return ((StiService)this).ServiceName;
	}

	public override StiDataColumnsCollection GetColumnsFromData(StiData data, StiDataSource dataSource)
	{
		return GetColumnsFromData(data, dataSource, CommandBehavior.SchemaOnly);
	}

	public override StiDataColumnsCollection GetColumnsFromData(StiData data, StiDataSource dataSource, CommandBehavior retrieveMode)
	{
		StiDataColumnsCollection stiDataColumnsCollection = new StiDataColumnsCollection();
		if (data.ViewData is StiUserData stiUserData)
		{
			stiDataColumnsCollection.AddRange(stiUserData.Columns);
		}
		foreach (StiDataColumn item in stiDataColumnsCollection)
		{
			item.Name = StiNameValidator.CorrectName(item.Name, dataSource?.Dictionary?.Report);
		}
		return stiDataColumnsCollection;
	}

	public override StiDataParametersCollection GetParametersFromData(StiData data, StiDataSource dataSource)
	{
		return new StiDataParametersCollection();
	}

	public override Type GetDataSourceType()
	{
		return typeof(StiUserSource);
	}

	public override Type[] GetDataTypes()
	{
		return new Type[1] { typeof(StiUserData) };
	}

	public override void ConnectDataSourceToData(StiDictionary dictionary, StiDataSource dataSource, bool loadData)
	{
		StiDataLeader.Disconnect(dataSource);
		StiUserData stiUserData = null;
		StiDataStoreSource stiDataStoreSource = dataSource as StiDataStoreSource;
		string text = stiDataStoreSource.NameInSource.ToLower(CultureInfo.InvariantCulture);
		foreach (StiData item in dataSource.Dictionary.DataStore)
		{
			if (item.Name.ToLower(CultureInfo.InvariantCulture) == text && item.Data is StiUserData)
			{
				stiUserData = item.Data as StiUserData;
				break;
			}
		}
		if (stiUserData == null)
		{
			string text2 = null;
			if (text.StartsWith("view", StringComparison.InvariantCulture))
			{
				text2 = text.Substring(4);
			}
			foreach (StiData item2 in dataSource.Dictionary.DataStore)
			{
				if (item2.Data is StiUserData)
				{
					string text3 = item2.Name.ToLower(CultureInfo.InvariantCulture);
					string text4 = null;
					if (text3.StartsWith("view", StringComparison.InvariantCulture))
					{
						text4 = text3.Substring(4);
					}
					if (text3 == text || text3 == text2 || text4 == text || text4 == text2)
					{
						stiUserData = item2.Data as StiUserData;
						break;
					}
				}
			}
		}
		if (stiUserData != null)
		{
			stiDataStoreSource.DataTable = GetDataTableFromData(dataSource, stiUserData);
		}
	}

	private bool IsNullableType(Type type)
	{
		if (type.IsGenericType)
		{
			return type.GetGenericTypeDefinition() == typeof(Nullable<>);
		}
		return false;
	}

	private DataTable GetDataTableFromData(StiDataSource dataSource, StiUserData userData)
	{
		userData.InvokeConnect(EventArgs.Empty);
		DataTable dataTable = new DataTable(dataSource.Name);
		foreach (StiDataColumn column in userData.Columns)
		{
			if (IsNullableType(column.Type))
			{
				dataTable.Columns.Add(column.Name, column.Type.GetGenericArguments()[0]);
			}
			else
			{
				dataTable.Columns.Add(column.Name, column.Type);
			}
		}
		if (userData.Columns.Count == 0)
		{
			foreach (StiDataColumn column2 in dataSource.Columns)
			{
				dataTable.Columns.Add(column2.Name, column2.Type);
			}
		}
		for (int i = 0; i < userData.Count; i++)
		{
			DataRow dataRow = dataTable.NewRow();
			foreach (DataColumn column3 in dataTable.Columns)
			{
				try
				{
					StiUserGetDataEventArgs stiUserGetDataEventArgs = new StiUserGetDataEventArgs(dataSource as StiUserSource, i, column3.ColumnName);
					userData.InvokeGetData(stiUserGetDataEventArgs);
					dataRow[column3.ColumnName] = stiUserGetDataEventArgs.Data;
				}
				catch (Exception e)
				{
					StiLogService.Write(((object)this).GetType(), e);
					if (!StiOptions.Engine.HideExceptions)
					{
						throw;
					}
				}
			}
			dataTable.Rows.Add(dataRow);
		}
		userData.InvokeDisconnent(EventArgs.Empty);
		return dataTable;
	}
}
