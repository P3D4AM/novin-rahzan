using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Data.Connectors.Google;
using Stimulsoft.Report.Dictionary.DataSources.Google;
using Stimulsoft.Report.Dictionary.Databases.Google;

namespace Stimulsoft.Report.Dictionary.Adapters.Google;

public class StiGoogleAnalyticsAdapterService : StiNoSqlAdapterService
{
	public override Type GetDataSourceType()
	{
		return typeof(StiGoogleAnalyticsSource);
	}

	public override StiNoSqlDataConnector CreateConnector(string connectionString = null)
	{
		return (StiNoSqlDataConnector)(object)StiGoogleAnalyticsConnector.Get(connectionString);
	}

	public override StiDataColumnsCollection GetColumnsFromData(StiData data, StiDataSource dataSource)
	{
		return GetColumnsFromData(data, dataSource, CommandBehavior.SchemaOnly);
	}

	public override StiDataColumnsCollection GetColumnsFromData(StiData data, StiDataSource dataSource, CommandBehavior retrieveMode)
	{
		StiDataColumnsCollection dataColumns = new StiDataColumnsCollection();
		StiGoogleAnalyticsSource source = dataSource as StiGoogleAnalyticsSource;
		if (!(dataSource.Dictionary.Databases.Cast<StiDatabase>().FirstOrDefault((StiDatabase d) => d.Name.ToLowerInvariant() == source.NameInSource.ToLowerInvariant()) is StiGoogleAnalyticsDatabase stiGoogleAnalyticsDatabase))
		{
			return dataColumns;
		}
		StiNoSqlDataConnector val = CreateConnector(stiGoogleAnalyticsDatabase.ConnectionString);
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
		StiGoogleAnalyticsSource googleAnalyticsSource = dataSource as StiGoogleAnalyticsSource;
		if (dictionary.Databases.Cast<StiDatabase>().FirstOrDefault((StiDatabase d) => d is StiGoogleAnalyticsDatabase && d.Name.ToLowerInvariant() == googleAnalyticsSource.NameInSource.ToLowerInvariant()) is StiGoogleAnalyticsDatabase stiGoogleAnalyticsDatabase)
		{
			StiNoSqlDataConnector val = CreateConnector(stiGoogleAnalyticsDatabase.ConnectionString);
			dataSource.DataTable = val.GetDataTable(googleAnalyticsSource.Name, googleAnalyticsSource.Query, (int?)null, (int?)null);
		}
	}
}
