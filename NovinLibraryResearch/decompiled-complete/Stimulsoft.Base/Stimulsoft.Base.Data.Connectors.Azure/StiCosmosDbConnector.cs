using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Stimulsoft.Base.Data.Connectors.Azure.Helper;

namespace Stimulsoft.Base.Data.Connectors.Azure;

public class StiCosmosDbConnector : StiDbNoSqlDataConnector
{
	public override StiConnectionIdent ConnectionIdent => StiConnectionIdent.CosmosDbDataSource;

	public override StiConnectionOrder ConnectionOrder => StiConnectionOrder.CosmosDbDataSource;

	public override string Name => "Cosmos DB";

	public override bool AllowTestConnection
	{
		get
		{
			try
			{
				object cosmosDbProvider = GetCosmosDbProvider();
				MethodInfo method = cosmosDbProvider.GetType().GetMethod("AllowTestConnection");
				return (bool)method.Invoke(cosmosDbProvider, null);
			}
			catch
			{
				return false;
			}
		}
	}

	public override string[] NuGetPackages => new string[1] { "Stimulsoft.Data.CosmosDB" };

	public override string NuGetVersion => "2021.4.4";

	public StiCosmosDbApi Api { get; }

	private object GetCosmosDbProvider()
	{
		if (Api == StiCosmosDbApi.SQL)
		{
			return new StiCosmosSqlApiHelper(base.ConnectionString, NuGetVersion).GetCosmosDbProvider();
		}
		return new StiCosmosMongoDbApiHelper(base.ConnectionString, NuGetVersion).GetCosmosDbProvider();
	}

	public override StiTestConnectionResult TestConnection()
	{
		if (Api == StiCosmosDbApi.SQL)
		{
			return new StiCosmosSqlApiHelper(base.ConnectionString, NuGetVersion).TestConnection();
		}
		return new StiCosmosMongoDbApiHelper(base.ConnectionString, NuGetVersion).TestConnection();
	}

	public override StiDataSchema RetrieveSchema(bool allowException = false)
	{
		if (Api == StiCosmosDbApi.SQL)
		{
			return new StiCosmosSqlApiHelper(base.ConnectionString, NuGetVersion).RetrieveSchema();
		}
		return new StiCosmosMongoDbApiHelper(base.ConnectionString, NuGetVersion).RetrieveSchema(allowException);
	}

	public override string GetSampleConnectionString()
	{
		if (Api == StiCosmosDbApi.SQL)
		{
			return StiCosmosSqlApiHelper.GetSampleConnectionString();
		}
		return StiCosmosMongoDbApiHelper.GetSampleConnectionString();
	}

	public override List<StiDataColumnSchema> GetColumns(string collectionName)
	{
		if (Api == StiCosmosDbApi.SQL)
		{
			return new StiCosmosSqlApiHelper(base.ConnectionString, NuGetVersion).GetColumns(collectionName);
		}
		return new StiCosmosMongoDbApiHelper(base.ConnectionString, NuGetVersion).GetColumns(collectionName);
	}

	public override DataTable GetDataTable(string collectionName, string query, int? index = null, int? count = null)
	{
		if (Api == StiCosmosDbApi.SQL)
		{
			return new StiCosmosSqlApiHelper(base.ConnectionString, NuGetVersion).GetDataTable(collectionName, query, index, count);
		}
		return new StiCosmosMongoDbApiHelper(base.ConnectionString, NuGetVersion).GetDataTable(collectionName, query, index, count);
	}

	public string ConvertDateTimeToJsonStr(DateTime date)
	{
		try
		{
			object cosmosDbProvider = GetCosmosDbProvider();
			MethodInfo method = cosmosDbProvider.GetType().GetMethod("ConvertDateTimeToJsonStr");
			return (string)method.Invoke(cosmosDbProvider, new object[1] { date });
		}
		catch
		{
			return null;
		}
	}

	public static StiCosmosDbConnector Get(string connectionString = null)
	{
		Enum.TryParse<StiCosmosDbApi>(StiConnectionStringHelper.GetConnectionStringKey(connectionString, "Api"), ignoreCase: true, out var result);
		return new StiCosmosDbConnector(connectionString, result);
	}

	public StiCosmosDbConnector(string connectionString, StiCosmosDbApi api)
		: base(connectionString)
	{
		base.FolderAssembly = "Stimulsoft.Data.CosmosDB-" + NuGetVersion;
		base.NameAssembly = "Stimulsoft.Data.CosmosDB.dll";
		Api = api;
	}
}
