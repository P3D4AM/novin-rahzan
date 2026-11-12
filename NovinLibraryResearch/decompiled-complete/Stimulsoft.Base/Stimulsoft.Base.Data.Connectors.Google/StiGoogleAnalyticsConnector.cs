using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Stimulsoft.Base.Data.Connectors.Google;

public class StiGoogleAnalyticsConnector : StiDbNoSqlDataConnector
{
	private StiDataAssemblyHelper assemblyHelper;

	private object googleAnalyticsProvider;

	public override StiConnectionIdent ConnectionIdent => StiConnectionIdent.GoogleAnalyticsDataSource;

	public override StiConnectionOrder ConnectionOrder => StiConnectionOrder.GoogleAnalyticsDataSource;

	public override string Name => "Google Analytics";

	public override bool IsAvailable => GetAssemblyData()?.IsAllowed ?? false;

	public override string[] NuGetPackages => new string[1] { "Stimulsoft.Data.GoogleAnalytics" };

	public override string NuGetVersion => "2021.4.3";

	private StiDataAssemblyHelper GetAssemblyData()
	{
		if (assemblyHelper != null)
		{
			return assemblyHelper;
		}
		assemblyHelper = new StiDataAssemblyHelper("Stimulsoft.Data.GoogleAnalytics.dll", "Stimulsoft.Data.GoogleAnalytics-" + NuGetVersion);
		return assemblyHelper;
	}

	private object GetGoogleAnalyticsProvider()
	{
		if (googleAnalyticsProvider != null)
		{
			return googleAnalyticsProvider;
		}
		StiDataAssemblyHelper assemblyData = GetAssemblyData();
		googleAnalyticsProvider = assemblyData.CreateObject("Stimulsoft.Data.GoogleAnalytics.StiGoogleAnalyticsProvider", base.ConnectionString);
		return googleAnalyticsProvider;
	}

	private object GetStiGoogleAnalyticsReportingProviderr()
	{
		if (googleAnalyticsProvider != null)
		{
			return googleAnalyticsProvider;
		}
		StiDataAssemblyHelper assemblyData = GetAssemblyData();
		googleAnalyticsProvider = assemblyData.CreateObject("Stimulsoft.Data.GoogleAnalytics.StiGoogleAnalyticsReportingProvider", base.ConnectionString);
		return googleAnalyticsProvider;
	}

	internal async Task<List<KeyValuePair<string, string>>> GetAccountsAsync()
	{
		return await Task.Run(() => GetAccounts());
	}

	internal List<KeyValuePair<string, string>> GetAccounts()
	{
		object obj = GetGoogleAnalyticsProvider();
		MethodInfo method = obj.GetType().GetMethod("GetAccounts");
		return (List<KeyValuePair<string, string>>)method.Invoke(obj, null);
	}

	internal async Task<List<KeyValuePair<string, string>>> GetPropertiesAsync(string accountId)
	{
		return await Task.Run(() => GetProperties(accountId));
	}

	internal List<KeyValuePair<string, string>> GetProperties(string accountId)
	{
		object obj = GetGoogleAnalyticsProvider();
		MethodInfo method = obj.GetType().GetMethod("GetProperties");
		return (List<KeyValuePair<string, string>>)method.Invoke(obj, new object[1] { accountId });
	}

	internal async Task<List<KeyValuePair<string, string>>> GetViewsAsync(string accountId, string propertyId)
	{
		return await Task.Run(() => GetViews(accountId, propertyId));
	}

	internal List<KeyValuePair<string, string>> GetViews(string accountId, string propertyId)
	{
		object obj = GetGoogleAnalyticsProvider();
		MethodInfo method = obj.GetType().GetMethod("GetViews");
		return (List<KeyValuePair<string, string>>)method.Invoke(obj, new object[2] { accountId, propertyId });
	}

	internal List<KeyValuePair<string, Dictionary<string, string>>> GetMetrics()
	{
		object obj = GetGoogleAnalyticsProvider();
		MethodInfo method = obj.GetType().GetMethod("GetAllMetrics");
		return (List<KeyValuePair<string, Dictionary<string, string>>>)method.Invoke(obj, null);
	}

	internal List<KeyValuePair<string, Dictionary<string, string>>> GetDimensions()
	{
		object obj = GetGoogleAnalyticsProvider();
		MethodInfo method = obj.GetType().GetMethod("GetAllDimensions");
		return (List<KeyValuePair<string, Dictionary<string, string>>>)method.Invoke(obj, null);
	}

	public override StiTestConnectionResult TestConnection()
	{
		try
		{
			object obj = GetGoogleAnalyticsProvider();
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
			object stiGoogleAnalyticsReportingProviderr = GetStiGoogleAnalyticsReportingProviderr();
			MethodInfo method = stiGoogleAnalyticsReportingProviderr.GetType().GetMethod("GetEmptyDatasetOfSameSchema");
			dataSet = method.Invoke(stiGoogleAnalyticsReportingProviderr, null) as DataSet;
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
		stiDataSchema.Tables = stiDataSchema.Tables.OrderBy((StiDataTableSchema e) => e.Name).ToList();
		return stiDataSchema;
	}

	public override string GetSampleConnectionString()
	{
		return "Base64EncodedAuthSecret=<secret>;AccountId=<accountId>;PropertyId=<propertyId>;ViewId=<viewId>;Metrics=<metrics>;Dimensions=<dimensions>";
	}

	public override List<StiDataColumnSchema> GetColumns(string collectionName)
	{
		return RetrieveSchema().Tables.FirstOrDefault((StiDataTableSchema table) => table.Name.ToLowerInvariant() == collectionName.ToLowerInvariant())?.Columns;
	}

	public override DataTable GetDataTable(string collectionName, string query, int? index = null, int? count = null)
	{
		try
		{
			object stiGoogleAnalyticsReportingProviderr = GetStiGoogleAnalyticsReportingProviderr();
			MethodInfo method = stiGoogleAnalyticsReportingProviderr.GetType().GetMethod("GetTable");
			return method.Invoke(stiGoogleAnalyticsReportingProviderr, new object[4] { collectionName, query, index, count }) as DataTable;
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

	public static StiGoogleAnalyticsConnector Get(string connectionString = null)
	{
		return new StiGoogleAnalyticsConnector(connectionString);
	}

	public StiGoogleAnalyticsConnector(string connectionString = null)
		: base(connectionString)
	{
	}
}
