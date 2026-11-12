using System;
using System.Collections.Specialized;
using System.Text;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Dmf;
using Microsoft.SqlServer.Management.Facets;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;

namespace Microsoft.SqlServer.Management.Smo;

[EvaluationMode(AutomatedPolicyEvaluationMode.CheckOnSchedule)]
[PhysicalFacet(PhysicalFacetOptions.ReadOnly)]
[SfcElementType("DdlTrigger")]
[SfcElement(SfcElementFlags.Standalone)]
public sealed class ServerDdlTrigger : DdlTriggerBase, ISfcSupportsDesignMode
{
	internal class PropertyMetadataProvider : SqlPropertyMetadataProvider
	{
		private static int[] versionCount = new int[10] { 0, 0, 17, 18, 18, 18, 18, 18, 18, 18 };

		private static int[] cloudVersionCount = new int[3];

		private static int sqlDwPropertyCount = 0;

		internal static StaticMetadata[] sqlDwStaticMetadata = new StaticMetadata[0];

		internal static StaticMetadata[] cloudStaticMetadata = new StaticMetadata[0];

		internal static StaticMetadata[] staticMetadata = new StaticMetadata[18]
		{
			new StaticMetadata("AnsiNullsStatus", expensive: false, readOnly: false, typeof(bool)),
			new StaticMetadata("AssemblyName", expensive: false, readOnly: false, typeof(string)),
			new StaticMetadata("BodyStartIndex", expensive: true, readOnly: true, typeof(int)),
			new StaticMetadata("ClassName", expensive: false, readOnly: false, typeof(string)),
			new StaticMetadata("CreateDate", expensive: false, readOnly: true, typeof(DateTime)),
			new StaticMetadata("DateLastModified", expensive: false, readOnly: true, typeof(DateTime)),
			new StaticMetadata("DdlTriggerEvents", expensive: true, readOnly: false, typeof(ServerDdlTriggerEventSet)),
			new StaticMetadata("ExecutionContext", expensive: false, readOnly: false, typeof(ServerDdlTriggerExecutionContext)),
			new StaticMetadata("ExecutionContextLogin", expensive: false, readOnly: false, typeof(string)),
			new StaticMetadata("ID", expensive: false, readOnly: true, typeof(int)),
			new StaticMetadata("ImplementationType", expensive: false, readOnly: false, typeof(ImplementationType)),
			new StaticMetadata("IsEnabled", expensive: false, readOnly: false, typeof(bool)),
			new StaticMetadata("IsEncrypted", expensive: false, readOnly: false, typeof(bool)),
			new StaticMetadata("IsSystemObject", expensive: false, readOnly: true, typeof(bool)),
			new StaticMetadata("MethodName", expensive: false, readOnly: false, typeof(string)),
			new StaticMetadata("QuotedIdentifierStatus", expensive: false, readOnly: false, typeof(bool)),
			new StaticMetadata("Text", expensive: true, readOnly: true, typeof(string)),
			new StaticMetadata("PolicyHealthState", expensive: true, readOnly: false, typeof(PolicyHealthState))
		};

		public override int Count
		{
			get
			{
				if (base.DatabaseEngineType == DatabaseEngineType.SqlAzureDatabase)
				{
					if (base.DatabaseEngineEdition == DatabaseEngineEdition.SqlDataWarehouse)
					{
						return sqlDwPropertyCount;
					}
					int num = ((currentVersionIndex < cloudVersionCount.Length) ? currentVersionIndex : (cloudVersionCount.Length - 1));
					return cloudVersionCount[num];
				}
				int num2 = ((currentVersionIndex < versionCount.Length) ? currentVersionIndex : (versionCount.Length - 1));
				return versionCount[num2];
			}
		}

		protected override int[] VersionCount
		{
			get
			{
				if (base.DatabaseEngineType == DatabaseEngineType.SqlAzureDatabase)
				{
					if (base.DatabaseEngineEdition == DatabaseEngineEdition.SqlDataWarehouse)
					{
						return new int[1] { sqlDwPropertyCount };
					}
					return cloudVersionCount;
				}
				return versionCount;
			}
		}

		internal PropertyMetadataProvider(ServerVersion version, DatabaseEngineType databaseEngineType, DatabaseEngineEdition databaseEngineEdition)
			: base(version, databaseEngineType, databaseEngineEdition)
		{
		}

		public override int PropertyNameToIDLookup(string propertyName)
		{
			if (base.DatabaseEngineType == DatabaseEngineType.SqlAzureDatabase)
			{
				_ = base.DatabaseEngineEdition;
				_ = 6;
				return -1;
			}
			return propertyName switch
			{
				"AnsiNullsStatus" => 0, 
				"AssemblyName" => 1, 
				"BodyStartIndex" => 2, 
				"ClassName" => 3, 
				"CreateDate" => 4, 
				"DateLastModified" => 5, 
				"DdlTriggerEvents" => 6, 
				"ExecutionContext" => 7, 
				"ExecutionContextLogin" => 8, 
				"ID" => 9, 
				"ImplementationType" => 10, 
				"IsEnabled" => 11, 
				"IsEncrypted" => 12, 
				"IsSystemObject" => 13, 
				"MethodName" => 14, 
				"QuotedIdentifierStatus" => 15, 
				"Text" => 16, 
				"PolicyHealthState" => 17, 
				_ => -1, 
			};
		}

		internal new static int[] GetVersionArray(DatabaseEngineType databaseEngineType, DatabaseEngineEdition databaseEngineEdition)
		{
			if (databaseEngineType == DatabaseEngineType.SqlAzureDatabase)
			{
				if (databaseEngineEdition == DatabaseEngineEdition.SqlDataWarehouse)
				{
					return new int[1] { sqlDwPropertyCount };
				}
				return cloudVersionCount;
			}
			return versionCount;
		}

		public override StaticMetadata GetStaticMetadata(int id)
		{
			if (base.DatabaseEngineType == DatabaseEngineType.SqlAzureDatabase)
			{
				if (base.DatabaseEngineEdition == DatabaseEngineEdition.SqlDataWarehouse)
				{
					return sqlDwStaticMetadata[id];
				}
				return cloudStaticMetadata[id];
			}
			return staticMetadata[id];
		}

		internal new static StaticMetadata[] GetStaticMetadataArray(DatabaseEngineType databaseEngineType, DatabaseEngineEdition databaseEngineEdition)
		{
			if (databaseEngineType == DatabaseEngineType.SqlAzureDatabase)
			{
				if (databaseEngineEdition == DatabaseEngineEdition.SqlDataWarehouse)
				{
					return sqlDwStaticMetadata;
				}
				return cloudStaticMetadata;
			}
			return staticMetadata;
		}
	}

	internal enum ServerDdlTriggerEventValues
	{
		AddRoleMember,
		AddSensitivityClassification,
		AddServerRoleMember,
		AddSignature,
		AddSignatureSchemaObject,
		AlterApplicationRole,
		AlterAssembly,
		AlterAsymmetricKey,
		AlterAudit,
		AlterAuthorizationDatabase,
		AlterAuthorizationServer,
		AlterAvailabilityGroup,
		AlterBrokerPriority,
		AlterCertificate,
		AlterColumnEncryptionKey,
		AlterCredential,
		AlterCryptographicProvider,
		AlterDatabase,
		AlterDatabaseAuditSpecification,
		AlterDatabaseEncryptionKey,
		AlterDatabaseScopedConfiguration,
		AlterEndpoint,
		AlterEventSession,
		AlterExtendedProperty,
		AlterExternalLanguage,
		AlterExternalLibrary,
		AlterExternalResourcePool,
		AlterFulltextCatalog,
		AlterFulltextIndex,
		AlterFulltextStoplist,
		AlterFunction,
		AlterIndex,
		AlterInstance,
		AlterLinkedServer,
		AlterLogin,
		AlterMasterKey,
		AlterMessage,
		AlterMessageType,
		AlterPartitionFunction,
		AlterPartitionScheme,
		AlterPlanGuide,
		AlterProcedure,
		AlterQueue,
		AlterRemoteServer,
		AlterRemoteServiceBinding,
		AlterResourceGovernorConfig,
		AlterResourcePool,
		AlterRole,
		AlterRoute,
		AlterSchema,
		AlterSearchPropertyList,
		AlterSecurityPolicy,
		AlterSequence,
		AlterServerAudit,
		AlterServerAuditSpecification,
		AlterServerRole,
		AlterService,
		AlterServiceMasterKey,
		AlterSymmetricKey,
		AlterTable,
		AlterTrigger,
		AlterUser,
		AlterView,
		AlterWorkloadGroup,
		AlterXmlSchemaCollection,
		BindDefault,
		BindRule,
		CreateApplicationRole,
		CreateAssembly,
		CreateAsymmetricKey,
		CreateAudit,
		CreateAvailabilityGroup,
		CreateBrokerPriority,
		CreateCertificate,
		CreateColumnEncryptionKey,
		CreateColumnMasterKey,
		CreateContract,
		CreateCredential,
		CreateCryptographicProvider,
		CreateDatabase,
		CreateDatabaseAuditSpecification,
		CreateDatabaseEncryptionKey,
		CreateDefault,
		CreateEndpoint,
		CreateEventNotification,
		CreateEventSession,
		CreateExtendedProcedure,
		CreateExtendedProperty,
		CreateExternalLanguage,
		CreateExternalLibrary,
		CreateExternalResourcePool,
		CreateFulltextCatalog,
		CreateFulltextIndex,
		CreateFulltextStoplist,
		CreateFunction,
		CreateIndex,
		CreateLinkedServer,
		CreateLinkedServerLogin,
		CreateLogin,
		CreateMasterKey,
		CreateMessage,
		CreateMessageType,
		CreatePartitionFunction,
		CreatePartitionScheme,
		CreatePlanGuide,
		CreateProcedure,
		CreateQueue,
		CreateRemoteServer,
		CreateRemoteServiceBinding,
		CreateResourcePool,
		CreateRole,
		CreateRoute,
		CreateRule,
		CreateSchema,
		CreateSearchPropertyList,
		CreateSecurityPolicy,
		CreateSequence,
		CreateServerAudit,
		CreateServerAuditSpecification,
		CreateServerRole,
		CreateService,
		CreateSpatialIndex,
		CreateStatistics,
		CreateSymmetricKey,
		CreateSynonym,
		CreateTable,
		CreateTrigger,
		CreateType,
		CreateUser,
		CreateView,
		CreateWorkloadGroup,
		CreateXmlIndex,
		CreateXmlSchemaCollection,
		DenyDatabase,
		DenyServer,
		DropApplicationRole,
		DropAssembly,
		DropAsymmetricKey,
		DropAudit,
		DropAvailabilityGroup,
		DropBrokerPriority,
		DropCertificate,
		DropColumnEncryptionKey,
		DropColumnMasterKey,
		DropContract,
		DropCredential,
		DropCryptographicProvider,
		DropDatabase,
		DropDatabaseAuditSpecification,
		DropDatabaseEncryptionKey,
		DropDefault,
		DropEndpoint,
		DropEventNotification,
		DropEventSession,
		DropExtendedProcedure,
		DropExtendedProperty,
		DropExternalLanguage,
		DropExternalLibrary,
		DropExternalResourcePool,
		DropFulltextCatalog,
		DropFulltextIndex,
		DropFulltextStoplist,
		DropFunction,
		DropIndex,
		DropLinkedServer,
		DropLinkedServerLogin,
		DropLogin,
		DropMasterKey,
		DropMessage,
		DropMessageType,
		DropPartitionFunction,
		DropPartitionScheme,
		DropPlanGuide,
		DropProcedure,
		DropQueue,
		DropRemoteServer,
		DropRemoteServiceBinding,
		DropResourcePool,
		DropRole,
		DropRoleMember,
		DropRoute,
		DropRule,
		DropSchema,
		DropSearchPropertyList,
		DropSecurityPolicy,
		DropSensitivityClassification,
		DropSequence,
		DropServerAudit,
		DropServerAuditSpecification,
		DropServerRole,
		DropServerRoleMember,
		DropService,
		DropSignature,
		DropSignatureSchemaObject,
		DropStatistics,
		DropSymmetricKey,
		DropSynonym,
		DropTable,
		DropTrigger,
		DropType,
		DropUser,
		DropView,
		DropWorkloadGroup,
		DropXmlSchemaCollection,
		GrantDatabase,
		GrantServer,
		Logon,
		Rename,
		RevokeDatabase,
		RevokeServer,
		UnbindDefault,
		UnbindRule,
		UpdateStatistics
	}

	[SfcObject(SfcObjectRelationship.ParentObject)]
	public Server Parent
	{
		get
		{
			CheckObjectState();
			return base.ParentColl.ParentInstance as Server;
		}
		set
		{
			SetParentImpl(value);
		}
	}

	public static string UrnSuffix => "DdlTrigger";

	public ServerDdlTriggerEventSet DdlTriggerEvents
	{
		get
		{
			if (base.State == SqlSmoState.Creating)
			{
				Property property = base.Properties.Get("DdlTriggerEvents");
				if (property.Value == null)
				{
					property.Value = new ServerDdlTriggerEventSet();
				}
			}
			return (ServerDdlTriggerEventSet)base.Properties.GetValueWithNullReplacement("DdlTriggerEvents");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("DdlTriggerEvents", value);
		}
	}

	bool ISfcSupportsDesignMode.IsDesignMode => base.IsDesignMode;

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public bool AnsiNullsStatus
	{
		get
		{
			return (bool)base.Properties.GetValueWithNullReplacement("AnsiNullsStatus");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("AnsiNullsStatus", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone)]
	[SfcReference(typeof(SqlAssembly), "Server[@Name = '{0}']/Database[@Name = 'master']/SqlAssembly[@Name = '{1}']", new string[] { "Parent.ConnectionContext.TrueName", "AssemblyName" })]
	[CLSCompliant(false)]
	public string AssemblyName
	{
		get
		{
			return (string)base.Properties.GetValueWithNullReplacement("AssemblyName");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("AssemblyName", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Expensive | SfcPropertyFlags.Standalone)]
	public int BodyStartIndex => (int)base.Properties.GetValueWithNullReplacement("BodyStartIndex");

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public string ClassName
	{
		get
		{
			return (string)base.Properties.GetValueWithNullReplacement("ClassName");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("ClassName", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public DateTime CreateDate => (DateTime)base.Properties.GetValueWithNullReplacement("CreateDate");

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public DateTime DateLastModified => (DateTime)base.Properties.GetValueWithNullReplacement("DateLastModified");

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public ServerDdlTriggerExecutionContext ExecutionContext
	{
		get
		{
			return (ServerDdlTriggerExecutionContext)base.Properties.GetValueWithNullReplacement("ExecutionContext");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("ExecutionContext", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone)]
	[SfcReference(typeof(Login), "Server[@Name = '{0}']/Login[@Name = '{1}']", new string[] { "Parent.ConnectionContext.TrueName", "ExecutionContextLogin" })]
	[CLSCompliant(false)]
	public string ExecutionContextLogin
	{
		get
		{
			return (string)base.Properties.GetValueWithNullReplacement("ExecutionContextLogin");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("ExecutionContextLogin", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public int ID => (int)base.Properties.GetValueWithNullReplacement("ID");

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public ImplementationType ImplementationType
	{
		get
		{
			return (ImplementationType)base.Properties.GetValueWithNullReplacement("ImplementationType");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("ImplementationType", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public bool IsEnabled
	{
		get
		{
			return (bool)base.Properties.GetValueWithNullReplacement("IsEnabled");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("IsEnabled", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public bool IsEncrypted
	{
		get
		{
			return (bool)base.Properties.GetValueWithNullReplacement("IsEncrypted");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("IsEncrypted", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public bool IsSystemObject => (bool)base.Properties.GetValueWithNullReplacement("IsSystemObject");

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public string MethodName
	{
		get
		{
			return (string)base.Properties.GetValueWithNullReplacement("MethodName");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("MethodName", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public bool QuotedIdentifierStatus
	{
		get
		{
			return (bool)base.Properties.GetValueWithNullReplacement("QuotedIdentifierStatus");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("QuotedIdentifierStatus", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Expensive | SfcPropertyFlags.Standalone)]
	public string Text => (string)base.Properties.GetValueWithNullReplacement("Text");

	internal ServerDdlTrigger(AbstractCollectionBase parentColl, ObjectKeyBase key, SqlSmoState state)
		: base(parentColl, key, state)
	{
	}

	public ServerDdlTrigger()
	{
	}

	public ServerDdlTrigger(Server server, string name)
	{
		ValidateName(name);
		key = new SimpleObjectKey(name);
		Parent = server;
	}

	public ServerDdlTrigger(Server parent, string name, ServerDdlTriggerEventSet events, string textBody)
	{
		ValidateName(name);
		key = new SimpleObjectKey(name);
		Parent = parent;
		base.Properties.Get("DdlTriggerEvents").Value = events;
		TextBody = textBody;
		base.Properties.Get("ImplementationType").Value = ImplementationType.TransactSql;
	}

	public ServerDdlTrigger(Server parent, string name, ServerDdlTriggerEventSet events, string assemblyName, string className, string method)
	{
		ValidateName(name);
		key = new SimpleObjectKey(name);
		Parent = parent;
		base.Properties.Get("DdlTriggerEvents").Value = events;
		base.Properties.Get("ImplementationType").Value = ImplementationType.SqlClr;
		base.Properties.Get("AssemblyName").Value = assemblyName;
		base.Properties.Get("ClassName").Value = className;
		base.Properties.Get("MethodName").Value = method;
	}

	internal override void ScriptDrop(StringCollection queries, ScriptingPreferences sp)
	{
		SqlSmoObject.ThrowIfBelowVersion90(sp.TargetServerVersionInternal);
		StringBuilder stringBuilder = new StringBuilder(Globals.INIT_BUFFER_SIZE);
		string arg = FormatFullNameForScripting(sp);
		ScriptIncludeHeaders(stringBuilder, sp, UrnSuffix);
		if (sp.IncludeScripts.ExistenceCheck && sp.TargetServerVersionInternal < SqlServerVersionInternal.Version130)
		{
			stringBuilder.AppendLine(GetIfNotExistStatement(sp, ""));
		}
		stringBuilder.AppendFormat(SmoApplication.DefaultCulture, "DROP TRIGGER {0}{1} ON ALL SERVER", (sp.IncludeScripts.ExistenceCheck && sp.TargetServerVersionInternal >= SqlServerVersionInternal.Version130) ? "IF EXISTS " : string.Empty, arg);
		queries.Add(stringBuilder.ToString());
	}

	internal override void AddDdlTriggerEvents(StringBuilder sb, ScriptingPreferences sp)
	{
		ServerDdlTriggerEventSet propValueOptional = GetPropValueOptional("DdlTriggerEvents", new ServerDdlTriggerEventSet());
		int num = 0;
		for (int i = 0; i < propValueOptional.NumberOfElements; i++)
		{
			if (propValueOptional.GetBitAt(i))
			{
				if (num++ > 0)
				{
					sb.Append(Globals.commaspace);
				}
				sb.Append(StringFromServerDdlTriggerEvent(i));
			}
		}
		if (num == 0)
		{
			throw new PropertyNotSetException("DdlTriggerEvents");
		}
	}

	internal override void ValidateProperty(Property prop, object value)
	{
		if (prop.Name == "IsEncrypted")
		{
			ScriptNameObjectBase.Validate_set_TextObjectDDLProperty(prop, value);
		}
	}

	protected override bool IsObjectDirty()
	{
		if (!base.IsObjectDirty())
		{
			return IsEventSetDirty();
		}
		return true;
	}

	protected override void CleanObject()
	{
		base.CleanObject();
		ServerDdlTriggerEventSet serverDdlTriggerEventSet = (ServerDdlTriggerEventSet)base.Properties.Get("DdlTriggerEvents").Value;
		if (serverDdlTriggerEventSet != null)
		{
			serverDdlTriggerEventSet.Dirty = false;
		}
	}

	protected override bool IsEventSetDirty()
	{
		bool result = false;
		ServerDdlTriggerEventSet serverDdlTriggerEventSet = (ServerDdlTriggerEventSet)base.Properties.Get("DdlTriggerEvents").Value;
		if (serverDdlTriggerEventSet != null)
		{
			result = serverDdlTriggerEventSet.Dirty;
		}
		return result;
	}

	internal override string GetIfNotExistStatement(ScriptingPreferences sp, string prefix)
	{
		return string.Format(SmoApplication.DefaultCulture, Scripts.INCLUDE_EXISTS_SERVER_DDL_TRIGGER, prefix, FormatFullNameForScripting(sp, nameIsIndentifier: false));
	}

	internal override SqlPropertyMetadataProvider GetPropertyMetadataProvider()
	{
		return new PropertyMetadataProvider(base.ServerVersion, DatabaseEngineType, DatabaseEngineEdition);
	}

	private string StringFromServerDdlTriggerEvent(int evt)
	{
		return evt switch
		{
			0 => "ADD_ROLE_MEMBER", 
			1 => "ADD_SENSITIVITY_CLASSIFICATION", 
			2 => "ADD_SERVER_ROLE_MEMBER", 
			3 => "ADD_SIGNATURE", 
			4 => "ADD_SIGNATURE_SCHEMA_OBJECT", 
			5 => "ALTER_APPLICATION_ROLE", 
			6 => "ALTER_ASSEMBLY", 
			7 => "ALTER_ASYMMETRIC_KEY", 
			8 => "ALTER_AUDIT", 
			9 => "ALTER_AUTHORIZATION_DATABASE", 
			10 => "ALTER_AUTHORIZATION_SERVER", 
			11 => "ALTER_AVAILABILITY_GROUP", 
			12 => "ALTER_BROKER_PRIORITY", 
			13 => "ALTER_CERTIFICATE", 
			14 => "ALTER_COLUMN_ENCRYPTION_KEY", 
			15 => "ALTER_CREDENTIAL", 
			16 => "ALTER_CRYPTOGRAPHIC_PROVIDER", 
			17 => "ALTER_DATABASE", 
			18 => "ALTER_DATABASE_AUDIT_SPECIFICATION", 
			19 => "ALTER_DATABASE_ENCRYPTION_KEY", 
			20 => "ALTER_DATABASE_SCOPED_CONFIGURATION", 
			21 => "ALTER_ENDPOINT", 
			22 => "ALTER_EVENT_SESSION", 
			23 => "ALTER_EXTENDED_PROPERTY", 
			24 => "ALTER_EXTERNAL_LANGUAGE", 
			25 => "ALTER_EXTERNAL_LIBRARY", 
			26 => "ALTER_EXTERNAL_RESOURCE_POOL", 
			27 => "ALTER_FULLTEXT_CATALOG", 
			28 => "ALTER_FULLTEXT_INDEX", 
			29 => "ALTER_FULLTEXT_STOPLIST", 
			30 => "ALTER_FUNCTION", 
			31 => "ALTER_INDEX", 
			32 => "ALTER_INSTANCE", 
			33 => "ALTER_LINKED_SERVER", 
			34 => "ALTER_LOGIN", 
			35 => "ALTER_MASTER_KEY", 
			36 => "ALTER_MESSAGE", 
			37 => "ALTER_MESSAGE_TYPE", 
			38 => "ALTER_PARTITION_FUNCTION", 
			39 => "ALTER_PARTITION_SCHEME", 
			40 => "ALTER_PLAN_GUIDE", 
			41 => "ALTER_PROCEDURE", 
			42 => "ALTER_QUEUE", 
			43 => "ALTER_REMOTE_SERVER", 
			44 => "ALTER_REMOTE_SERVICE_BINDING", 
			45 => "ALTER_RESOURCE_GOVERNOR_CONFIG", 
			46 => "ALTER_RESOURCE_POOL", 
			47 => "ALTER_ROLE", 
			48 => "ALTER_ROUTE", 
			49 => "ALTER_SCHEMA", 
			50 => "ALTER_SEARCH_PROPERTY_LIST", 
			51 => "ALTER_SECURITY_POLICY", 
			52 => "ALTER_SEQUENCE", 
			53 => "ALTER_SERVER_AUDIT", 
			54 => "ALTER_SERVER_AUDIT_SPECIFICATION", 
			55 => "ALTER_SERVER_ROLE", 
			56 => "ALTER_SERVICE", 
			57 => "ALTER_SERVICE_MASTER_KEY", 
			58 => "ALTER_SYMMETRIC_KEY", 
			59 => "ALTER_TABLE", 
			60 => "ALTER_TRIGGER", 
			61 => "ALTER_USER", 
			62 => "ALTER_VIEW", 
			63 => "ALTER_WORKLOAD_GROUP", 
			64 => "ALTER_XML_SCHEMA_COLLECTION", 
			65 => "BIND_DEFAULT", 
			66 => "BIND_RULE", 
			67 => "CREATE_APPLICATION_ROLE", 
			68 => "CREATE_ASSEMBLY", 
			69 => "CREATE_ASYMMETRIC_KEY", 
			70 => "CREATE_AUDIT", 
			71 => "CREATE_AVAILABILITY_GROUP", 
			72 => "CREATE_BROKER_PRIORITY", 
			73 => "CREATE_CERTIFICATE", 
			74 => "CREATE_COLUMN_ENCRYPTION_KEY", 
			75 => "CREATE_COLUMN_MASTER_KEY", 
			76 => "CREATE_CONTRACT", 
			77 => "CREATE_CREDENTIAL", 
			78 => "CREATE_CRYPTOGRAPHIC_PROVIDER", 
			79 => "CREATE_DATABASE", 
			80 => "CREATE_DATABASE_AUDIT_SPECIFICATION", 
			81 => "CREATE_DATABASE_ENCRYPTION_KEY", 
			82 => "CREATE_DEFAULT", 
			83 => "CREATE_ENDPOINT", 
			84 => "CREATE_EVENT_NOTIFICATION", 
			85 => "CREATE_EVENT_SESSION", 
			86 => "CREATE_EXTENDED_PROCEDURE", 
			87 => "CREATE_EXTENDED_PROPERTY", 
			88 => "CREATE_EXTERNAL_LANGUAGE", 
			89 => "CREATE_EXTERNAL_LIBRARY", 
			90 => "CREATE_EXTERNAL_RESOURCE_POOL", 
			91 => "CREATE_FULLTEXT_CATALOG", 
			92 => "CREATE_FULLTEXT_INDEX", 
			93 => "CREATE_FULLTEXT_STOPLIST", 
			94 => "CREATE_FUNCTION", 
			95 => "CREATE_INDEX", 
			96 => "CREATE_LINKED_SERVER", 
			97 => "CREATE_LINKED_SERVER_LOGIN", 
			98 => "CREATE_LOGIN", 
			99 => "CREATE_MASTER_KEY", 
			100 => "CREATE_MESSAGE", 
			101 => "CREATE_MESSAGE_TYPE", 
			102 => "CREATE_PARTITION_FUNCTION", 
			103 => "CREATE_PARTITION_SCHEME", 
			104 => "CREATE_PLAN_GUIDE", 
			105 => "CREATE_PROCEDURE", 
			106 => "CREATE_QUEUE", 
			107 => "CREATE_REMOTE_SERVER", 
			108 => "CREATE_REMOTE_SERVICE_BINDING", 
			109 => "CREATE_RESOURCE_POOL", 
			110 => "CREATE_ROLE", 
			111 => "CREATE_ROUTE", 
			112 => "CREATE_RULE", 
			113 => "CREATE_SCHEMA", 
			114 => "CREATE_SEARCH_PROPERTY_LIST", 
			115 => "CREATE_SECURITY_POLICY", 
			116 => "CREATE_SEQUENCE", 
			117 => "CREATE_SERVER_AUDIT", 
			118 => "CREATE_SERVER_AUDIT_SPECIFICATION", 
			119 => "CREATE_SERVER_ROLE", 
			120 => "CREATE_SERVICE", 
			121 => "CREATE_SPATIAL_INDEX", 
			122 => "CREATE_STATISTICS", 
			123 => "CREATE_SYMMETRIC_KEY", 
			124 => "CREATE_SYNONYM", 
			125 => "CREATE_TABLE", 
			126 => "CREATE_TRIGGER", 
			127 => "CREATE_TYPE", 
			128 => "CREATE_USER", 
			129 => "CREATE_VIEW", 
			130 => "CREATE_WORKLOAD_GROUP", 
			131 => "CREATE_XML_INDEX", 
			132 => "CREATE_XML_SCHEMA_COLLECTION", 
			133 => "DENY_DATABASE", 
			134 => "DENY_SERVER", 
			135 => "DROP_APPLICATION_ROLE", 
			136 => "DROP_ASSEMBLY", 
			137 => "DROP_ASYMMETRIC_KEY", 
			138 => "DROP_AUDIT", 
			139 => "DROP_AVAILABILITY_GROUP", 
			140 => "DROP_BROKER_PRIORITY", 
			141 => "DROP_CERTIFICATE", 
			142 => "DROP_COLUMN_ENCRYPTION_KEY", 
			143 => "DROP_COLUMN_MASTER_KEY", 
			144 => "DROP_CONTRACT", 
			145 => "DROP_CREDENTIAL", 
			146 => "DROP_CRYPTOGRAPHIC_PROVIDER", 
			147 => "DROP_DATABASE", 
			148 => "DROP_DATABASE_AUDIT_SPECIFICATION", 
			149 => "DROP_DATABASE_ENCRYPTION_KEY", 
			150 => "DROP_DEFAULT", 
			151 => "DROP_ENDPOINT", 
			152 => "DROP_EVENT_NOTIFICATION", 
			153 => "DROP_EVENT_SESSION", 
			154 => "DROP_EXTENDED_PROCEDURE", 
			155 => "DROP_EXTENDED_PROPERTY", 
			156 => "DROP_EXTERNAL_LANGUAGE", 
			157 => "DROP_EXTERNAL_LIBRARY", 
			158 => "DROP_EXTERNAL_RESOURCE_POOL", 
			159 => "DROP_FULLTEXT_CATALOG", 
			160 => "DROP_FULLTEXT_INDEX", 
			161 => "DROP_FULLTEXT_STOPLIST", 
			162 => "DROP_FUNCTION", 
			163 => "DROP_INDEX", 
			164 => "DROP_LINKED_SERVER", 
			165 => "DROP_LINKED_SERVER_LOGIN", 
			166 => "DROP_LOGIN", 
			167 => "DROP_MASTER_KEY", 
			168 => "DROP_MESSAGE", 
			169 => "DROP_MESSAGE_TYPE", 
			170 => "DROP_PARTITION_FUNCTION", 
			171 => "DROP_PARTITION_SCHEME", 
			172 => "DROP_PLAN_GUIDE", 
			173 => "DROP_PROCEDURE", 
			174 => "DROP_QUEUE", 
			175 => "DROP_REMOTE_SERVER", 
			176 => "DROP_REMOTE_SERVICE_BINDING", 
			177 => "DROP_RESOURCE_POOL", 
			178 => "DROP_ROLE", 
			179 => "DROP_ROLE_MEMBER", 
			180 => "DROP_ROUTE", 
			181 => "DROP_RULE", 
			182 => "DROP_SCHEMA", 
			183 => "DROP_SEARCH_PROPERTY_LIST", 
			184 => "DROP_SECURITY_POLICY", 
			185 => "DROP_SENSITIVITY_CLASSIFICATION", 
			186 => "DROP_SEQUENCE", 
			187 => "DROP_SERVER_AUDIT", 
			188 => "DROP_SERVER_AUDIT_SPECIFICATION", 
			189 => "DROP_SERVER_ROLE", 
			190 => "DROP_SERVER_ROLE_MEMBER", 
			191 => "DROP_SERVICE", 
			192 => "DROP_SIGNATURE", 
			193 => "DROP_SIGNATURE_SCHEMA_OBJECT", 
			194 => "DROP_STATISTICS", 
			195 => "DROP_SYMMETRIC_KEY", 
			196 => "DROP_SYNONYM", 
			197 => "DROP_TABLE", 
			198 => "DROP_TRIGGER", 
			199 => "DROP_TYPE", 
			200 => "DROP_USER", 
			201 => "DROP_VIEW", 
			202 => "DROP_WORKLOAD_GROUP", 
			203 => "DROP_XML_SCHEMA_COLLECTION", 
			204 => "GRANT_DATABASE", 
			205 => "GRANT_SERVER", 
			206 => "LOGON", 
			207 => "RENAME", 
			208 => "REVOKE_DATABASE", 
			209 => "REVOKE_SERVER", 
			210 => "UNBIND_DEFAULT", 
			211 => "UNBIND_RULE", 
			212 => "UPDATE_STATISTICS", 
			_ => throw new SmoException(ExceptionTemplatesImpl.UnknownEnumeration("ServerDdlTriggerEvent")), 
		};
	}
}
