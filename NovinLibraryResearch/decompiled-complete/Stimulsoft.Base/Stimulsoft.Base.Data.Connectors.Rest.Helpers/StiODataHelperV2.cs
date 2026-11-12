using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Net;
using System.Reflection;

namespace Stimulsoft.Base.Data.Connectors.Rest.Helpers;

public class StiODataHelperV2
{
	private StiDataAssemblyHelper assemblyHelper;

	private object odataProvider;

	private object model;

	private string bearerAccessToken;

	private string nuGetVersion;

	public CookieContainer CookieContainer { get; set; }

	public string ConnectionString { get; private set; }

	public string Address
	{
		get
		{
			string text = StiConnectionStringHelper.GetConnectionStringKey(ConnectionString, "Address=") ?? StiConnectionStringHelper.GetConnectionStringKey(ConnectionString);
			return text ?? ConnectionString;
		}
	}

	public string UserName => StiConnectionStringHelper.GetConnectionStringKey(ConnectionString, "UserName");

	public string Password => StiConnectionStringHelper.GetConnectionStringKey(ConnectionString, "Password");

	public string ClientId => StiConnectionStringHelper.GetConnectionStringKey(ConnectionString, "Client_Id");

	public string AddressBearer => StiConnectionStringHelper.GetConnectionStringKey(ConnectionString, "AddressBearer");

	public string Token => StiConnectionStringHelper.GetConnectionStringKey(ConnectionString, "Token");

	public NameValueCollection Headers { get; protected set; }

	[Browsable(false)]
	public bool AllowException { get; set; }

	private StiDataAssemblyHelper GetAssemblyBaseData()
	{
		if (assemblyHelper != null)
		{
			return assemblyHelper;
		}
		assemblyHelper = new StiDataAssemblyHelper("Stimulsoft.Data.OData.dll", "Stimulsoft.Data.OData-" + nuGetVersion);
		return assemblyHelper;
	}

	private object GetODataProvider()
	{
		if (odataProvider != null)
		{
			return odataProvider;
		}
		if (!string.IsNullOrWhiteSpace(Token))
		{
			bearerAccessToken = Token;
		}
		else if (!string.IsNullOrWhiteSpace(AddressBearer) && string.IsNullOrWhiteSpace(bearerAccessToken))
		{
			bearerAccessToken = StiODataHelper.GetBearerAccessToken(AddressBearer, UserName, Password, ClientId);
		}
		string text = (string.IsNullOrWhiteSpace(bearerAccessToken) ? ConnectionString : ("Address=" + Address + ";BearerAccessToken=" + bearerAccessToken));
		StiDataAssemblyHelper assemblyBaseData = GetAssemblyBaseData();
		odataProvider = assemblyBaseData.CreateObject("Stimulsoft.Data.OData.StiODataProvider", text, CookieContainer, Headers, AllowException);
		return odataProvider;
	}

	private object GetEdmModel()
	{
		if (model != null)
		{
			return model;
		}
		object oDataProvider = GetODataProvider();
		MethodInfo method = oDataProvider.GetType().GetMethod("GetEdmModel");
		model = method.Invoke(oDataProvider, null);
		return model;
	}

	private List<string> GetTableNames()
	{
		object edmModel = GetEdmModel();
		object oDataProvider = GetODataProvider();
		MethodInfo method = oDataProvider.GetType().GetMethod("GetTableNames");
		object obj = method.Invoke(oDataProvider, new object[1] { edmModel });
		return obj as List<string>;
	}

	public StiDataSchema RetrieveSchema()
	{
		StiDataSchema stiDataSchema = new StiDataSchema(StiConnectionIdent.ODataDataSource);
		object edmModel = GetEdmModel();
		List<string> tableNames = GetTableNames();
		foreach (string item2 in tableNames)
		{
			StiDataTableSchema stiDataTableSchema = new StiDataTableSchema(item2, item2);
			object oDataProvider = GetODataProvider();
			MethodInfo method = oDataProvider.GetType().GetMethod("GetColumnsInfo");
			List<KeyValuePair<string, string>> list = method.Invoke(oDataProvider, new object[2] { edmModel, item2 }) as List<KeyValuePair<string, string>>;
			foreach (KeyValuePair<string, string> item3 in list)
			{
				Type netType = StiODataHelper.GetNetType(item3.Value);
				StiDataColumnSchema item = new StiDataColumnSchema(item3.Key, netType);
				stiDataTableSchema.Columns.Add(item);
			}
			stiDataSchema.Tables.Add(stiDataTableSchema);
		}
		return stiDataSchema;
	}

	public void FillDataTable(DataTable table, string query)
	{
		try
		{
			Func<object, Type, object> func = StiConvert.ChangeType;
			object oDataProvider = GetODataProvider();
			MethodInfo method = oDataProvider.GetType().GetMethod("FillDataTable");
			method.Invoke(oDataProvider, new object[3] { table, query, func });
		}
		catch
		{
			if (AllowException)
			{
				throw;
			}
		}
	}

	public List<StiDataColumnSchema> GetColumns(string tableName)
	{
		List<StiDataColumnSchema> list = new List<StiDataColumnSchema>();
		object edmModel = GetEdmModel();
		object oDataProvider = GetODataProvider();
		MethodInfo method = oDataProvider.GetType().GetMethod("GetColumnsInfo");
		List<KeyValuePair<string, string>> list2 = method.Invoke(oDataProvider, new object[2] { edmModel, tableName }) as List<KeyValuePair<string, string>>;
		foreach (KeyValuePair<string, string> item2 in list2)
		{
			Type netType = StiODataHelper.GetNetType(item2.Value);
			StiDataColumnSchema item = new StiDataColumnSchema(item2.Key, netType);
			list.Add(item);
		}
		return list;
	}

	public StiODataHelperV2(string connectionString, string nuGetVersion, CookieContainer cookieContainer, NameValueCollection headers = null)
	{
		this.nuGetVersion = nuGetVersion;
		CookieContainer = cookieContainer;
		ConnectionString = connectionString.Replace("\r\n", "").Replace(" ", "");
		Headers = headers ?? new NameValueCollection();
		ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
	}
}
