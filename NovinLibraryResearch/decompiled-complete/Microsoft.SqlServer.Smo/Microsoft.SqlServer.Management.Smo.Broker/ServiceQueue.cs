using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Dmf;
using Microsoft.SqlServer.Management.Facets;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;
using Microsoft.SqlServer.Server;

namespace Microsoft.SqlServer.Management.Smo.Broker;

[EvaluationMode(AutomatedPolicyEvaluationMode.CheckOnSchedule)]
[PhysicalFacet]
[LocalizedPropertyResources("Microsoft.SqlServer.Management.Smo.Broker.BrokerLocalizableResources", true)]
[TypeConverter(typeof(LocalizableTypeConverter))]
public sealed class ServiceQueue : ScriptSchemaObjectBase, IExtendedProperties, ICreatable, IAlterable, IDroppable, IDropIfExists, IScriptable, ISfcSupportsDesignMode, IObjectPermission
{
	internal class PropertyMetadataProvider : SqlPropertyMetadataProvider
	{
		private static int[] versionCount = new int[10] { 0, 0, 15, 16, 17, 17, 17, 17, 17, 17 };

		private static int[] cloudVersionCount = new int[3];

		private static int sqlDwPropertyCount = 0;

		internal static StaticMetadata[] sqlDwStaticMetadata = new StaticMetadata[0];

		internal static StaticMetadata[] cloudStaticMetadata = new StaticMetadata[0];

		internal static StaticMetadata[] staticMetadata = new StaticMetadata[17]
		{
			new StaticMetadata("ActivationExecutionContext", expensive: false, readOnly: false, typeof(ActivationExecutionContext)),
			new StaticMetadata("CreateDate", expensive: false, readOnly: true, typeof(DateTime)),
			new StaticMetadata("DateLastModified", expensive: false, readOnly: true, typeof(DateTime)),
			new StaticMetadata("ExecutionContextPrincipal", expensive: false, readOnly: false, typeof(string)),
			new StaticMetadata("FileGroup", expensive: false, readOnly: false, typeof(string)),
			new StaticMetadata("ID", expensive: false, readOnly: true, typeof(int)),
			new StaticMetadata("IsActivationEnabled", expensive: false, readOnly: false, typeof(bool)),
			new StaticMetadata("IsEnqueueEnabled", expensive: false, readOnly: false, typeof(bool)),
			new StaticMetadata("IsRetentionEnabled", expensive: false, readOnly: false, typeof(bool)),
			new StaticMetadata("IsSystemObject", expensive: false, readOnly: true, typeof(bool)),
			new StaticMetadata("MaxReaders", expensive: false, readOnly: false, typeof(short)),
			new StaticMetadata("ProcedureDatabase", expensive: false, readOnly: false, typeof(string)),
			new StaticMetadata("ProcedureName", expensive: false, readOnly: false, typeof(string)),
			new StaticMetadata("ProcedureSchema", expensive: false, readOnly: false, typeof(string)),
			new StaticMetadata("RowCount", expensive: true, readOnly: true, typeof(long)),
			new StaticMetadata("PolicyHealthState", expensive: true, readOnly: false, typeof(PolicyHealthState)),
			new StaticMetadata("IsPoisonMessageHandlingEnabled", expensive: false, readOnly: false, typeof(bool))
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
				"ActivationExecutionContext" => 0, 
				"CreateDate" => 1, 
				"DateLastModified" => 2, 
				"ExecutionContextPrincipal" => 3, 
				"FileGroup" => 4, 
				"ID" => 5, 
				"IsActivationEnabled" => 6, 
				"IsEnqueueEnabled" => 7, 
				"IsRetentionEnabled" => 8, 
				"IsSystemObject" => 9, 
				"MaxReaders" => 10, 
				"ProcedureDatabase" => 11, 
				"ProcedureName" => 12, 
				"ProcedureSchema" => 13, 
				"RowCount" => 14, 
				"PolicyHealthState" => 15, 
				"IsPoisonMessageHandlingEnabled" => 16, 
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

	private ServiceQueueEvents events;

	public static string UrnSuffix => "ServiceQueue";

	[SfcKey(0)]
	[SfcReference(typeof(Schema), typeof(SchemaCustomResolver), "Resolve", new string[] { })]
	[SfcProperty(SfcPropertyFlags.ReadOnlyAfterCreation | SfcPropertyFlags.Standalone)]
	[CLSCompliant(false)]
	public override string Schema
	{
		get
		{
			return base.Schema;
		}
		set
		{
			base.Schema = value;
		}
	}

	[SfcKey(1)]
	[SfcProperty(SfcPropertyFlags.ReadOnlyAfterCreation | SfcPropertyFlags.Standalone)]
	public override string Name
	{
		get
		{
			return base.Name;
		}
		set
		{
			base.Name = value;
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public double RowCountAsDouble => Convert.ToDouble(base.Properties["RowCount"].Value, SmoApplication.DefaultCulture);

	[SfcObject(SfcContainerRelationship.ChildContainer, SfcContainerCardinality.ZeroToAny, typeof(ExtendedProperty))]
	public ExtendedPropertyCollection ExtendedProperties
	{
		get
		{
			ThrowIfBelowVersion80();
			CheckObjectState();
			if (m_ExtendedProperties == null)
			{
				m_ExtendedProperties = new ExtendedPropertyCollection(this);
			}
			return m_ExtendedProperties;
		}
	}

	[SfcObject(SfcObjectRelationship.ParentObject)]
	public ServiceBroker Parent
	{
		get
		{
			CheckObjectState();
			return base.ParentColl.ParentInstance as ServiceBroker;
		}
		set
		{
			SetParentImpl(value);
		}
	}

	internal override UserPermissionCollection Permissions => GetUserPermissions();

	bool ISfcSupportsDesignMode.IsDesignMode => base.IsDesignMode;

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public ActivationExecutionContext ActivationExecutionContext
	{
		get
		{
			return (ActivationExecutionContext)base.Properties.GetValueWithNullReplacement("ActivationExecutionContext");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("ActivationExecutionContext", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public DateTime CreateDate => (DateTime)base.Properties.GetValueWithNullReplacement("CreateDate");

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public DateTime DateLastModified => (DateTime)base.Properties.GetValueWithNullReplacement("DateLastModified");

	[SfcProperty(SfcPropertyFlags.Standalone)]
	[SfcReference(typeof(User), "Server[@Name = '{0}']/Database[@Name = '{1}']/User[@Name = '{2}']", new string[] { "Parent.Parent.Parent.ConnectionContext.TrueName", "Parent.Parent.Name", "ExecutionContextPrincipal" })]
	[CLSCompliant(false)]
	public string ExecutionContextPrincipal
	{
		get
		{
			return (string)base.Properties.GetValueWithNullReplacement("ExecutionContextPrincipal");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("ExecutionContextPrincipal", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public string FileGroup
	{
		get
		{
			return (string)base.Properties.GetValueWithNullReplacement("FileGroup");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("FileGroup", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public int ID => (int)base.Properties.GetValueWithNullReplacement("ID");

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public bool IsActivationEnabled
	{
		get
		{
			return (bool)base.Properties.GetValueWithNullReplacement("IsActivationEnabled");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("IsActivationEnabled", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public bool IsEnqueueEnabled
	{
		get
		{
			return (bool)base.Properties.GetValueWithNullReplacement("IsEnqueueEnabled");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("IsEnqueueEnabled", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public bool IsPoisonMessageHandlingEnabled
	{
		get
		{
			return (bool)base.Properties.GetValueWithNullReplacement("IsPoisonMessageHandlingEnabled");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("IsPoisonMessageHandlingEnabled", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public bool IsRetentionEnabled
	{
		get
		{
			return (bool)base.Properties.GetValueWithNullReplacement("IsRetentionEnabled");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("IsRetentionEnabled", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public bool IsSystemObject => (bool)base.Properties.GetValueWithNullReplacement("IsSystemObject");

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public short MaxReaders
	{
		get
		{
			return (short)base.Properties.GetValueWithNullReplacement("MaxReaders");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("MaxReaders", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public string ProcedureDatabase
	{
		get
		{
			return (string)base.Properties.GetValueWithNullReplacement("ProcedureDatabase");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("ProcedureDatabase", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone)]
	[SfcReference(typeof(StoredProcedure), "Server[@Name = '{0}']/Database[@Name = '{1}']/StoredProcedure[@Name='{2}' and @Schema='{3}']", new string[] { "Parent.Parent.Parent.ConnectionContext.TrueName", "Parent.Parent.Name", "ProcedureName", "ProcedureSchema" })]
	[CLSCompliant(false)]
	public string ProcedureName
	{
		get
		{
			return (string)base.Properties.GetValueWithNullReplacement("ProcedureName");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("ProcedureName", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone, "dbo")]
	public string ProcedureSchema
	{
		get
		{
			return (string)base.Properties.GetValueWithNullReplacement("ProcedureSchema");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("ProcedureSchema", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Expensive | SfcPropertyFlags.Standalone)]
	public long RowCount => (long)base.Properties.GetValueWithNullReplacement("RowCount");

	public ServiceQueueEvents Events
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			if (SqlContext.IsAvailable)
			{
				throw new SmoException(ExceptionTemplatesImpl.SmoSQLCLRUnAvailable);
			}
			if (events == null)
			{
				events = new ServiceQueueEvents(this);
			}
			return events;
		}
	}

	internal ServiceQueue(AbstractCollectionBase parentColl, ObjectKeyBase key, SqlSmoState state)
		: base(parentColl, key, state)
	{
	}

	public void Create()
	{
		CreateImpl();
	}

	internal override void ScriptCreate(StringCollection queries, ScriptingPreferences sp)
	{
		SqlSmoObject.ThrowIfBelowVersion90(sp.TargetServerVersionInternal);
		GetDDL(queries, sp, bCreate: true);
	}

	public void Drop()
	{
		DropImpl();
	}

	public void DropIfExists()
	{
		DropImpl(isDropIfExists: true);
	}

	private void GetDDL(StringCollection queries, ScriptingPreferences sp, bool bCreate)
	{
		StringBuilder stringBuilder = new StringBuilder(Globals.INIT_BUFFER_SIZE);
		ScriptIncludeHeaders(stringBuilder, sp, UrnSuffix);
		string arg = FormatFullNameForScripting(sp);
		if (bCreate && sp.IncludeScripts.ExistenceCheck)
		{
			stringBuilder.AppendFormat(SmoApplication.DefaultCulture, Scripts.INCLUDE_EXISTS_SERVICE_QUEUE, "NOT", FormatFullNameForScripting(sp, nameIsIndentifier: false));
			stringBuilder.Append(sp.NewLine);
		}
		stringBuilder.AppendFormat(SmoApplication.DefaultCulture, "{0} QUEUE {1} ", bCreate ? "CREATE" : "ALTER", arg);
		bool flag = false;
		object propValueOptional = GetPropValueOptional("IsEnqueueEnabled");
		if (propValueOptional != null)
		{
			stringBuilder.AppendFormat(SmoApplication.DefaultCulture, "WITH STATUS = {0} ", ((bool)propValueOptional) ? "ON" : "OFF");
			flag = true;
		}
		propValueOptional = GetPropValueOptional("IsRetentionEnabled");
		if (propValueOptional != null)
		{
			if (!flag)
			{
				stringBuilder.AppendFormat(SmoApplication.DefaultCulture, "WITH ");
				flag = true;
			}
			else
			{
				stringBuilder.AppendFormat(SmoApplication.DefaultCulture, ", ");
			}
			stringBuilder.AppendFormat(SmoApplication.DefaultCulture, "RETENTION = {0} ", ((bool)propValueOptional) ? "ON" : "OFF");
		}
		bool flag2 = false;
		StringBuilder stringBuilder2 = null;
		string text = (string)GetPropValueOptional("ProcedureName");
		if (text != null && text.Length > 0)
		{
			flag2 = true;
			stringBuilder2 = new StringBuilder(Globals.INIT_BUFFER_SIZE);
			string text2 = (string)GetPropValueOptional("ProcedureDatabase");
			if (text2 != null && text2.Length > 0)
			{
				stringBuilder2.Append(SqlSmoObject.MakeSqlBraket(text2));
				stringBuilder2.Append(Globals.Dot);
			}
			text2 = (string)GetPropValueOptional("ProcedureSchema");
			if (text2 != null && text2.Length > 0)
			{
				stringBuilder2.Append(SqlSmoObject.MakeSqlBraket(text2));
				stringBuilder2.Append(Globals.Dot);
			}
			stringBuilder2.Append(SqlSmoObject.MakeSqlBraket(text));
			text = stringBuilder2.ToString();
		}
		bool flag3 = !sp.ForDirectExecution;
		propValueOptional = GetPropValueOptional("IsActivationEnabled");
		if ((!flag3 && propValueOptional != null) || (flag3 && flag2 && text != string.Empty))
		{
			if (!flag)
			{
				stringBuilder.AppendFormat(SmoApplication.DefaultCulture, "WITH ");
				flag = true;
			}
			else
			{
				stringBuilder.AppendFormat(SmoApplication.DefaultCulture, ", ");
			}
			stringBuilder.AppendFormat(SmoApplication.DefaultCulture, "ACTIVATION ( ");
			if (!bCreate && flag2 && text == string.Empty)
			{
				stringBuilder.AppendFormat(SmoApplication.DefaultCulture, " DROP ");
			}
			else
			{
				propValueOptional = GetPropValue("IsActivationEnabled");
				bool flag4 = (bool)propValueOptional;
				stringBuilder.AppendFormat(SmoApplication.DefaultCulture, " STATUS = {0} ", flag4 ? "ON" : "OFF");
				if (text != null && text.Length > 0)
				{
					stringBuilder.AppendFormat(SmoApplication.DefaultCulture, ", PROCEDURE_NAME = {0} ", text);
				}
				propValueOptional = GetPropValueOptional("MaxReaders");
				if (propValueOptional != null)
				{
					stringBuilder.AppendFormat(SmoApplication.DefaultCulture, ", MAX_QUEUE_READERS = {0} ", ((short)propValueOptional).ToString(SmoApplication.DefaultCulture));
				}
				propValueOptional = GetPropValueOptional("ActivationExecutionContext");
				if (propValueOptional != null)
				{
					string arg2 = "SELF";
					switch ((ActivationExecutionContext)propValueOptional)
					{
					case ActivationExecutionContext.Owner:
						arg2 = "OWNER";
						break;
					case ActivationExecutionContext.ExecuteAsUser:
						arg2 = (string)GetPropValueOptional("ExecutionContextPrincipal");
						if (arg2 != null && arg2.Length > 0)
						{
							arg2 = SqlSmoObject.MakeSqlString(arg2);
							break;
						}
						throw new WrongPropertyValueException(ExceptionTemplatesImpl.ExecutionContextPrincipalIsNotSpecified);
					}
					stringBuilder.AppendFormat(SmoApplication.DefaultCulture, ", EXECUTE AS {0} ", arg2);
				}
				else
				{
					stringBuilder.AppendFormat(SmoApplication.DefaultCulture, ", EXECUTE AS SELF ");
				}
			}
			stringBuilder.AppendFormat(SmoApplication.DefaultCulture, " )");
		}
		if (sp.TargetServerVersionInternal >= SqlServerVersionInternal.Version105 && (base.ServerVersion.Major > 10 || (base.ServerVersion.Major >= 10 && base.ServerVersion.Minor >= 50)))
		{
			object propValueOptional2 = GetPropValueOptional("IsPoisonMessageHandlingEnabled");
			if (propValueOptional2 != null)
			{
				if (!flag)
				{
					stringBuilder.AppendFormat(SmoApplication.DefaultCulture, "WITH ");
					flag = true;
				}
				else
				{
					stringBuilder.AppendFormat(SmoApplication.DefaultCulture, ", ");
				}
				stringBuilder.AppendFormat(SmoApplication.DefaultCulture, "POISON_MESSAGE_HANDLING (STATUS = {0}) ", ((bool)propValueOptional2) ? "ON" : "OFF");
			}
		}
		if (bCreate)
		{
			string text3 = (string)GetPropValueOptional("FileGroup");
			if (text3 != null && text3.Length > 0)
			{
				stringBuilder.AppendFormat(SmoApplication.DefaultCulture, " ON [{0}] ", SqlSmoObject.SqlBraket(text3));
			}
		}
		queries.Add(stringBuilder.ToString());
	}

	internal override void ScriptDrop(StringCollection queries, ScriptingPreferences sp)
	{
		SqlSmoObject.ThrowIfBelowVersion90(sp.TargetServerVersionInternal);
		StringBuilder stringBuilder = new StringBuilder(Globals.INIT_BUFFER_SIZE);
		ScriptIncludeHeaders(stringBuilder, sp, UrnSuffix);
		if (sp.IncludeScripts.ExistenceCheck)
		{
			stringBuilder.AppendFormat(SmoApplication.DefaultCulture, Scripts.INCLUDE_EXISTS_SERVICE_QUEUE, "", FormatFullNameForScripting(sp, nameIsIndentifier: false));
			stringBuilder.Append(sp.NewLine);
		}
		stringBuilder.AppendFormat(SmoApplication.DefaultCulture, "DROP QUEUE {0}", FormatFullNameForScripting(sp));
		queries.Add(stringBuilder.ToString());
	}

	public void Reorganize()
	{
		ThrowIfBelowVersion130();
		CheckObjectState(throwIfNotCreated: true);
		ReorganizeImpl(null);
	}

	public void Reorganize(bool lobCompaction)
	{
		ThrowIfBelowVersion130();
		CheckObjectState(throwIfNotCreated: true);
		ReorganizeImpl(lobCompaction);
	}

	private void ReorganizeImpl(bool? lobCompaction)
	{
		try
		{
			StringCollection stringCollection = new StringCollection();
			stringCollection.Add(string.Format(SmoApplication.DefaultCulture, Scripts.USEDB, SqlSmoObject.SqlBraket(GetDBName())));
			StringBuilder stringBuilder = new StringBuilder(Globals.INIT_BUFFER_SIZE);
			stringBuilder.AppendFormat(SmoApplication.DefaultCulture, "ALTER QUEUE {0} REORGANIZE", FullQualifiedName);
			if (lobCompaction.HasValue)
			{
				stringBuilder.AppendFormat(SmoApplication.DefaultCulture, " WITH (LOB_COMPACTION = {0})", lobCompaction.Value ? "ON" : "OFF");
			}
			stringCollection.Add(stringBuilder.ToString());
			ExecutionManager.ExecuteNonQuery(stringCollection);
		}
		catch (Exception ex)
		{
			SqlSmoObject.FilterException(ex);
			throw new FailedOperationException(ExceptionTemplatesImpl.Reorganize, this, ex);
		}
	}

	public void Rebuild()
	{
		ThrowIfBelowVersion130();
		CheckObjectState(throwIfNotCreated: true);
		RebuildImpl(null);
	}

	public void Rebuild(int maxDop)
	{
		ThrowIfBelowVersion130();
		CheckObjectState(throwIfNotCreated: true);
		RebuildImpl(maxDop);
	}

	private void RebuildImpl(int? maxDop)
	{
		try
		{
			StringCollection stringCollection = new StringCollection();
			stringCollection.Add(string.Format(SmoApplication.DefaultCulture, Scripts.USEDB, SqlSmoObject.SqlBraket(GetDBName())));
			StringBuilder stringBuilder = new StringBuilder(Globals.INIT_BUFFER_SIZE);
			stringBuilder.AppendFormat(SmoApplication.DefaultCulture, "ALTER QUEUE {0} REBUILD", FullQualifiedName);
			if (maxDop.HasValue)
			{
				stringBuilder.AppendFormat(SmoApplication.DefaultCulture, " WITH (MAXDOP = {0})", maxDop.Value);
			}
			stringCollection.Add(stringBuilder.ToString());
			ExecutionManager.ExecuteNonQuery(stringCollection);
		}
		catch (Exception ex)
		{
			SqlSmoObject.FilterException(ex);
			throw new FailedOperationException(ExceptionTemplatesImpl.Rebuild, this, ex);
		}
	}

	public void MoveTo(string fileGroup)
	{
		ThrowIfBelowVersion130();
		CheckObjectState(throwIfNotCreated: true);
		MoveToImpl(fileGroup, null);
	}

	public void MoveTo(string fileGroup, int maxDop)
	{
		ThrowIfBelowVersion130();
		CheckObjectState(throwIfNotCreated: true);
		MoveToImpl(fileGroup, maxDop);
	}

	private void MoveToImpl(string fileGroup, int? maxDop)
	{
		try
		{
			StringCollection stringCollection = new StringCollection();
			stringCollection.Add(string.Format(SmoApplication.DefaultCulture, Scripts.USEDB, SqlSmoObject.SqlBraket(GetDBName())));
			StringBuilder stringBuilder = new StringBuilder(Globals.INIT_BUFFER_SIZE);
			stringBuilder.AppendFormat(SmoApplication.DefaultCulture, "ALTER QUEUE {0} MOVE TO [{1}]", FullQualifiedName, SqlSmoObject.SqlBraket(fileGroup));
			if (maxDop.HasValue)
			{
				stringBuilder.AppendFormat(SmoApplication.DefaultCulture, " WITH (MAXDOP = {0})", maxDop.Value);
			}
			stringCollection.Add(stringBuilder.ToString());
			ExecutionManager.ExecuteNonQuery(stringCollection);
		}
		catch (Exception ex)
		{
			SqlSmoObject.FilterException(ex);
			throw new FailedOperationException(ExceptionTemplatesImpl.Rebuild, this, ex);
		}
	}

	public void Alter()
	{
		AlterImpl();
	}

	internal override void ScriptAlter(StringCollection queries, ScriptingPreferences sp)
	{
		if (IsObjectDirty())
		{
			GetDDL(queries, sp, bCreate: false);
		}
	}

	public StringCollection Script()
	{
		return ScriptImpl();
	}

	public StringCollection Script(ScriptingOptions scriptingOptions)
	{
		return ScriptImpl(scriptingOptions);
	}

	public ServiceQueue()
	{
	}

	public ServiceQueue(ServiceBroker serviceBroker, string name)
	{
		ValidateName(name);
		key = new SchemaObjectKey(name, null);
		Parent = serviceBroker;
	}

	public ServiceQueue(ServiceBroker serviceBroker, string name, string schema)
	{
		ValidateName(name);
		key = new SchemaObjectKey(name, schema);
		Parent = serviceBroker;
	}

	internal override SqlPropertyMetadataProvider GetPropertyMetadataProvider()
	{
		return new PropertyMetadataProvider(base.ServerVersion, DatabaseEngineType, DatabaseEngineEdition);
	}

	internal override object GetPropertyDefaultValue(string propname)
	{
		if (propname == "ProcedureSchema")
		{
			return "dbo";
		}
		return base.GetPropertyDefaultValue(propname);
	}

	public void Deny(ObjectPermissionSet permission, string[] granteeNames)
	{
		PermissionWorker.Execute(PermissionState.Deny, this, permission, granteeNames, null, grantGrant: false, cascade: false, null);
	}

	public void Deny(ObjectPermissionSet permission, string[] granteeNames, bool cascade)
	{
		PermissionWorker.Execute(PermissionState.Deny, this, permission, granteeNames, null, grantGrant: false, cascade, null);
	}

	public void Grant(ObjectPermissionSet permission, string[] granteeNames)
	{
		PermissionWorker.Execute(PermissionState.Grant, this, permission, granteeNames, null, grantGrant: false, cascade: false, null);
	}

	public void Grant(ObjectPermissionSet permission, string[] granteeNames, bool grantGrant)
	{
		PermissionWorker.Execute(PermissionState.Grant, this, permission, granteeNames, null, grantGrant, cascade: false, null);
	}

	public void Grant(ObjectPermissionSet permission, string[] granteeNames, bool grantGrant, string asRole)
	{
		PermissionWorker.Execute(PermissionState.Grant, this, permission, granteeNames, null, grantGrant, cascade: false, asRole);
	}

	public void Revoke(ObjectPermissionSet permission, string[] granteeNames)
	{
		PermissionWorker.Execute(PermissionState.Revoke, this, permission, granteeNames, null, grantGrant: false, cascade: false, null);
	}

	public void Revoke(ObjectPermissionSet permission, string[] granteeNames, bool revokeGrant, bool cascade)
	{
		PermissionWorker.Execute(PermissionState.Revoke, this, permission, granteeNames, null, revokeGrant, cascade, null);
	}

	public void Revoke(ObjectPermissionSet permission, string[] granteeNames, bool revokeGrant, bool cascade, string asRole)
	{
		PermissionWorker.Execute(PermissionState.Revoke, this, permission, granteeNames, null, revokeGrant, cascade, asRole);
	}

	public void Deny(ObjectPermissionSet permission, string granteeName)
	{
		PermissionWorker.Execute(PermissionState.Deny, this, permission, new string[1] { granteeName }, null, grantGrant: false, cascade: false, null);
	}

	public void Deny(ObjectPermissionSet permission, string granteeName, bool cascade)
	{
		PermissionWorker.Execute(PermissionState.Deny, this, permission, new string[1] { granteeName }, null, grantGrant: false, cascade, null);
	}

	public void Grant(ObjectPermissionSet permission, string granteeName)
	{
		PermissionWorker.Execute(PermissionState.Grant, this, permission, new string[1] { granteeName }, null, grantGrant: false, cascade: false, null);
	}

	public void Grant(ObjectPermissionSet permission, string granteeName, bool grantGrant)
	{
		PermissionWorker.Execute(PermissionState.Grant, this, permission, new string[1] { granteeName }, null, grantGrant, cascade: false, null);
	}

	public void Grant(ObjectPermissionSet permission, string granteeName, bool grantGrant, string asRole)
	{
		PermissionWorker.Execute(PermissionState.Grant, this, permission, new string[1] { granteeName }, null, grantGrant, cascade: false, asRole);
	}

	public void Revoke(ObjectPermissionSet permission, string granteeName)
	{
		PermissionWorker.Execute(PermissionState.Revoke, this, permission, new string[1] { granteeName }, null, grantGrant: false, cascade: false, null);
	}

	public void Revoke(ObjectPermissionSet permission, string granteeName, bool revokeGrant, bool cascade)
	{
		PermissionWorker.Execute(PermissionState.Revoke, this, permission, new string[1] { granteeName }, null, revokeGrant, cascade, null);
	}

	public void Revoke(ObjectPermissionSet permission, string granteeName, bool revokeGrant, bool cascade, string asRole)
	{
		PermissionWorker.Execute(PermissionState.Revoke, this, permission, new string[1] { granteeName }, null, revokeGrant, cascade, asRole);
	}

	public ObjectPermissionInfo[] EnumObjectPermissions()
	{
		return (ObjectPermissionInfo[])PermissionWorker.EnumPermissions(PermissionWorker.PermissionEnumKind.Object, this, null, null);
	}

	public ObjectPermissionInfo[] EnumObjectPermissions(string granteeName)
	{
		return (ObjectPermissionInfo[])PermissionWorker.EnumPermissions(PermissionWorker.PermissionEnumKind.Object, this, granteeName, null);
	}

	public ObjectPermissionInfo[] EnumObjectPermissions(ObjectPermissionSet permissions)
	{
		return (ObjectPermissionInfo[])PermissionWorker.EnumPermissions(PermissionWorker.PermissionEnumKind.Object, this, null, permissions);
	}

	public ObjectPermissionInfo[] EnumObjectPermissions(string granteeName, ObjectPermissionSet permissions)
	{
		return (ObjectPermissionInfo[])PermissionWorker.EnumPermissions(PermissionWorker.PermissionEnumKind.Object, this, granteeName, permissions);
	}
}
