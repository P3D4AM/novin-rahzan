using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base;

public class StiAzureTableStorageConnector : StiDbNoSqlDataConnector
{
	private StiDataAssemblyHelper assemblyHelper;

	private object azureTableStorageProvider;

	public override StiConnectionIdent ConnectionIdent => StiConnectionIdent.AzureTableStorage;

	public override StiConnectionOrder ConnectionOrder => StiConnectionOrder.AzureTableStorage;

	public override string Name => "Azure Table Storage";

	public override bool AllowTestConnection
	{
		get
		{
			try
			{
				object obj = GetAzureTableStorageProvider();
				MethodInfo method = obj.GetType().GetMethod("AllowTestConnection");
				return (bool)method.Invoke(obj, null);
			}
			catch
			{
				return false;
			}
		}
	}

	public override string[] NuGetPackages => new string[1] { "Stimulsoft.Data.AzureTableStorage" };

	public override string NuGetVersion => "2021.4.3";

	private StiDataAssemblyHelper GetAssemblyData()
	{
		if (assemblyHelper != null)
		{
			return assemblyHelper;
		}
		assemblyHelper = new StiDataAssemblyHelper("Stimulsoft.Data.AzureTableStorage.dll", "Stimulsoft.Data.AzureTableStorage-" + NuGetVersion);
		return assemblyHelper;
	}

	private object GetAzureTableStorageProvider()
	{
		if (azureTableStorageProvider != null)
		{
			return azureTableStorageProvider;
		}
		StiDataAssemblyHelper assemblyData = GetAssemblyData();
		azureTableStorageProvider = assemblyData.CreateObject("Stimulsoft.Data.AzureTableStorage.StiAzureTableStorageProvider", base.ConnectionString);
		return azureTableStorageProvider;
	}

	public override StiTestConnectionResult TestConnection()
	{
		try
		{
			object obj = GetAzureTableStorageProvider();
			MethodInfo method = obj.GetType().GetMethod("TestConnection");
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
		if (string.IsNullOrEmpty(base.ConnectionString))
		{
			return null;
		}
		StiDataSchema stiDataSchema = new StiDataSchema(ConnectionIdent);
		try
		{
			object obj = GetAzureTableStorageProvider();
			MethodInfo method = obj.GetType().GetMethod("GetTableNames");
			List<string> list = method.Invoke(obj, null) as List<string>;
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

	public override string GetSampleConnectionString()
	{
		return "DefaultEndpointsProtocol=https;AccountName=<AccountName>;AccountKey=<AccountKey>;EndpointSuffix=core.windows.net";
	}

	public override List<StiDataColumnSchema> GetColumns(string collectionName)
	{
		return (from c in GetDataTable(collectionName, null, 0, 5)?.Columns.Cast<DataColumn>().ToList()
			select new StiDataColumnSchema(c.ColumnName, c.DataType)).ToList();
	}

	public override DataTable GetDataTable(string collectionName, string query, int? index = null, int? count = null)
	{
		try
		{
			object obj = GetAzureTableStorageProvider();
			MethodInfo method = obj.GetType().GetMethod("GetTableJsonData");
			List<string> list = method.Invoke(obj, new object[3] { collectionName, index, count }) as List<string>;
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
			return (dataSet != null && dataSet.Tables.Count > 0) ? dataSet.Tables[0] : null;
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
			object obj = GetAzureTableStorageProvider();
			MethodInfo method = obj.GetType().GetMethod("ConvertDateTimeToJsonStr");
			return (string)method.Invoke(obj, new object[1] { date });
		}
		catch
		{
			return null;
		}
	}

	public static StiAzureTableStorageConnector Get(string connectionString = null)
	{
		return new StiAzureTableStorageConnector(connectionString);
	}

	public StiAzureTableStorageConnector(string connectionString = null)
		: base(connectionString)
	{
		base.FolderAssembly = "Stimulsoft.Data.AzureTableStorage-" + NuGetVersion;
		base.NameAssembly = "Stimulsoft.Data.AzureTableStorage.dll";
	}
}
