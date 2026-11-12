using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Diagnostics.STrace;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;

namespace Microsoft.SqlServer.Management.Collector;

public sealed class CollectorConfigStore : SfcInstance, ISfcAlterable, IAlterable, ISfcDomain, ISfcDomainLite, ISfcHasConnection
{
	public class Key : DomainRootKey
	{
		public string name;

		public string Name
		{
			get
			{
				return name;
			}
			protected set
			{
				name = value;
			}
		}

		public Key()
			: base(null)
		{
		}

		internal Key(ISfcDomain root)
			: base(root)
		{
		}

		public override bool Equals(object obj)
		{
			return this == obj;
		}

		public override bool Equals(SfcKey key)
		{
			return this == key;
		}

		public new bool Equals(object obj1, object obj2)
		{
			return obj1 == obj2;
		}

		public static bool operator ==(object obj, Key rhs)
		{
			bool flag = obj == null;
			bool flag2 = (object)rhs == null;
			if (flag && flag2)
			{
				return true;
			}
			if (!flag && !flag2 && (object)rhs != null)
			{
				return (Key)obj == rhs;
			}
			return false;
		}

		public static bool operator ==(Key lhs, object obj)
		{
			bool flag = (object)lhs == null;
			bool flag2 = obj == null;
			if (flag && flag2)
			{
				return true;
			}
			if (!flag && !flag2 && (object)lhs != null)
			{
				return (Key)obj == lhs;
			}
			return false;
		}

		public static bool operator ==(Key lhs, Key rhs)
		{
			if ((object)lhs == (object)rhs)
			{
				return true;
			}
			if ((object)lhs == null || (object)rhs == null)
			{
				return false;
			}
			return lhs.IsEqual(rhs);
		}

		private bool IsEqual(Key key)
		{
			return string.CompareOrdinal(name, key.name) == 0;
		}

		public static bool operator !=(object obj, Key rhs)
		{
			return !(obj == rhs);
		}

		public static bool operator !=(Key lhs, object obj)
		{
			return !(lhs == obj);
		}

		public static bool operator !=(Key lhs, Key rhs)
		{
			return !(lhs == rhs);
		}

		public override int GetHashCode()
		{
			return GetType().GetHashCode();
		}

		public override string GetUrnFragment()
		{
			return string.Format("{0}[@Name='{1}']", "CollectorConfigStore", base.Domain.DomainInstanceName);
		}
	}

	private static Guid SP_USAGE_COLLECTOR_GUID = new Guid("B83A3832-37CD-4C7A-88F6-6696DA5A7E43");

	private static Guid TABLE_ANALYSIS_COLLECTOR_GUID = new Guid("3006169E-D33A-4101-A1DE-9F0DABF7D84E");

	internal static Guid[] TransactionPerformanceCollectors = new Guid[2] { TABLE_ANALYSIS_COLLECTOR_GUID, SP_USAGE_COLLECTOR_GUID };

	private static TraceContext tc = TraceContext.GetTraceContext("DC_OM", "Microsoft.SqlServer.Management.Collector.CollectorConfigStore");

	private const int CACH_DIRECTORY_MAX_LENGTH = 255;

	private readonly object syncLock = new object();

	private CollectionSetCollection collectionSets;

	private SfcObjectQuery objectQuery;

	internal const string typeName = "CollectorConfigStore";

	private SqlStoreConnection storeConnection;

	private SfcConnectionContext context;

	[SfcProperty]
	public string MDWInstance
	{
		get
		{
			return (string)base.Properties["MDWInstance"].Value;
		}
		set
		{
			Helpers.CheckPropertyValueLength(value, 128, "MDWInstance");
			tc.TraceInformation("Setting MDWInstance to: {0}", value);
			if (DBNull.Value.Equals(value))
			{
				base.Properties["MDWInstance"].Value = null;
			}
			else
			{
				base.Properties["MDWInstance"].Value = value;
			}
		}
	}

	[SfcProperty]
	public string MDWDatabase
	{
		get
		{
			return (string)base.Properties["MDWDatabase"].Value;
		}
		set
		{
			Helpers.CheckPropertyValueLength(value, 128, "MDWDatabse");
			tc.TraceInformation("Setting MDWDatabase to: {0}", value);
			if (DBNull.Value.Equals(value))
			{
				base.Properties["MDWDatabase"].Value = null;
			}
			else
			{
				base.Properties["MDWDatabase"].Value = value;
			}
		}
	}

	[SfcProperty]
	public string CacheDirectory
	{
		get
		{
			return (string)base.Properties["CacheDirectory"].Value;
		}
		set
		{
			Helpers.CheckPropertyValueLength(value, 255, "CacheDirectory");
			tc.TraceInformation("Setting CacheDirectory to: {0}", value);
			if (DBNull.Value.Equals(value))
			{
				base.Properties["CacheDirectory"].Value = null;
			}
			else
			{
				base.Properties["CacheDirectory"].Value = value;
			}
		}
	}

	[SfcProperty]
	public int CacheWindow
	{
		get
		{
			object value = base.Properties["CacheWindow"].Value;
			if (value == null)
			{
				return 0;
			}
			return (int)value;
		}
		set
		{
			tc.TraceInformation("Setting CacheWindow to: {0}", value);
			if (DBNull.Value.Equals(value))
			{
				tc.Assert(condition: false, "CacheWindow should not be null");
				base.Properties["CacheWindow"].Value = 1;
			}
			else
			{
				base.Properties["CacheWindow"].Value = value;
			}
		}
	}

	[SfcProperty(SfcPropertyFlags.Data)]
	public bool Enabled
	{
		get
		{
			object value = base.Properties["Enabled"].Value;
			if (value == null)
			{
				return false;
			}
			return Convert.ToBoolean(value);
		}
	}

	[SfcObject(SfcContainerRelationship.ChildContainer, SfcContainerCardinality.ZeroToAny, typeof(CollectionSet))]
	public CollectionSetCollection CollectionSets
	{
		get
		{
			if (collectionSets == null)
			{
				collectionSets = new CollectionSetCollection(this);
			}
			return collectionSets;
		}
	}

	[SfcObject(SfcObjectRelationship.Object, SfcObjectCardinality.One)]
	internal SfcObjectQuery SfcObjectQuery
	{
		get
		{
			if (objectQuery == null)
			{
				objectQuery = new SfcObjectQuery(this);
			}
			return objectQuery;
		}
	}

	[SfcIgnore]
	internal object SyncLock => syncLock;

	[SfcIgnore]
	public Key IdentityKey => (Key)base.AbstractIdentityKey;

	SfcConnectionContext ISfcHasConnection.ConnectionContext
	{
		get
		{
			if (context == null)
			{
				context = new SfcConnectionContext(this);
			}
			return context;
		}
	}

	string ISfcDomainLite.DomainName => "Collector";

	string ISfcDomainLite.DomainInstanceName => storeConnection.ServerConnection.TrueName;

	public void ConfigureTransactionPerformanceCollectors(SqlConnection warehouseConnection)
	{
		if (warehouseConnection == null)
		{
			throw new ArgumentNullException("warehouseConnection");
		}
		using (tc.GetMethodContext("ConfigureTransactionPerformanceCollectors"))
		{
			CreateStoredProcCollector(warehouseConnection);
			CreateTableAnalysisCollector(warehouseConnection);
		}
	}

	public CollectorConfigStore()
	{
		using (tc.GetMethodContext("CollectorConfigStore"))
		{
		}
	}

	public CollectorConfigStore(SqlStoreConnection connection)
	{
		using MethodTraceContext methodTraceContext = tc.GetMethodContext("CollectorConfigStore");
		if (connection == null)
		{
			throw new CollectorException(ExceptionTemplatesSR.SqlConnectionCannotBeNull);
		}
		methodTraceContext.TraceParameterIn("connection", "(non-null)");
		storeConnection = connection;
		MarkRootAsConnected();
	}

	ISfcScript ISfcAlterable.ScriptAlter()
	{
		using (tc.GetMethodContext("ScriptAlter"))
		{
			return new SfcTSqlScript(GetAlterScript())
			{
				ExecutionMode = SfcTSqlExecutor.ExecutionMode.NonQuery
			};
		}
	}

	public void Alter()
	{
		using MethodTraceContext methodTraceContext = tc.GetMethodContext("Alter");
		methodTraceContext.TraceInformation("Altering CollectorConfigStore");
		lock (SyncLock)
		{
			AlterImpl();
		}
	}

	public StringCollection EnumTypes()
	{
		using (tc.GetMethodContext("EnumTypes"))
		{
			string sqlCommand = "SELECT name FROM [msdb].[dbo].[syscollector_collector_types]";
			SqlStoreConnection obj = (SqlStoreConnection)((ISfcHasConnection)this).GetConnection();
			StringCollection stringCollection = new StringCollection();
			using (SqlDataReader sqlDataReader = obj.ServerConnection.ExecuteReader(sqlCommand))
			{
				while (sqlDataReader.Read())
				{
					stringCollection.Add((string)sqlDataReader[0]);
				}
			}
			return stringCollection;
		}
	}

	public void EnableCollector()
	{
		using MethodTraceContext methodTraceContext = tc.GetMethodContext("EnableCollector");
		methodTraceContext.TraceInformation("Enabling data collector");
		ExecuteScript("Exec [msdb].[dbo].[sp_syscollector_enable_collector]", ExceptionTemplatesSR.UnableToEnableCollector);
		base.Properties["Enabled"].Value = true;
		SfcApplication.Events.OnObjectAltered(this, new SfcObjectAlteredEventArgs(base.Urn, this));
	}

	public void DisableCollector()
	{
		using MethodTraceContext methodTraceContext = tc.GetMethodContext("DisableCollector");
		methodTraceContext.TraceInformation("Disabling data collector");
		ExecuteScript("Exec [msdb].[dbo].[sp_syscollector_disable_collector]", ExceptionTemplatesSR.UnableToDisableCollector);
		base.Properties["Enabled"].Value = false;
		SfcApplication.Events.OnObjectAltered(this, new SfcObjectAlteredEventArgs(base.Urn, this));
	}

	private string GetAlterScript()
	{
		using (tc.GetMethodContext("GetAlterScript"))
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Begin Transaction");
			stringBuilder.AppendLine("Begin Try");
			stringBuilder.AppendLine(ScriptAlterProperty("[msdb].[dbo].[sp_syscollector_set_warehouse_instance_name]", MDWInstance, "instance_name"));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine(ScriptAlterProperty("[msdb].[dbo].[sp_syscollector_set_warehouse_database_name]", MDWDatabase, "database_name"));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine(ScriptAlterProperty("[msdb].[dbo].[sp_syscollector_set_cache_window]", CacheWindow, "cache_window"));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine(ScriptAlterProperty("[msdb].[dbo].[sp_syscollector_set_cache_directory]", CacheDirectory, "cache_directory"));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Commit Transaction;");
			stringBuilder.AppendLine("End Try");
			stringBuilder.AppendLine("Begin Catch");
			stringBuilder.AppendLine("Rollback Transaction;");
			stringBuilder.AppendLine(RethrowErrorScript());
			stringBuilder.AppendLine("End Catch;");
			return stringBuilder.ToString();
		}
	}

	private string ScriptAlterProperty(string procName, object propValue, string paramName)
	{
		using (tc.GetMethodContext("ScriptAlterProperty"))
		{
			TsqlProcFormatter tsqlProcFormatter = new TsqlProcFormatter();
			List<TsqlProcFormatter.RuntimeArg> list = new List<TsqlProcFormatter.RuntimeArg>();
			tsqlProcFormatter.Procedure = procName;
			if (propValue != null)
			{
				tsqlProcFormatter.Arguments.Add(new TsqlProcFormatter.SprocArg(paramName, required: true));
				if (propValue is string)
				{
					list.Add(new TsqlProcFormatter.RuntimeArg(typeof(string), propValue));
				}
				else if (propValue is int)
				{
					list.Add(new TsqlProcFormatter.RuntimeArg(typeof(int), propValue));
				}
			}
			return tsqlProcFormatter.GenerateScript(this, list);
		}
	}

	private void ExecuteScript(string script, string exceptionString)
	{
		using (tc.GetMethodContext("ExecuteScript"))
		{
			SqlStoreConnection sqlStoreConnection = (SqlStoreConnection)((ISfcHasConnection)this).GetConnection();
			try
			{
				lock (sqlStoreConnection)
				{
					sqlStoreConnection.ServerConnection.ExecuteNonQuery(script);
				}
			}
			catch (Exception innerException)
			{
				throw new CollectorException(exceptionString, innerException);
			}
		}
	}

	internal static string RethrowErrorScript()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("DECLARE @ErrorMessage NVARCHAR(4000);");
		stringBuilder.AppendLine("DECLARE @ErrorSeverity INT;");
		stringBuilder.AppendLine("DECLARE @ErrorState INT;");
		stringBuilder.AppendLine("DECLARE @ErrorNumber INT;");
		stringBuilder.AppendLine("DECLARE @ErrorLine INT;");
		stringBuilder.AppendLine("DECLARE @ErrorProcedure NVARCHAR(200);");
		stringBuilder.AppendLine("SELECT @ErrorLine = ERROR_LINE(),");
		stringBuilder.AppendLine("       @ErrorSeverity = ERROR_SEVERITY(),");
		stringBuilder.AppendLine("       @ErrorState = ERROR_STATE(),");
		stringBuilder.AppendLine("       @ErrorNumber = ERROR_NUMBER(),");
		stringBuilder.AppendLine("       @ErrorMessage = ERROR_MESSAGE(),");
		stringBuilder.AppendLine("       @ErrorProcedure = ISNULL(ERROR_PROCEDURE(), '-');");
		stringBuilder.AppendLine("RAISERROR (14684, @ErrorSeverity, 1 , @ErrorNumber, @ErrorSeverity, @ErrorState, @ErrorProcedure, @ErrorLine, @ErrorMessage);");
		return stringBuilder.ToString();
	}

	internal bool IsCleanupProcSupportedOnServer()
	{
		using (tc.GetMethodContext("IsCleanupProcSupportedOnServer"))
		{
			if (((SqlStoreConnection)((ISfcHasConnection)this).GetConnection()).ServerVersion.Major >= 12)
			{
				return true;
			}
			return false;
		}
	}

	private void AlterUID(Guid currentVal, Guid newVal)
	{
		using (tc.GetMethodContext("AlterUID"))
		{
			string script = string.Format("UPDATE msdb.dbo.syscollector_collection_sets_internal\r\nSET collection_set_uid = '{1}'\r\nWHERE collection_set_uid = '{0}'", currentVal.ToString(), newVal.ToString());
			ExecuteScript(script, "temp");
		}
	}

	protected override SfcKey CreateIdentityKey()
	{
		return new Key(this);
	}

	protected override ISfcCollection GetChildCollection(string elementType)
	{
		if (elementType == "CollectionSet")
		{
			return CollectionSets;
		}
		throw new CollectorException(ExceptionTemplatesSR.NoSuchCollection(elementType));
	}

	ISfcConnection ISfcHasConnection.GetConnection()
	{
		return ((ISfcHasConnection)this).GetConnection(SfcObjectQueryMode.SingleActiveQuery);
	}

	void ISfcHasConnection.SetConnection(ISfcConnection connection)
	{
		using (tc.GetMethodContext("SetConnection"))
		{
			if (connection is ServerConnection)
			{
				storeConnection = new SqlStoreConnection(((ServerConnection)connection).SqlConnectionObject);
				return;
			}
			if (connection is SqlStoreConnection)
			{
				storeConnection = (SqlStoreConnection)connection;
				return;
			}
			if (connection == null)
			{
				throw new CollectorException(ExceptionTemplatesSR.SqlConnectionCannotBeNull);
			}
			throw new CollectorException(ExceptionTemplatesSR.UnsupportedConnectionType(connection.GetType().FullName));
		}
	}

	ISfcConnection ISfcHasConnection.GetConnection(SfcObjectQueryMode activeQueriesMode)
	{
		switch (activeQueriesMode)
		{
		case SfcObjectQueryMode.SingleActiveQuery:
			return storeConnection;
		case SfcObjectQueryMode.MultipleActiveQueries:
			if (storeConnection.ServerConnection.MultipleActiveResultSets)
			{
				return storeConnection;
			}
			return null;
		default:
			return null;
		}
	}

	Type ISfcDomain.GetType(string typeName)
	{
		return typeName switch
		{
			"CollectorConfigStore" => typeof(CollectorConfigStore), 
			"CollectionSet" => typeof(CollectionSet), 
			"CollectionItem" => typeof(CollectionItem), 
			_ => throw new CollectorException(ExceptionTemplatesSR.UnsupportedObjectType(typeName, "CollectorConfigStore")), 
		};
	}

	ISfcExecutionEngine ISfcDomain.GetExecutionEngine()
	{
		return new SfcTSqlExecutionEngine((ServerConnection)storeConnection.ToEnumeratorObject());
	}

	SfcKey ISfcDomain.GetKey(IUrnFragment urnFragment)
	{
		return urnFragment.Name switch
		{
			"CollectorConfigStore" => new Key(this), 
			"CollectionSet" => new CollectionSet.Key(urnFragment.FieldDictionary), 
			"CollectionItem" => new CollectionItem.Key(urnFragment.FieldDictionary), 
			_ => throw new CollectorException(ExceptionTemplatesSR.UnsupportedObjectType("CollectorConfigStore", "CollectorConfigStore")), 
		};
	}

	SfcTypeMetadata ISfcDomain.GetTypeMetadata(string typeName)
	{
		switch (typeName)
		{
		case "CollectionItem":
			return CollectionItem.GetTypeMetadata();
		case "CollectionSet":
		case "CollectorConfigStore":
			return null;
		default:
			throw new CollectorException(ExceptionTemplatesSR.UnsupportedObjectType(typeName, "CollectorConfigStore"));
		}
	}

	bool ISfcDomain.UseSfcStateManagement()
	{
		return true;
	}

	int ISfcDomainLite.GetLogicalVersion()
	{
		return 1;
	}

	private void CreateMDWTable(SqlConnection mdwConnection, string tableName, List<SqlParameter> columns)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat("IF (OBJECT_ID('[custom_snapshots].[{0}]', 'U') IS NULL) \r\nBEGIN \r\nCREATE TABLE [custom_snapshots].[{0}] (", tableName);
		bool flag = true;
		foreach (SqlParameter column in columns)
		{
			stringBuilder.AppendLine();
			if (!flag)
			{
				stringBuilder.Append("    ,");
			}
			else
			{
				stringBuilder.Append("    ");
				flag = false;
			}
			if (column.Size != 0)
			{
				stringBuilder.AppendFormat("[{0}] {1}({2})", column.ParameterName, column.SqlDbType.ToString(), column.Size);
			}
			else
			{
				stringBuilder.AppendFormat("[{0}] {1}", column.ParameterName, column.SqlDbType.ToString());
			}
		}
		stringBuilder.AppendFormat("\r\n    ,[database_name] NVARCHAR(128)\r\n    ,[collection_time] DATETIMEOFFSET(7)\r\n    ,[snapshot_id] INT CONSTRAINT [FK_{0}_snapshots_internal] FOREIGN KEY(snapshot_id) \r\nREFERENCES core.snapshots_internal (snapshot_id) ON DELETE CASCADE\r\n); \r\nCREATE NONCLUSTERED INDEX [IDX_{0}] ON  [custom_snapshots].[{0}] (snapshot_id) WITH (ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON);\r\nEND;", tableName);
		try
		{
			if (mdwConnection.State != ConnectionState.Open)
			{
				mdwConnection.Open();
			}
			new SqlCommand(stringBuilder.ToString(), mdwConnection).ExecuteNonQuery();
		}
		catch (Exception innerException)
		{
			throw new CollectorException(ExceptionTemplatesSR.UnableToCreateMDWObjects, innerException);
		}
	}

	private void CreateStoredProcCollector(SqlConnection warehouseConnection)
	{
		using MethodTraceContext methodTraceContext = tc.GetMethodContext("CreateStoredProcCollector");
		CollectionSet collectionSet = CollectionSets[new CollectionSet.Key(ExceptionTemplatesSR.StoredProcAnalysisCollectorName)];
		if (collectionSet != null)
		{
			methodTraceContext.TraceInformation("'{0}' collection set exists on the server. Existing collection set will be deleted.", ExceptionTemplatesSR.StoredProcAnalysisCollectorName);
			collectionSet.Drop();
		}
		collectionSet = new CollectionSet(this, ExceptionTemplatesSR.StoredProcAnalysisCollectorName);
		collectionSet.Description = ExceptionTemplatesSR.StoredProcAnalysisCollectorDesc;
		collectionSet.CollectionMode = CollectionSet.CollectionModes.Cached;
		collectionSet.ScheduleName = "CollectorSchedule_Every_30min";
		collectionSet.DaysUntilExpiration = 30;
		CollectionItem collectionItem = new CollectionItem(collectionSet, ExceptionTemplatesSR.SPStatsName);
		string text = "storedprocedure_usage_stats";
		collectionItem.CollectionFrequency = TimeSpan.FromMinutes(15.0);
		collectionItem.TypeName = "Generic T-SQL Query Collector Type";
		collectionItem.Parameters = $"<ns:TSQLQueryCollector xmlns:ns=\"DataCollectorType\">\r\n<Query>\r\n<Value>--we need to filter out native stored procedures\r\n--but uses_native_compilation is only defined on SQL14+ instances\r\n--so, we should only execute the query if native compilation is supported\r\nDECLARE @IsNativeSPSupported BIT\r\nSELECT @IsNativeSPSupported = CAST(ISNULL(SERVERPROPERTY('ISXTPSUPPORTED'), 0) AS BIT)\r\nDECLARE @NativeProcs TABLE(proc_id int NOT NULL)\r\nDECLARE @command NVARCHAR(1000)\r\nSET @command = N'SELECT object_id FROM sys.sql_modules WHERE uses_native_compilation = 1'\r\nIF @IsNativeSPSupported = 1 BEGIN \r\nINSERT INTO @NativeProcs EXEC (@command)\r\nEND\r\n\r\nSELECT  d.object_id , \r\n        d.database_id AS [db_id], \r\n        db_name(d.database_id) AS [db_name], \r\n        OBJECT_NAME(object_id, database_id) AS [sp_name],\r\n        d.cached_time,\r\n        d.total_worker_time, \r\n        d.total_logical_reads, \r\n        d.total_logical_writes, \r\n        d.total_physical_reads, \r\n        d.total_elapsed_time, \r\n        d.execution_count \r\nFROM sys.dm_exec_procedure_stats AS d \r\nWHERE d.database_id > 4 \r\nAND d.database_id != 32767 \r\nAND d.object_id NOT IN (SELECT proc_id FROM @NativeProcs)</Value>\r\n<OutputTable>{text}</OutputTable>\r\n</Query>\r\n<Databases>\r\n<Database>master</Database>\r\n</Databases>\r\n</ns:TSQLQueryCollector>";
		collectionSet.CollectionItems.Add(collectionItem);
		List<SqlParameter> list = new List<SqlParameter>();
		list.Add(new SqlParameter("object_id", SqlDbType.Int));
		list.Add(new SqlParameter("db_id", SqlDbType.Int));
		list.Add(new SqlParameter("db_name", SqlDbType.NVarChar, 128));
		list.Add(new SqlParameter("sp_name", SqlDbType.NVarChar, 128));
		list.Add(new SqlParameter("cached_time", SqlDbType.DateTime));
		list.Add(new SqlParameter("total_worker_time", SqlDbType.BigInt));
		list.Add(new SqlParameter("total_logical_reads", SqlDbType.BigInt));
		list.Add(new SqlParameter("total_logical_writes", SqlDbType.BigInt));
		list.Add(new SqlParameter("total_physical_reads", SqlDbType.BigInt));
		list.Add(new SqlParameter("total_elapsed_time", SqlDbType.BigInt));
		list.Add(new SqlParameter("execution_count", SqlDbType.BigInt));
		CreateMDWTable(warehouseConnection, text, list);
		CollectionItem collectionItem2 = new CollectionItem(collectionSet, ExceptionTemplatesSR.SPTableRefs);
		text = "storedprocedure_table_references";
		collectionItem2.CollectionFrequency = TimeSpan.FromHours(24.0);
		collectionItem2.TypeName = "Generic T-SQL Query Collector Type";
		collectionItem2.Parameters = $"<ns:TSQLQueryCollector xmlns:ns=\"DataCollectorType\">\r\n<Query>\r\n<Value>--we need to filter out native stored procedures\r\n--but uses_native_compilation is only defined on SQL14+ instances\r\n--so, we should only execute the query if native compilation is supported\r\nDECLARE @IsNativeSPSupported BIT\r\nSELECT @IsNativeSPSupported = CAST(ISNULL(SERVERPROPERTY('ISXTPSUPPORTED'), 0) AS BIT)\r\nDECLARE @NativeProcs TABLE(proc_id int NOT NULL)\r\nDECLARE @command NVARCHAR(1000)\r\nSET @command = N'SELECT object_id FROM sys.sql_modules WHERE uses_native_compilation = 1'\r\nIF @IsNativeSPSupported = 1 BEGIN \r\nINSERT INTO @NativeProcs EXEC (@command)\r\nEND\r\n\r\nSELECT DB_ID() AS db_id,\r\n    dep.referencing_id AS referencing_object_id,\r\n    OBJECT_NAME(dep.referencing_id) AS referencing_object_name,\r\n    -- we can have the full name including even the server name for the referenced objects!\r\n    -- great for detecting linked servers and cross-database transactions!\r\n    dep.referenced_server_name,\r\n    dep.referenced_database_name,\r\n    dep.referenced_schema_name,\r\n    dep.referenced_id AS referenced_entity_id,\r\n    dep.referenced_entity_name\r\nFROM sys.sql_expression_dependencies dep\r\nWHERE \r\n    dep.referencing_id NOT IN (SELECT proc_id FROM @NativeProcs)\r\n    -- we only want proc references\r\n    AND \r\n    dep.referencing_id IN (SELECT object_id FROM sys.procedures WHERE is_ms_shipped = 0) \r\n    -- this eliminates any non-table references within the proc''s resident database. \r\n    -- Outside the proc''s resident database, we can''t get any information because the ID is null.\r\n    AND dep.referenced_class = 1 \r\n    AND (dep.referenced_id IN (SELECT object_id FROM sys.tables) OR dep.referenced_id IS NULL)\r\n    AND is_ambiguous = 0\r\n    AND is_caller_dependent = 0</Value>\r\n<OutputTable>{text}</OutputTable>\r\n</Query>\r\n<Databases UseUserDatabases=\"true\" />\r\n</ns:TSQLQueryCollector>";
		collectionSet.CollectionItems.Add(collectionItem2);
		list = new List<SqlParameter>();
		list.Add(new SqlParameter("db_id", SqlDbType.Int));
		list.Add(new SqlParameter("referencing_object_id", SqlDbType.Int));
		list.Add(new SqlParameter("referencing_object_name", SqlDbType.NVarChar, 128));
		list.Add(new SqlParameter("referenced_server_name", SqlDbType.NVarChar, 128));
		list.Add(new SqlParameter("referenced_database_name", SqlDbType.NVarChar, 128));
		list.Add(new SqlParameter("referenced_schema_name", SqlDbType.NVarChar, 128));
		list.Add(new SqlParameter("referenced_entity_id", SqlDbType.Int));
		list.Add(new SqlParameter("referenced_entity_name", SqlDbType.NVarChar, 128));
		CreateMDWTable(warehouseConnection, text, list);
		try
		{
			collectionSet.Create();
			AlterUID(collectionSet.UId, SP_USAGE_COLLECTOR_GUID);
			collectionSet.Refresh();
		}
		catch (Exception innerException)
		{
			throw new CollectorException(ExceptionTemplatesSR.UnableToCreateCollectionSet(collectionSet.Name), innerException);
		}
	}

	private void CreateTableAnalysisCollector(SqlConnection warehouseConnection)
	{
		using MethodTraceContext methodTraceContext = tc.GetMethodContext("CreateTableAnalysisCollector");
		CollectionSet collectionSet = CollectionSets[new CollectionSet.Key(ExceptionTemplatesSR.TableAnalysisCollectorName)];
		if (collectionSet != null)
		{
			methodTraceContext.TraceInformation("'{0}' collection set exists on the server. Existing collection set will be deleted.", ExceptionTemplatesSR.TableAnalysisCollectorName);
			collectionSet.Drop();
		}
		collectionSet = new CollectionSet(this, ExceptionTemplatesSR.TableAnalysisCollectorName);
		collectionSet.Description = ExceptionTemplatesSR.TableAnalysisCollectorDesc;
		collectionSet.CollectionMode = CollectionSet.CollectionModes.Cached;
		collectionSet.ScheduleName = "CollectorSchedule_Every_15min";
		collectionSet.DaysUntilExpiration = 30;
		CollectionItem collectionItem = new CollectionItem(collectionSet, ExceptionTemplatesSR.TableStatsName);
		string text = "table_usage_stats";
		collectionItem.CollectionFrequency = TimeSpan.FromMinutes(15.0);
		collectionItem.TypeName = "Generic T-SQL Query Collector Type";
		collectionItem.Parameters = $"<ns:TSQLQueryCollector xmlns:ns=\"DataCollectorType\">\r\n<Query>\r\n<Value>--we need to filter out memory optimized tables\r\n--but is_memory_optimized is only defined on SQL14+ instances\r\n--so, we should only execute the query if hekaton is supported\r\nDECLARE @IsXtpSupported BIT\r\nSELECT @IsXtpSupported = CAST(ISNULL(SERVERPROPERTY('ISXTPSUPPORTED'), 0) AS BIT)\r\nDECLARE @MemoryOptimizedTables TABLE(table_id int NOT NULL)\r\nDECLARE @command NVARCHAR(1000)\r\nSET @command = N'SELECT [object_id] FROM sys.tables WHERE [is_memory_optimized] = 1'\r\nIF @IsXtpSupported = 1 BEGIN \r\nINSERT INTO @MemoryOptimizedTables EXEC (@command)\r\nEND\r\nSELECT\r\n    i.database_id,\r\n    i.object_id AS table_id,\r\n    obj.name as table_name,\r\n    i.index_id,\r\n    -- access statistics\r\n    i.singleton_lookup_count,\r\n    i.range_scan_count,\r\n    -- contention statistics\r\n    i.page_latch_wait_count,\r\n    i.page_latch_wait_in_ms,\r\n    i.page_lock_count,\r\n    i.page_lock_wait_count,\r\n    i.page_lock_wait_in_ms\r\nFROM sys.dm_db_index_operational_stats(DB_ID(), NULL, NULL, NULL) i\r\nJOIN sys.objects obj ON ( obj.object_id = i.object_id AND obj.is_ms_shipped = 0)\r\nWHERE i.object_id NOT IN (SELECT * FROM @MemoryOptimizedTables)</Value>\r\n<OutputTable>{text}</OutputTable>\r\n</Query>\r\n<Databases UseUserDatabases=\"true\">\r\n<Database>tempdb</Database>\r\n</Databases>\r\n</ns:TSQLQueryCollector>";
		collectionSet.CollectionItems.Add(collectionItem);
		List<SqlParameter> list = new List<SqlParameter>();
		list.Add(new SqlParameter("database_id", SqlDbType.Int));
		list.Add(new SqlParameter("table_id", SqlDbType.Int));
		list.Add(new SqlParameter("table_name", SqlDbType.NVarChar, 128));
		list.Add(new SqlParameter("index_id", SqlDbType.Int));
		list.Add(new SqlParameter("singleton_lookup_count", SqlDbType.BigInt));
		list.Add(new SqlParameter("range_scan_count", SqlDbType.BigInt));
		list.Add(new SqlParameter("page_latch_wait_count", SqlDbType.BigInt));
		list.Add(new SqlParameter("page_latch_wait_in_ms", SqlDbType.BigInt));
		list.Add(new SqlParameter("page_lock_count", SqlDbType.BigInt));
		list.Add(new SqlParameter("page_lock_wait_count", SqlDbType.BigInt));
		list.Add(new SqlParameter("page_lock_wait_in_ms", SqlDbType.BigInt));
		CreateMDWTable(warehouseConnection, text, list);
		CollectionItem collectionItem2 = new CollectionItem(collectionSet, ExceptionTemplatesSR.TransactionCount);
		text = "database_transaction_stats";
		collectionItem2.CollectionFrequency = TimeSpan.FromMinutes(15.0);
		collectionItem2.TypeName = "Generic T-SQL Query Collector Type";
		collectionItem2.Parameters = $"<ns:TSQLQueryCollector xmlns:ns=\"DataCollectorType\">\r\n<Query>\r\n<Value>SELECT\r\n    db.database_id as database_id,  \r\n    p.cntr_value AS transaction_count \r\nFROM sys.dm_os_performance_counters p,\r\n     sys.databases db\r\nWHERE p.counter_name LIKE 'Transactions/sec%'\r\n      AND p.instance_name = db.name COLLATE Latin1_General_BIN2\r\n      AND p.instance_name = DB_NAME() COLLATE Latin1_General_BIN2</Value>\r\n<OutputTable>{text}</OutputTable>\r\n</Query>\r\n<Databases UseUserDatabases=\"true\">\r\n<Database>tempdb</Database>\r\n</Databases>\r\n</ns:TSQLQueryCollector>";
		collectionSet.CollectionItems.Add(collectionItem2);
		list = new List<SqlParameter>();
		list.Add(new SqlParameter("database_id", SqlDbType.Int));
		list.Add(new SqlParameter("transaction_count", SqlDbType.BigInt));
		CreateMDWTable(warehouseConnection, text, list);
		CollectionItem collectionItem3 = new CollectionItem(collectionSet, ExceptionTemplatesSR.MigrationBlockers);
		text = "metadata_table_blockers";
		collectionItem3.CollectionFrequency = TimeSpan.FromDays(1.0);
		collectionItem3.TypeName = "Generic T-SQL Query Collector Type";
		collectionItem3.Parameters = $"<ns:TSQLQueryCollector xmlns:ns=\"DataCollectorType\">\r\n<Query>\r\n<Value>--we need to filter out memory optimized tables\r\n--but is_memory_optimized is only defined on SQL14+ instances\r\n--so, we should only execute the query if hekaton is supported\r\nDECLARE @IsXtpSupported BIT\r\nDECLARE @SQLMAJORVERSION INT\r\nDECLARE @SQL13AZURE12 BIT\r\n\r\nSELECT @IsXtpSupported = CAST(ISNULL(SERVERPROPERTY('ISXTPSUPPORTED'), 0) AS BIT)\r\nSELECT @SQLMAJORVERSION  = LEFT(CONVERT(SYSNAME,SERVERPROPERTY('ProductVersion')), CHARINDEX('.', CONVERT(SYSNAME,SERVERPROPERTY('ProductVersion')), 0)-1)\r\nSET @SQL13AZURE12 = 0\r\nIF @IsXtpSupported = 1\r\nBEGIN\r\nIF CAST(SERVERPROPERTY('ENGINEEDITION') AS INT) = 5\r\nBEGIN\r\nIF @SQLMAJORVERSION &gt;= 12\r\nSET @SQL13AZURE12 = 1\r\nEND\r\nELSE\r\nBEGIN\r\nIF @SQLMAJORVERSION &gt;= 13\r\nSET @SQL13AZURE12 = 1\r\nEND\r\nEND\r\n\r\nDECLARE @MemoryOptimizedTables TABLE(table_id int NOT NULL)\r\nDECLARE @DefaultContraintTables TABLE(table_id int NOT NULL)\r\nDECLARE @command NVARCHAR(1000)\r\n\r\nSET @command = N'SELECT [object_id] FROM sys.tables WHERE [is_memory_optimized] = 1'\r\nIF @IsXtpSupported = 1 BEGIN \r\nINSERT INTO @MemoryOptimizedTables EXEC (@command)\r\nEND\r\nSET @command = N'SELECT [parent_object_id] FROM sys.default_constraints'\r\nIF @SQL13AZURE12 = 0 BEGIN \r\nINSERT INTO @DefaultContraintTables EXEC (@command)\r\nEND\r\n\r\nSELECT\r\n    db_id AS database_id,\r\n    table_id, \r\n    OBJECT_NAME(table_id, db_id) AS table_name,\r\n    COUNT(constraint_name) AS migration_rating\r\nFROM (\r\n    SELECT  DB_ID() AS [db_id],\r\n            t.parent_object_id AS [table_id], \r\n            t.name AS [constraint_name]\r\n    FROM sys.check_constraints t \r\n    JOIN sys.tables tbl ON tbl.object_id = t.parent_object_id\r\n    WHERE tbl.is_ms_shipped = 0 \r\n        AND object_name(t.parent_object_id,DB_ID()) IS NOT NULL\r\n    UNION\r\n    SELECT  DB_ID() AS [db_id],\r\n            t.parent_object_id AS [table_id], \r\n            t.name AS [constraint_name]\r\n    FROM sys.default_constraints t \r\n    JOIN sys.tables tbl ON tbl.object_id = t.parent_object_id\r\n    WHERE tbl.is_ms_shipped = 0 \r\n        AND object_name(t.parent_object_id,DB_ID()) IS NOT NULL\r\n        AND t.parent_object_id IN (SELECT * FROM @DefaultContraintTables)\r\n    UNION\r\n    SELECT  DB_ID() AS [db_id],\r\n            t.parent_object_id AS [table_id], \r\n            t.name AS [constraint_name]\r\n    FROM sys.foreign_keys t \r\n    JOIN sys.tables tbl ON tbl.object_id = t.parent_object_id\r\n    WHERE tbl.is_ms_shipped = 0 \r\n        AND object_name(t.parent_object_id,DB_ID()) IS NOT NULL\r\n    UNION\r\n    SELECT  DB_ID() AS [db_id],\r\n            t.object_id AS [table_id], \r\n            t.name AS [constraint_name]\r\n    FROM sys.indexes t \r\n    JOIN sys.tables tbl ON tbl.object_id = t.object_id\r\n    WHERE tbl.is_ms_shipped = 0 \r\n        AND object_name(t.object_id,DB_ID()) IS NOT NULL \r\n        AND (((t.is_unique = 1 OR t.is_unique_constraint = 1) AND t.is_primary_key = 0)\r\n        OR t.type > 2)\r\n    UNION\r\n    SELECT DB_ID() AS [db_id],\r\n        t.object_id AS [table_id], \r\n        c.name AS [constraint_name] \r\n    FROM sys.index_columns ic\r\n    JOIN sys.tables t ON t.object_id = ic.object_id\r\n    JOIN sys.all_columns c ON ic.column_id = c.column_id AND t.object_id = c.object_id\r\n    WHERE t.is_ms_shipped = 0 AND c.is_nullable = 1\r\n    UNION\r\n    SELECT  DB_ID() AS [db_id],\r\n            tb.object_id AS [table_id],\r\n            tr.name AS [constraint_name]\r\n    FROM sys.triggers tr \r\n    JOIN sys.tables tb ON tr.parent_id = tb.object_id \r\n    WHERE tr.is_ms_shipped = 0 \r\n        AND tr.parent_class = 1\r\n    UNION\r\n    SELECT DB_ID() AS [db_id],\r\n        t.object_id AS [table_id], \r\n        t.name AS [constraint_name]\r\n    FROM sys.all_columns t \r\n    JOIN sys.tables tbl ON tbl.object_id = t.object_id\r\n    JOIN sys.types tp ON t.user_type_id = tp.user_type_id\r\n    LEFT JOIN sys.identity_columns ic ON t.object_id = ic.object_id AND t.column_id = ic.column_id\r\n    WHERE tbl.is_ms_shipped = 0 AND\r\n        (t.is_computed = 1\r\n        OR t.is_sparse = 1\r\n        OR (t.is_identity = 1 AND (ic.increment_value != 1 OR ic.seed_value != 1))\r\n        OR t.user_type_id != t.system_type_id\r\n        OR tp.is_assembly_type = 1\r\n        OR UPPER(tp.name) IN ('DATETIMEOFFSET', 'GEOGRAPHY', 'GEOMETRY', 'SQL_VARIANT', 'HIERARCHYID', 'XML', 'IMAGE', 'XML', 'TEXT', 'NTEXT', 'TIMESTAMP')\r\n        OR t.is_filestream = 1\r\n        OR t.max_length = -1)\r\n) AS t\r\nGROUP BY db_id, table_id\r\nHAVING table_id NOT IN (SELECT * FROM @MemoryOptimizedTables)</Value>\r\n<OutputTable>{text}</OutputTable>\r\n</Query>\r\n<Databases UseUserDatabases=\"true\">\r\n<Database>tempdb</Database>\r\n</Databases>\r\n</ns:TSQLQueryCollector>";
		collectionSet.CollectionItems.Add(collectionItem3);
		list = new List<SqlParameter>();
		list.Add(new SqlParameter("database_id", SqlDbType.Int));
		list.Add(new SqlParameter("table_id", SqlDbType.Int));
		list.Add(new SqlParameter("table_name", SqlDbType.NVarChar, 128));
		list.Add(new SqlParameter("migration_rating", SqlDbType.Int));
		CreateMDWTable(warehouseConnection, text, list);
		try
		{
			collectionSet.Create();
			AlterUID(collectionSet.UId, TABLE_ANALYSIS_COLLECTOR_GUID);
			collectionSet.Refresh();
		}
		catch (Exception innerException)
		{
			throw new CollectorException(ExceptionTemplatesSR.UnableToCreateCollectionSet(collectionSet.Name), innerException);
		}
	}
}
