using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Data.Connectors.OnlineServices;

namespace Stimulsoft.Report.Dictionary.Adapters.OnlineService;

public class StiDataWorldAdapterService : StiNoSqlAdapterService
{
	public override Type GetDataSourceType()
	{
		return typeof(StiDataWorldSource);
	}

	public override StiNoSqlDataConnector CreateConnector(string connectionString = null)
	{
		return (StiNoSqlDataConnector)(object)StiDataWorldConnector.Get(connectionString);
	}

	public override StiDataColumnsCollection GetColumnsFromData(StiData data, StiDataSource dataSource)
	{
		return GetColumnsFromData(data, dataSource, CommandBehavior.SchemaOnly);
	}

	public override StiDataColumnsCollection GetColumnsFromData(StiData data, StiDataSource dataSource, CommandBehavior retrieveMode)
	{
		StiDataColumnsCollection dataColumns = new StiDataColumnsCollection();
		StiDataWorldSource source = dataSource as StiDataWorldSource;
		if (!(dataSource.Dictionary.Databases.Cast<StiDatabase>().FirstOrDefault((StiDatabase d) => d.Name.ToLowerInvariant() == source.NameInSource.ToLowerInvariant()) is StiDataWorldDatabase stiDataWorldDatabase))
		{
			return dataColumns;
		}
		StiNoSqlDataConnector val = CreateConnector(stiDataWorldDatabase.ConnectionString);
		List<StiDataColumnSchema> columns = val.GetColumns(source.Name);
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
		StiDataWorldSource dataWorldSource = dataSource as StiDataWorldSource;
		if (!(dictionary.Databases.Cast<StiDatabase>().FirstOrDefault((StiDatabase d) => d is StiDataWorldDatabase && d.Name.ToLowerInvariant() == dataWorldSource.NameInSource.ToLowerInvariant()) is StiDataWorldDatabase stiDataWorldDatabase))
		{
			return;
		}
		StiNoSqlDataConnector val = CreateConnector(stiDataWorldDatabase.ConnectionString);
		string text = dataWorldSource.Query;
		StiReport report = dictionary.Report;
		foreach (StiVariable variable in dictionary.Variables)
		{
			string newValue = report.ToString(report[variable.Name]);
			if (variable.Type == typeof(DateTime) && report[variable.Name] is DateTime)
			{
				newValue = ((StiMongoDbConnector)val).ConvertDateTimeToJsonStr((DateTime)report[variable.Name]);
			}
			string text2 = "Param(" + variable.Name + ")";
			if (text.Contains(text2))
			{
				text = text.Replace(text2, newValue);
			}
		}
		dataSource.DataTable = val.GetDataTable(dataWorldSource.Name, text, (int?)null, (int?)null);
	}
}
