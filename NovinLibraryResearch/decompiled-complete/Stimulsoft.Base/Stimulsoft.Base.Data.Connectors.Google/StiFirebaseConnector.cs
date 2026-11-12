using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Stimulsoft.Base.Data.Connectors.Google;

public class StiFirebaseConnector : StiDbNoSqlDataConnector
{
	private StiDataAssemblyHelper assemblyHelper;

	private object firebaseProvider;

	public string AuthSecret { get; set; }

	public string BasePath { get; set; }

	public override StiConnectionIdent ConnectionIdent => StiConnectionIdent.FirebirdDataSource;

	public override StiConnectionOrder ConnectionOrder => StiConnectionOrder.FirebirdDataSource;

	public override string Name => "Firebase";

	public override bool IsAvailable => GetAssemblyData()?.IsAllowed ?? false;

	public override string[] NuGetPackages => new string[1] { "Stimulsoft.Data.Firebase" };

	public override string NuGetVersion => "2021.4.3";

	private StiDataAssemblyHelper GetAssemblyData()
	{
		if (assemblyHelper != null)
		{
			return assemblyHelper;
		}
		assemblyHelper = new StiDataAssemblyHelper("Stimulsoft.Data.Firebase.dll", "Stimulsoft.Data.Firebase-" + NuGetVersion);
		return assemblyHelper;
	}

	private object GetFirebaseProvider()
	{
		if (firebaseProvider != null)
		{
			return firebaseProvider;
		}
		StiDataAssemblyHelper assemblyData = GetAssemblyData();
		firebaseProvider = assemblyData.CreateObject("Stimulsoft.Data.Firebase.StiFirebaseProvider", base.ConnectionString);
		return firebaseProvider;
	}

	public override string GetSampleConnectionString()
	{
		return "AuthSecret=<secret>;BasePath=<path>";
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
			object obj = GetFirebaseProvider();
			MethodInfo method = obj.GetType().GetMethod("GetJsonData");
			string text = method.Invoke(obj, new object[3] { collectionName, index, count }) as string;
			DataSet dataSet = ((StiBaseOptions.DefaultJsonConverterVersion == StiJsonConverterVersion.ConverterV2) ? StiJsonToDataSetConverterV2.GetDataSet(text) : StiJsonToDataSetConverter.GetDataSet(text));
			if (dataSet != null && dataSet.Tables.Contains(collectionName))
			{
				return dataSet.Tables[collectionName].Copy();
			}
			return null;
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

	public static StiFirebaseConnector Get(string connectionString = null)
	{
		return new StiFirebaseConnector(connectionString);
	}

	public override StiTestConnectionResult TestConnection()
	{
		try
		{
			object obj = GetFirebaseProvider();
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

	public DataSet GetDataSet()
	{
		object obj = GetFirebaseProvider();
		MethodInfo method = obj.GetType().GetMethod("GetJsonData");
		string text = method.Invoke(obj, new object[3] { "", 0, 0 }) as string;
		if (StiBaseOptions.DefaultJsonConverterVersion != StiJsonConverterVersion.ConverterV2)
		{
			return StiJsonToDataSetConverter.GetDataSet(text);
		}
		return StiJsonToDataSetConverterV2.GetDataSet(text);
	}

	public override StiDataSchema RetrieveSchema(bool allowException = false)
	{
		DataSet dataSet = GetDataSet();
		if (dataSet == null)
		{
			return null;
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

	public StiFirebaseConnector(string connectionString = null)
		: base(connectionString)
	{
	}
}
