using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Data.Connectors.Azure;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dictionary.DataSources.Azure;
using Stimulsoft.Report.Dictionary.Databases.Azure;

namespace Stimulsoft.Report.Dictionary.Adapters.Azure;

public class StiCosmosDbAdapterService : StiNoSqlAdapterService
{
	public override string TestConnection(string connectionString)
	{
		try
		{
			StiNoSqlDataConnector val = CreateConnector(connectionString);
			StiTestConnectionResult val2 = val.TestConnection();
			if (val2.Success)
			{
				return StiLocalization.Get("DesignerFx", "ConnectionSuccessfull");
			}
			return val2.Notice;
		}
		catch (Exception ex)
		{
			return StiLocalization.Get("DesignerFx", "ConnectionError") + ": " + ex.Message;
		}
	}

	public override Type GetDataSourceType()
	{
		return typeof(StiCosmosDbSource);
	}

	public override StiNoSqlDataConnector CreateConnector(string connectionString = null)
	{
		return (StiNoSqlDataConnector)(object)StiCosmosDbConnector.Get(connectionString);
	}

	public override StiDataColumnsCollection GetColumnsFromData(StiData data, StiDataSource dataSource)
	{
		return GetColumnsFromData(data, dataSource, CommandBehavior.SchemaOnly);
	}

	public override StiDataColumnsCollection GetColumnsFromData(StiData data, StiDataSource dataSource, CommandBehavior retrieveMode)
	{
		StiDataColumnsCollection dataColumns = new StiDataColumnsCollection();
		StiCosmosDbSource source = dataSource as StiCosmosDbSource;
		if (!(dataSource.Dictionary.Databases.Cast<StiDatabase>().FirstOrDefault((StiDatabase d) => d.Name.ToLowerInvariant() == source.NameInSource.ToLowerInvariant()) is StiCosmosDbDatabase stiCosmosDbDatabase))
		{
			return dataColumns;
		}
		StiNoSqlDataConnector val = CreateConnector(stiCosmosDbDatabase.ConnectionString);
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
		StiCosmosDbSource cosmosSource = dataSource as StiCosmosDbSource;
		if (!(dictionary.Databases.Cast<StiDatabase>().FirstOrDefault((StiDatabase d) => d is StiCosmosDbDatabase && d.Name.ToLowerInvariant() == cosmosSource.NameInSource.ToLowerInvariant()) is StiCosmosDbDatabase stiCosmosDbDatabase))
		{
			return;
		}
		StiNoSqlDataConnector val = CreateConnector(stiCosmosDbDatabase.ConnectionString);
		string text = cosmosSource.Query;
		StiReport report = dictionary.Report;
		foreach (StiVariable variable in dictionary.Variables)
		{
			string newValue = report.ToString(report[variable.Name]);
			if (variable.Type == typeof(DateTime) && report[variable.Name] is DateTime)
			{
				newValue = ((StiCosmosDbConnector)val).ConvertDateTimeToJsonStr((DateTime)report[variable.Name]);
			}
			string text2 = "Param(" + variable.Name + ")";
			if (text.Contains(text2))
			{
				text = text.Replace(text2, newValue);
			}
		}
		dataSource.DataTable = val.GetDataTable(cosmosSource.Name, text, (int?)null, (int?)null);
	}
}
