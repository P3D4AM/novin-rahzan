#define TRACE
using System;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Xml;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;

namespace Microsoft.SqlServer.Management.RegisteredServers;

public sealed class RegisteredServersStore : ServerGroupParent, ISfcDomain, ISfcDomainLite, ISfcHasConnection
{
	public delegate bool ExceptionDelegate(object obj);

	public sealed class Key : DomainRootKey
	{
		public Key()
			: base(null)
		{
		}

		internal Key(ISfcDomain root)
			: base(root)
		{
		}

		public override int GetHashCode()
		{
			return GetType().GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return this == obj;
		}

		public new static bool Equals(object obj1, object obj2)
		{
			return obj1 as Key == obj2 as Key;
		}

		public override bool Equals(SfcKey key)
		{
			return this == key;
		}

		public static bool operator ==(object obj, Key rightOperand)
		{
			if (obj == null || obj is Key)
			{
				return (Key)obj == rightOperand;
			}
			return false;
		}

		public static bool operator ==(Key leftOperand, object obj)
		{
			if (obj == null || obj is Key)
			{
				return leftOperand == (Key)obj;
			}
			return false;
		}

		public static bool operator ==(Key leftOperand, Key rightOperand)
		{
			if ((object)leftOperand == (object)rightOperand)
			{
				return true;
			}
			if ((object)leftOperand == null || (object)rightOperand == null)
			{
				return false;
			}
			return leftOperand.IsEqual(rightOperand);
		}

		public static bool operator !=(object obj, Key rightOperand)
		{
			return !(obj == rightOperand);
		}

		public static bool operator !=(Key leftOperand, object obj)
		{
			return !(leftOperand == obj);
		}

		public static bool operator !=(Key leftOperand, Key rightOperand)
		{
			return !(leftOperand == rightOperand);
		}

		private bool IsEqual(Key other)
		{
			return base.Domain == other.Domain;
		}

		public override string GetUrnFragment()
		{
			return "RegisteredServersStore";
		}
	}

	private static RegisteredServersStore localFileStore;

	private static object lockObject;

	private bool isSerializeOnCreation = true;

	public static ExceptionDelegate ExceptionDelegates;

	private bool useStoreCredentialPersistenceType;

	private CredentialPersistenceType credentialPersistenceType;

	internal static readonly string databaseEngineServerGroupName;

	internal static readonly string analysisServicesServerGroupName;

	internal static readonly string reportingServicesServerGroupName;

	internal static readonly string integrationServicesServerGroupName;

	internal static readonly string sqlServerCompactEditionServerGroupName;

	internal static readonly string centralManagementServerGroupName;

	private ServerGroupCollection serverGroups;

	private ServerGroup databaseEngineServerGroup;

	private ServerGroup centralManagementServerGroup;

	private AzureDataStudioConnectionStore azureDataStudioConnectionStore;

	private ServerGroup analysisServicesServerGroup;

	private ServerGroup reportingServicesServerGroup;

	private ServerGroup integrationServicesServerGroup;

	private ServerGroup sqlServerCompactEditionServerGroup;

	private ServerConnection serverConnection;

	private string localXmlStorageFile;

	private static NopExecutionEngine nopExecutionEngine;

	private SfcConnectionContext connectionContext;

	internal const string typeName = "RegisteredServersStore";

	private const string savePathSuffix = "\\Microsoft\\SQL Server Management Studio";

	private const string savePathLegacySuffixFormat = "\\Microsoft\\Microsoft SQL Server\\{0}\\Tools\\Shell";

	private const string regSqlServerFileName = "RegSrvr.xml";

	private const string DatabaseEngineServerTypeGuid = "8c91a03d-f9b4-46c0-a305-b5dcc79ff907";

	private const string SqlServerCompactEditionServerTypeGuid = "6b04a4a7-9b37-4028-ac2d-f0a39e50fb57";

	private const string AnalysisServicesServerTypeGuid = "1396ffcb-10d7-4f8c-aaef-4696d541f554";

	private const string IntegrationServicesServerTypeGuid = "19d20860-9e9a-4aff-a80d-f72b41b5e931";

	private const string ReportingServicesServerTypeGuid = "3a0f2e46-847b-4332-9b7e-fc78e43a49b0";

	public static RegisteredServersStore LocalFileStore
	{
		get
		{
			lock (lockObject)
			{
				if (localFileStore == null)
				{
					localFileStore = InitializeLocalRegisteredServersStore(null);
				}
			}
			return localFileStore;
		}
	}

	internal bool UseStoreCredentialPersistenceType
	{
		get
		{
			return useStoreCredentialPersistenceType;
		}
		set
		{
			useStoreCredentialPersistenceType = value;
		}
	}

	internal CredentialPersistenceType CredentialPersistenceType
	{
		get
		{
			return credentialPersistenceType;
		}
		set
		{
			credentialPersistenceType = value;
		}
	}

	[SfcIgnore]
	public bool IsLocal => LocalXmlStorageFile != null;

	[SfcIgnore]
	public string DisplayName
	{
		get
		{
			if (!IsLocal)
			{
				return RegSvrStrings.CentralManagementServersDisplayName;
			}
			return RegSvrStrings.LocalServerStoreDisplayName;
		}
	}

	public static string LocalServerStoreDisplayName => RegSvrStrings.LocalServerStoreDisplayName;

	public static string CentralManagementServersDisplayName => RegSvrStrings.CentralManagementServersDisplayName;

	[SfcIgnore]
	internal bool IsSerializeOnCreation => isSerializeOnCreation;

	public string DatabaseEngineServerGroupName => databaseEngineServerGroupName;

	public string AnalysisServicesServerGroupName => analysisServicesServerGroupName;

	public string ReportingServicesServerGroupName => reportingServicesServerGroupName;

	public string IntegrationServicesServerGroupName => integrationServicesServerGroupName;

	public string SqlServerCompactEditionServerGroupName => sqlServerCompactEditionServerGroupName;

	public string CentralManagementServerGroupName => centralManagementServerGroupName;

	[SfcObject(SfcContainerRelationship.ChildContainer, SfcContainerCardinality.ZeroToAny, typeof(ServerGroup))]
	public ServerGroupCollection ServerGroups
	{
		get
		{
			if (serverGroups == null)
			{
				serverGroups = new ServerGroupCollection(this, IsLocal ? null : new ServerComparer(ServerConnection, "msdb"));
			}
			return serverGroups;
		}
	}

	[SfcIgnore]
	public ServerGroup DatabaseEngineServerGroup
	{
		get
		{
			if (databaseEngineServerGroup == null)
			{
				databaseEngineServerGroup = ServerGroups[DatabaseEngineServerGroupName];
				if (databaseEngineServerGroup == null && IsLocal)
				{
					databaseEngineServerGroup = new ServerGroup(this, DatabaseEngineServerGroupName);
					databaseEngineServerGroup.ServerType = ServerType.DatabaseEngine;
					databaseEngineServerGroup.Create();
				}
			}
			return databaseEngineServerGroup;
		}
	}

	[SfcIgnore]
	public ServerGroup CentralManagementServerGroup
	{
		get
		{
			if (!IsLocal)
			{
				return null;
			}
			if (centralManagementServerGroup == null)
			{
				centralManagementServerGroup = ServerGroups[CentralManagementServerGroupName];
				if (centralManagementServerGroup == null)
				{
					centralManagementServerGroup = new ServerGroup(this, CentralManagementServerGroupName);
					centralManagementServerGroup.ServerType = ServerType.DatabaseEngine;
					centralManagementServerGroup.Create();
				}
			}
			return centralManagementServerGroup;
		}
	}

	[SfcIgnore]
	public AzureDataStudioConnectionStore AzureDataStudioConnectionStore
	{
		get
		{
			if (!IsLocal)
			{
				return null;
			}
			return azureDataStudioConnectionStore ?? (azureDataStudioConnectionStore = AzureDataStudioConnectionStore.LoadAzureDataStudioConnections());
		}
	}

	[SfcIgnore]
	public ServerGroup AnalysisServicesServerGroup
	{
		get
		{
			if (analysisServicesServerGroup == null)
			{
				analysisServicesServerGroup = ServerGroups[AnalysisServicesServerGroupName];
				if (analysisServicesServerGroup == null && IsLocal)
				{
					analysisServicesServerGroup = new ServerGroup(this, AnalysisServicesServerGroupName);
					analysisServicesServerGroup.ServerType = ServerType.AnalysisServices;
					analysisServicesServerGroup.Create();
				}
			}
			return analysisServicesServerGroup;
		}
	}

	[SfcIgnore]
	public ServerGroup ReportingServicesServerGroup
	{
		get
		{
			if (reportingServicesServerGroup == null)
			{
				reportingServicesServerGroup = ServerGroups[ReportingServicesServerGroupName];
				if (reportingServicesServerGroup == null && IsLocal)
				{
					reportingServicesServerGroup = new ServerGroup(this, ReportingServicesServerGroupName);
					reportingServicesServerGroup.ServerType = ServerType.ReportingServices;
					reportingServicesServerGroup.Create();
				}
			}
			return reportingServicesServerGroup;
		}
	}

	[SfcIgnore]
	public ServerGroup IntegrationServicesServerGroup
	{
		get
		{
			if (integrationServicesServerGroup == null)
			{
				integrationServicesServerGroup = ServerGroups[IntegrationServicesServerGroupName];
				if (integrationServicesServerGroup == null && IsLocal)
				{
					integrationServicesServerGroup = new ServerGroup(this, IntegrationServicesServerGroupName);
					integrationServicesServerGroup.ServerType = ServerType.IntegrationServices;
					integrationServicesServerGroup.Create();
				}
			}
			return integrationServicesServerGroup;
		}
	}

	[SfcIgnore]
	public ServerGroup SqlServerCompactEditionServerGroup
	{
		get
		{
			if (sqlServerCompactEditionServerGroup == null)
			{
				sqlServerCompactEditionServerGroup = ServerGroups[SqlServerCompactEditionServerGroupName];
				if (sqlServerCompactEditionServerGroup == null && IsLocal)
				{
					sqlServerCompactEditionServerGroup = new ServerGroup(this, SqlServerCompactEditionServerGroupName);
					sqlServerCompactEditionServerGroup.ServerType = ServerType.SqlServerEverywhere;
					sqlServerCompactEditionServerGroup.Create();
				}
			}
			return sqlServerCompactEditionServerGroup;
		}
	}

	[SfcIgnore]
	public ServerConnection ServerConnection => serverConnection;

	private string LocalXmlStorageFile => localXmlStorageFile;

	[SfcIgnore]
	public string DomainInstanceName
	{
		get
		{
			if (ServerConnection == null)
			{
				Trace.Assert(!string.IsNullOrEmpty(LocalXmlStorageFile));
				return LocalXmlStorageFile;
			}
			return ServerConnection.TrueName;
		}
	}

	[SfcIgnore]
	public string DomainName => "RegisteredServers";

	internal static NopExecutionEngine NopExecutionEngine => nopExecutionEngine;

	SfcConnectionContext ISfcHasConnection.ConnectionContext
	{
		get
		{
			if (connectionContext == null)
			{
				connectionContext = new SfcConnectionContext(this);
			}
			return connectionContext;
		}
	}

	public static event EventHandler LocalFileStoreReloaded;

	public RegisteredServersStore()
	{
		localXmlStorageFile = string.Empty;
	}

	public RegisteredServersStore(ServerConnection sharedRegisteredServersStoreConnection)
	{
		localXmlStorageFile = null;
		serverConnection = sharedRegisteredServersStoreConnection;
		MarkRootAsConnected();
	}

	public static void ReloadLocalFileStore()
	{
		lock (lockObject)
		{
			localFileStore = null;
		}
		if (RegisteredServersStore.LocalFileStoreReloaded != null)
		{
			RegisteredServersStore.LocalFileStoreReloaded(null, EventArgs.Empty);
		}
	}

	public static RegisteredServersStore InitializeLocalRegisteredServersStore(string registeredServersXmlFile)
	{
		string text = registeredServersXmlFile ?? GetLocalXmlFilePath();
		if (registeredServersXmlFile == null && !File.Exists(text))
		{
			MigrateLocalXmlFileFromLegacySSMS(text);
		}
		RegisteredServersStore registeredServersStore = InitChildObjects(text);
		registeredServersStore.localXmlStorageFile = text;
		return registeredServersStore;
	}

	private static void MigrateLocalXmlFileFromLegacySSMS(string pathToMigratedLocalXmlFile)
	{
		int[] array = new int[5] { 140, 130, 120, 110, 100 };
		foreach (int num in array)
		{
			string text = Path.Combine(GetSettingsDir($"\\Microsoft\\Microsoft SQL Server\\{num}\\Tools\\Shell"), "RegSrvr.xml");
			if (TryDeserializeLocalXmlFile(text, out var _, out var _))
			{
				try
				{
					new FileInfo(pathToMigratedLocalXmlFile).Directory.Create();
					File.Copy(text, pathToMigratedLocalXmlFile);
					break;
				}
				catch
				{
					break;
				}
			}
		}
	}

	private static bool TryDeserializeLocalXmlFile(string pathToLocalXmlFile, out RegisteredServersStore registeredServersStore, out Exception exception)
	{
		registeredServersStore = null;
		exception = null;
		if (File.Exists(pathToLocalXmlFile))
		{
			using XmlTextReader xmlReader = new XmlTextReader(pathToLocalXmlFile)
			{
				DtdProcessing = DtdProcessing.Prohibit
			};
			SfcSerializer sfcSerializer = new SfcSerializer();
			try
			{
				registeredServersStore = (RegisteredServersStore)sfcSerializer.Deserialize(xmlReader, SfcObjectState.Existing);
				return true;
			}
			catch (Exception ex)
			{
				exception = ex;
			}
		}
		return false;
	}

	private static RegisteredServersStore InitChildObjects(string file)
	{
		if (TryDeserializeLocalXmlFile(file, out var registeredServersStore, out var exception))
		{
			return registeredServersStore;
		}
		if (exception != null)
		{
			RegisteredServerException ex = new RegisteredServerException(RegSvrStrings.FailedToDeserialize, exception);
			if (ExceptionDelegates == null || !ExceptionDelegates(ex))
			{
				throw ex;
			}
		}
		return new RegisteredServersStore();
	}

	public void UpgradeFromSqlServer2005(string downlevelFile)
	{
		try
		{
			if (downlevelFile == null)
			{
				throw new ArgumentNullException(downlevelFile);
			}
			if (!IsLocal)
			{
				throw new RegisteredServerException(RegSvrStrings.LocalStoreOnly);
			}
			using XmlReader reader = XmlReader.Create(downlevelFile);
			InitFromSqlServer2005Store(reader);
			Serialize();
		}
		catch (Exception ex)
		{
			FilterException(ex);
			throw new RegisteredServerException(RegSvrStrings.FailedOperation(RegSvrStrings.Upgrade), ex);
		}
	}

	private void InitFromSqlServer2005Store(XmlReader reader)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(reader);
		if (xmlDocument.FirstChild.NodeType != XmlNodeType.XmlDeclaration)
		{
			throw new InvalidSqlServer2005StoreFormatException(RegSvrStrings.InvalidSqlServer2005FileFormat(xmlDocument.FirstChild.Name));
		}
		if (xmlDocument.FirstChild.NextSibling == null || xmlDocument.FirstChild.NextSibling != xmlDocument.LastChild)
		{
			throw new InvalidSqlServer2005StoreFormatException(RegSvrStrings.InvalidSqlServer2005FileFormat(xmlDocument.LastChild.Name));
		}
		XmlNode nextSibling = xmlDocument.FirstChild.NextSibling;
		if (nextSibling.NodeType != XmlNodeType.Element || nextSibling.Name != "RegisteredServers")
		{
			throw new InvalidSqlServer2005StoreFormatException(RegSvrStrings.InvalidSqlServer2005FileFormat(nextSibling.Name));
		}
		foreach (XmlNode childNode in nextSibling.ChildNodes)
		{
			if (childNode.NodeType != XmlNodeType.Element || childNode.Name != "ServerType")
			{
				throw new InvalidSqlServer2005StoreFormatException(RegSvrStrings.InvalidSqlServer2005FileFormat(childNode.Name));
			}
			string text = ReadXmlAttribute(childNode, "id", throwIfMissing: true) as string;
			ServerGroup serverGroup = null;
			serverGroup = text switch
			{
				"8c91a03d-f9b4-46c0-a305-b5dcc79ff907" => DatabaseEngineServerGroup, 
				"1396ffcb-10d7-4f8c-aaef-4696d541f554" => AnalysisServicesServerGroup, 
				"3a0f2e46-847b-4332-9b7e-fc78e43a49b0" => ReportingServicesServerGroup, 
				"19d20860-9e9a-4aff-a80d-f72b41b5e931" => IntegrationServicesServerGroup, 
				"6b04a4a7-9b37-4028-ac2d-f0a39e50fb57" => SqlServerCompactEditionServerGroup, 
				_ => throw new InvalidSqlServer2005StoreFormatException(RegSvrStrings.InvalidSqlServer2005FileFormat(childNode.Name)), 
			};
			Trace.Assert(serverGroup != null);
			try
			{
				isSerializeOnCreation = false;
				foreach (XmlNode childNode2 in childNode.ChildNodes)
				{
					AddGroupMember(serverGroup, childNode2);
				}
			}
			finally
			{
				isSerializeOnCreation = true;
			}
		}
	}

	private void AddGroupMember(ServerGroup group, XmlNode member)
	{
		Trace.Assert(group != null);
		Trace.Assert(member != null);
		if (member.NodeType != XmlNodeType.Element || (member.Name != "Group" && member.Name != "Server"))
		{
			throw new InvalidSqlServer2005StoreFormatException(RegSvrStrings.InvalidSqlServer2005FileFormat(member.Name));
		}
		if (member.Name == "Group")
		{
			string name = ReadXmlAttribute(member, "name", throwIfMissing: true) as string;
			bool flag = false;
			ServerGroup serverGroup = group.ServerGroups[name];
			if (serverGroup == null)
			{
				flag = true;
				serverGroup = new ServerGroup(group, name);
			}
			if (ReadXmlAttribute(member, "description", throwIfMissing: false) is string description)
			{
				serverGroup.Description = description;
			}
			if (flag)
			{
				serverGroup.Create();
			}
			else
			{
				serverGroup.Validate(ValidationMethod.Alter, throwOnFirst: true, new ValidationState());
				SfcApplication.Events.OnObjectAltered(serverGroup, new SfcObjectAlteredEventArgs(serverGroup.Urn, serverGroup));
			}
			{
				foreach (XmlNode childNode in member.ChildNodes)
				{
					AddGroupMember(serverGroup, childNode);
				}
				return;
			}
		}
		Trace.Assert(member.Name == "Server");
		string name2 = ReadXmlAttribute(member, "name", throwIfMissing: true) as string;
		bool flag2 = false;
		RegisteredServer registeredServer = group.RegisteredServers[name2];
		if (registeredServer == null)
		{
			flag2 = true;
			registeredServer = new RegisteredServer(group, name2);
		}
		if (ReadXmlAttribute(member, "description", throwIfMissing: false) is string description2)
		{
			registeredServer.Description = description2;
		}
		if (member.FirstChild != member.LastChild)
		{
			throw new InvalidSqlServer2005StoreFormatException(RegSvrStrings.InvalidSqlServer2005FileFormat(member.Name));
		}
		XmlNode firstChild = member.FirstChild;
		if (firstChild == null || firstChild.NodeType != XmlNodeType.Element || firstChild.Name != "ConnectionInformation")
		{
			throw new InvalidSqlServer2005StoreFormatException(RegSvrStrings.InvalidSqlServer2005FileFormat(firstChild.Name));
		}
		DbConnectionStringBuilder dbConnectionStringBuilder = new DbConnectionStringBuilder();
		bool flag3 = true;
		string value = string.Empty;
		string text = string.Empty;
		foreach (XmlNode childNode2 in firstChild.ChildNodes)
		{
			switch (childNode2.Name)
			{
			case "ServerName":
				registeredServer.ServerName = childNode2.InnerText;
				break;
			case "AuthenticationType":
				flag3 = int.Parse(childNode2.InnerText) == 0;
				break;
			case "UserName":
				value = childNode2.InnerText;
				break;
			case "Password":
				text = childNode2.InnerText;
				break;
			case "AdvancedOptions":
				if (registeredServer.ServerType == ServerType.AnalysisServices)
				{
					AddAsAdvancedOptions(dbConnectionStringBuilder, childNode2);
				}
				else
				{
					AddAdvancedOptions(dbConnectionStringBuilder, childNode2);
				}
				break;
			}
		}
		if (registeredServer.ServerName == null)
		{
			registeredServer.ServerName = registeredServer.Name;
		}
		if (registeredServer.ServerType == ServerType.AnalysisServices)
		{
			dbConnectionStringBuilder["data source"] = registeredServer.ServerName;
			dbConnectionStringBuilder["provider"] = "MSOLAP";
		}
		else
		{
			dbConnectionStringBuilder["server"] = registeredServer.ServerName;
			if (flag3)
			{
				dbConnectionStringBuilder["trusted_connection"] = "true";
			}
			else
			{
				dbConnectionStringBuilder["user id"] = value;
				if (!string.IsNullOrEmpty(text))
				{
					dbConnectionStringBuilder["password"] = registeredServer.ProtectData(text, encrypt: false);
				}
			}
		}
		registeredServer.CredentialPersistenceType = CredentialPersistenceType.PersistLoginNameAndPassword;
		registeredServer.ConnectionString = dbConnectionStringBuilder.ToString();
		if (flag2)
		{
			registeredServer.Create();
			return;
		}
		registeredServer.Validate(ValidationMethod.Alter, throwOnFirst: true, new ValidationState());
		SfcApplication.Events.OnObjectAltered(registeredServer, new SfcObjectAlteredEventArgs(registeredServer.Urn, registeredServer));
	}

	private void AddAsAdvancedOptions(DbConnectionStringBuilder connString, XmlNode property)
	{
		Trace.Assert(property != null && property.Name == "AdvancedOptions");
		foreach (XmlNode childNode in property.ChildNodes)
		{
			switch (childNode.Name.ToLower())
			{
			case "connection_timeout":
				connString["connect timeout"] = childNode.InnerText;
				break;
			case "exec_timeout":
				connString["timeout"] = childNode.InnerText;
				break;
			case "encrypt_connection":
				connString["encrypt"] = childNode.InnerText;
				break;
			case "database":
				connString["initial catalog"] = childNode.InnerText;
				break;
			default:
				throw new InvalidSqlServer2005StoreFormatException(RegSvrStrings.InvalidSqlServer2005FileFormat(childNode.Name));
			}
		}
	}

	private void AddAdvancedOptions(DbConnectionStringBuilder connString, XmlNode property)
	{
		Trace.Assert(property != null && property.Name == "AdvancedOptions");
		foreach (XmlNode childNode in property.ChildNodes)
		{
			switch (childNode.Name.ToLower())
			{
			case "packet_size":
				connString["packet size"] = childNode.InnerText;
				break;
			case "connection_timeout":
				connString["connection timeout"] = childNode.InnerText;
				break;
			case "encrypt_connection":
				connString["encrypt"] = childNode.InnerText;
				break;
			case "database":
				connString["database"] = childNode.InnerText;
				break;
			case "protocol":
				connString["network library"] = childNode.InnerText;
				break;
			default:
				connString[childNode.Name.ToLower()] = childNode.InnerText;
				break;
			case "exec_timeout":
				break;
			}
		}
	}

	private object ReadXmlAttribute(XmlNode node, string name, bool throwIfMissing)
	{
		XmlAttribute xmlAttribute = node.Attributes[name];
		if (xmlAttribute == null)
		{
			if (throwIfMissing)
			{
				throw new InvalidSqlServer2005StoreFormatException(RegSvrStrings.InvalidSqlServer2005FileFormat(node.Name));
			}
			return null;
		}
		return xmlAttribute.Value;
	}

	internal void Serialize()
	{
		if (string.IsNullOrEmpty(localXmlStorageFile))
		{
			localXmlStorageFile = GetLocalXmlFilePath();
		}
		using XmlTextWriter xmlTextWriter = new XmlTextWriter(localXmlStorageFile, null);
		xmlTextWriter.Formatting = Formatting.Indented;
		SfcSerializer sfcSerializer = new SfcSerializer();
		sfcSerializer.Serialize(this);
		try
		{
			sfcSerializer.Write(xmlTextWriter);
		}
		catch (SfcSerializationException)
		{
			throw;
		}
	}

	internal static void Export(SfcInstance obj, string file, CredentialPersistenceType cpt)
	{
		Trace.Assert(obj is RegisteredServer || obj is ServerGroup);
		try
		{
			if (file == null)
			{
				throw new ArgumentNullException("file");
			}
			GetStore(obj).CredentialPersistenceType = cpt;
			GetStore(obj).UseStoreCredentialPersistenceType = true;
			using XmlTextWriter xmlTextWriter = new XmlTextWriter(file, null);
			xmlTextWriter.Formatting = Formatting.Indented;
			SfcSerializer sfcSerializer = new SfcSerializer();
			sfcSerializer.FilterPropertyHandler += FilterProperty;
			sfcSerializer.Serialize(obj);
			sfcSerializer.Write(xmlTextWriter);
		}
		catch (Exception ex)
		{
			FilterException(ex);
			try
			{
				if (File.Exists(file))
				{
					File.Delete(file);
				}
			}
			catch
			{
			}
			throw new RegisteredServerException(RegSvrStrings.FailedOperation(RegSvrStrings.Export), ex);
		}
		finally
		{
			GetStore(obj).UseStoreCredentialPersistenceType = false;
		}
	}

	internal static object FilterProperty(SfcSerializer serializer, FilterPropertyEventArgs propertyArgs)
	{
		if (serializer == null)
		{
			throw new ArgumentNullException("serializer");
		}
		if (propertyArgs == null)
		{
			throw new ArgumentNullException("propertyArgs");
		}
		if (propertyArgs.Instance.GetType() == typeof(RegisteredServer) && propertyArgs.PropertyName == "ConnectionStringWithEncryptedPassword")
		{
			RegisteredServer registeredServer = propertyArgs.Instance as RegisteredServer;
			CredentialPersistenceType cpt = registeredServer.CredentialPersistenceType;
			if (registeredServer.GetStore().UseStoreCredentialPersistenceType)
			{
				cpt = registeredServer.GetStore().CredentialPersistenceType;
			}
			return registeredServer.GetConnectionStringWithEncryptedPassword(cpt);
		}
		if (propertyArgs.Instance.GetType() == typeof(RegisteredServer) && propertyArgs.PropertyName == "CredentialPersistenceType")
		{
			RegisteredServer registeredServer2 = propertyArgs.Instance as RegisteredServer;
			if (registeredServer2.GetStore().UseStoreCredentialPersistenceType)
			{
				CredentialPersistenceType credentialPersistenceType = registeredServer2.GetStore().CredentialPersistenceType;
				if (registeredServer2.CredentialPersistenceType == CredentialPersistenceType.PersistLoginName && credentialPersistenceType == CredentialPersistenceType.PersistLoginNameAndPassword)
				{
					return CredentialPersistenceType.PersistLoginName;
				}
				return credentialPersistenceType;
			}
			return registeredServer2.CredentialPersistenceType;
		}
		return propertyArgs.Instance.Properties[propertyArgs.PropertyName].Value;
	}

	internal static void FilterException(Exception e)
	{
		if (e is OutOfMemoryException)
		{
			throw e;
		}
	}

	protected internal override SfcKey CreateIdentityKey()
	{
		return new Key(this);
	}

	public ISfcExecutionEngine GetExecutionEngine()
	{
		if (IsLocal)
		{
			return NopExecutionEngine;
		}
		return new SfcTSqlExecutionEngine(ServerConnection);
	}

	static RegisteredServersStore()
	{
		lockObject = new object();
		databaseEngineServerGroupName = "DatabaseEngineServerGroup";
		analysisServicesServerGroupName = "AnalysisServicesServerGroup";
		reportingServicesServerGroupName = "ReportingServicesServerGroup";
		integrationServicesServerGroupName = "IntegrationServicesServerGroup";
		sqlServerCompactEditionServerGroupName = "SqlServerCompactEditionServerGroup";
		centralManagementServerGroupName = "CentralManagementServerGroup";
		nopExecutionEngine = new NopExecutionEngine();
	}

	public SfcKey GetKey(IUrnFragment urnFragment)
	{
		return urnFragment.Name switch
		{
			"RegisteredServersStore" => new Key(this), 
			"ServerGroup" => new ServerGroup.Key(urnFragment.FieldDictionary), 
			"RegisteredServer" => new RegisteredServer.Key(urnFragment.FieldDictionary), 
			_ => throw new InvalidArgumentException(urnFragment.Name), 
		};
	}

	public Type GetType(string typeName)
	{
		return typeName switch
		{
			"RegisteredServersStore" => typeof(RegisteredServersStore), 
			"ServerGroup" => typeof(ServerGroup), 
			"RegisteredServer" => typeof(RegisteredServer), 
			_ => null, 
		};
	}

	public SfcTypeMetadata GetTypeMetadata(string typeName)
	{
		throw new NotImplementedException("The method or operation is not implemented.");
	}

	public bool UseSfcStateManagement()
	{
		return true;
	}

	int ISfcDomainLite.GetLogicalVersion()
	{
		return 1;
	}

	ISfcConnection ISfcHasConnection.GetConnection(SfcObjectQueryMode activeQueriesMode)
	{
		switch (activeQueriesMode)
		{
		case SfcObjectQueryMode.SingleActiveQuery:
			return ServerConnection;
		case SfcObjectQueryMode.MultipleActiveQueries:
			if (ServerConnection != null && ServerConnection.MultipleActiveResultSets)
			{
				return ServerConnection;
			}
			return null;
		case SfcObjectQueryMode.CachedQuery:
			return null;
		default:
			return null;
		}
	}

	void ISfcHasConnection.SetConnection(ISfcConnection connection)
	{
		throw new NotSupportedException();
	}

	ISfcConnection ISfcHasConnection.GetConnection()
	{
		return ServerConnection;
	}

	protected internal override ISfcCollection GetChildCollection(string elementType)
	{
		if (elementType == "ServerGroup")
		{
			return ServerGroups;
		}
		throw new RegisteredServerException(RegSvrStrings.NoSuchCollection(elementType));
	}

	private static string GetLocalXmlFilePath()
	{
		return Path.Combine(GetSettingsDir("\\Microsoft\\SQL Server Management Studio"), "RegSrvr.xml");
	}

	private static string GetSettingsDir(string suffix)
	{
		string text = string.Format(CultureInfo.InvariantCulture, "{0}{1}", System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), suffix);
		EnsureDirExists(text);
		return text;
	}

	private static void EnsureDirExists(string directory)
	{
		if (!Directory.Exists(directory))
		{
			Directory.CreateDirectory(directory);
		}
	}

	internal static RegisteredServersStore GetStore(SfcInstance instance)
	{
		Trace.Assert(instance is ServerGroup || instance is RegisteredServer);
		ServerGroup serverGroup = null;
		serverGroup = ((!(instance is RegisteredServer)) ? (instance as ServerGroup) : ((RegisteredServer)instance).Parent);
		int num = 0;
		while (!(serverGroup.Parent is RegisteredServersStore) && num < 10000)
		{
			serverGroup = serverGroup.Parent as ServerGroup;
			num++;
		}
		return serverGroup.Parent as RegisteredServersStore;
	}

	public override void Discover(ISfcDependencyDiscoveryObjectSink sink)
	{
		if (sink.Action != SfcDependencyAction.Serialize)
		{
			return;
		}
		foreach (ServerGroup serverGroup in ServerGroups)
		{
			sink.Add(SfcDependencyDirection.Inbound, serverGroup, SfcTypeRelation.ContainedChild, discovered: false);
		}
	}
}
