using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Stimulsoft.Base.Data.Connectors.Google;

public class StiBigQueryConnector : StiDbNoSqlDataConnector
{
	private StiDataAssemblyHelper assemblyHelper;

	private object bigQueryProvider;

	public override StiConnectionIdent ConnectionIdent => StiConnectionIdent.BigQueryDataSource;

	public override StiConnectionOrder ConnectionOrder => StiConnectionOrder.BigQueryDataSource;

	public override string Name => "BigQuery";

	public override bool IsAvailable => GetAssemblyData()?.IsAllowed ?? false;

	public override string[] NuGetPackages => new string[1] { "Stimulsoft.Data.BigQuery" };

	public override string NuGetVersion => "2021.4.3";

	private StiDataAssemblyHelper GetAssemblyData()
	{
		if (assemblyHelper != null)
		{
			return assemblyHelper;
		}
		assemblyHelper = new StiDataAssemblyHelper("Stimulsoft.Data.BigQuery.dll", "Stimulsoft.Data.BigQuery-" + NuGetVersion);
		return assemblyHelper;
	}

	private object GetBigQueryProvider()
	{
		if (bigQueryProvider != null)
		{
			return bigQueryProvider;
		}
		StiDataAssemblyHelper assemblyData = GetAssemblyData();
		bigQueryProvider = assemblyData.CreateObject("Stimulsoft.Data.BigQuery.StiBigQueryProvider", base.ConnectionString);
		return bigQueryProvider;
	}

	public override StiTestConnectionResult TestConnection()
	{
		try
		{
			object obj = GetBigQueryProvider();
			MethodInfo method = obj.GetType().GetMethod("IsConnectionAvailable");
			return ((bool)method.Invoke(obj, null)) ? StiTestConnectionResult.MakeFine() : StiTestConnectionResult.MakeWrong("Couldn't open connection to server");
		}
		catch (Exception ex)
		{
			if (ex.InnerException != null)
			{
				return StiTestConnectionResult.MakeWrong(ex.InnerException.Message);
			}
			return StiTestConnectionResult.MakeWrong(ex.Message);
		}
	}

	public override StiDataSchema RetrieveSchema(bool allowException = false)
	{
		DataSet dataSet;
		try
		{
			object obj = GetBigQueryProvider();
			MethodInfo method = obj.GetType().GetMethod("GetEmptyDatasetOfSameSchema");
			dataSet = method.Invoke(obj, null) as DataSet;
		}
		catch (Exception ex)
		{
			if (ex.InnerException != null)
			{
				throw ex.InnerException;
			}
			throw;
		}
		StiDataSchema stiDataSchema = new StiDataSchema(ConnectionIdent);
		foreach (DataTable table in dataSet.Tables)
		{
			StiDataTableSchema stiDataTableSchema = StiDataTableSchema.NewTable(table.TableName);
			foreach (DataColumn column in table.Columns)
			{
				stiDataTableSchema.Columns.Add(new StiDataColumnSchema
				{
					Name = column.ColumnName,
					Type = column.DataType
				});
			}
			stiDataSchema.Tables.Add(stiDataTableSchema);
		}
		foreach (DataRelation relation in dataSet.Relations)
		{
			stiDataSchema.Relations.Add(new StiDataRelationSchema
			{
				Name = relation.RelationName,
				ParentSourceName = relation.ParentTable.TableName,
				ChildSourceName = relation.ChildTable.TableName,
				ParentColumns = relation.ParentColumns.Select((DataColumn col) => col.ColumnName).ToList(),
				ChildColumns = relation.ChildColumns.Select((DataColumn col) => col.ColumnName).ToList()
			});
		}
		stiDataSchema.Relations = stiDataSchema.Relations.OrderBy((StiDataRelationSchema e) => e.Name).ToList();
		stiDataSchema.Tables = stiDataSchema.Tables.OrderBy((StiDataTableSchema e) => e.Name).ToList();
		return stiDataSchema;
	}

	public override string GetSampleConnectionString()
	{
		return "Base64EncodedAuthSecret=<secret>;ProjectId=<projectID>;DatasetId=<datasetID>";
	}

	public override List<StiDataColumnSchema> GetColumns(string collectionName)
	{
		return RetrieveSchema().Tables.FirstOrDefault((StiDataTableSchema table) => table.Name.ToLowerInvariant() == collectionName.ToLowerInvariant())?.Columns;
	}

	public override DataTable GetDataTable(string collectionName, string query, int? index = null, int? count = null)
	{
		try
		{
			object obj = GetBigQueryProvider();
			MethodInfo method = obj.GetType().GetMethod("QueryOneTable");
			return method.Invoke(obj, new object[4] { collectionName, query, index, count }) as DataTable;
		}
		catch (Exception ex)
		{
			if (ex.InnerException != null)
			{
				throw ex.InnerException;
			}
			throw;
		}
	}

	public static StiBigQueryConnector Get(string connectionString = null)
	{
		return new StiBigQueryConnector(connectionString);
	}

	public StiBigQueryConnector(string connectionString = null)
		: base(connectionString)
	{
	}
}
