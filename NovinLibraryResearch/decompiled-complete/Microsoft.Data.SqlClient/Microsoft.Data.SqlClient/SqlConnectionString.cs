using System;
using System.Collections;
using System.Globalization;
using System.Security;
using System.Security.Permissions;
using Microsoft.Data.Common;

namespace Microsoft.Data.SqlClient;

internal sealed class SqlConnectionString : DbConnectionOptions
{
	internal static class DEFAULT
	{
		private const string _emptyString = "";

		internal const ApplicationIntent ApplicationIntent = ApplicationIntent.ReadWrite;

		internal const string Application_Name = "Framework Microsoft SqlClient Data Provider";

		internal const bool Asynchronous = false;

		internal const string AttachDBFilename = "";

		internal const PoolBlockingPeriod PoolBlockingPeriod = PoolBlockingPeriod.Auto;

		internal const int Command_Timeout = 30;

		internal const int Connect_Timeout = 15;

		internal const bool Connection_Reset = true;

		internal const bool Context_Connection = false;

		internal const string Current_Language = "";

		internal const string Data_Source = "";

		internal const bool Encrypt = false;

		internal const bool Enlist = true;

		internal const string FailoverPartner = "";

		internal const string Initial_Catalog = "";

		internal const bool Integrated_Security = false;

		internal const int Load_Balance_Timeout = 0;

		internal const bool MARS = false;

		internal const int Max_Pool_Size = 100;

		internal const int Min_Pool_Size = 0;

		internal const bool MultiSubnetFailover = false;

		internal static readonly bool TransparentNetworkIPResolution = DbConnectionStringDefaults.TransparentNetworkIPResolution;

		internal const string Network_Library = "";

		internal const int Packet_Size = 8000;

		internal const string Password = "";

		internal const bool Persist_Security_Info = false;

		internal const bool Pooling = true;

		internal const bool TrustServerCertificate = false;

		internal const string Type_System_Version = "";

		internal const string User_ID = "";

		internal const bool User_Instance = false;

		internal const bool Replication = false;

		internal const int Connect_Retry_Count = 1;

		internal const int Connect_Retry_Interval = 10;

		internal static readonly SqlAuthenticationMethod Authentication = SqlAuthenticationMethod.NotSpecified;

		internal static readonly SqlConnectionColumnEncryptionSetting ColumnEncryptionSetting = SqlConnectionColumnEncryptionSetting.Disabled;

		internal const string EnclaveAttestationUrl = "";

		internal static readonly SqlConnectionAttestationProtocol AttestationProtocol = SqlConnectionAttestationProtocol.NotSpecified;

		internal static readonly SqlConnectionIPAddressPreference s_IPAddressPreference = SqlConnectionIPAddressPreference.IPv4First;
	}

	internal static class KEY
	{
		internal const string ApplicationIntent = "application intent";

		internal const string Application_Name = "application name";

		internal const string AsynchronousProcessing = "asynchronous processing";

		internal const string AttachDBFilename = "attachdbfilename";

		internal const string PoolBlockingPeriod = "pool blocking period";

		internal const string ColumnEncryptionSetting = "column encryption setting";

		internal const string EnclaveAttestationUrl = "enclave attestation url";

		internal const string AttestationProtocol = "attestation protocol";

		internal const string IPAddressPreference = "ip address preference";

		internal const string Connect_Timeout = "connect timeout";

		internal const string Command_Timeout = "command timeout";

		internal const string Connection_Reset = "connection reset";

		internal const string Context_Connection = "context connection";

		internal const string Current_Language = "current language";

		internal const string Data_Source = "data source";

		internal const string Encrypt = "encrypt";

		internal const string Enlist = "enlist";

		internal const string FailoverPartner = "failover partner";

		internal const string Initial_Catalog = "initial catalog";

		internal const string Integrated_Security = "integrated security";

		internal const string Load_Balance_Timeout = "load balance timeout";

		internal const string MARS = "multiple active result sets";

		internal const string Max_Pool_Size = "max pool size";

		internal const string Min_Pool_Size = "min pool size";

		internal const string MultiSubnetFailover = "multi subnet failover";

		internal const string TransparentNetworkIPResolution = "transparent network ip resolution";

		internal const string Network_Library = "network library";

		internal const string Packet_Size = "packet size";

		internal const string Password = "password";

		internal const string Persist_Security_Info = "persist security info";

		internal const string Pooling = "pooling";

		internal const string TransactionBinding = "transaction binding";

		internal const string TrustServerCertificate = "trust server certificate";

		internal const string Type_System_Version = "type system version";

		internal const string User_ID = "user id";

		internal const string User_Instance = "user instance";

		internal const string Workstation_Id = "workstation id";

		internal const string Replication = "replication";

		internal const string Connect_Retry_Count = "connect retry count";

		internal const string Connect_Retry_Interval = "connect retry interval";

		internal const string Authentication = "authentication";
	}

	private static class SYNONYM
	{
		internal const string IPADDRESSPREFERENCE = "ipaddresspreference";

		internal const string APPLICATIONINTENT = "applicationintent";

		internal const string APP = "app";

		internal const string Async = "async";

		internal const string EXTENDED_PROPERTIES = "extended properties";

		internal const string INITIAL_FILE_NAME = "initial file name";

		internal const string CONNECTRETRYCOUNT = "connectretrycount";

		internal const string CONNECTRETRYINTERVAL = "connectretryinterval";

		internal const string CONNECTION_TIMEOUT = "connection timeout";

		internal const string TIMEOUT = "timeout";

		internal const string LANGUAGE = "language";

		internal const string ADDR = "addr";

		internal const string ADDRESS = "address";

		internal const string SERVER = "server";

		internal const string NETWORK_ADDRESS = "network address";

		internal const string DATABASE = "database";

		internal const string TRUSTED_CONNECTION = "trusted_connection";

		internal const string Connection_Lifetime = "connection lifetime";

		internal const string MULTIPLEACTIVERESULTSETS = "multipleactiveresultsets";

		internal const string MULTISUBNETFAILOVER = "multisubnetfailover";

		internal const string NET = "net";

		internal const string NETWORK = "network";

		internal const string Pwd = "pwd";

		internal const string PERSISTSECURITYINFO = "persistsecurityinfo";

		internal const string POOLBLOCKINGPERIOD = "poolblockingperiod";

		internal const string TRANSPARENTNETWORKIPRESOLUTION = "transparentnetworkipresolution";

		internal const string TRUSTSERVERCERTIFICATE = "trustservercertificate";

		internal const string UID = "uid";

		internal const string User = "user";

		internal const string WSID = "wsid";
	}

	internal static class NETLIB
	{
		internal const string AppleTalk = "dbmsadsn";

		internal const string BanyanVines = "dbmsvinn";

		internal const string IPXSPX = "dbmsspxn";

		internal const string Multiprotocol = "dbmsrpcn";

		internal const string NamedPipes = "dbnmpntw";

		internal const string SharedMemory = "dbmslpcn";

		internal const string TCPIP = "dbmssocn";

		internal const string VIA = "dbmsgnet";
	}

	internal enum TypeSystem
	{
		Latest = 2008,
		SQLServer2000 = 2000,
		SQLServer2005 = 2005,
		SQLServer2008 = 2008,
		SQLServer2012 = 2012
	}

	internal static class TYPESYSTEMVERSION
	{
		internal const string Latest = "Latest";

		internal const string SQL_Server_2000 = "SQL Server 2000";

		internal const string SQL_Server_2005 = "SQL Server 2005";

		internal const string SQL_Server_2008 = "SQL Server 2008";

		internal const string SQL_Server_2012 = "SQL Server 2012";
	}

	internal enum TransactionBindingEnum
	{
		ImplicitUnbind,
		ExplicitUnbind
	}

	internal static class TRANSACIONBINDING
	{
		internal const string ImplicitUnbind = "Implicit Unbind";

		internal const string ExplicitUnbind = "Explicit Unbind";
	}

	internal const int SynonymCount = 30;

	private static Hashtable _sqlClientSynonyms;

	private static Hashtable _netlibMapping;

	private readonly bool _integratedSecurity;

	private readonly PoolBlockingPeriod _poolBlockingPeriod;

	private readonly bool _connectionReset;

	private readonly bool _contextConnection;

	private readonly bool _encrypt;

	private readonly bool _trustServerCertificate;

	private readonly bool _enlist;

	private readonly bool _mars;

	private readonly bool _persistSecurityInfo;

	private readonly bool _pooling;

	private readonly bool _replication;

	private readonly bool _userInstance;

	private readonly bool _multiSubnetFailover;

	private readonly bool _transparentNetworkIPResolution;

	private readonly SqlAuthenticationMethod _authType;

	private readonly SqlConnectionColumnEncryptionSetting _columnEncryptionSetting;

	private readonly string _enclaveAttestationUrl;

	private readonly SqlConnectionAttestationProtocol _attestationProtocol;

	private readonly SqlConnectionIPAddressPreference _ipAddressPreference;

	private readonly int _commandTimeout;

	private readonly int _connectTimeout;

	private readonly int _loadBalanceTimeout;

	private readonly int _maxPoolSize;

	private readonly int _minPoolSize;

	private readonly int _packetSize;

	private readonly int _connectRetryCount;

	private readonly int _connectRetryInterval;

	private readonly ApplicationIntent _applicationIntent;

	private readonly string _applicationName;

	private readonly string _attachDBFileName;

	private readonly string _currentLanguage;

	private readonly string _dataSource;

	private readonly string _localDBInstance;

	private readonly string _failoverPartner;

	private readonly string _initialCatalog;

	private readonly string _password;

	private readonly string _userID;

	private readonly string _networkLibrary;

	private readonly string _workstationId;

	private readonly TypeSystem _typeSystemVersion;

	private readonly Version _typeSystemAssemblyVersion;

	private static readonly Version constTypeSystemAsmVersion10 = new Version("10.0.0.0");

	private static readonly Version constTypeSystemAsmVersion11 = new Version("11.0.0.0");

	private readonly TransactionBindingEnum _transactionBinding;

	private readonly string _expandedAttachDBFilename;

	internal bool IntegratedSecurity => _integratedSecurity;

	internal bool Asynchronous => true;

	internal PoolBlockingPeriod PoolBlockingPeriod => _poolBlockingPeriod;

	internal bool ConnectionReset => true;

	internal bool ContextConnection => _contextConnection;

	internal bool Encrypt => _encrypt;

	internal bool TrustServerCertificate => _trustServerCertificate;

	internal bool Enlist => _enlist;

	internal bool MARS => _mars;

	internal bool MultiSubnetFailover => _multiSubnetFailover;

	internal bool TransparentNetworkIPResolution => _transparentNetworkIPResolution;

	internal SqlAuthenticationMethod Authentication => _authType;

	internal SqlConnectionColumnEncryptionSetting ColumnEncryptionSetting => _columnEncryptionSetting;

	internal string EnclaveAttestationUrl => _enclaveAttestationUrl;

	internal SqlConnectionAttestationProtocol AttestationProtocol => _attestationProtocol;

	internal SqlConnectionIPAddressPreference IPAddressPreference => _ipAddressPreference;

	internal string Certificate => null;

	internal bool UsesCertificate => false;

	internal bool PersistSecurityInfo => _persistSecurityInfo;

	internal bool Pooling => _pooling;

	internal bool Replication => _replication;

	internal bool UserInstance => _userInstance;

	internal int CommandTimeout => _commandTimeout;

	internal int ConnectTimeout => _connectTimeout;

	internal int LoadBalanceTimeout => _loadBalanceTimeout;

	internal int MaxPoolSize => _maxPoolSize;

	internal int MinPoolSize => _minPoolSize;

	internal int PacketSize => _packetSize;

	internal int ConnectRetryCount => _connectRetryCount;

	internal int ConnectRetryInterval => _connectRetryInterval;

	internal ApplicationIntent ApplicationIntent => _applicationIntent;

	internal string ApplicationName => _applicationName;

	internal string AttachDBFilename => _attachDBFileName;

	internal string CurrentLanguage => _currentLanguage;

	internal string DataSource => _dataSource;

	internal string LocalDBInstance => _localDBInstance;

	internal string FailoverPartner => _failoverPartner;

	internal string InitialCatalog => _initialCatalog;

	internal string NetworkLibrary => _networkLibrary;

	internal string Password => _password;

	internal string UserID => _userID;

	internal string WorkstationId => _workstationId;

	internal TypeSystem TypeSystemVersion => _typeSystemVersion;

	internal Version TypeSystemAssemblyVersion => _typeSystemAssemblyVersion;

	internal TransactionBindingEnum TransactionBinding => _transactionBinding;

	internal bool EnforceLocalHost
	{
		get
		{
			if (_expandedAttachDBFilename != null)
			{
				return _localDBInstance == null;
			}
			return false;
		}
	}

	internal SqlConnectionString(string connectionString)
		: base(connectionString, GetParseSynonyms(), useOdbcRules: false)
	{
		bool inProc = InOutOfProcHelper.InProc;
		_integratedSecurity = ConvertValueToIntegratedSecurity();
		ConvertValueToBoolean("asynchronous processing", defaultValue: false);
		_poolBlockingPeriod = ConvertValueToPoolBlockingPeriod();
		_connectionReset = ConvertValueToBoolean("connection reset", defaultValue: true);
		_contextConnection = ConvertValueToBoolean("context connection", defaultValue: false);
		_encrypt = ConvertValueToEncrypt();
		_enlist = ConvertValueToBoolean("enlist", ADP.IsWindowsNT);
		_mars = ConvertValueToBoolean("multiple active result sets", defaultValue: false);
		_persistSecurityInfo = ConvertValueToBoolean("persist security info", defaultValue: false);
		_pooling = ConvertValueToBoolean("pooling", defaultValue: true);
		_replication = ConvertValueToBoolean("replication", defaultValue: false);
		_userInstance = ConvertValueToBoolean("user instance", defaultValue: false);
		_multiSubnetFailover = ConvertValueToBoolean("multi subnet failover", defaultValue: false);
		_transparentNetworkIPResolution = ConvertValueToBoolean("transparent network ip resolution", DEFAULT.TransparentNetworkIPResolution);
		_connectTimeout = ConvertValueToInt32("connect timeout", 15);
		_commandTimeout = ConvertValueToInt32("command timeout", 30);
		_loadBalanceTimeout = ConvertValueToInt32("load balance timeout", 0);
		_maxPoolSize = ConvertValueToInt32("max pool size", 100);
		_minPoolSize = ConvertValueToInt32("min pool size", 0);
		_packetSize = ConvertValueToInt32("packet size", 8000);
		_connectRetryCount = ConvertValueToInt32("connect retry count", 1);
		_connectRetryInterval = ConvertValueToInt32("connect retry interval", 10);
		_applicationIntent = ConvertValueToApplicationIntent();
		_applicationName = ConvertValueToString("application name", "Framework Microsoft SqlClient Data Provider");
		_attachDBFileName = ConvertValueToString("attachdbfilename", "");
		_currentLanguage = ConvertValueToString("current language", "");
		_dataSource = ConvertValueToString("data source", "");
		_localDBInstance = LocalDBAPI.GetLocalDbInstanceNameFromServerName(_dataSource);
		_failoverPartner = ConvertValueToString("failover partner", "");
		_initialCatalog = ConvertValueToString("initial catalog", "");
		_networkLibrary = ConvertValueToString("network library", null);
		_password = ConvertValueToString("password", "");
		_trustServerCertificate = ConvertValueToBoolean("trust server certificate", defaultValue: false);
		_authType = ConvertValueToAuthenticationType();
		_columnEncryptionSetting = ConvertValueToColumnEncryptionSetting();
		_enclaveAttestationUrl = ConvertValueToString("enclave attestation url", "");
		_attestationProtocol = ConvertValueToAttestationProtocol();
		_ipAddressPreference = ConvertValueToIPAddressPreference();
		string text = ConvertValueToString("type system version", null);
		string text2 = ConvertValueToString("transaction binding", null);
		_userID = ConvertValueToString("user id", "");
		_workstationId = ConvertValueToString("workstation id", null);
		if (_contextConnection)
		{
			if (!inProc)
			{
				throw SQL.ContextUnavailableOutOfProc();
			}
			foreach (DictionaryEntry item in base.Parsetable)
			{
				if ((string)item.Key != "context connection" && (string)item.Key != "type system version")
				{
					throw SQL.ContextAllowsLimitedKeywords();
				}
			}
		}
		if (!_encrypt)
		{
			object obj = ADP.LocalMachineRegistryValue("Software\\Microsoft\\MSSQLServer\\Client\\SuperSocketNetLib", "Encrypt");
			if (obj is int && 1 == (int)obj)
			{
				_encrypt = true;
			}
		}
		if (_loadBalanceTimeout < 0)
		{
			throw ADP.InvalidConnectionOptionValue("load balance timeout");
		}
		if (_connectTimeout < 0)
		{
			throw ADP.InvalidConnectionOptionValue("connect timeout");
		}
		if (_commandTimeout < 0)
		{
			throw ADP.InvalidConnectionOptionValue("command timeout");
		}
		if (_maxPoolSize < 1)
		{
			throw ADP.InvalidConnectionOptionValue("max pool size");
		}
		if (_minPoolSize < 0)
		{
			throw ADP.InvalidConnectionOptionValue("min pool size");
		}
		if (_maxPoolSize < _minPoolSize)
		{
			throw ADP.InvalidMinMaxPoolSizeValues();
		}
		if (_packetSize < 512 || 32768 < _packetSize)
		{
			throw SQL.InvalidPacketSizeValue();
		}
		if (_networkLibrary != null)
		{
			string key = _networkLibrary.Trim().ToLower(CultureInfo.InvariantCulture);
			Hashtable hashtable = NetlibMapping();
			if (!hashtable.ContainsKey(key))
			{
				throw ADP.InvalidConnectionOptionValue("network library");
			}
			_networkLibrary = (string)hashtable[key];
		}
		else
		{
			_networkLibrary = "";
		}
		ValidateValueLength(_applicationName, 128, "application name");
		ValidateValueLength(_currentLanguage, 128, "current language");
		ValidateValueLength(_dataSource, 128, "data source");
		ValidateValueLength(_failoverPartner, 128, "failover partner");
		ValidateValueLength(_initialCatalog, 128, "initial catalog");
		ValidateValueLength(_password, 128, "password");
		ValidateValueLength(_userID, 128, "user id");
		if (_workstationId != null)
		{
			ValidateValueLength(_workstationId, 128, "workstation id");
		}
		if (!string.Equals("", _failoverPartner, StringComparison.OrdinalIgnoreCase))
		{
			if (_multiSubnetFailover)
			{
				throw SQL.MultiSubnetFailoverWithFailoverPartner(serverProvidedFailoverPartner: false, null);
			}
			if (string.Equals("", _initialCatalog, StringComparison.OrdinalIgnoreCase))
			{
				throw ADP.MissingConnectionOptionValue("failover partner", "initial catalog");
			}
		}
		string datadir = null;
		_expandedAttachDBFilename = DbConnectionOptions.ExpandDataDirectory("attachdbfilename", _attachDBFileName, ref datadir);
		if (_expandedAttachDBFilename != null)
		{
			if (0 <= _expandedAttachDBFilename.IndexOf('|'))
			{
				throw ADP.InvalidConnectionOptionValue("attachdbfilename");
			}
			ValidateValueLength(_expandedAttachDBFilename, 260, "attachdbfilename");
			if (_localDBInstance == null)
			{
				string host = _dataSource;
				string protocol = _networkLibrary;
				TdsParserStaticMethods.AliasRegistryLookup(ref host, ref protocol);
				VerifyLocalHostAndFixup(ref host, enforceLocalHost: true, fixup: false);
			}
		}
		else
		{
			if (0 <= _attachDBFileName.IndexOf('|'))
			{
				throw ADP.InvalidConnectionOptionValue("attachdbfilename");
			}
			ValidateValueLength(_attachDBFileName, 260, "attachdbfilename");
		}
		_typeSystemAssemblyVersion = constTypeSystemAsmVersion10;
		if (_userInstance && !ADP.IsEmpty(_failoverPartner))
		{
			throw SQL.UserInstanceFailoverNotCompatible();
		}
		if (ADP.IsEmpty(text))
		{
			text = "Latest";
		}
		if (text.Equals("Latest", StringComparison.OrdinalIgnoreCase))
		{
			_typeSystemVersion = TypeSystem.Latest;
		}
		else if (text.Equals("SQL Server 2000", StringComparison.OrdinalIgnoreCase))
		{
			if (_contextConnection)
			{
				throw SQL.ContextAllowsOnlyTypeSystem2005();
			}
			_typeSystemVersion = TypeSystem.SQLServer2000;
		}
		else if (text.Equals("SQL Server 2005", StringComparison.OrdinalIgnoreCase))
		{
			_typeSystemVersion = TypeSystem.SQLServer2005;
		}
		else if (text.Equals("SQL Server 2008", StringComparison.OrdinalIgnoreCase))
		{
			_typeSystemVersion = TypeSystem.Latest;
		}
		else
		{
			if (!text.Equals("SQL Server 2012", StringComparison.OrdinalIgnoreCase))
			{
				throw ADP.InvalidConnectionOptionValue("type system version");
			}
			_typeSystemVersion = TypeSystem.SQLServer2012;
			_typeSystemAssemblyVersion = constTypeSystemAsmVersion11;
		}
		if (ADP.IsEmpty(text2))
		{
			text2 = "Implicit Unbind";
		}
		if (text2.Equals("Implicit Unbind", StringComparison.OrdinalIgnoreCase))
		{
			_transactionBinding = TransactionBindingEnum.ImplicitUnbind;
		}
		else
		{
			if (!text2.Equals("Explicit Unbind", StringComparison.OrdinalIgnoreCase))
			{
				throw ADP.InvalidConnectionOptionValue("transaction binding");
			}
			_transactionBinding = TransactionBindingEnum.ExplicitUnbind;
		}
		if (_connectRetryCount < 0 || _connectRetryCount > 255)
		{
			throw ADP.InvalidConnectRetryCountValue();
		}
		if (_connectRetryInterval < 1 || _connectRetryInterval > 60)
		{
			throw ADP.InvalidConnectRetryIntervalValue();
		}
		if (Authentication != 0 && _integratedSecurity)
		{
			throw SQL.AuthenticationAndIntegratedSecurity();
		}
		if (Authentication == SqlAuthenticationMethod.ActiveDirectoryIntegrated && (HasUserIdKeyword || HasPasswordKeyword))
		{
			throw SQL.IntegratedWithUserIDAndPassword();
		}
		if (Authentication == SqlAuthenticationMethod.ActiveDirectoryInteractive && HasPasswordKeyword)
		{
			throw SQL.InteractiveWithPassword();
		}
		if (Authentication == SqlAuthenticationMethod.ActiveDirectoryDeviceCodeFlow && (HasUserIdKeyword || HasPasswordKeyword))
		{
			throw SQL.DeviceFlowWithUsernamePassword();
		}
		if (Authentication == SqlAuthenticationMethod.ActiveDirectoryManagedIdentity && HasPasswordKeyword)
		{
			throw SQL.NonInteractiveWithPassword("Active Directory Managed Identity");
		}
		if (Authentication == SqlAuthenticationMethod.ActiveDirectoryMSI && HasPasswordKeyword)
		{
			throw SQL.NonInteractiveWithPassword("Active Directory MSI");
		}
		if (Authentication == SqlAuthenticationMethod.ActiveDirectoryDefault && HasPasswordKeyword)
		{
			throw SQL.NonInteractiveWithPassword("Active Directory Default");
		}
	}

	internal SqlConnectionString(SqlConnectionString connectionOptions, string dataSource, bool userInstance, bool? setEnlistValue)
		: base(connectionOptions)
	{
		_integratedSecurity = connectionOptions._integratedSecurity;
		_connectionReset = connectionOptions._connectionReset;
		_contextConnection = connectionOptions._contextConnection;
		_encrypt = connectionOptions._encrypt;
		if (setEnlistValue.HasValue)
		{
			_enlist = setEnlistValue.Value;
		}
		else
		{
			_enlist = connectionOptions._enlist;
		}
		_mars = connectionOptions._mars;
		_persistSecurityInfo = connectionOptions._persistSecurityInfo;
		_pooling = connectionOptions._pooling;
		_replication = connectionOptions._replication;
		_userInstance = userInstance;
		_commandTimeout = connectionOptions._commandTimeout;
		_connectTimeout = connectionOptions._connectTimeout;
		_loadBalanceTimeout = connectionOptions._loadBalanceTimeout;
		_poolBlockingPeriod = connectionOptions._poolBlockingPeriod;
		_maxPoolSize = connectionOptions._maxPoolSize;
		_minPoolSize = connectionOptions._minPoolSize;
		_multiSubnetFailover = connectionOptions._multiSubnetFailover;
		_transparentNetworkIPResolution = connectionOptions._transparentNetworkIPResolution;
		_packetSize = connectionOptions._packetSize;
		_applicationName = connectionOptions._applicationName;
		_attachDBFileName = connectionOptions._attachDBFileName;
		_currentLanguage = connectionOptions._currentLanguage;
		_dataSource = dataSource;
		_localDBInstance = LocalDBAPI.GetLocalDbInstanceNameFromServerName(_dataSource);
		_failoverPartner = connectionOptions._failoverPartner;
		_initialCatalog = connectionOptions._initialCatalog;
		_password = connectionOptions._password;
		_userID = connectionOptions._userID;
		_networkLibrary = connectionOptions._networkLibrary;
		_workstationId = connectionOptions._workstationId;
		_expandedAttachDBFilename = connectionOptions._expandedAttachDBFilename;
		_typeSystemVersion = connectionOptions._typeSystemVersion;
		_typeSystemAssemblyVersion = connectionOptions._typeSystemAssemblyVersion;
		_transactionBinding = connectionOptions._transactionBinding;
		_applicationIntent = connectionOptions._applicationIntent;
		_connectRetryCount = connectionOptions._connectRetryCount;
		_connectRetryInterval = connectionOptions._connectRetryInterval;
		_authType = connectionOptions._authType;
		_columnEncryptionSetting = connectionOptions._columnEncryptionSetting;
		_enclaveAttestationUrl = connectionOptions._enclaveAttestationUrl;
		_attestationProtocol = connectionOptions._attestationProtocol;
		ValidateValueLength(_dataSource, 128, "data source");
	}

	protected internal override PermissionSet CreatePermissionSet()
	{
		PermissionSet permissionSet = new PermissionSet(PermissionState.None);
		permissionSet.AddPermission((IPermission?)(object)new SqlClientPermission(this));
		return permissionSet;
	}

	protected internal override string Expand()
	{
		if (_expandedAttachDBFilename != null)
		{
			return ExpandKeyword("attachdbfilename", _expandedAttachDBFilename);
		}
		return base.Expand();
	}

	private static bool CompareHostName(ref string host, string name, bool fixup)
	{
		bool result = false;
		if (host.Equals(name, StringComparison.OrdinalIgnoreCase))
		{
			if (fixup)
			{
				host = ".";
			}
			result = true;
		}
		else if (host.StartsWith(name + "\\", StringComparison.OrdinalIgnoreCase))
		{
			if (fixup)
			{
				host = "." + host.Substring(name.Length);
			}
			result = true;
		}
		return result;
	}

	internal static Hashtable GetParseSynonyms()
	{
		Hashtable hashtable = _sqlClientSynonyms;
		if (hashtable == null)
		{
			hashtable = new Hashtable(71);
			hashtable.Add("application intent", "application intent");
			hashtable.Add("application name", "application name");
			hashtable.Add("asynchronous processing", "asynchronous processing");
			hashtable.Add("attachdbfilename", "attachdbfilename");
			hashtable.Add("pool blocking period", "pool blocking period");
			hashtable.Add("connect timeout", "connect timeout");
			hashtable.Add("command timeout", "command timeout");
			hashtable.Add("connection reset", "connection reset");
			hashtable.Add("context connection", "context connection");
			hashtable.Add("current language", "current language");
			hashtable.Add("data source", "data source");
			hashtable.Add("encrypt", "encrypt");
			hashtable.Add("enlist", "enlist");
			hashtable.Add("failover partner", "failover partner");
			hashtable.Add("initial catalog", "initial catalog");
			hashtable.Add("integrated security", "integrated security");
			hashtable.Add("load balance timeout", "load balance timeout");
			hashtable.Add("multiple active result sets", "multiple active result sets");
			hashtable.Add("max pool size", "max pool size");
			hashtable.Add("min pool size", "min pool size");
			hashtable.Add("multi subnet failover", "multi subnet failover");
			hashtable.Add("transparent network ip resolution", "transparent network ip resolution");
			hashtable.Add("network library", "network library");
			hashtable.Add("packet size", "packet size");
			hashtable.Add("password", "password");
			hashtable.Add("persist security info", "persist security info");
			hashtable.Add("pooling", "pooling");
			hashtable.Add("replication", "replication");
			hashtable.Add("trust server certificate", "trust server certificate");
			hashtable.Add("transaction binding", "transaction binding");
			hashtable.Add("type system version", "type system version");
			hashtable.Add("column encryption setting", "column encryption setting");
			hashtable.Add("enclave attestation url", "enclave attestation url");
			hashtable.Add("attestation protocol", "attestation protocol");
			hashtable.Add("user id", "user id");
			hashtable.Add("user instance", "user instance");
			hashtable.Add("workstation id", "workstation id");
			hashtable.Add("connect retry count", "connect retry count");
			hashtable.Add("connect retry interval", "connect retry interval");
			hashtable.Add("authentication", "authentication");
			hashtable.Add("ip address preference", "ip address preference");
			hashtable.Add("applicationintent", "application intent");
			hashtable.Add("app", "application name");
			hashtable.Add("async", "asynchronous processing");
			hashtable.Add("extended properties", "attachdbfilename");
			hashtable.Add("initial file name", "attachdbfilename");
			hashtable.Add("connection timeout", "connect timeout");
			hashtable.Add("connectretrycount", "connect retry count");
			hashtable.Add("connectretryinterval", "connect retry interval");
			hashtable.Add("timeout", "connect timeout");
			hashtable.Add("language", "current language");
			hashtable.Add("addr", "data source");
			hashtable.Add("address", "data source");
			hashtable.Add("multipleactiveresultsets", "multiple active result sets");
			hashtable.Add("multisubnetfailover", "multi subnet failover");
			hashtable.Add("network address", "data source");
			hashtable.Add("server", "data source");
			hashtable.Add("database", "initial catalog");
			hashtable.Add("trusted_connection", "integrated security");
			hashtable.Add("connection lifetime", "load balance timeout");
			hashtable.Add("net", "network library");
			hashtable.Add("network", "network library");
			hashtable.Add("pwd", "password");
			hashtable.Add("poolblockingperiod", "pool blocking period");
			hashtable.Add("persistsecurityinfo", "persist security info");
			hashtable.Add("transparentnetworkipresolution", "transparent network ip resolution");
			hashtable.Add("trustservercertificate", "trust server certificate");
			hashtable.Add("uid", "user id");
			hashtable.Add("user", "user id");
			hashtable.Add("wsid", "workstation id");
			hashtable.Add("ipaddresspreference", "ip address preference");
			_sqlClientSynonyms = hashtable;
		}
		return hashtable;
	}

	internal string ObtainWorkstationId()
	{
		string text = WorkstationId;
		if (text == null)
		{
			text = ADP.MachineName();
			ValidateValueLength(text, 128, "workstation id");
		}
		return text;
	}

	internal static Hashtable NetlibMapping()
	{
		Hashtable hashtable = _netlibMapping;
		if (hashtable == null)
		{
			hashtable = new Hashtable(8);
			hashtable.Add("dbmssocn", "tcp");
			hashtable.Add("dbnmpntw", "np");
			hashtable.Add("dbmsrpcn", "rpc");
			hashtable.Add("dbmsvinn", "bv");
			hashtable.Add("dbmsadsn", "adsp");
			hashtable.Add("dbmsspxn", "spx");
			hashtable.Add("dbmsgnet", "via");
			hashtable.Add("dbmslpcn", "lpc");
			_netlibMapping = hashtable;
		}
		return hashtable;
	}

	internal static bool ValidProtocol(string protocol)
	{
		switch (protocol)
		{
		case "tcp":
		case "np":
		case "via":
		case "lpc":
			return true;
		default:
			return false;
		}
	}

	private void ValidateValueLength(string value, int limit, string key)
	{
		if (limit < value.Length)
		{
			throw ADP.InvalidConnectionOptionValueLength(key, limit);
		}
	}

	internal static void VerifyLocalHostAndFixup(ref string host, bool enforceLocalHost, bool fixup)
	{
		if (ADP.IsEmpty(host))
		{
			if (fixup)
			{
				host = ".";
			}
		}
		else
		{
			if (CompareHostName(ref host, ".", fixup) || CompareHostName(ref host, "(local)", fixup))
			{
				return;
			}
			string computerNameDnsFullyQualified = ADP.GetComputerNameDnsFullyQualified();
			if (!CompareHostName(ref host, computerNameDnsFullyQualified, fixup))
			{
				int num = computerNameDnsFullyQualified.IndexOf('.');
				if ((num <= 0 || !CompareHostName(ref host, computerNameDnsFullyQualified.Substring(0, num), fixup)) && enforceLocalHost)
				{
					throw ADP.InvalidConnectionOptionValue("attachdbfilename");
				}
			}
		}
	}

	internal ApplicationIntent ConvertValueToApplicationIntent()
	{
		object obj = base.Parsetable["application intent"];
		if (obj == null)
		{
			return ApplicationIntent.ReadWrite;
		}
		try
		{
			return DbConnectionStringBuilderUtil.ConvertToApplicationIntent("application intent", obj);
		}
		catch (FormatException inner)
		{
			throw ADP.InvalidConnectionOptionValue("application intent", inner);
		}
		catch (OverflowException inner2)
		{
			throw ADP.InvalidConnectionOptionValue("application intent", inner2);
		}
	}

	internal PoolBlockingPeriod ConvertValueToPoolBlockingPeriod()
	{
		object obj = base.Parsetable["pool blocking period"];
		if (obj == null)
		{
			return PoolBlockingPeriod.Auto;
		}
		try
		{
			return DbConnectionStringBuilderUtil.ConvertToPoolBlockingPeriod("pool blocking period", obj);
		}
		catch (FormatException inner)
		{
			throw ADP.InvalidConnectionOptionValue("pool blocking period", inner);
		}
		catch (OverflowException inner2)
		{
			throw ADP.InvalidConnectionOptionValue("pool blocking period", inner2);
		}
	}

	internal SqlAuthenticationMethod ConvertValueToAuthenticationType()
	{
		object obj = base.Parsetable["authentication"];
		if (!(obj is string value))
		{
			return DEFAULT.Authentication;
		}
		try
		{
			return DbConnectionStringBuilderUtil.ConvertToAuthenticationType("authentication", value);
		}
		catch (FormatException inner)
		{
			throw ADP.InvalidConnectionOptionValue("authentication", inner);
		}
		catch (OverflowException inner2)
		{
			throw ADP.InvalidConnectionOptionValue("authentication", inner2);
		}
	}

	internal SqlConnectionColumnEncryptionSetting ConvertValueToColumnEncryptionSetting()
	{
		object obj = base.Parsetable["column encryption setting"];
		if (!(obj is string value))
		{
			return DEFAULT.ColumnEncryptionSetting;
		}
		try
		{
			return DbConnectionStringBuilderUtil.ConvertToColumnEncryptionSetting("column encryption setting", value);
		}
		catch (FormatException inner)
		{
			throw ADP.InvalidConnectionOptionValue("column encryption setting", inner);
		}
		catch (OverflowException inner2)
		{
			throw ADP.InvalidConnectionOptionValue("column encryption setting", inner2);
		}
	}

	internal SqlConnectionAttestationProtocol ConvertValueToAttestationProtocol()
	{
		object obj = base.Parsetable["attestation protocol"];
		if (!(obj is string value))
		{
			return DEFAULT.AttestationProtocol;
		}
		try
		{
			return DbConnectionStringBuilderUtil.ConvertToAttestationProtocol("attestation protocol", value);
		}
		catch (FormatException inner)
		{
			throw ADP.InvalidConnectionOptionValue("attestation protocol", inner);
		}
		catch (OverflowException inner2)
		{
			throw ADP.InvalidConnectionOptionValue("attestation protocol", inner2);
		}
	}

	internal SqlConnectionIPAddressPreference ConvertValueToIPAddressPreference()
	{
		object obj = base.Parsetable["ip address preference"];
		if (!(obj is string value))
		{
			return DEFAULT.s_IPAddressPreference;
		}
		try
		{
			return DbConnectionStringBuilderUtil.ConvertToIPAddressPreference("ip address preference", value);
		}
		catch (FormatException inner)
		{
			throw ADP.InvalidConnectionOptionValue("ip address preference", inner);
		}
		catch (OverflowException inner2)
		{
			throw ADP.InvalidConnectionOptionValue("ip address preference", inner2);
		}
	}

	internal bool ConvertValueToEncrypt()
	{
		object obj = base.Parsetable["authentication"];
		bool defaultValue = obj != null;
		return ConvertValueToBoolean("encrypt", defaultValue);
	}
}
