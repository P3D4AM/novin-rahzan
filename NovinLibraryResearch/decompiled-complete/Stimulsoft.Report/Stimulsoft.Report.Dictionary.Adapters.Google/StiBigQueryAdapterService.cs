using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Data.Connectors.Google;
using Stimulsoft.Report.Dictionary.DataSources.Google;
using Stimulsoft.Report.Dictionary.Databases.Google;

namespace Stimulsoft.Report.Dictionary.Adapters.Google;

public class StiBigQueryAdapterService : StiNoSqlAdapterService
{
	public override Type GetDataSourceType()
	{
		return typeof(StiBigQuerySource);
	}

	public override StiNoSqlDataConnector CreateConnector(string connectionString = null)
	{
		return (StiNoSqlDataConnector)(object)StiBigQueryConnector.Get(connectionString);
	}

	public override StiDataColumnsCollection GetColumnsFromData(StiData data, StiDataSource dataSource)
	{
		return GetColumnsFromData(data, dataSource, CommandBehavior.SchemaOnly);
	}

	public override StiDataColumnsCollection GetColumnsFromData(StiData data, StiDataSource dataSource, CommandBehavior retrieveMode)
	{
		StiDataColumnsCollection dataColumns = new StiDataColumnsCollection();
		StiBigQuerySource source = dataSource as StiBigQuerySource;
		if (!(dataSource.Dictionary.Databases.Cast<StiDatabase>().FirstOrDefault((StiDatabase d) => d.Name.ToLowerInvariant() == source.NameInSource.ToLowerInvariant()) is StiBigQueryDatabase stiBigQueryDatabase))
		{
			return dataColumns;
		}
		StiNoSqlDataConnector val = CreateConnector(stiBigQueryDatabase.ConnectionString);
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
		StiDataLeader.Disconnect(dataSource);
		if (!loadData)
		{
			dataSource.DataTable = new DataTable();
			return;
		}
		StiBigQuerySource bigQuerySource = dataSource as StiBigQuerySource;
		if (dictionary.Databases.Cast<StiDatabase>().FirstOrDefault((StiDatabase d) => d is StiBigQueryDatabase && d.Name.ToLowerInvariant() == bigQuerySource.NameInSource.ToLowerInvariant()) is StiBigQueryDatabase stiBigQueryDatabase)
		{
			StiNoSqlDataConnector val = CreateConnector(stiBigQueryDatabase.ConnectionString);
			dataSource.DataTable = val.GetDataTable(bigQuerySource.Name, bigQuerySource.Query, (int?)null, (int?)null);
		}
	}
}
