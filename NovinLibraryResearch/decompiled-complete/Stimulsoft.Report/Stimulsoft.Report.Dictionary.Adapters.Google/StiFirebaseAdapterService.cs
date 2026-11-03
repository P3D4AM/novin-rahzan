using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Data.Connectors.Google;
using Stimulsoft.Report.Dictionary.DataSources.Google;
using Stimulsoft.Report.Dictionary.Databases.Google;

namespace Stimulsoft.Report.Dictionary.Adapters.Google;

public class StiFirebaseAdapterService : StiNoSqlAdapterService
{
	public override Type GetDataSourceType()
	{
		return typeof(StiFirebaseSource);
	}

	public override StiNoSqlDataConnector CreateConnector(string connectionString = null)
	{
		return (StiNoSqlDataConnector)(object)StiFirebaseConnector.Get(connectionString);
	}

	public override StiDataColumnsCollection GetColumnsFromData(StiData data, StiDataSource dataSource)
	{
		return GetColumnsFromData(data, dataSource, CommandBehavior.SchemaOnly);
	}

	public override StiDataColumnsCollection GetColumnsFromData(StiData data, StiDataSource dataSource, CommandBehavior retrieveMode)
	{
		StiDataColumnsCollection dataColumns = new StiDataColumnsCollection();
		StiFirebaseSource source = dataSource as StiFirebaseSource;
		if (!(dataSource.Dictionary.Databases.Cast<StiDatabase>().FirstOrDefault((StiDatabase d) => d.Name.ToLowerInvariant() == source.NameInSource.ToLowerInvariant()) is StiFirebaseDatabase stiFirebaseDatabase))
		{
			return dataColumns;
		}
		StiNoSqlDataConnector val = CreateConnector(stiFirebaseDatabase.ConnectionString);
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
		StiFirebaseSource firebaase = dataSource as StiFirebaseSource;
		if (dictionary.Databases.Cast<StiDatabase>().FirstOrDefault((StiDatabase d) => d is StiFirebaseDatabase && d.Name.ToLowerInvariant() == firebaase.NameInSource.ToLowerInvariant()) is StiFirebaseDatabase stiFirebaseDatabase)
		{
			StiNoSqlDataConnector val = CreateConnector(stiFirebaseDatabase.ConnectionString);
			dataSource.DataTable = val.GetDataTable(firebaase.Name, "", (int?)null, (int?)null);
		}
	}
}
