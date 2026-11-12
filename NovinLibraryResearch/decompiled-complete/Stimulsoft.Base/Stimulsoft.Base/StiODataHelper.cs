using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using Stimulsoft.Base.Data.Connectors;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base;

public class StiODataHelper
{
	private string bearerAccessToken;

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

	public StiDataSchema RetrieveSchema()
	{
		if (string.IsNullOrEmpty(ConnectionString))
		{
			return null;
		}
		StiDataSchema stiDataSchema = new StiDataSchema(StiConnectionIdent.ODataDataSource);
		try
		{
			using (WebClient webClient = GetDefaultWebClient())
			{
				string s = webClient.DownloadString(StiUrl.Combine(Address, "$metadata"));
				XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
				xmlReaderSettings.DtdProcessing = DtdProcessing.Prohibit;
				using StringReader input = new StringReader(s);
				using XmlReader xmlReader = XmlReader.Create(input, xmlReaderSettings);
				XElement val = XElement.Load(xmlReader);
				XNamespace namespaceOfPrefix = val.GetNamespaceOfPrefix("edmx");
				XNamespace defaultNamespace = ((XContainer)((XContainer)val).Element(namespaceOfPrefix + "DataServices")).Elements().First().GetDefaultNamespace();
				XElement val2 = ((XContainer)((XContainer)val).Element(namespaceOfPrefix + "DataServices")).Elements().FirstOrDefault();
				string text = ((val2.Attribute(XName.op_Implicit("Namespace")) != null) ? ((string)val2.Attribute(XName.op_Implicit("Namespace"))) : null);
				Hashtable hashtable = new Hashtable();
				Hashtable hashtable2 = new Hashtable();
				foreach (XElement item2 in from e in ((XContainer)val2).Elements()
					where e.Name.LocalName == "EntityType" || e.Name.LocalName == "ComplexType"
					select e)
				{
					try
					{
						string text2 = (string)item2.Attribute(XName.op_Implicit("Name"));
						string text3 = ((item2.Attribute(XName.op_Implicit("BaseType")) != null) ? ((string)item2.Attribute(XName.op_Implicit("BaseType"))) : null);
						if (string.IsNullOrWhiteSpace(text2))
						{
							continue;
						}
						IEnumerable<XElement> enumerable = ((XContainer)item2).Elements(defaultNamespace + "Property");
						StiDataTableSchema stiDataTableSchema = new StiDataTableSchema(text2, text2);
						if (text3 != null)
						{
							string key = text3.Replace(text + ".", "");
							hashtable2[key] = stiDataTableSchema;
						}
						foreach (XElement item3 in enumerable)
						{
							try
							{
								string text4 = (string)item3.Attribute(XName.op_Implicit("Name"));
								if (!string.IsNullOrWhiteSpace(text4))
								{
									bool flag = item3.Attribute(XName.op_Implicit("Nullable")) != null && item3.Attribute(XName.op_Implicit("Nullable")).Value == "true";
									string dbType = (string)item3.Attribute(XName.op_Implicit("Type"));
									Type type = GetNetType(dbType);
									if (flag)
									{
										type = typeof(Nullable<>).MakeGenericType(type);
									}
									StiDataColumnSchema item = new StiDataColumnSchema(text4, type);
									stiDataTableSchema.Columns.Add(item);
								}
							}
							catch
							{
							}
						}
						hashtable[text + "." + stiDataTableSchema.Name] = stiDataTableSchema;
						goto IL_02e2;
					}
					catch
					{
						goto IL_02e2;
					}
					IL_02e2:
					foreach (string tableName2 in hashtable2.Keys)
					{
						StiDataTableSchema stiDataTableSchema2 = hashtable2[tableName2] as StiDataTableSchema;
						StiDataTableSchema stiDataTableSchema3 = stiDataSchema.Tables.FirstOrDefault((StiDataTableSchema t) => t.Name == tableName2);
						if (stiDataTableSchema3 == null)
						{
							continue;
						}
						foreach (StiDataColumnSchema column2 in stiDataTableSchema3.Columns)
						{
							if (!stiDataTableSchema2.Columns.Any((StiDataColumnSchema c) => c.Name == column2.Name))
							{
								stiDataTableSchema2.Columns.Add(column2);
							}
						}
					}
				}
				foreach (XElement item4 in from e in ((XContainer)val2).Elements()
					where e.Name.LocalName == "EntityContainer"
					select e)
				{
					foreach (XElement item5 in from e in ((XContainer)item4).Elements()
						where e.Name.LocalName == "EntitySet"
						select e)
					{
						try
						{
							string text5 = (string)item5.Attribute(XName.op_Implicit("Name"));
							string key2 = (string)item5.Attribute(XName.op_Implicit("EntityType"));
							if (string.IsNullOrWhiteSpace(text5))
							{
								continue;
							}
							StiDataTableSchema stiDataTableSchema4 = new StiDataTableSchema(text5, text5);
							if (hashtable[key2] is StiDataTableSchema stiDataTableSchema5)
							{
								stiDataTableSchema4.Columns.AddRange(stiDataTableSchema5.Columns);
							}
							stiDataSchema.Tables.Add(stiDataTableSchema4);
							goto IL_04ed;
						}
						catch
						{
							goto IL_04ed;
						}
						IL_04ed:
						foreach (string tableName in hashtable2.Keys)
						{
							StiDataTableSchema stiDataTableSchema6 = hashtable2[tableName] as StiDataTableSchema;
							StiDataTableSchema stiDataTableSchema7 = stiDataSchema.Tables.FirstOrDefault((StiDataTableSchema t) => t.Name == tableName);
							if (stiDataTableSchema7 == null)
							{
								continue;
							}
							foreach (StiDataColumnSchema column in stiDataTableSchema7.Columns)
							{
								if (!stiDataTableSchema6.Columns.Any((StiDataColumnSchema c) => c.Name == column.Name))
								{
									stiDataTableSchema6.Columns.Add(column);
								}
							}
						}
					}
				}
			}
			return stiDataSchema;
		}
		catch
		{
			if (AllowException)
			{
				throw;
			}
			return null;
		}
	}

	public void FillDataTable(DataTable table, string query)
	{
		if (string.IsNullOrEmpty(ConnectionString))
		{
			return;
		}
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			using WebClient webClient = GetDefaultWebClient();
			string address = StiUrl.Combine(Address, query);
			string text = webClient.DownloadString(address);
			object obj = null;
			try
			{
				obj = JsonConvert.DeserializeObject(text);
			}
			catch
			{
			}
			if (obj != null && obj is JObject)
			{
				JArray jArray = null;
				if (obj is JObject jObject)
				{
					foreach (JToken item in jObject.Children())
					{
						if (item is JProperty { Name: "value" } jProperty && jProperty.Value is JArray)
						{
							jArray = jProperty.Value as JArray;
						}
					}
				}
				if (jArray == null)
				{
					return;
				}
				{
					foreach (JObject childrenToken in jArray.ChildrenTokens)
					{
						DataRow dataRow = table.NewRow();
						foreach (JProperty childrenToken2 in childrenToken.ChildrenTokens)
						{
							try
							{
								string name = childrenToken2.Name;
								JToken value = childrenToken2.Value;
								DataColumn dataColumn = table.Columns[name];
								if (dataColumn != null)
								{
									object obj3 = StiConvert.ChangeType(value, dataColumn.DataType);
									dataRow[name] = obj3 ?? DBNull.Value;
								}
							}
							catch
							{
							}
						}
						table.Rows.Add(dataRow);
					}
					return;
				}
			}
			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
			xmlReaderSettings.DtdProcessing = DtdProcessing.Prohibit;
			using StringReader input = new StringReader(text);
			using XmlReader xmlReader = XmlReader.Create(input, xmlReaderSettings);
			XElement val = XElement.Load(xmlReader);
			XElement val2 = ((XContainer)val).Elements().FirstOrDefault((Func<XElement, bool>)((XElement e) => e.Name.LocalName == "title"));
			if (val2 != null)
			{
				table.TableName = val2.Value;
			}
			foreach (XElement item2 in from e in ((XContainer)val).Elements()
				where e.Name.LocalName == "entry"
				select e)
			{
				XElement val3 = ((XContainer)item2).Elements().FirstOrDefault((Func<XElement, bool>)((XElement e) => e.Name.LocalName == "content"));
				if (val3 == null)
				{
					continue;
				}
				XElement val4 = ((XContainer)val3).Elements().FirstOrDefault((Func<XElement, bool>)((XElement e) => e.Name.LocalName.EndsWith("properties")));
				if (val4 == null)
				{
					continue;
				}
				DataRow dataRow2 = table.NewRow();
				try
				{
					XElement val5 = ((XContainer)item2).Elements().FirstOrDefault((Func<XElement, bool>)((XElement e) => e.Name.LocalName == "title"));
					if (val5 != null && table.Columns["Name"] != null)
					{
						dataRow2["Name"] = val5.Value;
					}
				}
				catch
				{
				}
				try
				{
					XElement val6 = ((XContainer)item2).Elements().FirstOrDefault((Func<XElement, bool>)((XElement e) => e.Name.LocalName == "summary"));
					if (val6 != null && table.Columns["Description"] != null)
					{
						dataRow2["Description"] = val6.Value;
					}
				}
				catch
				{
				}
				foreach (XElement item3 in ((XContainer)val4).Elements())
				{
					try
					{
						string text2 = item3.Name.LocalName.Replace("d:", "");
						string value2 = item3.Value;
						DataColumn dataColumn2 = table.Columns[text2];
						if (dataColumn2 != null)
						{
							object obj7 = StiConvert.ChangeType(value2, dataColumn2.DataType);
							dataRow2[text2] = obj7 ?? DBNull.Value;
						}
					}
					catch
					{
					}
				}
				table.Rows.Add(dataRow2);
			}
		}
		catch
		{
			if (AllowException)
			{
				throw;
			}
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
	}

	public List<StiDataColumnSchema> GetColumns(string collectionName)
	{
		if (string.IsNullOrEmpty(ConnectionString))
		{
			return null;
		}
		List<StiDataColumnSchema> list = new List<StiDataColumnSchema>();
		foreach (StiDataTableSchema table in RetrieveSchema().Tables)
		{
			if (!(table.Name == collectionName))
			{
				continue;
			}
			foreach (StiDataColumnSchema column in table.Columns)
			{
				list.Add(new StiDataColumnSchema(column.Name, column.Type));
			}
		}
		return list;
	}

	public StiTestConnectionResult TestConnection()
	{
		try
		{
			using WebClient webClient = GetDefaultWebClient();
			webClient.DownloadString(Address);
		}
		catch (Exception ex)
		{
			return StiTestConnectionResult.MakeWrong(ex.Message);
		}
		return StiTestConnectionResult.MakeFine();
	}

	public static Type GetNetType(string dbType)
	{
		if (string.IsNullOrWhiteSpace(dbType))
		{
			return null;
		}
		dbType = dbType.ToLowerInvariant();
		if (dbType.StartsWith("edm."))
		{
			dbType = dbType.Replace("edm.", "");
		}
		switch (dbType)
		{
		case "int64":
			return typeof(long);
		case "int32":
			return typeof(int);
		case "int16":
			return typeof(short);
		case "byte":
			return typeof(byte);
		case "sbyte":
			return typeof(sbyte);
		case "int":
			return typeof(int);
		case "boolean":
			return typeof(bool);
		case "decimal":
			return typeof(decimal);
		case "float":
			return typeof(float);
		case "double":
			return typeof(double);
		case "time":
		case "datetime":
			return typeof(DateTime);
		case "datetimeoffset":
			return typeof(DateTimeOffset);
		case "guid":
			return typeof(Guid);
		case "binary":
			return typeof(byte[]);
		default:
			return typeof(string);
		}
	}

	public static string GetBearerAccessToken(string url, string userName, string password, string clientId)
	{
		using (WebClient webClient = new WebClient())
		{
			webClient.Headers["Content-Type"] = "application/json";
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			string data = "{\"grant_type\":\"password\",\"username\":\"" + userName + "\",\"password\":\"" + password + "\",\"client_id\":\"" + clientId + "\"}";
			string json = webClient.UploadString(url, "POST", data);
			string text = JObject.Parse(json)["access_token"].ToString();
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
		}
		return null;
	}

	private WebClient GetDefaultWebClient()
	{
		WebClient webClient = new WebClient();
		webClient.Proxy = StiProxy.GetProxy();
		webClient.Encoding = StiBaseOptions.WebClientEncoding;
		webClient.Headers.Add(Headers);
		if (!string.IsNullOrWhiteSpace(Token))
		{
			webClient.Headers["Authorization"] = "Bearer " + Token;
		}
		else if (!string.IsNullOrWhiteSpace(AddressBearer))
		{
			if (string.IsNullOrWhiteSpace(bearerAccessToken))
			{
				bearerAccessToken = GetBearerAccessToken(AddressBearer, UserName, Password, ClientId);
				if (!string.IsNullOrWhiteSpace(bearerAccessToken))
				{
					webClient.Headers["Authorization"] = "Bearer " + bearerAccessToken;
				}
			}
		}
		else if (!string.IsNullOrWhiteSpace(UserName))
		{
			webClient.UseDefaultCredentials = false;
			webClient.Credentials = new NetworkCredential(UserName, Password);
		}
		return webClient;
	}

	public StiODataHelper(string connectionString, NameValueCollection headers = null)
	{
		ConnectionString = connectionString.Replace("\r\n", "").Replace(" ", "");
		Headers = headers ?? new NameValueCollection();
		ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
	}
}
