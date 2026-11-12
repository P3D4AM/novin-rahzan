using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Data.Connectors.Azure.Helper;

public class StiCosmosSqlApiHelper
{
	private static StiDataAssemblyHelper assemblyHelper;

	private string nugetVersion;

	private string connectionString;

	public StiDataAssemblyHelper GetAssemblyData()
	{
		if (assemblyHelper != null)
		{
			return assemblyHelper;
		}
		assemblyHelper = new StiDataAssemblyHelper("Stimulsoft.Data.CosmosDB.dll", "Stimulsoft.Data.CosmosDB-" + nugetVersion);
		return assemblyHelper;
	}

	public StiTestConnectionResult TestConnection()
	{
		try
		{
			object cosmosDbProvider = GetCosmosDbProvider();
			MethodInfo method = cosmosDbProvider.GetType().GetMethod("TestConnection");
			return ((bool)method.Invoke(cosmosDbProvider, null)) ? StiTestConnectionResult.MakeFine() : StiTestConnectionResult.MakeWrong("Couldn't open connection to server");
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

	public static string GetSampleConnectionString()
	{
		return "Database=<myDataBase>;AccountEndpoint=<EndpointUrl>;AccountKey=<PrimaryKey>;";
	}

	public object GetCosmosDbProvider()
	{
		return GetAssemblyData().CreateObject("Stimulsoft.Data.CosmosDB.StiCosmosDbSqlApiProvider", connectionString);
	}

	public StiDataSchema RetrieveSchema(bool allowException = false)
	{
		if (string.IsNullOrEmpty(connectionString))
		{
			return null;
		}
		StiDataSchema stiDataSchema = new StiDataSchema(StiConnectionIdent.CosmosDbDataSource);
		try
		{
			object cosmosDbProvider = GetCosmosDbProvider();
			MethodInfo method = cosmosDbProvider.GetType().GetMethod("GetTableNames");
			List<string> list = method.Invoke(cosmosDbProvider, null) as List<string>;
			foreach (string item in list)
			{
				StiDataTableSchema stiDataTableSchema = StiDataTableSchema.NewTable(item);
				try
				{
					List<StiDataColumnSchema> columns = GetColumns(item);
					if (columns != null)
					{
						stiDataTableSchema.Columns = columns;
					}
				}
				catch
				{
				}
				stiDataSchema.Tables.Add(stiDataTableSchema);
			}
			return stiDataSchema.Sort();
		}
		catch (Exception)
		{
			if (allowException)
			{
				throw;
			}
			return null;
		}
	}

	public List<StiDataColumnSchema> GetColumns(string collectionName)
	{
		return (from c in GetDataTable(collectionName, "SELECT TOP 1 * from " + collectionName.Replace("-", "_"), 0, 1)?.Columns.Cast<DataColumn>().ToList()
			select new StiDataColumnSchema(c.ColumnName, c.DataType)).ToList();
	}

	public DataTable GetDataTable(string collectionName, string query, int? index = null, int? count = null)
	{
		try
		{
			object cosmosDbProvider = GetCosmosDbProvider();
			MethodInfo method = cosmosDbProvider.GetType().GetMethod("GetTableJsonQueryData");
			List<string> list = method.Invoke(cosmosDbProvider, new object[4] { collectionName, query, index, count }) as List<string>;
			List<JToken> list2 = new List<JToken>();
			foreach (string item2 in list)
			{
				try
				{
					JToken item = JToken.Parse(item2);
					list2.Add(item);
				}
				catch
				{
				}
			}
			DataSet dataSet = StiJsonToDataSetConverter.GetDataSet(list2, useOneTable: true);
			return (dataSet != null && dataSet.Tables.Count > 0) ? dataSet.Tables[0].Copy() : null;
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

	public StiCosmosSqlApiHelper(string connectionString, string nugetVersion)
	{
		this.nugetVersion = nugetVersion;
		this.connectionString = connectionString.Replace("\r\n", "").Replace(" ", "");
	}
}
