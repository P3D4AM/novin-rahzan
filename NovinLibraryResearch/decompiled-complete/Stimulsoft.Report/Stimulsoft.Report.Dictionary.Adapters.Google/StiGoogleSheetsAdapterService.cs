using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Data.Connectors.Google;
using Stimulsoft.Report.Dictionary.DataSources.Google;
using Stimulsoft.Report.Dictionary.Databases.Google;

namespace Stimulsoft.Report.Dictionary.Adapters.Google;

public class StiGoogleSheetsAdapterService : StiDataStoreAdapterService
{
	public override Type GetDataSourceType()
	{
		return typeof(StiGoogleSheetsSource);
	}

	public override void ConnectDataSourceToData(StiDictionary dictionary, StiDataSource dataSource, bool loadData)
	{
		dataSource.Disconnect();
		if (!loadData)
		{
			dataSource.DataTable = new DataTable();
			return;
		}
		StiGoogleSheetsSource googleSheetsTableSource = dataSource as StiGoogleSheetsSource;
		if (dictionary.Databases.Cast<StiDatabase>().FirstOrDefault((StiDatabase d) => d is StiGoogleSheetsDatabase && d.Name.ToLowerInvariant() == googleSheetsTableSource.NameInSource.ToLowerInvariant()) is StiGoogleSheetsDatabase stiGoogleSheetsDatabase)
		{
			StiGoogleSheetsConnector val = CreateConnector(stiGoogleSheetsDatabase.ClientId, stiGoogleSheetsDatabase.ClientSecret, stiGoogleSheetsDatabase.SpreadsheetId, stiGoogleSheetsDatabase.FirstRowIsHeader);
			dataSource.DataTable = val.GetDataTable(googleSheetsTableSource.Name, "", (int?)null, (int?)null);
		}
	}

	public StiGoogleSheetsConnector CreateConnector(string clientId, string clientSecret, string spreadsheetId, bool firstRowIsHeader)
	{
		return StiGoogleSheetsConnector.Get(clientId, clientSecret, spreadsheetId, firstRowIsHeader);
	}

	public override StiDataColumnsCollection GetColumnsFromData(StiData data, StiDataSource dataSource)
	{
		return GetColumnsFromData(data, dataSource, CommandBehavior.SchemaOnly);
	}

	public override StiDataColumnsCollection GetColumnsFromData(StiData data, StiDataSource dataSource, CommandBehavior retrieveMode)
	{
		StiDataColumnsCollection dataColumns = new StiDataColumnsCollection();
		StiGoogleSheetsSource source = dataSource as StiGoogleSheetsSource;
		if (!(dataSource.Dictionary.Databases.Cast<StiDatabase>().FirstOrDefault((StiDatabase d) => d.Name.ToLowerInvariant() == source.NameInSource.ToLowerInvariant()) is StiGoogleSheetsDatabase stiGoogleSheetsDatabase))
		{
			return dataColumns;
		}
		StiGoogleSheetsConnector val = CreateConnector(stiGoogleSheetsDatabase.ClientId, stiGoogleSheetsDatabase.ClientSecret, stiGoogleSheetsDatabase.SpreadsheetId, stiGoogleSheetsDatabase.FirstRowIsHeader);
		List<StiDataColumnSchema> columns = val.GetColumns(source.Name);
		columns.ForEach(delegate(StiDataColumnSchema column)
		{
			dataColumns.Add(new StiDataColumn(((StiObjectSchema)column).Name, column.Type));
		});
		return dataColumns;
	}

	public void CreateConnectionInDataStore(StiDictionary dictionary, StiGoogleSheetsDatabase database)
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
				StiGoogleSheetsConnector data = CreateConnector(database.ClientId, database.ClientSecret, database.SpreadsheetId, database.FirstRowIsHeader);
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

	public override string GetDataCategoryName(StiData data)
	{
		return data.Name;
	}

	public override Type[] GetDataTypes()
	{
		return new Type[1] { typeof(StiGoogleSheetsSource) };
	}

	public override StiDataParametersCollection GetParametersFromData(StiData data, StiDataSource dataSource)
	{
		return new StiDataParametersCollection();
	}
}
