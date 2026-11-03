using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiAzureBlobStorageAdapterService : StiNoSqlAdapterService
{
	public override Type GetDataSourceType()
	{
		return typeof(StiAzureBlobStorageSource);
	}

	public override StiNoSqlDataConnector CreateConnector(string connectionString = null)
	{
		return (StiNoSqlDataConnector)(object)StiAzureBlobStorageConnector.Get(connectionString);
	}

	public override StiDataColumnsCollection GetColumnsFromData(StiData data, StiDataSource dataSource)
	{
		return GetColumnsFromData(data, dataSource, CommandBehavior.SchemaOnly);
	}

	public override StiDataColumnsCollection GetColumnsFromData(StiData data, StiDataSource dataSource, CommandBehavior retrieveMode)
	{
		StiDataColumnsCollection dataColumns = new StiDataColumnsCollection();
		StiAzureBlobStorageSource azureBlobStorageSource = dataSource as StiAzureBlobStorageSource;
		string text = (dataSource.Dictionary.Databases.Cast<StiDatabase>().FirstOrDefault((StiDatabase database) => database is StiAzureBlobStorageDatabase && database.Name.ToLowerInvariant().Equals(azureBlobStorageSource.NameInSource.ToLowerInvariant())) as StiAzureBlobStorageDatabase)?.ConnectionString;
		if (text == null)
		{
			return dataColumns;
		}
		List<StiDataColumnSchema> columns = CreateConnector(text).GetColumns(azureBlobStorageSource.Query);
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
		StiAzureBlobStorageSource azureBlobStorageSource = dataSource as StiAzureBlobStorageSource;
		string text = (dataSource.Dictionary.Databases.Cast<StiDatabase>().FirstOrDefault((StiDatabase database) => database is StiAzureBlobStorageDatabase && database.Name.ToLowerInvariant().Equals(azureBlobStorageSource.GetCategoryName().ToLowerInvariant())) as StiAzureBlobStorageDatabase)?.ConnectionString;
		if (text != null)
		{
			dataSource.DataTable = CreateConnector(text).GetDataTable(azureBlobStorageSource.Query, azureBlobStorageSource.Query, (int?)null, (int?)null);
		}
	}
}
