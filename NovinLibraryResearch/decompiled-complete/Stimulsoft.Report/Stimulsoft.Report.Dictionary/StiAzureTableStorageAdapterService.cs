using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiAzureTableStorageAdapterService : StiNoSqlAdapterService
{
	public override Type GetDataSourceType()
	{
		return typeof(StiAzureTableStorageSource);
	}

	public override StiNoSqlDataConnector CreateConnector(string connectionString = null)
	{
		return (StiNoSqlDataConnector)(object)StiAzureTableStorageConnector.Get(connectionString);
	}

	public override StiDataColumnsCollection GetColumnsFromData(StiData data, StiDataSource dataSource)
	{
		return GetColumnsFromData(data, dataSource, CommandBehavior.SchemaOnly);
	}

	public override StiDataColumnsCollection GetColumnsFromData(StiData data, StiDataSource dataSource, CommandBehavior retrieveMode)
	{
		StiDataColumnsCollection dataColumns = new StiDataColumnsCollection();
		StiAzureTableStorageSource source = dataSource as StiAzureTableStorageSource;
		if (!(dataSource.Dictionary.Databases.Cast<StiDatabase>().FirstOrDefault((StiDatabase d) => d.Name.ToLowerInvariant() == source.NameInSource.ToLowerInvariant()) is StiAzureTableStorageDatabase stiAzureTableStorageDatabase))
		{
			return dataColumns;
		}
		StiNoSqlDataConnector val = CreateConnector(stiAzureTableStorageDatabase.ConnectionString);
		List<StiDataColumnSchema> columns = val.GetColumns("items");
		columns.ForEach(delegate(StiDataColumnSchema column)
		{
			dataColumns.Add(new StiDataColumn(((StiObjectSchema)column).Name, column.Type));
		});
		return dataColumns;
	}

	public override StiDataParametersCollection GetParametersFromData(StiData data, StiDataSource dataSource)
	{
		return new StiDataParametersCollection();
	}

	public override void ConnectDataSourceToData(StiDictionary dictionary, StiDataSource dataSource, bool loadData)
	{
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		StiDataLeader.Disconnect(dataSource);
		if (!loadData)
		{
			dataSource.DataTable = new DataTable();
			return;
		}
		StiAzureTableStorageSource azureTableSource = dataSource as StiAzureTableStorageSource;
		if (!(dictionary.Databases.Cast<StiDatabase>().FirstOrDefault((StiDatabase d) => d is StiAzureTableStorageDatabase && d.Name.ToLowerInvariant() == azureTableSource.NameInSource.ToLowerInvariant()) is StiAzureTableStorageDatabase stiAzureTableStorageDatabase))
		{
			return;
		}
		StiNoSqlDataConnector val = CreateConnector(stiAzureTableStorageDatabase.ConnectionString);
		string text = azureTableSource.Query;
		StiReport report = dictionary.Report;
		foreach (StiVariable variable in dictionary.Variables)
		{
			string newValue = report.ToString(report[variable.Name]);
			if (variable.Type == typeof(DateTime) && report[variable.Name] is DateTime)
			{
				newValue = ((StiAzureTableStorageConnector)val).ConvertDateTimeToJsonStr((DateTime)report[variable.Name]);
			}
			string text2 = "Param(" + variable.Name + ")";
			if (text.Contains(text2))
			{
				text = text.Replace(text2, newValue);
			}
		}
		dataSource.DataTable = val.GetDataTable(azureTableSource.Name, text, (int?)null, (int?)null);
	}
}
