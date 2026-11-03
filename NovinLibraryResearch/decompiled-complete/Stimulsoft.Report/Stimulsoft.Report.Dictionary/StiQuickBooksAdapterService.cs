using System;
using System.Data;
using System.Linq;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiQuickBooksAdapterService : StiSqlAdapterService
{
	public override Type GetDataSourceType()
	{
		return typeof(StiQuickBooksSource);
	}

	public override StiSqlDataConnector CreateConnector(string connectionString = null)
	{
		return (StiSqlDataConnector)(object)StiQuickBooksConnector.Get(connectionString);
	}

	public override StiDataColumnsCollection GetColumnsFromData(StiData data, StiDataSource dataSource)
	{
		return GetColumnsFromData(data, dataSource, CommandBehavior.SchemaOnly);
	}

	public override StiDataColumnsCollection GetColumnsFromData(StiData data, StiDataSource dataSource, CommandBehavior retrieveMode)
	{
		StiDataColumnsCollection stiDataColumnsCollection = new StiDataColumnsCollection();
		StiSqlSource stiSqlSource = dataSource as StiSqlSource;
		try
		{
			if (!string.IsNullOrEmpty(stiSqlSource.SqlCommand))
			{
				object viewData = data.ViewData;
				StiQuickBooksConnector val = (StiQuickBooksConnector)((viewData is StiQuickBooksConnector) ? viewData : null);
				DataTable dataTable = val.GetDataTable((string)null, stiSqlSource.SqlCommand);
				foreach (DataColumn column in dataTable.Columns)
				{
					Type type = Type.GetType(column.DataType.ToString());
					if (type == null)
					{
						type = typeof(object);
					}
					stiDataColumnsCollection.Add(new StiDataColumn(column.ColumnName, column.ColumnName, type));
				}
			}
		}
		catch (Exception e)
		{
			StiLogService.Write(((object)this).GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
		return stiDataColumnsCollection;
	}

	public override StiDataParametersCollection GetParametersFromData(StiData data, StiDataSource dataSource)
	{
		return new StiDataParametersCollection();
	}

	public override void ConnectDataSourceToData(StiDictionary dictionary, StiDataSource dataSource, bool loadData)
	{
		if (!loadData)
		{
			return;
		}
		StiDataLeader.Disconnect(dataSource);
		StiQuickBooksSource quickBooksSource = dataSource as StiQuickBooksSource;
		if (dictionary.Databases.ToList().FirstOrDefault((StiDatabase d) => d.Name == quickBooksSource.NameInSource) is StiQuickBooksDatabase stiQuickBooksDatabase)
		{
			StiSqlDataConnector obj = CreateConnector(stiQuickBooksDatabase.ConnectionString);
			StiQuickBooksConnector val = (StiQuickBooksConnector)(object)((obj is StiQuickBooksConnector) ? obj : null);
			DataTable table = new DataTable(quickBooksSource.Name);
			quickBooksSource.Columns.ToList().ForEach(delegate(StiDataColumn c)
			{
				table.Columns.Add(c.Name, c.Type);
			});
			((StiRestDataConnector)val).FillDataTable(table, quickBooksSource.SqlCommand);
			stiQuickBooksDatabase.ConnectionString = ((StiSqlDataConnector)val).ConnectionString;
			quickBooksSource.DataTable = table;
		}
	}

	public override void CreateConnectionInDataStore(StiDictionary dictionary, StiSqlDatabase database)
	{
		try
		{
			if (database.Name != null)
			{
				StiData stiData = dictionary.DataStore.ToList().FirstOrDefault((StiData d) => d.Name != null && d.Name.ToLowerInvariant() == database.Name.ToLowerInvariant());
				if (stiData != null)
				{
					dictionary.DataStore.Remove(stiData);
				}
				StiSqlDataConnector data = CreateConnector(database.ConnectionString);
				dictionary.DataStore.Add(new StiData(database.Name, data)
				{
					IsReportData = true
				});
			}
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
}
