using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using Stimulsoft.Base.Json;

namespace Stimulsoft.Base.Data.Connectors.OnlineServices.Helpers;

public class StiDataWorldHelper
{
	private string urlBase = "https://api.data.world/v0/";

	public string ConnectionString { get; }

	public string Owner => StiConnectionStringHelper.GetConnectionStringKey(ConnectionString, "Owner");

	public string Token => StiConnectionStringHelper.GetConnectionStringKey(ConnectionString, "Token");

	public string Database => StiConnectionStringHelper.GetConnectionStringKey(ConnectionString, "Database");

	private WebClient GetDefaultWebClient()
	{
		WebClient webClient = new WebClient();
		webClient.Proxy = StiProxy.GetProxy();
		webClient.Encoding = StiBaseOptions.WebClientEncoding;
		webClient.Headers["Authorization"] = "Bearer " + Token;
		ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
		return webClient;
	}

	public List<string> GetTableNames()
	{
		using WebClient webClient = GetDefaultWebClient();
		string text = Uri.EscapeDataString("SELECT * FROM Tables");
		string text2 = "sql/" + Owner.ToLower() + "/" + Database.ToLower() + "?query=" + text;
		Uri address = new Uri(urlBase + text2);
		string value = webClient.DownloadString(address);
		List<StiDataWorldMetadataTable> source = JsonConvert.DeserializeObject<List<StiDataWorldMetadataTable>>(value);
		return (from t in source
			where t.Dataset.ToLower() == Database.ToLower()
			select t.TableId).ToList();
	}

	public List<StiDataColumnSchema> GetColumns(string collectionName)
	{
		return (from c in GetDataTable(collectionName, "select * from " + collectionName + " limit 29")?.Columns.Cast<DataColumn>().ToList()
			select new StiDataColumnSchema(c.ColumnName, c.DataType)).ToList();
	}

	public DataTable GetDataTable(string collectionName, string query)
	{
		try
		{
			using WebClient webClient = GetDefaultWebClient();
			string text = "sql/" + Owner.ToLower() + "/" + Database.ToLower();
			if (string.IsNullOrEmpty(query))
			{
				query = Uri.EscapeDataString("select * from " + collectionName);
			}
			Uri address = new Uri(urlBase + text + "?query=" + query);
			string text2 = webClient.DownloadString(address);
			DataSet dataSet = ((StiBaseOptions.DefaultJsonConverterVersion == StiJsonConverterVersion.ConverterV2) ? StiJsonToDataSetConverterV2.GetDataSet(text2) : StiJsonToDataSetConverter.GetDataSet(text2));
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

	internal StiTestConnectionResult TestConnection()
	{
		try
		{
			using WebClient webClient = GetDefaultWebClient();
			Uri address = new Uri(urlBase + "users/" + Owner.ToLower());
			string text = webClient.DownloadString(address);
			return StiTestConnectionResult.MakeFine();
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

	public StiDataSchema RetrieveSchema(bool allowException = false)
	{
		if (string.IsNullOrEmpty(ConnectionString))
		{
			return null;
		}
		StiDataSchema stiDataSchema = new StiDataSchema(StiConnectionIdent.DataWorldDataSource);
		try
		{
			List<string> tableNames = GetTableNames();
			foreach (string item in tableNames)
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

	public StiDataWorldHelper(string connectionString)
	{
		ConnectionString = connectionString;
	}
}
