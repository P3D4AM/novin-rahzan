using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Net;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiODataDatabaseConverter))]
public class StiODataDatabase : StiSqlDatabase
{
	public override StiComponentId ComponentId => StiComponentId.StiODataDatabase;

	public override StiConnectionType ConnectionType => StiConnectionType.Rest;

	[StiSerializable]
	[TypeConverter(typeof(StiEnumConverter))]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	public StiODataVersion Version { get; set; } = (StiODataVersion)1;


	[Browsable(false)]
	[StiBrowsable(false)]
	public CookieContainer CookieContainer { get; set; }

	[Browsable(false)]
	[StiBrowsable(false)]
	[Description("List of headers used for http requests to load data.")]
	public NameValueCollection Headers { get; protected set; } = new NameValueCollection();


	public override StiDatabase CreateNew()
	{
		return new StiODataDatabase();
	}

	public override StiDataConnector CreateConnector(string connectionString = null)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		StiODataConnector val = StiODataConnector.Get(connectionString, Headers);
		val.AllowException = base.AllowException;
		val.Version = Version;
		val.CookieContainer = CookieContainer;
		return (StiDataConnector)(object)val;
	}

	public override StiSqlSource CreateDataSource(string nameInSource, string name)
	{
		return new StiODataSource(nameInSource, name);
	}

	protected override Type GetDataAdapterType()
	{
		return typeof(StiODataAdapterService);
	}

	public override string GetConnectionStringHelper()
	{
		return "StiODataConnectionHelper";
	}

	public override string MapUserNameAndPassword(string userName, string password)
	{
		return "UserName = " + userName + "; Password = " + password;
	}

	public override void RegData(StiDictionary dictionary, bool loadData)
	{
		CookieContainer = dictionary.Report.CookieContainer;
	}

	public StiODataDatabase()
		: this(string.Empty, string.Empty)
	{
	}

	public StiODataDatabase(string name, string connectionString)
		: base(name, connectionString)
	{
	}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


	public StiODataDatabase(string name, string alias, string connectionString)
		: base(name, alias, connectionString)
	{
	}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


	public StiODataDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword)
		: base(name, alias, connectionString, promptUserNameAndpassword)
	{
	}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


	public StiODataDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword, string key)
		: base(name, alias, connectionString, promptUserNameAndpassword, key)
	{
	}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


	public StiODataDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword, string key, StiODataVersion version)
		: base(name, alias, connectionString, promptUserNameAndpassword, key)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		Version = version;
	}
}
