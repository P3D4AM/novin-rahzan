using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Stimulsoft.Base;

public class StiMongoDbConnector : StiDbNoSqlDataConnector
{
	private StiDataAssemblyHelper assemblyHelper;

	private object mongoProvider;

	public override StiConnectionIdent ConnectionIdent => StiConnectionIdent.MongoDbDataSource;

	public override StiConnectionOrder ConnectionOrder => StiConnectionOrder.MongoDbDataSource;

	public override string Name => "MongoDB";

	public override bool AllowTestConnection
	{
		get
		{
			try
			{
				object mongoDBProvider = GetMongoDBProvider();
				MethodInfo method = mongoDBProvider.GetType().GetMethod("AllowTestConnection");
				return (bool)method.Invoke(mongoDBProvider, null);
			}
			catch
			{
				return false;
			}
		}
	}

	public override string[] NuGetPackages => new string[1] { "Stimulsoft.Data.MongoDB" };

	public override string NuGetVersion => "2021.4.4";

	private StiDataAssemblyHelper GetAssemblyData()
	{
		if (assemblyHelper != null)
		{
			return assemblyHelper;
		}
		assemblyHelper = new StiDataAssemblyHelper("Stimulsoft.Data.MongoDB.dll", "Stimulsoft.Data.MongoDB-" + NuGetVersion);
		return assemblyHelper;
	}

	private object GetMongoDBProvider()
	{
		if (mongoProvider != null)
		{
			return mongoProvider;
		}
		StiDataAssemblyHelper assemblyData = GetAssemblyData();
		mongoProvider = assemblyData.CreateObject("Stimulsoft.Data.MongoDB.StiMongoDbProvider", base.ConnectionString);
		return mongoProvider;
	}

	public override StiTestConnectionResult TestConnection()
	{
		try
		{
			object mongoDBProvider = GetMongoDBProvider();
			MethodInfo method = mongoDBProvider.GetType().GetMethod("TestConnection");
			return ((bool)method.Invoke(mongoDBProvider, null)) ? StiTestConnectionResult.MakeFine() : StiTestConnectionResult.MakeWrong("Couldn't open connection to server");
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
		if (string.IsNullOrEmpty(base.ConnectionString))
		{
			return null;
		}
		StiDataSchema stiDataSchema = new StiDataSchema(ConnectionIdent);
		try
		{
			object mongoDBProvider = GetMongoDBProvider();
			MethodInfo method = mongoDBProvider.GetType().GetMethod("GetTableNames");
			List<string> list = method.Invoke(mongoDBProvider, null) as List<string>;
			foreach (string item in list)
			{
				StiDataTableSchema stiDataTableSchema = StiDataTableSchema.NewTable(item);
				try
				{
					switch (item)
					{
					case "system.users":
						continue;
					case "system.profile":
						continue;
					}
					List<StiDataColumnSchema> columns = GetColumns(item);
					if (columns != null)
					{
						stiDataTableSchema.Columns = columns;
					}
					goto IL_00ab;
				}
				catch
				{
					goto IL_00ab;
				}
				IL_00ab:
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

	public override string GetSampleConnectionString()
	{
		return "mongodb://<user>:<password>@localhost/test";
	}

	public override List<StiDataColumnSchema> GetColumns(string collectionName)
	{
		return (from c in GetDataTable(collectionName, null, 0, 7)?.Columns.Cast<DataColumn>().ToList()
			select new StiDataColumnSchema(c.ColumnName, c.DataType)).ToList();
	}

	public override DataTable GetDataTable(string collectionName, string query, int? index = null, int? count = null)
	{
		try
		{
			object mongoDBProvider = GetMongoDBProvider();
			MethodInfo method = mongoDBProvider.GetType().GetMethod("GetTableQueryData");
			return method.Invoke(mongoDBProvider, new object[4] { collectionName, query, index, count }) as DataTable;
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

	public string ConvertDateTimeToJsonStr(DateTime date)
	{
		try
		{
			object mongoDBProvider = GetMongoDBProvider();
			MethodInfo method = mongoDBProvider.GetType().GetMethod("ConvertDateTimeToJsonStr");
			object obj = method.Invoke(mongoDBProvider, new object[1] { date });
			return (string)obj;
		}
		catch
		{
			return null;
		}
	}

	public static StiMongoDbConnector Get(string connectionString = null)
	{
		return new StiMongoDbConnector(connectionString);
	}

	public StiMongoDbConnector(string connectionString = null)
		: base(connectionString)
	{
		base.FolderAssembly = "Stimulsoft.Data.MongoDB-" + NuGetVersion;
		base.NameAssembly = "Stimulsoft.Data.MongoDB.dll";
	}
}
