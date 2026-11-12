using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.Data.Common;
using System.Data.Sql;
using System.Globalization;
using System.Reflection;
using Microsoft.Data.Common;

namespace Microsoft.Data.SqlClient;

[DefaultProperty("DataSource")]
[TypeConverter(typeof(SqlConnectionStringBuilderConverter))]
public sealed class SqlConnectionStringBuilder : DbConnectionStringBuilder
{
	private enum Keywords
	{
		DataSource,
		FailoverPartner,
		AttachDBFilename,
		InitialCatalog,
		IntegratedSecurity,
		PersistSecurityInfo,
		UserID,
		Password,
		Enlist,
		Pooling,
		MinPoolSize,
		MaxPoolSize,
		PoolBlockingPeriod,
		AsynchronousProcessing,
		ConnectionReset,
		MultipleActiveResultSets,
		Replication,
		ConnectTimeout,
		Encrypt,
		TrustServerCertificate,
		LoadBalanceTimeout,
		NetworkLibrary,
		PacketSize,
		TypeSystemVersion,
		Authentication,
		ApplicationName,
		CurrentLanguage,
		WorkstationID,
		UserInstance,
		ContextConnection,
		TransactionBinding,
		ApplicationIntent,
		MultiSubnetFailover,
		TransparentNetworkIPResolution,
		ConnectRetryCount,
		ConnectRetryInterval,
		ColumnEncryptionSetting,
		EnclaveAttestationUrl,
		AttestationProtocol,
		CommandTimeout,
		IPAddressPreference,
		KeywordsCount
	}

	private sealed class NetworkLibraryConverter : TypeConverter
	{
		private const string NamedPipes = "Named Pipes (DBNMPNTW)";

		private const string SharedMemory = "Shared Memory (DBMSLPCN)";

		private const string TCPIP = "TCP/IP (DBMSSOCN)";

		private const string VIA = "VIA (DBMSGNET)";

		private StandardValuesCollection _standardValues;

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (!(typeof(string) == sourceType))
			{
				return base.CanConvertFrom(context, sourceType);
			}
			return true;
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string text)
			{
				string text2 = text.Trim();
				if (StringComparer.OrdinalIgnoreCase.Equals(text2, "Named Pipes (DBNMPNTW)"))
				{
					return "dbnmpntw";
				}
				if (StringComparer.OrdinalIgnoreCase.Equals(text2, "Shared Memory (DBMSLPCN)"))
				{
					return "dbmslpcn";
				}
				if (StringComparer.OrdinalIgnoreCase.Equals(text2, "TCP/IP (DBMSSOCN)"))
				{
					return "dbmssocn";
				}
				if (StringComparer.OrdinalIgnoreCase.Equals(text2, "VIA (DBMSGNET)"))
				{
					return "dbmsgnet";
				}
				return text2;
			}
			return base.ConvertFrom(context, culture, value);
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (!(typeof(string) == destinationType))
			{
				return base.CanConvertTo(context, destinationType);
			}
			return true;
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (value is string text && destinationType == typeof(string))
			{
				return text.Trim().ToLower(CultureInfo.InvariantCulture) switch
				{
					"dbnmpntw" => "Named Pipes (DBNMPNTW)", 
					"dbmslpcn" => "Shared Memory (DBMSLPCN)", 
					"dbmssocn" => "TCP/IP (DBMSSOCN)", 
					"dbmsgnet" => "VIA (DBMSGNET)", 
					_ => text, 
				};
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			SqlConnectionStringBuilder sqlConnectionStringBuilder = null;
			if (context != null)
			{
				sqlConnectionStringBuilder = context.Instance as SqlConnectionStringBuilder;
			}
			StandardValuesCollection standardValuesCollection = _standardValues;
			if (standardValuesCollection == null)
			{
				string[] values = new string[4] { "Named Pipes (DBNMPNTW)", "Shared Memory (DBMSLPCN)", "TCP/IP (DBMSSOCN)", "VIA (DBMSGNET)" };
				standardValuesCollection = (_standardValues = new StandardValuesCollection(values));
			}
			return standardValuesCollection;
		}
	}

	private sealed class SqlDataSourceConverter : StringConverter
	{
		private StandardValuesCollection _standardValues;

		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			StandardValuesCollection result = _standardValues;
			if (_standardValues == null)
			{
				DataTable dataSources = SqlClientFactory.Instance.CreateDataSourceEnumerator().GetDataSources();
				string name = typeof(SqlDataSourceEnumerator).GetField("ServerName", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null).ToString();
				string name2 = typeof(SqlDataSourceEnumerator).GetField("InstanceName", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null).ToString();
				DataColumn column = dataSources.Columns[name];
				DataColumn column2 = dataSources.Columns[name2];
				DataRowCollection rows = dataSources.Rows;
				string[] array = new string[rows.Count];
				for (int i = 0; i < array.Length; i++)
				{
					string text = rows[i][column] as string;
					if (!(rows[i][column2] is string { Length: not 0 } text2) || "MSSQLSERVER" == text2)
					{
						array[i] = text;
					}
					else
					{
						array[i] = text + "\\" + text2;
					}
				}
				Array.Sort(array);
				result = (_standardValues = new StandardValuesCollection(array));
			}
			return result;
		}
	}

	private sealed class SqlInitialCatalogConverter : StringConverter
	{
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return GetStandardValuesSupportedInternal(context);
		}

		private bool GetStandardValuesSupportedInternal(ITypeDescriptorContext context)
		{
			bool result = false;
			if (context != null && context.Instance is SqlConnectionStringBuilder sqlConnectionStringBuilder && 0 < sqlConnectionStringBuilder.DataSource.Length && (sqlConnectionStringBuilder.IntegratedSecurity || 0 < sqlConnectionStringBuilder.UserID.Length))
			{
				result = true;
			}
			return result;
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			if (GetStandardValuesSupportedInternal(context))
			{
				List<string> list = new List<string>();
				try
				{
					SqlConnectionStringBuilder sqlConnectionStringBuilder = (SqlConnectionStringBuilder)context.Instance;
					using SqlConnection sqlConnection = new SqlConnection();
					sqlConnection.ConnectionString = sqlConnectionStringBuilder.ConnectionString;
					sqlConnection.Open();
					DataTable schema = sqlConnection.GetSchema("DATABASES");
					foreach (DataRow row in schema.Rows)
					{
						string item = (string)row["database_name"];
						list.Add(item);
					}
				}
				catch (SqlException e)
				{
					ADP.TraceExceptionWithoutRethrow(e);
				}
				return new StandardValuesCollection(list);
			}
			return null;
		}
	}

	internal sealed class SqlConnectionStringBuilderConverter : ExpandableObjectConverter
	{
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (typeof(InstanceDescriptor) == destinationType)
			{
				return true;
			}
			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw ADP.ArgumentNull("destinationType");
			}
			if (typeof(InstanceDescriptor) == destinationType && value is SqlConnectionStringBuilder options)
			{
				return ConvertToInstanceDescriptor(options);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		private InstanceDescriptor ConvertToInstanceDescriptor(SqlConnectionStringBuilder options)
		{
			Type[] types = new Type[1] { typeof(string) };
			object[] arguments = new object[1] { options.ConnectionString };
			ConstructorInfo constructor = typeof(SqlConnectionStringBuilder).GetConstructor(types);
			return new InstanceDescriptor(constructor, arguments);
		}
	}

	internal const int KeywordsCount = 41;

	private static readonly string[] _validKeywords;

	private static readonly Dictionary<string, Keywords> _keywords;

	private ApplicationIntent _applicationIntent;

	private string _applicationName = "Framework Microsoft SqlClient Data Provider";

	private string _attachDBFilename = "";

	private string _currentLanguage = "";

	private string _dataSource = "";

	private string _failoverPartner = "";

	private string _initialCatalog = "";

	private string _networkLibrary = "";

	private string _password = "";

	private string _transactionBinding = "Implicit Unbind";

	private string _typeSystemVersion = "Latest";

	private string _userID = "";

	private string _workstationID = "";

	private int _commandTimeout = 30;

	private int _connectTimeout = 15;

	private int _loadBalanceTimeout;

	private int _maxPoolSize = 100;

	private int _minPoolSize;

	private int _packetSize = 8000;

	private int _connectRetryCount = 1;

	private int _connectRetryInterval = 10;

	private bool _asynchronousProcessing;

	private bool _connectionReset = true;

	private bool _contextConnection;

	private bool _encrypt;

	private bool _trustServerCertificate;

	private bool _enlist = true;

	private bool _integratedSecurity;

	private bool _multipleActiveResultSets;

	private bool _multiSubnetFailover;

	private bool _transparentNetworkIPResolution = DbConnectionStringDefaults.TransparentNetworkIPResolution;

	private bool _persistSecurityInfo;

	private bool _pooling = true;

	private bool _replication;

	private bool _userInstance;

	private SqlAuthenticationMethod _authentication = DbConnectionStringDefaults.Authentication;

	private SqlConnectionColumnEncryptionSetting _columnEncryptionSetting = DbConnectionStringDefaults.ColumnEncryptionSetting;

	private string _enclaveAttestationUrl = "";

	private SqlConnectionAttestationProtocol _attestationProtocol;

	private SqlConnectionIPAddressPreference _ipAddressPreference;

	private PoolBlockingPeriod _poolBlockingPeriod;

	public override object this[string keyword]
	{
		get
		{
			Keywords index = GetIndex(keyword);
			return GetAt(index);
		}
		set
		{
			if (value != null)
			{
				switch (GetIndex(keyword))
				{
				case Keywords.ApplicationIntent:
					ApplicationIntent = ConvertToApplicationIntent(keyword, value);
					break;
				case Keywords.ApplicationName:
					ApplicationName = ConvertToString(value);
					break;
				case Keywords.AttachDBFilename:
					AttachDBFilename = ConvertToString(value);
					break;
				case Keywords.CurrentLanguage:
					CurrentLanguage = ConvertToString(value);
					break;
				case Keywords.DataSource:
					DataSource = ConvertToString(value);
					break;
				case Keywords.FailoverPartner:
					FailoverPartner = ConvertToString(value);
					break;
				case Keywords.InitialCatalog:
					InitialCatalog = ConvertToString(value);
					break;
				case Keywords.NetworkLibrary:
					NetworkLibrary = ConvertToString(value);
					break;
				case Keywords.Password:
					Password = ConvertToString(value);
					break;
				case Keywords.UserID:
					UserID = ConvertToString(value);
					break;
				case Keywords.TransactionBinding:
					TransactionBinding = ConvertToString(value);
					break;
				case Keywords.TypeSystemVersion:
					TypeSystemVersion = ConvertToString(value);
					break;
				case Keywords.WorkstationID:
					WorkstationID = ConvertToString(value);
					break;
				case Keywords.CommandTimeout:
					CommandTimeout = ConvertToInt32(value);
					break;
				case Keywords.ConnectTimeout:
					ConnectTimeout = ConvertToInt32(value);
					break;
				case Keywords.LoadBalanceTimeout:
					LoadBalanceTimeout = ConvertToInt32(value);
					break;
				case Keywords.MaxPoolSize:
					MaxPoolSize = ConvertToInt32(value);
					break;
				case Keywords.MinPoolSize:
					MinPoolSize = ConvertToInt32(value);
					break;
				case Keywords.PacketSize:
					PacketSize = ConvertToInt32(value);
					break;
				case Keywords.IntegratedSecurity:
					IntegratedSecurity = ConvertToIntegratedSecurity(value);
					break;
				case Keywords.Authentication:
					Authentication = ConvertToAuthenticationType(keyword, value);
					break;
				case Keywords.ColumnEncryptionSetting:
					ColumnEncryptionSetting = ConvertToColumnEncryptionSetting(keyword, value);
					break;
				case Keywords.EnclaveAttestationUrl:
					EnclaveAttestationUrl = ConvertToString(value);
					break;
				case Keywords.AttestationProtocol:
					AttestationProtocol = ConvertToAttestationProtocol(keyword, value);
					break;
				case Keywords.IPAddressPreference:
					IPAddressPreference = ConvertToIPAddressPreference(keyword, value);
					break;
				case Keywords.AsynchronousProcessing:
					AsynchronousProcessing = ConvertToBoolean(value);
					break;
				case Keywords.PoolBlockingPeriod:
					PoolBlockingPeriod = ConvertToPoolBlockingPeriod(keyword, value);
					break;
				case Keywords.ConnectionReset:
					ConnectionReset = ConvertToBoolean(value);
					break;
				case Keywords.ContextConnection:
					ContextConnection = ConvertToBoolean(value);
					break;
				case Keywords.Encrypt:
					Encrypt = ConvertToBoolean(value);
					break;
				case Keywords.TrustServerCertificate:
					TrustServerCertificate = ConvertToBoolean(value);
					break;
				case Keywords.Enlist:
					Enlist = ConvertToBoolean(value);
					break;
				case Keywords.MultipleActiveResultSets:
					MultipleActiveResultSets = ConvertToBoolean(value);
					break;
				case Keywords.MultiSubnetFailover:
					MultiSubnetFailover = ConvertToBoolean(value);
					break;
				case Keywords.TransparentNetworkIPResolution:
					TransparentNetworkIPResolution = ConvertToBoolean(value);
					break;
				case Keywords.PersistSecurityInfo:
					PersistSecurityInfo = ConvertToBoolean(value);
					break;
				case Keywords.Pooling:
					Pooling = ConvertToBoolean(value);
					break;
				case Keywords.Replication:
					Replication = ConvertToBoolean(value);
					break;
				case Keywords.UserInstance:
					UserInstance = ConvertToBoolean(value);
					break;
				case Keywords.ConnectRetryCount:
					ConnectRetryCount = ConvertToInt32(value);
					break;
				case Keywords.ConnectRetryInterval:
					ConnectRetryInterval = ConvertToInt32(value);
					break;
				default:
					throw ADP.KeywordNotSupported(keyword);
				}
			}
			else
			{
				Remove(keyword);
			}
		}
	}

	[DisplayName("Application Intent")]
	[ResCategory("Initialization")]
	[ResDescription("Declares the application workload type when connecting to a server.")]
	[RefreshProperties(RefreshProperties.All)]
	public ApplicationIntent ApplicationIntent
	{
		get
		{
			return _applicationIntent;
		}
		set
		{
			if (!DbConnectionStringBuilderUtil.IsValidApplicationIntentValue(value))
			{
				throw ADP.InvalidEnumerationValue(typeof(ApplicationIntent), (int)value);
			}
			SetApplicationIntentValue(value);
			_applicationIntent = value;
		}
	}

	[DisplayName("Application Name")]
	[ResCategory("Context")]
	[ResDescription("The name of the application.")]
	[RefreshProperties(RefreshProperties.All)]
	public string ApplicationName
	{
		get
		{
			return _applicationName;
		}
		set
		{
			SetValue("Application Name", value);
			_applicationName = value;
		}
	}

	[DisplayName("Asynchronous Processing")]
	[Obsolete("AsynchronousProcessing has been deprecated. SqlConnection will ignore the 'Asynchronous Processing' keyword and always allow asynchronous processing.")]
	[ResCategory("Initialization")]
	[ResDescription("When true, enables usage of the Asynchronous functionality in the .NET Framework Data Provider.")]
	[RefreshProperties(RefreshProperties.All)]
	public bool AsynchronousProcessing
	{
		get
		{
			return _asynchronousProcessing;
		}
		set
		{
			SetValue("Asynchronous Processing", value);
			_asynchronousProcessing = value;
		}
	}

	[DisplayName("AttachDbFilename")]
	[ResCategory("Source")]
	[ResDescription("The name of the primary file, including the full path name, of an attachable database.")]
	[RefreshProperties(RefreshProperties.All)]
	[Editor("System.Windows.Forms.Design.FileNameEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public string AttachDBFilename
	{
		get
		{
			return _attachDBFilename;
		}
		set
		{
			SetValue("AttachDbFilename", value);
			_attachDBFilename = value;
		}
	}

	[DisplayName("Pool Blocking Period")]
	[ResCategory("Pooling")]
	[ResDescription("Defines the blocking period behavior for a connection pool.")]
	[RefreshProperties(RefreshProperties.All)]
	public PoolBlockingPeriod PoolBlockingPeriod
	{
		get
		{
			return _poolBlockingPeriod;
		}
		set
		{
			if (!DbConnectionStringBuilderUtil.IsValidPoolBlockingPeriodValue(value))
			{
				throw ADP.InvalidEnumerationValue(typeof(PoolBlockingPeriod), (int)value);
			}
			SetPoolBlockingPeriodValue(value);
			_poolBlockingPeriod = value;
		}
	}

	[DisplayName("Command Timeout")]
	[ResCategory("Initialization")]
	[ResDescription("Time to wait for command to execute.")]
	[RefreshProperties(RefreshProperties.All)]
	public int CommandTimeout
	{
		get
		{
			return _commandTimeout;
		}
		set
		{
			if (value < 0)
			{
				throw ADP.InvalidConnectionOptionValue("Command Timeout");
			}
			SetValue("Command Timeout", value);
			_commandTimeout = value;
		}
	}

	[Browsable(false)]
	[DisplayName("Connection Reset")]
	[Obsolete("ConnectionReset has been deprecated. SqlConnection will ignore the 'connection reset' keyword and always reset the connection.")]
	[ResCategory("Pooling")]
	[ResDescription("When true, indicates the connection state is reset when removed from the pool.")]
	[RefreshProperties(RefreshProperties.All)]
	public bool ConnectionReset
	{
		get
		{
			return _connectionReset;
		}
		set
		{
			SetValue("Connection Reset", value);
			_connectionReset = value;
		}
	}

	[DisplayName("Context Connection")]
	[Obsolete("ContextConnection has been deprecated. SqlConnection will ignore the 'Context Connection' keyword.")]
	[ResCategory("Source")]
	[ResDescription("When true, indicates the connection should be from the Sql Server context.  Available only when running in the Sql Server process.")]
	[RefreshProperties(RefreshProperties.All)]
	public bool ContextConnection
	{
		get
		{
			return _contextConnection;
		}
		set
		{
			SetValue("Context Connection", value);
			_contextConnection = value;
		}
	}

	[DisplayName("Connect Timeout")]
	[ResCategory("Initialization")]
	[ResDescription("The length of time (in seconds) to wait for a connection to the server before terminating the attempt and generating an error.")]
	[RefreshProperties(RefreshProperties.All)]
	public int ConnectTimeout
	{
		get
		{
			return _connectTimeout;
		}
		set
		{
			if (value < 0)
			{
				throw ADP.InvalidConnectionOptionValue("Connect Timeout");
			}
			SetValue("Connect Timeout", value);
			_connectTimeout = value;
		}
	}

	[DisplayName("Current Language")]
	[ResCategory("Initialization")]
	[ResDescription("The SQL Server Language record name.")]
	[RefreshProperties(RefreshProperties.All)]
	public string CurrentLanguage
	{
		get
		{
			return _currentLanguage;
		}
		set
		{
			SetValue("Current Language", value);
			_currentLanguage = value;
		}
	}

	[DisplayName("Data Source")]
	[ResCategory("Source")]
	[ResDescription("Indicates the name of the data source to connect to.")]
	[RefreshProperties(RefreshProperties.All)]
	[TypeConverter(typeof(SqlDataSourceConverter))]
	public string DataSource
	{
		get
		{
			return _dataSource;
		}
		set
		{
			SetValue("Data Source", value);
			_dataSource = value;
		}
	}

	[DisplayName("Encrypt")]
	[ResCategory("Security")]
	[ResDescription("When true, SQL Server uses SSL encryption for all data sent between the client and server if the server has a certificate installed.")]
	[RefreshProperties(RefreshProperties.All)]
	public bool Encrypt
	{
		get
		{
			return _encrypt;
		}
		set
		{
			SetValue("Encrypt", value);
			_encrypt = value;
		}
	}

	[DisplayName("Column Encryption Setting")]
	[ResCategory("Security")]
	[ResDescription("Default column encryption setting for all the commands on the connection.")]
	[RefreshProperties(RefreshProperties.All)]
	public SqlConnectionColumnEncryptionSetting ColumnEncryptionSetting
	{
		get
		{
			return _columnEncryptionSetting;
		}
		set
		{
			if (!DbConnectionStringBuilderUtil.IsValidColumnEncryptionSetting(value))
			{
				throw ADP.InvalidEnumerationValue(typeof(SqlConnectionColumnEncryptionSetting), (int)value);
			}
			SetColumnEncryptionSettingValue(value);
			_columnEncryptionSetting = value;
		}
	}

	[DisplayName("Enclave Attestation Url")]
	[ResCategory("Security")]
	[ResDescription("Specifies an endpoint of an enclave attestation service, which will be used to verify whether the enclave, configured in the SQL Server instance for computations on database columns encrypted using Always Encrypted, is valid and secure.")]
	[RefreshProperties(RefreshProperties.All)]
	public string EnclaveAttestationUrl
	{
		get
		{
			return _enclaveAttestationUrl;
		}
		set
		{
			SetValue("Enclave Attestation Url", value);
			_enclaveAttestationUrl = value;
		}
	}

	[DisplayName("Attestation Protocol")]
	[ResCategory("Security")]
	[ResDescription("Specifies an attestation protocol for its corresponding enclave attestation service.")]
	[RefreshProperties(RefreshProperties.All)]
	public SqlConnectionAttestationProtocol AttestationProtocol
	{
		get
		{
			return _attestationProtocol;
		}
		set
		{
			if (!DbConnectionStringBuilderUtil.IsValidAttestationProtocol(value))
			{
				throw ADP.InvalidEnumerationValue(typeof(SqlConnectionAttestationProtocol), (int)value);
			}
			SetAttestationProtocolValue(value);
			_attestationProtocol = value;
		}
	}

	[DisplayName("IP Address Preference")]
	[ResCategory("Security")]
	[ResDescription("Specifies an IP address preference when connecting to SQL instances.")]
	[RefreshProperties(RefreshProperties.All)]
	public SqlConnectionIPAddressPreference IPAddressPreference
	{
		get
		{
			return _ipAddressPreference;
		}
		set
		{
			if (!DbConnectionStringBuilderUtil.IsValidIPAddressPreference(value))
			{
				throw ADP.InvalidEnumerationValue(typeof(SqlConnectionIPAddressPreference), (int)value);
			}
			SetIPAddressPreferenceValue(value);
			_ipAddressPreference = value;
		}
	}

	[DisplayName("Trust Server Certificate")]
	[ResCategory("Security")]
	[ResDescription("When true (and encrypt=true), SQL Server uses SSL encryption for all data sent between the client and server without validating the server certificate.")]
	[RefreshProperties(RefreshProperties.All)]
	public bool TrustServerCertificate
	{
		get
		{
			return _trustServerCertificate;
		}
		set
		{
			SetValue("Trust Server Certificate", value);
			_trustServerCertificate = value;
		}
	}

	[DisplayName("Enlist")]
	[ResCategory("Pooling")]
	[ResDescription("Sessions in a Component Services (or MTS, if you are using Microsoft Windows NT) environment should automatically be enlisted in a global transaction where required.")]
	[RefreshProperties(RefreshProperties.All)]
	public bool Enlist
	{
		get
		{
			return _enlist;
		}
		set
		{
			SetValue("Enlist", value);
			_enlist = value;
		}
	}

	[DisplayName("Failover Partner")]
	[ResCategory("Source")]
	[ResDescription("The name or network address of the instance of SQL Server that acts as a failover partner.")]
	[RefreshProperties(RefreshProperties.All)]
	[TypeConverter(typeof(SqlDataSourceConverter))]
	public string FailoverPartner
	{
		get
		{
			return _failoverPartner;
		}
		set
		{
			SetValue("Failover Partner", value);
			_failoverPartner = value;
		}
	}

	[DisplayName("Initial Catalog")]
	[ResCategory("Source")]
	[ResDescription("The name of the initial catalog or database in the data source.")]
	[RefreshProperties(RefreshProperties.All)]
	[TypeConverter(typeof(SqlInitialCatalogConverter))]
	public string InitialCatalog
	{
		get
		{
			return _initialCatalog;
		}
		set
		{
			SetValue("Initial Catalog", value);
			_initialCatalog = value;
		}
	}

	[DisplayName("Integrated Security")]
	[ResCategory("Security")]
	[ResDescription("Whether the connection is to be a secure connection or not.")]
	[RefreshProperties(RefreshProperties.All)]
	public bool IntegratedSecurity
	{
		get
		{
			return _integratedSecurity;
		}
		set
		{
			SetValue("Integrated Security", value);
			_integratedSecurity = value;
		}
	}

	[DisplayName("Authentication")]
	[ResCategory("Security")]
	[ResDescription("Specifies the method of authenticating with SQL Server.")]
	[RefreshProperties(RefreshProperties.All)]
	public SqlAuthenticationMethod Authentication
	{
		get
		{
			return _authentication;
		}
		set
		{
			if (!DbConnectionStringBuilderUtil.IsValidAuthenticationTypeValue(value))
			{
				throw ADP.InvalidEnumerationValue(typeof(SqlAuthenticationMethod), (int)value);
			}
			SetAuthenticationValue(value);
			_authentication = value;
		}
	}

	internal string Certificate => null;

	[DisplayName("Load Balance Timeout")]
	[ResCategory("Pooling")]
	[ResDescription("The minimum amount of time (in seconds) for this connection to live in the pool before being destroyed.")]
	[RefreshProperties(RefreshProperties.All)]
	public int LoadBalanceTimeout
	{
		get
		{
			return _loadBalanceTimeout;
		}
		set
		{
			if (value < 0)
			{
				throw ADP.InvalidConnectionOptionValue("Load Balance Timeout");
			}
			SetValue("Load Balance Timeout", value);
			_loadBalanceTimeout = value;
		}
	}

	[DisplayName("Max Pool Size")]
	[ResCategory("Pooling")]
	[ResDescription("The maximum number of connections allowed in the pool.")]
	[RefreshProperties(RefreshProperties.All)]
	public int MaxPoolSize
	{
		get
		{
			return _maxPoolSize;
		}
		set
		{
			if (value < 1)
			{
				throw ADP.InvalidConnectionOptionValue("Max Pool Size");
			}
			SetValue("Max Pool Size", value);
			_maxPoolSize = value;
		}
	}

	[DisplayName("Connect Retry Count")]
	[ResCategory("Connection Resiliency")]
	[ResDescription("Number of attempts to restore connection.")]
	[RefreshProperties(RefreshProperties.All)]
	public int ConnectRetryCount
	{
		get
		{
			return _connectRetryCount;
		}
		set
		{
			if (value < 0 || value > 255)
			{
				throw ADP.InvalidConnectionOptionValue("Connect Retry Count");
			}
			SetValue("Connect Retry Count", value);
			_connectRetryCount = value;
		}
	}

	[DisplayName("Connect Retry Interval")]
	[ResCategory("Connection Resiliency")]
	[ResDescription("Delay between attempts to restore connection.")]
	[RefreshProperties(RefreshProperties.All)]
	public int ConnectRetryInterval
	{
		get
		{
			return _connectRetryInterval;
		}
		set
		{
			if (value < 1 || value > 60)
			{
				throw ADP.InvalidConnectionOptionValue("Connect Retry Interval");
			}
			SetValue("Connect Retry Interval", value);
			_connectRetryInterval = value;
		}
	}

	[DisplayName("Min Pool Size")]
	[ResCategory("Pooling")]
	[ResDescription("The minimum number of connections allowed in the pool.")]
	[RefreshProperties(RefreshProperties.All)]
	public int MinPoolSize
	{
		get
		{
			return _minPoolSize;
		}
		set
		{
			if (value < 0)
			{
				throw ADP.InvalidConnectionOptionValue("Min Pool Size");
			}
			SetValue("Min Pool Size", value);
			_minPoolSize = value;
		}
	}

	[DisplayName("Multiple Active Result Sets")]
	[ResCategory("Advanced")]
	[ResDescription("When true, multiple result sets can be returned and read from one connection.")]
	[RefreshProperties(RefreshProperties.All)]
	public bool MultipleActiveResultSets
	{
		get
		{
			return _multipleActiveResultSets;
		}
		set
		{
			SetValue("Multiple Active Result Sets", value);
			_multipleActiveResultSets = value;
		}
	}

	[DisplayName("Multi Subnet Failover")]
	[ResCategory("Source")]
	[ResDescription("If your application is connecting to a high-availability, disaster recovery (AlwaysOn) availability group (AG) on different subnets, MultiSubnetFailover=Yes configures SqlConnection to provide faster detection of and connection to the (currently) active server.")]
	[RefreshProperties(RefreshProperties.All)]
	public bool MultiSubnetFailover
	{
		get
		{
			return _multiSubnetFailover;
		}
		set
		{
			SetValue("Multi Subnet Failover", value);
			_multiSubnetFailover = value;
		}
	}

	[DisplayName("Transparent Network IP Resolution")]
	[ResCategory("Source")]
	[ResDescription("If your application connects to different networks, TransparentNetworkIPResolution=Yes configures SqlConnection to provide transparent connection resolution to the currently active server, independently of the network IP topology.")]
	[RefreshProperties(RefreshProperties.All)]
	public bool TransparentNetworkIPResolution
	{
		get
		{
			return _transparentNetworkIPResolution;
		}
		set
		{
			SetValue("Transparent Network IP Resolution", value);
			_transparentNetworkIPResolution = value;
		}
	}

	[DisplayName("Network Library")]
	[ResCategory("Advanced")]
	[ResDescription("The network library used to establish a connection to an instance of SQL Server.")]
	[RefreshProperties(RefreshProperties.All)]
	[TypeConverter(typeof(NetworkLibraryConverter))]
	public string NetworkLibrary
	{
		get
		{
			return _networkLibrary;
		}
		set
		{
			if (value != null)
			{
				value = value.Trim().ToLower(CultureInfo.InvariantCulture) switch
				{
					"dbmsadsn" => "dbmsadsn", 
					"dbmsvinn" => "dbmsvinn", 
					"dbmsspxn" => "dbmsspxn", 
					"dbmsrpcn" => "dbmsrpcn", 
					"dbnmpntw" => "dbnmpntw", 
					"dbmslpcn" => "dbmslpcn", 
					"dbmssocn" => "dbmssocn", 
					"dbmsgnet" => "dbmsgnet", 
					_ => throw ADP.InvalidConnectionOptionValue("Network Library"), 
				};
			}
			SetValue("Network Library", value);
			_networkLibrary = value;
		}
	}

	[DisplayName("Packet Size")]
	[ResCategory("Advanced")]
	[ResDescription("Size in bytes of the network packets used to communicate with an instance of SQL Server.")]
	[RefreshProperties(RefreshProperties.All)]
	public int PacketSize
	{
		get
		{
			return _packetSize;
		}
		set
		{
			if (value < 512 || 32768 < value)
			{
				throw SQL.InvalidPacketSizeValue();
			}
			SetValue("Packet Size", value);
			_packetSize = value;
		}
	}

	[DisplayName("Password")]
	[PasswordPropertyText(true)]
	[ResCategory("Security")]
	[ResDescription("Indicates the password to be used when connecting to the data source.")]
	[RefreshProperties(RefreshProperties.All)]
	public string Password
	{
		get
		{
			return _password;
		}
		set
		{
			SetValue("Password", value);
			_password = value;
		}
	}

	[DisplayName("Persist Security Info")]
	[ResCategory("Security")]
	[ResDescription("When false, security-sensitive information, such as the password, is not returned as part of the connection if the connection is open or has ever been in an open state.")]
	[RefreshProperties(RefreshProperties.All)]
	public bool PersistSecurityInfo
	{
		get
		{
			return _persistSecurityInfo;
		}
		set
		{
			SetValue("Persist Security Info", value);
			_persistSecurityInfo = value;
		}
	}

	[DisplayName("Pooling")]
	[ResCategory("Pooling")]
	[ResDescription("When true, the connection object is drawn from the appropriate pool, or if necessary, is created and added to the appropriate pool.")]
	[RefreshProperties(RefreshProperties.All)]
	public bool Pooling
	{
		get
		{
			return _pooling;
		}
		set
		{
			SetValue("Pooling", value);
			_pooling = value;
		}
	}

	[DisplayName("Replication")]
	[ResCategory("Replication")]
	[ResDescription("Used by SQL Server in Replication.")]
	[RefreshProperties(RefreshProperties.All)]
	public bool Replication
	{
		get
		{
			return _replication;
		}
		set
		{
			SetValue("Replication", value);
			_replication = value;
		}
	}

	[DisplayName("Transaction Binding")]
	[ResCategory("Advanced")]
	[ResDescription("Indicates binding behavior of connection to a System.Transactions Transaction when enlisted.")]
	[RefreshProperties(RefreshProperties.All)]
	public string TransactionBinding
	{
		get
		{
			return _transactionBinding;
		}
		set
		{
			SetValue("Transaction Binding", value);
			_transactionBinding = value;
		}
	}

	[DisplayName("Type System Version")]
	[ResCategory("Advanced")]
	[ResDescription("Indicates which server type system the provider will expose through the DataReader.")]
	[RefreshProperties(RefreshProperties.All)]
	public string TypeSystemVersion
	{
		get
		{
			return _typeSystemVersion;
		}
		set
		{
			SetValue("Type System Version", value);
			_typeSystemVersion = value;
		}
	}

	[DisplayName("User ID")]
	[ResCategory("Security")]
	[ResDescription("Indicates the user ID to be used when connecting to the data source.")]
	[RefreshProperties(RefreshProperties.All)]
	public string UserID
	{
		get
		{
			return _userID;
		}
		set
		{
			SetValue("User ID", value);
			_userID = value;
		}
	}

	[DisplayName("User Instance")]
	[ResCategory("Source")]
	[ResDescription("Indicates whether the connection will be re-directed to connect to an instance of SQL Server running under the user's account.")]
	[RefreshProperties(RefreshProperties.All)]
	public bool UserInstance
	{
		get
		{
			return _userInstance;
		}
		set
		{
			SetValue("User Instance", value);
			_userInstance = value;
		}
	}

	[DisplayName("Workstation ID")]
	[ResCategory("Context")]
	[ResDescription("The name of the workstation connecting to SQL Server.")]
	[RefreshProperties(RefreshProperties.All)]
	public string WorkstationID
	{
		get
		{
			return _workstationID;
		}
		set
		{
			SetValue("Workstation ID", value);
			_workstationID = value;
		}
	}

	public override bool IsFixedSize => true;

	public override ICollection Keys => new ReadOnlyCollection<string>(_validKeywords);

	public override ICollection Values
	{
		get
		{
			object[] array = new object[_validKeywords.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = GetAt((Keywords)i);
			}
			return new ReadOnlyCollection<object>(array);
		}
	}

	static SqlConnectionStringBuilder()
	{
		string[] array = new string[41];
		array[31] = "Application Intent";
		array[25] = "Application Name";
		array[13] = "Asynchronous Processing";
		array[2] = "AttachDbFilename";
		array[12] = "Pool Blocking Period";
		array[14] = "Connection Reset";
		array[29] = "Context Connection";
		array[17] = "Connect Timeout";
		array[39] = "Command Timeout";
		array[26] = "Current Language";
		array[0] = "Data Source";
		array[18] = "Encrypt";
		array[8] = "Enlist";
		array[1] = "Failover Partner";
		array[3] = "Initial Catalog";
		array[4] = "Integrated Security";
		array[20] = "Load Balance Timeout";
		array[11] = "Max Pool Size";
		array[10] = "Min Pool Size";
		array[15] = "Multiple Active Result Sets";
		array[32] = "Multi Subnet Failover";
		array[33] = "Transparent Network IP Resolution";
		array[21] = "Network Library";
		array[22] = "Packet Size";
		array[7] = "Password";
		array[5] = "Persist Security Info";
		array[9] = "Pooling";
		array[16] = "Replication";
		array[30] = "Transaction Binding";
		array[19] = "Trust Server Certificate";
		array[23] = "Type System Version";
		array[6] = "User ID";
		array[28] = "User Instance";
		array[27] = "Workstation ID";
		array[34] = "Connect Retry Count";
		array[35] = "Connect Retry Interval";
		array[24] = "Authentication";
		array[36] = "Column Encryption Setting";
		array[37] = "Enclave Attestation Url";
		array[38] = "Attestation Protocol";
		array[40] = "IP Address Preference";
		_validKeywords = array;
		_keywords = new Dictionary<string, Keywords>(71, StringComparer.OrdinalIgnoreCase)
		{
			{
				"Application Intent",
				Keywords.ApplicationIntent
			},
			{
				"Application Name",
				Keywords.ApplicationName
			},
			{
				"Asynchronous Processing",
				Keywords.AsynchronousProcessing
			},
			{
				"AttachDbFilename",
				Keywords.AttachDBFilename
			},
			{
				"Pool Blocking Period",
				Keywords.PoolBlockingPeriod
			},
			{
				"Connect Timeout",
				Keywords.ConnectTimeout
			},
			{
				"Command Timeout",
				Keywords.CommandTimeout
			},
			{
				"Connection Reset",
				Keywords.ConnectionReset
			},
			{
				"Context Connection",
				Keywords.ContextConnection
			},
			{
				"Current Language",
				Keywords.CurrentLanguage
			},
			{
				"Data Source",
				Keywords.DataSource
			},
			{
				"Encrypt",
				Keywords.Encrypt
			},
			{
				"Enlist",
				Keywords.Enlist
			},
			{
				"Failover Partner",
				Keywords.FailoverPartner
			},
			{
				"Initial Catalog",
				Keywords.InitialCatalog
			},
			{
				"Integrated Security",
				Keywords.IntegratedSecurity
			},
			{
				"Load Balance Timeout",
				Keywords.LoadBalanceTimeout
			},
			{
				"Multiple Active Result Sets",
				Keywords.MultipleActiveResultSets
			},
			{
				"Max Pool Size",
				Keywords.MaxPoolSize
			},
			{
				"Min Pool Size",
				Keywords.MinPoolSize
			},
			{
				"Multi Subnet Failover",
				Keywords.MultiSubnetFailover
			},
			{
				"Transparent Network IP Resolution",
				Keywords.TransparentNetworkIPResolution
			},
			{
				"Network Library",
				Keywords.NetworkLibrary
			},
			{
				"Packet Size",
				Keywords.PacketSize
			},
			{
				"Password",
				Keywords.Password
			},
			{
				"Persist Security Info",
				Keywords.PersistSecurityInfo
			},
			{
				"Pooling",
				Keywords.Pooling
			},
			{
				"Replication",
				Keywords.Replication
			},
			{
				"Transaction Binding",
				Keywords.TransactionBinding
			},
			{
				"Trust Server Certificate",
				Keywords.TrustServerCertificate
			},
			{
				"Type System Version",
				Keywords.TypeSystemVersion
			},
			{
				"User ID",
				Keywords.UserID
			},
			{
				"User Instance",
				Keywords.UserInstance
			},
			{
				"Workstation ID",
				Keywords.WorkstationID
			},
			{
				"Connect Retry Count",
				Keywords.ConnectRetryCount
			},
			{
				"Connect Retry Interval",
				Keywords.ConnectRetryInterval
			},
			{
				"Authentication",
				Keywords.Authentication
			},
			{
				"Column Encryption Setting",
				Keywords.ColumnEncryptionSetting
			},
			{
				"Enclave Attestation Url",
				Keywords.EnclaveAttestationUrl
			},
			{
				"Attestation Protocol",
				Keywords.AttestationProtocol
			},
			{
				"IP Address Preference",
				Keywords.IPAddressPreference
			},
			{
				"ipaddresspreference",
				Keywords.IPAddressPreference
			},
			{
				"app",
				Keywords.ApplicationName
			},
			{
				"applicationintent",
				Keywords.ApplicationIntent
			},
			{
				"async",
				Keywords.AsynchronousProcessing
			},
			{
				"extended properties",
				Keywords.AttachDBFilename
			},
			{
				"initial file name",
				Keywords.AttachDBFilename
			},
			{
				"connection timeout",
				Keywords.ConnectTimeout
			},
			{
				"connectretrycount",
				Keywords.ConnectRetryCount
			},
			{
				"connectretryinterval",
				Keywords.ConnectRetryInterval
			},
			{
				"timeout",
				Keywords.ConnectTimeout
			},
			{
				"language",
				Keywords.CurrentLanguage
			},
			{
				"addr",
				Keywords.DataSource
			},
			{
				"address",
				Keywords.DataSource
			},
			{
				"multipleactiveresultsets",
				Keywords.MultipleActiveResultSets
			},
			{
				"multisubnetfailover",
				Keywords.MultiSubnetFailover
			},
			{
				"network address",
				Keywords.DataSource
			},
			{
				"poolblockingperiod",
				Keywords.PoolBlockingPeriod
			},
			{
				"server",
				Keywords.DataSource
			},
			{
				"database",
				Keywords.InitialCatalog
			},
			{
				"trusted_connection",
				Keywords.IntegratedSecurity
			},
			{
				"connection lifetime",
				Keywords.LoadBalanceTimeout
			},
			{
				"net",
				Keywords.NetworkLibrary
			},
			{
				"network",
				Keywords.NetworkLibrary
			},
			{
				"pwd",
				Keywords.Password
			},
			{
				"persistsecurityinfo",
				Keywords.PersistSecurityInfo
			},
			{
				"transparentnetworkipresolution",
				Keywords.TransparentNetworkIPResolution
			},
			{
				"trustservercertificate",
				Keywords.TrustServerCertificate
			},
			{
				"uid",
				Keywords.UserID
			},
			{
				"user",
				Keywords.UserID
			},
			{
				"wsid",
				Keywords.WorkstationID
			}
		};
	}

	public SqlConnectionStringBuilder()
		: this(null)
	{
	}

	public SqlConnectionStringBuilder(string connectionString)
	{
		if (!ADP.IsEmpty(connectionString))
		{
			base.ConnectionString = connectionString;
		}
	}

	public override void Clear()
	{
		base.Clear();
		for (int i = 0; i < _validKeywords.Length; i++)
		{
			Reset((Keywords)i);
		}
	}

	public override bool ContainsKey(string keyword)
	{
		ADP.CheckArgumentNull(keyword, "keyword");
		return _keywords.ContainsKey(keyword);
	}

	private static bool ConvertToBoolean(object value)
	{
		return DbConnectionStringBuilderUtil.ConvertToBoolean(value);
	}

	private static int ConvertToInt32(object value)
	{
		return DbConnectionStringBuilderUtil.ConvertToInt32(value);
	}

	private static bool ConvertToIntegratedSecurity(object value)
	{
		return DbConnectionStringBuilderUtil.ConvertToIntegratedSecurity(value);
	}

	private static string ConvertToString(object value)
	{
		return DbConnectionStringBuilderUtil.ConvertToString(value);
	}

	private static ApplicationIntent ConvertToApplicationIntent(string keyword, object value)
	{
		return DbConnectionStringBuilderUtil.ConvertToApplicationIntent(keyword, value);
	}

	private static SqlAuthenticationMethod ConvertToAuthenticationType(string keyword, object value)
	{
		return DbConnectionStringBuilderUtil.ConvertToAuthenticationType(keyword, value);
	}

	private static PoolBlockingPeriod ConvertToPoolBlockingPeriod(string keyword, object value)
	{
		return DbConnectionStringBuilderUtil.ConvertToPoolBlockingPeriod(keyword, value);
	}

	private static SqlConnectionColumnEncryptionSetting ConvertToColumnEncryptionSetting(string keyword, object value)
	{
		return DbConnectionStringBuilderUtil.ConvertToColumnEncryptionSetting(keyword, value);
	}

	private static SqlConnectionAttestationProtocol ConvertToAttestationProtocol(string keyword, object value)
	{
		return DbConnectionStringBuilderUtil.ConvertToAttestationProtocol(keyword, value);
	}

	private static SqlConnectionIPAddressPreference ConvertToIPAddressPreference(string keyword, object value)
	{
		return DbConnectionStringBuilderUtil.ConvertToIPAddressPreference(keyword, value);
	}

	private object GetAt(Keywords index)
	{
		return index switch
		{
			Keywords.ApplicationIntent => ApplicationIntent, 
			Keywords.ApplicationName => ApplicationName, 
			Keywords.AsynchronousProcessing => AsynchronousProcessing, 
			Keywords.AttachDBFilename => AttachDBFilename, 
			Keywords.PoolBlockingPeriod => PoolBlockingPeriod, 
			Keywords.ConnectTimeout => ConnectTimeout, 
			Keywords.CommandTimeout => CommandTimeout, 
			Keywords.ConnectionReset => ConnectionReset, 
			Keywords.ContextConnection => ContextConnection, 
			Keywords.CurrentLanguage => CurrentLanguage, 
			Keywords.DataSource => DataSource, 
			Keywords.Encrypt => Encrypt, 
			Keywords.Enlist => Enlist, 
			Keywords.FailoverPartner => FailoverPartner, 
			Keywords.InitialCatalog => InitialCatalog, 
			Keywords.IntegratedSecurity => IntegratedSecurity, 
			Keywords.LoadBalanceTimeout => LoadBalanceTimeout, 
			Keywords.MultipleActiveResultSets => MultipleActiveResultSets, 
			Keywords.MaxPoolSize => MaxPoolSize, 
			Keywords.MinPoolSize => MinPoolSize, 
			Keywords.MultiSubnetFailover => MultiSubnetFailover, 
			Keywords.TransparentNetworkIPResolution => TransparentNetworkIPResolution, 
			Keywords.NetworkLibrary => NetworkLibrary, 
			Keywords.PacketSize => PacketSize, 
			Keywords.Password => Password, 
			Keywords.PersistSecurityInfo => PersistSecurityInfo, 
			Keywords.Pooling => Pooling, 
			Keywords.Replication => Replication, 
			Keywords.TransactionBinding => TransactionBinding, 
			Keywords.TrustServerCertificate => TrustServerCertificate, 
			Keywords.TypeSystemVersion => TypeSystemVersion, 
			Keywords.UserID => UserID, 
			Keywords.UserInstance => UserInstance, 
			Keywords.WorkstationID => WorkstationID, 
			Keywords.ConnectRetryCount => ConnectRetryCount, 
			Keywords.ConnectRetryInterval => ConnectRetryInterval, 
			Keywords.Authentication => Authentication, 
			Keywords.ColumnEncryptionSetting => ColumnEncryptionSetting, 
			Keywords.EnclaveAttestationUrl => EnclaveAttestationUrl, 
			Keywords.AttestationProtocol => AttestationProtocol, 
			Keywords.IPAddressPreference => IPAddressPreference, 
			_ => throw ADP.KeywordNotSupported(_validKeywords[(int)index]), 
		};
	}

	private Keywords GetIndex(string keyword)
	{
		ADP.CheckArgumentNull(keyword, "keyword");
		if (_keywords.TryGetValue(keyword, out var value))
		{
			return value;
		}
		throw ADP.KeywordNotSupported(keyword);
	}

	public override bool Remove(string keyword)
	{
		ADP.CheckArgumentNull(keyword, "keyword");
		if (_keywords.TryGetValue(keyword, out var value) && base.Remove(_validKeywords[(int)value]))
		{
			Reset(value);
			return true;
		}
		return false;
	}

	private void Reset(Keywords index)
	{
		switch (index)
		{
		case Keywords.ApplicationIntent:
			_applicationIntent = ApplicationIntent.ReadWrite;
			break;
		case Keywords.ApplicationName:
			_applicationName = "Framework Microsoft SqlClient Data Provider";
			break;
		case Keywords.AsynchronousProcessing:
			_asynchronousProcessing = false;
			break;
		case Keywords.AttachDBFilename:
			_attachDBFilename = "";
			break;
		case Keywords.Authentication:
			_authentication = DbConnectionStringDefaults.Authentication;
			break;
		case Keywords.PoolBlockingPeriod:
			_poolBlockingPeriod = PoolBlockingPeriod.Auto;
			break;
		case Keywords.ConnectTimeout:
			_connectTimeout = 15;
			break;
		case Keywords.CommandTimeout:
			_commandTimeout = 30;
			break;
		case Keywords.ConnectionReset:
			_connectionReset = true;
			break;
		case Keywords.ContextConnection:
			_contextConnection = false;
			break;
		case Keywords.CurrentLanguage:
			_currentLanguage = "";
			break;
		case Keywords.DataSource:
			_dataSource = "";
			break;
		case Keywords.Encrypt:
			_encrypt = false;
			break;
		case Keywords.Enlist:
			_enlist = true;
			break;
		case Keywords.FailoverPartner:
			_failoverPartner = "";
			break;
		case Keywords.InitialCatalog:
			_initialCatalog = "";
			break;
		case Keywords.IntegratedSecurity:
			_integratedSecurity = false;
			break;
		case Keywords.LoadBalanceTimeout:
			_loadBalanceTimeout = 0;
			break;
		case Keywords.MultipleActiveResultSets:
			_multipleActiveResultSets = false;
			break;
		case Keywords.MaxPoolSize:
			_maxPoolSize = 100;
			break;
		case Keywords.MinPoolSize:
			_minPoolSize = 0;
			break;
		case Keywords.MultiSubnetFailover:
			_multiSubnetFailover = false;
			break;
		case Keywords.TransparentNetworkIPResolution:
			_transparentNetworkIPResolution = DbConnectionStringDefaults.TransparentNetworkIPResolution;
			break;
		case Keywords.NetworkLibrary:
			_networkLibrary = "";
			break;
		case Keywords.PacketSize:
			_packetSize = 8000;
			break;
		case Keywords.Password:
			_password = "";
			break;
		case Keywords.PersistSecurityInfo:
			_persistSecurityInfo = false;
			break;
		case Keywords.Pooling:
			_pooling = true;
			break;
		case Keywords.ConnectRetryCount:
			_connectRetryCount = 1;
			break;
		case Keywords.ConnectRetryInterval:
			_connectRetryInterval = 10;
			break;
		case Keywords.Replication:
			_replication = false;
			break;
		case Keywords.TransactionBinding:
			_transactionBinding = "Implicit Unbind";
			break;
		case Keywords.TrustServerCertificate:
			_trustServerCertificate = false;
			break;
		case Keywords.TypeSystemVersion:
			_typeSystemVersion = "Latest";
			break;
		case Keywords.UserID:
			_userID = "";
			break;
		case Keywords.UserInstance:
			_userInstance = false;
			break;
		case Keywords.WorkstationID:
			_workstationID = "";
			break;
		case Keywords.ColumnEncryptionSetting:
			_columnEncryptionSetting = DbConnectionStringDefaults.ColumnEncryptionSetting;
			break;
		case Keywords.EnclaveAttestationUrl:
			_enclaveAttestationUrl = "";
			break;
		case Keywords.AttestationProtocol:
			_attestationProtocol = SqlConnectionAttestationProtocol.NotSpecified;
			break;
		case Keywords.IPAddressPreference:
			_ipAddressPreference = SqlConnectionIPAddressPreference.IPv4First;
			break;
		default:
			throw ADP.KeywordNotSupported(_validKeywords[(int)index]);
		}
	}

	private void SetValue(string keyword, bool value)
	{
		base[keyword] = value.ToString(null);
	}

	private void SetValue(string keyword, int value)
	{
		base[keyword] = value.ToString((IFormatProvider?)null);
	}

	private void SetValue(string keyword, string value)
	{
		ADP.CheckArgumentNull(value, keyword);
		base[keyword] = value;
	}

	private void SetApplicationIntentValue(ApplicationIntent value)
	{
		base["Application Intent"] = DbConnectionStringBuilderUtil.ApplicationIntentToString(value);
	}

	private void SetPoolBlockingPeriodValue(PoolBlockingPeriod value)
	{
		base["Pool Blocking Period"] = DbConnectionStringBuilderUtil.PoolBlockingPeriodToString(value);
	}

	private void SetAuthenticationValue(SqlAuthenticationMethod value)
	{
		base["Authentication"] = DbConnectionStringBuilderUtil.AuthenticationTypeToString(value);
	}

	private void SetColumnEncryptionSettingValue(SqlConnectionColumnEncryptionSetting value)
	{
		base["Column Encryption Setting"] = DbConnectionStringBuilderUtil.ColumnEncryptionSettingToString(value);
	}

	private void SetAttestationProtocolValue(SqlConnectionAttestationProtocol value)
	{
		base["Attestation Protocol"] = DbConnectionStringBuilderUtil.AttestationProtocolToString(value);
	}

	private void SetIPAddressPreferenceValue(SqlConnectionIPAddressPreference value)
	{
		base["IP Address Preference"] = DbConnectionStringBuilderUtil.IPAddressPreferenceToString(value);
	}

	public override bool ShouldSerialize(string keyword)
	{
		ADP.CheckArgumentNull(keyword, "keyword");
		if (_keywords.TryGetValue(keyword, out var value))
		{
			return base.ShouldSerialize(_validKeywords[(int)value]);
		}
		return false;
	}

	public override bool TryGetValue(string keyword, out object value)
	{
		if (_keywords.TryGetValue(keyword, out var value2))
		{
			value = GetAt(value2);
			return true;
		}
		value = null;
		return false;
	}
}
