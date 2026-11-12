using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using Stimulsoft.Base.Data.Connectors;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base;

public class StiQuickBooksConnector : StiRestDataConnector, IDbConnection, IDisposable
{
	private string stimulsoftClientId = "ABPQz3WAKlBBQy07UBdcwkbeAZGG10hu9EpukpQinSUVXOM2iu";

	private string stimulsoftClientSecret = "DAAaTBmTphB0fJfsjdPw2TlzEaWbZyYrRGOA6PMV";

	private string oauth2Url = "https://appcenter.intuit.com/connect/oauth2";

	private string bearerUrl = "https://oauth.platform.intuit.com/oauth2/v1/tokens/bearer";

	private string baseUrl = "https://quickbooks.api.intuit.com/v3";

	private string stimulsoftRedirectUrl = "https://developer.intuit.com/v2/OAuth2Playground/RedirectUrl";

	private string responseType = "code";

	private string scope = "com.intuit.quickbooks.accounting";

	private string state = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 8);

	public override StiConnectionIdent ConnectionIdent => StiConnectionIdent.QuickBooksDataSource;

	public override StiConnectionOrder ConnectionOrder => StiConnectionOrder.QuickBooksDataSource;

	public override string Name => "QuickBooks";

	public override bool IsAvailable => true;

	public bool UseApp
	{
		get
		{
			if (bool.TryParse(StiConnectionStringHelper.GetConnectionStringKey(base.ConnectionString, "UseApp"), out var result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.ConnectionString = StiConnectionStringHelper.SetConnectionStringKey(base.ConnectionString, "UseApp", value.ToString());
		}
	}

	public string ClientId
	{
		get
		{
			return StiConnectionStringHelper.GetConnectionStringKey(base.ConnectionString, "ClientId");
		}
		set
		{
			base.ConnectionString = StiConnectionStringHelper.SetConnectionStringKey(base.ConnectionString, "ClientId", value);
		}
	}

	public string ClientIdPrivate
	{
		get
		{
			if (UseApp)
			{
				return ClientId;
			}
			return stimulsoftClientId;
		}
		set
		{
			ClientId = value;
		}
	}

	public string ClientSecret
	{
		get
		{
			return StiConnectionStringHelper.GetConnectionStringKey(base.ConnectionString, "ClientSecret");
		}
		set
		{
			base.ConnectionString = StiConnectionStringHelper.SetConnectionStringKey(base.ConnectionString, "ClientSecret", value);
		}
	}

	private string ClientSecretPrivate
	{
		get
		{
			if (UseApp)
			{
				return ClientSecret;
			}
			return stimulsoftClientSecret;
		}
		set
		{
			ClientSecret = value;
		}
	}

	public string RedirectURL
	{
		get
		{
			return StiConnectionStringHelper.GetConnectionStringKey(base.ConnectionString, "RedirectURL");
		}
		set
		{
			base.ConnectionString = StiConnectionStringHelper.SetConnectionStringKey(base.ConnectionString, "RedirectURL", value);
		}
	}

	private string RedirectURLPrivate
	{
		get
		{
			if (UseApp)
			{
				return RedirectURL;
			}
			return stimulsoftRedirectUrl;
		}
		set
		{
			RedirectURL = value;
		}
	}

	public string AuthorizationCode
	{
		get
		{
			return StiConnectionStringHelper.GetConnectionStringKey(base.ConnectionString, "AuthorizationCode");
		}
		set
		{
			base.ConnectionString = StiConnectionStringHelper.SetConnectionStringKey(base.ConnectionString, "AuthorizationCode", value);
		}
	}

	public string RealmId
	{
		get
		{
			return StiConnectionStringHelper.GetConnectionStringKey(base.ConnectionString, "RealmId");
		}
		set
		{
			base.ConnectionString = StiConnectionStringHelper.SetConnectionStringKey(base.ConnectionString, "RealmId", value);
		}
	}

	public string AccessToken
	{
		get
		{
			return StiConnectionStringHelper.GetConnectionStringKey(base.ConnectionString, "AccessToken");
		}
		set
		{
			base.ConnectionString = StiConnectionStringHelper.SetConnectionStringKey(base.ConnectionString, "AccessToken", value);
		}
	}

	public string RefreshToken
	{
		get
		{
			return StiConnectionStringHelper.GetConnectionStringKey(base.ConnectionString, "RefreshToken");
		}
		set
		{
			base.ConnectionString = StiConnectionStringHelper.SetConnectionStringKey(base.ConnectionString, "RefreshToken", value);
		}
	}

	public int ConnectionTimeout => 30;

	public string Database
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public ConnectionState State => ConnectionState.Closed;

	public void FillAuthorizationCode()
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Invalid comparison between Unknown and I4
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		StiDataAssemblyHelper stiDataAssemblyHelper = new StiDataAssemblyHelper("Stimulsoft.Report.Win.dll");
		IStiQuickBooksOAuthForm stiQuickBooksOAuthForm = stiDataAssemblyHelper.CreateObject("Stimulsoft.Report.Win.Dictionary.StiQuickBooksOAuthForm", GetAuthorizationUrl(), RedirectURLPrivate) as IStiQuickBooksOAuthForm;
		if ((int)stiQuickBooksOAuthForm.ShowDialog() == 1)
		{
			RealmId = stiQuickBooksOAuthForm.RealmId;
			AuthorizationCode = stiQuickBooksOAuthForm.AuthorizationCode;
		}
		else if (!string.IsNullOrEmpty(stiQuickBooksOAuthForm.Error))
		{
			MessageBox.Show(stiQuickBooksOAuthForm.Error, "Error", (MessageBoxButtons)0, (MessageBoxIcon)48);
		}
	}

	private WebClient GetDefaultWebClient()
	{
		WebClient webClient = new WebClient();
		webClient.Proxy = StiProxy.GetProxy();
		webClient.Encoding = StiBaseOptions.WebClientEncoding;
		webClient.Headers["Authorization"] = "Bearer " + AccessToken;
		webClient.Headers["Accept"] = "application/json";
		ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
		return webClient;
	}

	private string GetAuthorizationUrl()
	{
		string text = oauth2Url + "?client_id=" + ClientIdPrivate + "&response_type=" + responseType + "&scope=" + scope + "&redirect_uri=" + RedirectURLPrivate + "&state=" + state;
		if (!string.IsNullOrWhiteSpace(RealmId))
		{
			text = text + "&realm_id=" + RealmId;
		}
		return text;
	}

	public void FillTokens()
	{
		WebClient defaultWebClient = GetDefaultWebClient();
		defaultWebClient.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(ClientIdPrivate + ":" + ClientSecretPrivate));
		defaultWebClient.Headers["Content-Type"] = "application/x-www-form-urlencoded";
		string json = defaultWebClient.UploadString(bearerUrl, "POST", "code=" + AuthorizationCode + "&redirect_uri=" + RedirectURLPrivate + "&grant_type=authorization_code");
		JObject jObject = JObject.Parse(json);
		AccessToken = jObject["access_token"].ToString();
		RefreshToken = jObject["refresh_token"].ToString();
	}

	public void RefreshAccessToken()
	{
		WebClient defaultWebClient = GetDefaultWebClient();
		defaultWebClient.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(ClientIdPrivate + ":" + ClientSecretPrivate));
		defaultWebClient.Headers["Content-Type"] = "application/x-www-form-urlencoded";
		string json = defaultWebClient.UploadString(bearerUrl, "POST", "refresh_token=" + RefreshToken + "&grant_type=refresh_token");
		JObject jObject = JObject.Parse(json);
		AccessToken = jObject["access_token"].ToString();
		RefreshToken = jObject["refresh_token"].ToString();
	}

	private List<string> GetTableNames()
	{
		return new List<string>
		{
			"Account", "AccountListDetail", "APAgingDetail", "APAgingSummary", "ARAgingDetail", "ARAgingSummary", "Attachable", "BalanceSheet", "Batch", "Bill",
			"BillPayment", "Budget", "CashFlow", "ChangeDataCapture", "Class", "CompanyCurrency", "CompanyInfo", "CreditMemo", "Customer", "CustomerBalance",
			"CustomerBalanceDetail", "CustomerIncome", "CustomerType", "Department", "Deposit", "Employee", "Entitlements", "Estimate", "Exchangerate", "GeneralLedger",
			"InventoryValuationSummary", "Invoice", "Item", "JournalCode", "JournalEntry", "JournalReport", "JournalReportFR", "Payment", "PaymentMethod", "Preferences",
			"ProfitAndLoss", "ProfitAndLossDetail", "Purchase", "PurchaseOrder", "RefundReceipt", "SalesByClassSummary", "SalesByCustomer", "SalesByDepartment", "SalesByProduct", "SalesReceipt",
			"TaxClassification", "TaxCode", "TaxRate", "TaxService", "TaxSummary", "TaxAgency", "Term", "TimeActivity", "TransactionList", "Transfer",
			"TrialBalance", "Vendor", "VendorBalance", "VendorBalanceDetail", "VendorCredit", "VendorExpenses"
		};
	}

	private List<StiDataColumnSchema> GetColumns(string collectionName)
	{
		return (from c in new DataTable(collectionName)?.Columns.Cast<DataColumn>().ToList()
			select new StiDataColumnSchema(c.ColumnName, c.DataType)).ToList();
	}

	public override StiDataSchema RetrieveSchema(bool allowException = false)
	{
		StiDataSchema stiDataSchema = new StiDataSchema(StiConnectionIdent.QuickBooksDataSource);
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

	public DataTable GetDataTable(string collectionName, string query)
	{
		try
		{
			if (string.IsNullOrEmpty(query))
			{
				query = Uri.EscapeDataString("SELECT * FROM " + collectionName);
			}
			string text = ExecuteQuery(query);
			DataSet dataSet = StiJsonToDataSetConverterV2.GetDataSet(text);
			return (dataSet != null && dataSet.Tables.Count > 0) ? dataSet.Tables[0].Copy() : new DataTable();
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

	public override void FillDataTable(DataTable table, string query)
	{
		DataTable dataTable = GetDataTable(null, query);
		foreach (DataRow row in dataTable.Rows)
		{
			DataRow dataRow2 = table.NewRow();
			foreach (DataColumn column in table.Columns)
			{
				if (dataTable.Columns.Contains(column.ColumnName) && dataTable.Columns[column.ColumnName].DataType == column.DataType)
				{
					dataRow2[column] = row[dataTable.Columns[column.ColumnName]];
				}
			}
			table.Rows.Add(dataRow2);
		}
	}

	private string ExecuteQuery(string query)
	{
		try
		{
			if (string.IsNullOrWhiteSpace(RealmId))
			{
				FillAuthorizationCode();
			}
			if (string.IsNullOrWhiteSpace(AccessToken))
			{
				if (!string.IsNullOrWhiteSpace(RefreshToken))
				{
					RefreshAccessToken();
				}
				else if (string.IsNullOrWhiteSpace(AuthorizationCode))
				{
					FillAuthorizationCode();
					FillTokens();
				}
				else
				{
					FillTokens();
				}
			}
			WebClient defaultWebClient = GetDefaultWebClient();
			string address = baseUrl + "/company/" + RealmId + "/query?query=" + Uri.EscapeDataString(query);
			string json = defaultWebClient.DownloadString(address);
			JToken first = JObject.Parse(json)["QueryResponse"].First;
			if (first != null)
			{
				RemoveUnsupportedColumns(first.First);
				CorrectRefColumns(first.First);
				return "{" + first.ToString() + "}";
			}
			return "{}";
		}
		catch (WebException ex)
		{
			Stream responseStream = ex.Response.GetResponseStream();
			StreamReader streamReader = new StreamReader(responseStream);
			string json2 = streamReader.ReadToEnd();
			JObject jObject = JObject.Parse(json2);
			if (jObject["fault"]?["error"]?[0]?["code"]?.Value<string>() == "3200")
			{
				AccessToken = "";
				return ExecuteQuery(query);
			}
			if (jObject["fault"]?["error"]?[0]?["code"]?.Value<string>() == "3100")
			{
				RealmId = "";
				RefreshToken = "";
				AccessToken = "";
				return ExecuteQuery(query);
			}
			if (jObject["error"]?.Value<string>() == "invalid_grant")
			{
				RefreshToken = "";
				AccessToken = "";
				AuthorizationCode = "";
				return ExecuteQuery(query);
			}
		}
		catch
		{
		}
		return "{}";
	}

	private void RemoveUnsupportedColumns(JToken json)
	{
		List<JProperty> list = new List<JProperty>();
		foreach (JToken item in (IEnumerable<JToken>)json)
		{
			foreach (JProperty item2 in (IEnumerable<JToken>)item)
			{
				if (char.IsLower(item2.Name[0]))
				{
					list.Add(item2);
				}
				if (!(item2.Value is JValue) && item2.Name.LastIndexOf("Ref") != item2.Name.Length - 3)
				{
					list.Add(item2);
				}
			}
		}
		foreach (JProperty item3 in list)
		{
			item3.Remove();
		}
	}

	private void CorrectRefColumns(JToken json)
	{
		foreach (JToken item in (IEnumerable<JToken>)json)
		{
			foreach (JProperty item2 in (IEnumerable<JToken>)item)
			{
				if (!(item2.Value is JValue) && item2.Name.LastIndexOf("Ref") == item2.Name.Length - 3)
				{
					item2.Value = item2.Value["value"];
				}
			}
		}
	}

	public override string GetSampleConnectionString()
	{
		return "";
	}

	public static StiQuickBooksConnector Get(string connectionString = null)
	{
		return new StiQuickBooksConnector(connectionString);
	}

	public IDbTransaction BeginTransaction()
	{
		throw new NotImplementedException();
	}

	public IDbTransaction BeginTransaction(IsolationLevel il)
	{
		throw new NotImplementedException();
	}

	public void Close()
	{
		throw new NotImplementedException();
	}

	public void ChangeDatabase(string databaseName)
	{
		throw new NotImplementedException();
	}

	public IDbCommand CreateCommand()
	{
		throw new NotImplementedException();
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public void Open()
	{
		throw new NotImplementedException();
	}

	public StiQuickBooksConnector(string connectionString = "")
		: base(connectionString)
	{
	}
}
