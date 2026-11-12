using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using Microsoft.SqlServer.Diagnostics.STrace;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;

namespace Microsoft.SqlServer.Management.Collector;

public sealed class CollectionSet : SfcInstance, ISfcCreatable, ICreatable, ISfcDroppable, IDroppable, ISfcAlterable, IAlterable, ISfcRenamable, ISfcDiscoverObject
{
	public enum CollectionModes : short
	{
		Cached,
		NonCached
	}

	public enum ExecutionStatus : short
	{
		Running,
		Finished,
		Failed,
		Warning
	}

	public enum RuntimeExecutionMode : short
	{
		Collection,
		Upload
	}

	public class Key : SfcKey
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

		public Key(Key other)
		{
			name = other.name;
		}

		public Key(string name)
		{
			this.name = name;
		}

		internal Key(Dictionary<string, object> filedDict)
		{
			name = (string)filedDict["Name"];
		}

		public override bool Equals(object obj)
		{
			return this == obj;
		}

		public new bool Equals(object obj1, object obj2)
		{
			return obj1 == obj2;
		}

		public override bool Equals(SfcKey key)
		{
			return this == key;
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
			return name.GetHashCode();
		}

		public override string ToString()
		{
			return name;
		}

		private bool IsEqual(Key key)
		{
			return string.CompareOrdinal(name, key.name) == 0;
		}

		public override string GetUrnFragment()
		{
			return string.Format("{0}[@Name='{1}']", "CollectionSet", SfcSecureString.EscapeSquote(name));
		}
	}

	private sealed class ObjectFactory : SfcObjectFactory
	{
		private static readonly ObjectFactory instance;

		public static ObjectFactory Instance => instance;

		static ObjectFactory()
		{
			instance = new ObjectFactory();
		}

		private ObjectFactory()
		{
		}

		protected override SfcInstance CreateImpl()
		{
			return new CollectionSet();
		}
	}

	private static TraceContext tc = TraceContext.GetTraceContext("DC_OM", "Microsoft.SqlServer.Management.Collector.CollectionSet");

	private const int DESCRIPTION_MAX_LENGTH = 4000;

	private readonly short MAXIMUM_LOGGING_LEVEL = 2;

	private readonly short MINIMUM_LOGGING_LEVEL;

	private readonly short DEFAULT_DAYS_UNTIL_EXPIRATION = 730;

	private object syncLock;

	private CollectionItemCollection collectionItems;

	internal const string typeName = "CollectionSet";

	[SfcObject(SfcContainerRelationship.ChildContainer, SfcContainerCardinality.ZeroToAny, typeof(CollectionItem))]
	public CollectionItemCollection CollectionItems
	{
		get
		{
			if (collectionItems == null)
			{
				collectionItems = new CollectionItemCollection(this);
			}
			return collectionItems;
		}
	}

	[SfcProperty(SfcPropertyFlags.Data)]
	public int Id
	{
		get
		{
			object value = base.Properties["Id"].Value;
			if (value == null)
			{
				return 0;
			}
			return (int)value;
		}
		private set
		{
			tc.TraceVerbose("Setting Id to: {0}", value);
			base.Properties["Id"].Value = value;
		}
	}

	[SfcProperty]
	public Guid UId
	{
		get
		{
			object value = base.Properties["UId"].Value;
			if (value == null)
			{
				return Guid.Empty;
			}
			return (Guid)value;
		}
		set
		{
			tc.TraceVerbose("Setting UId to: {0}", value.ToString());
			base.Properties["UId"].Value = value;
		}
	}

	[SfcProperty]
	[SfcKey(0)]
	public string Name
	{
		get
		{
			return (string)base.Properties["Name"].Value;
		}
		set
		{
			if (SfcObjectState.Pending == State)
			{
				CheckNewName(value);
				tc.TraceVerbose("Setting Name to: {0}", value);
				base.Properties["Name"].Value = value;
				return;
			}
			throw new CollectorException(ExceptionTemplatesSR.CannotSetObjectNameExistingObject);
		}
	}

	[SfcProperty(Required = false)]
	public string TargetName
	{
		get
		{
			return base.Properties["TargetName"].Value as string;
		}
		set
		{
			Helpers.CheckPropertyValueLength(value, 128, "TargetName");
			tc.TraceVerbose("Setting TargetName to: {0}", value);
			if (DBNull.Value.Equals(value))
			{
				base.Properties["TargetName"].Value = null;
			}
			else
			{
				base.Properties["TargetName"].Value = value;
			}
		}
	}

	[SfcProperty(Required = false)]
	public string ScheduleName
	{
		get
		{
			return base.Properties["ScheduleName"].Value as string;
		}
		set
		{
			Helpers.CheckPropertyValueLength(value, 128, "ScheduleName");
			tc.TraceVerbose("Setting ScheduleName to: {0}", value);
			if (DBNull.Value.Equals(value))
			{
				base.Properties["ScheduleName"].Value = null;
			}
			else
			{
				base.Properties["ScheduleName"].Value = value;
			}
		}
	}

	[SfcProperty(Required = false)]
	public string ProxyName
	{
		get
		{
			return base.Properties["ProxyName"].Value as string;
		}
		set
		{
			Helpers.CheckPropertyValueLength(value, 128, "ProxyName");
			tc.TraceVerbose("Setting ProxyName to: {0}", value);
			if (DBNull.Value.Equals(value))
			{
				base.Properties["ProxyName"].Value = null;
			}
			else
			{
				base.Properties["ProxyName"].Value = value;
			}
		}
	}

	[SfcProperty]
	public CollectionModes CollectionMode
	{
		get
		{
			object value = base.Properties["CollectionMode"].Value;
			if (value == null)
			{
				return CollectionModes.Cached;
			}
			return (CollectionModes)value;
		}
		set
		{
			short num = (short)value;
			if (num < 0 || num > 1)
			{
				throw new CollectorException(ExceptionTemplatesSR.InvalidCollectionMode(Name));
			}
			tc.TraceVerbose("Setting CollectionMode to: {0}", value);
			base.Properties["CollectionMode"].Value = value;
		}
	}

	[SfcProperty]
	public short LoggingLevel
	{
		get
		{
			object value = base.Properties["LoggingLevel"].Value;
			if (value == null)
			{
				return 0;
			}
			return (short)value;
		}
		set
		{
			short num = value;
			if (num < MINIMUM_LOGGING_LEVEL || num > MAXIMUM_LOGGING_LEVEL)
			{
				throw new CollectorException(ExceptionTemplatesSR.InvalidLoggingLevel(Name));
			}
			tc.TraceVerbose("Setting LoggingLevel to: {0}", value);
			base.Properties["LoggingLevel"].Value = value;
		}
	}

	[SfcProperty(SfcPropertyFlags.Data)]
	public bool IsRunning
	{
		get
		{
			object value = base.Properties["IsRunning"].Value;
			if (value == null)
			{
				return false;
			}
			return Convert.ToBoolean(value);
		}
		private set
		{
			tc.TraceVerbose("Setting IsRunning to: {0}", value);
			base.Properties["IsRunning"].Value = value;
		}
	}

	[SfcProperty(SfcPropertyFlags.Data)]
	public bool IsSystem
	{
		get
		{
			object value = base.Properties["IsSystem"].Value;
			if (value == null)
			{
				return false;
			}
			return Convert.ToBoolean(value);
		}
		private set
		{
			tc.TraceVerbose("Setting IsSystem to: {0}", value);
			base.Properties["IsSystem"].Value = value;
		}
	}

	public bool IsTransactionPerformance
	{
		get
		{
			Guid[] transactionPerformanceCollectors = CollectorConfigStore.TransactionPerformanceCollectors;
			foreach (Guid guid in transactionPerformanceCollectors)
			{
				if (guid.Equals(UId))
				{
					return true;
				}
			}
			return false;
		}
	}

	[SfcProperty(Required = false)]
	public string Description
	{
		get
		{
			return base.Properties["Description"].Value as string;
		}
		set
		{
			Helpers.CheckPropertyValueLength(value, 4000, "Description");
			tc.TraceVerbose("Setting Description to: {0}", value);
			if (DBNull.Value.Equals(value))
			{
				base.Properties["Description"].Value = null;
			}
			else
			{
				base.Properties["Description"].Value = value;
			}
		}
	}

	[SfcProperty(Required = false)]
	public short DaysUntilExpiration
	{
		get
		{
			object value = base.Properties["DaysUntilExpiration"].Value;
			if (value == null)
			{
				return 0;
			}
			return (short)value;
		}
		set
		{
			if (value < 0)
			{
				throw new CollectorException(ExceptionTemplatesSR.InvalidDaysUntilExpiration(Name));
			}
			tc.TraceVerbose("Setting DaysUntilExpiration to: {0}", value);
			base.Properties["DaysUntilExpiration"].Value = value;
		}
	}

	public new SfcObjectState State => base.State;

	[SfcIgnore]
	internal object SyncLock
	{
		get
		{
			if (syncLock == null)
			{
				syncLock = Parent.SyncLock;
			}
			return syncLock;
		}
	}

	public Key IdentityKey => (Key)base.AbstractIdentityKey;

	public new CollectorConfigStore Parent
	{
		get
		{
			return (CollectorConfigStore)base.Parent;
		}
		set
		{
			base.Parent = value;
		}
	}

	public CollectionSet()
	{
		using (tc.GetMethodContext("CollectionSet"))
		{
			IsRunning = false;
			IsSystem = false;
			Id = 0;
		}
	}

	public CollectionSet(CollectorConfigStore store, string name)
	{
		using MethodTraceContext methodTraceContext = tc.GetMethodContext("CollectionSet");
		if (store == null)
		{
			throw new CollectorException(ExceptionTemplatesSR.ConfigStoreCannotBeNull);
		}
		methodTraceContext.TraceParameterIn("store", (store.IdentityKey != null) ? store.IdentityKey.ToString() : "null");
		methodTraceContext.TraceParameterIn("name", name);
		Name = name;
		Parent = store;
		IsRunning = false;
		IsSystem = false;
		Id = 0;
		LoggingLevel = 1;
		DaysUntilExpiration = DEFAULT_DAYS_UNTIL_EXPIRATION;
		syncLock = Parent.SyncLock;
	}

	ISfcScript ISfcCreatable.ScriptCreate()
	{
		using (tc.GetMethodContext("ScriptCreate"))
		{
			return new SfcTSqlScript(GetCreateScript())
			{
				ExecutionMode = SfcTSqlExecutor.ExecutionMode.WithResults
			};
		}
	}

	public void Create()
	{
		using MethodTraceContext methodTraceContext = tc.GetMethodContext("Create");
		methodTraceContext.TraceInformation("Creating new collection set '{0}'", Name);
		if (string.IsNullOrEmpty(Name))
		{
			throw new CollectorException(ExceptionTemplatesSR.CollectionSetHasNoName);
		}
		lock (SyncLock)
		{
			CreateImpl();
		}
	}

	public ISfcScript ScriptAlter()
	{
		using (tc.GetMethodContext("ScriptAlter"))
		{
			return new SfcTSqlScript(GetAlterScript())
			{
				ExecutionMode = SfcTSqlExecutor.ExecutionMode.WithResults
			};
		}
	}

	public void Alter()
	{
		using MethodTraceContext methodTraceContext = tc.GetMethodContext("Alter");
		methodTraceContext.TraceInformation("Altering collection set '{0}'", Name);
		if (string.IsNullOrEmpty(Name))
		{
			throw new CollectorException(ExceptionTemplatesSR.CollectionSetHasNoName);
		}
		lock (SyncLock)
		{
			AlterImpl();
		}
	}

	ISfcScript ISfcDroppable.ScriptDrop()
	{
		using (tc.GetMethodContext("ScriptDrop"))
		{
			return new SfcTSqlScript(GetDropScript())
			{
				ExecutionMode = SfcTSqlExecutor.ExecutionMode.NonQuery
			};
		}
	}

	public void Drop()
	{
		using MethodTraceContext methodTraceContext = tc.GetMethodContext("Drop");
		methodTraceContext.TraceInformation("Dropping collection set '{0}'", Name);
		lock (SyncLock)
		{
			DropImpl();
		}
	}

	public void Rename(string newName)
	{
		using MethodTraceContext methodTraceContext = tc.GetMethodContext("Rename");
		methodTraceContext.TraceParameterIn("newName", newName);
		methodTraceContext.TraceInformation("Renaming collection set '{0}' to '{1}'", Name, newName);
		CheckNewName(newName);
		lock (SyncLock)
		{
			RenameImpl(new Key(newName));
		}
	}

	void ISfcRenamable.Rename(SfcKey newKey)
	{
		using MethodTraceContext methodTraceContext = tc.GetMethodContext("Rename");
		Key key = (Key)newKey;
		if (null == key)
		{
			throw new CollectorException(ExceptionTemplatesSR.NameNotNull);
		}
		methodTraceContext.TraceParameterIn("newKey", key);
		CheckNewName(key.Name);
		lock (SyncLock)
		{
			RenameImpl(newKey);
		}
	}

	ISfcScript ISfcRenamable.ScriptRename(SfcKey newKey)
	{
		using (tc.GetMethodContext("ScriptRename"))
		{
			return new SfcTSqlScript(GetRenameScript(newKey as Key));
		}
	}

	public void Stop()
	{
		using (tc.GetMethodContext("Stop"))
		{
			tc.TraceInformation("Stopping collection set {0}", Name);
			StartStopUpload(GetStopScript(), modifyIsRunning: true, isRunningVal: false, ExceptionTemplatesSR.UnableToStopCollectionSet(Name));
		}
	}

	public void Start()
	{
		using (tc.GetMethodContext("Start"))
		{
			tc.TraceInformation("Starting collection set {0}", Name);
			StartStopUpload(GetStartScript(), modifyIsRunning: true, isRunningVal: true, ExceptionTemplatesSR.UnableToStartCollectionSet(Name));
		}
	}

	public void Upload()
	{
		using (tc.GetMethodContext("Upload"))
		{
			tc.TraceInformation("Uploading collection set {0}", Name);
			StartStopUpload(GetUploadScript(), modifyIsRunning: false, isRunningVal: false, ExceptionTemplatesSR.UnableToUploadCollectionSet(Name));
		}
	}

	public void RunOnce()
	{
		using (tc.GetMethodContext("RunOnce"))
		{
			tc.TraceInformation("RunOnce collection set {0}", Name);
			StartStopUpload(GetRunOnceScript(), modifyIsRunning: false, isRunningVal: false, ExceptionTemplatesSR.UnableToRunCollectionSet(Name));
		}
	}

	public void Cleanup()
	{
		using (tc.GetMethodContext("Cleanup"))
		{
			tc.TraceInformation("Cleanup collection set {0}", Name);
			StartStopUpload(GetCleanupScript(), modifyIsRunning: true, isRunningVal: false, ExceptionTemplatesSR.UnableToCleanupCollectionSet(Name));
		}
	}

	void ISfcDiscoverObject.Discover(ISfcDependencyDiscoveryObjectSink sink)
	{
		using (tc.GetMethodContext("Discover"))
		{
			tc.TraceParameterIn("sink.Action", (sink != null) ? sink.Action.ToString() : "(null)");
			switch (sink.Action)
			{
			case SfcDependencyAction.Create:
				if (collectionItems == null)
				{
					break;
				}
				{
					foreach (CollectionItem collectionItem in collectionItems)
					{
						if (collectionItem.State != SfcObjectState.ToBeDropped)
						{
							sink.Add(SfcDependencyDirection.Inbound, collectionItem, SfcTypeRelation.RequiredChild, discovered: false);
						}
					}
					break;
				}
			case SfcDependencyAction.Drop:
				if (collectionItems != null)
				{
					sink.Add(SfcDependencyDirection.Inbound, collectionItems.GetEnumerator(), SfcTypeRelation.RequiredChild, discovered: false);
				}
				break;
			case SfcDependencyAction.Alter:
				if (collectionItems != null)
				{
					sink.Add(SfcDependencyDirection.Inbound, collectionItems.GetEnumerator(), SfcTypeRelation.RequiredChild, discovered: false);
				}
				break;
			case SfcDependencyAction.Rename:
			case SfcDependencyAction.Move:
				break;
			default:
				tc.TraceError("Action '{0}' not supported", sink.Action.ToString());
				throw new NotSupportedException(ExceptionTemplatesSR.UnsupportedAction);
			}
		}
	}

	protected override void PostCreate(object executionResult)
	{
		using MethodTraceContext methodTraceContext = tc.GetMethodContext("PostCreate");
		if (GetDomain().ConnectionContext.Mode == SfcConnectionContextMode.Offline && executionResult == null)
		{
			return;
		}
		methodTraceContext.Assert(executionResult != null, "executionResult is null");
		DataSet dataSet = (DataSet)executionResult;
		int num = 0;
		DataTable dataTable = dataSet.Tables[num++];
		Id = (int)dataTable.Rows[0][0];
		UId = (Guid)dataTable.Rows[0][1];
		foreach (CollectionItem collectionItem in CollectionItems)
		{
			dataTable = dataSet.Tables[num++];
			collectionItem.SetId((int)dataTable.Rows[0][0]);
		}
	}

	protected override void PostAlter(object executionResult)
	{
		using (tc.GetMethodContext("PostAlter"))
		{
			if (GetDomain().ConnectionContext.Mode == SfcConnectionContextMode.Offline && executionResult == null)
			{
				return;
			}
			DataSet dataSet = executionResult as DataSet;
			int num = 0;
			foreach (CollectionItem collectionItem in CollectionItems)
			{
				if (collectionItem.Id == 0)
				{
					int id = (int)dataSet.Tables[num++].Rows[0][0];
					collectionItem.SetId(id);
				}
			}
		}
	}

	protected override void PostDrop(object executionResult)
	{
		using (tc.GetMethodContext("PostDrop"))
		{
			if (CollectionItems != null)
			{
				CollectionItems.Clear();
			}
		}
	}

	private void StartStopUpload(string script, bool modifyIsRunning, bool isRunningVal, string exptnString)
	{
		using MethodTraceContext methodTraceContext = tc.GetMethodContext("StartStopUpload");
		methodTraceContext.TraceParameterIn("script", script);
		methodTraceContext.TraceParameterIn("modifyIsRunning", modifyIsRunning);
		methodTraceContext.TraceParameterIn("isRunningVal", isRunningVal);
		methodTraceContext.TraceParameterIn("exptnString", exptnString);
		if (SfcObjectState.Existing != State)
		{
			throw new CollectorException(ExceptionTemplatesSR.CollectionSetDoesNotExist(Name));
		}
		SqlStoreConnection sqlStoreConnection = (SqlStoreConnection)((ISfcHasConnection)Parent).GetConnection();
		try
		{
			lock (sqlStoreConnection)
			{
				sqlStoreConnection.ServerConnection.ExecuteNonQuery(script);
			}
			if (modifyIsRunning)
			{
				IsRunning = isRunningVal;
				SfcApplication.Events.OnObjectAltered(this, new SfcObjectAlteredEventArgs(base.Urn, this));
			}
		}
		catch (Exception innerException)
		{
			throw new CollectorException(exptnString, innerException);
		}
	}

	private string GetCreateScript()
	{
		using (tc.GetMethodContext("GetCreateScript"))
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Begin Transaction");
			stringBuilder.AppendLine("Begin Try");
			string setParamName = null;
			stringBuilder.AppendLine(GetCreateSetScript(ref setParamName));
			foreach (CollectionItem collectionItem in CollectionItems)
			{
				stringBuilder.AppendLine(collectionItem.GetCreateScript(setParamName));
			}
			stringBuilder.AppendLine("Commit Transaction;");
			stringBuilder.AppendLine("End Try");
			stringBuilder.AppendLine("Begin Catch");
			stringBuilder.AppendLine("Rollback Transaction;");
			stringBuilder.AppendLine(CollectorConfigStore.RethrowErrorScript());
			stringBuilder.AppendLine("End Catch;");
			return stringBuilder.ToString();
		}
	}

	private string GetCreateSetScript(ref string setParamName)
	{
		using (tc.GetMethodContext("GetCreateSetScript"))
		{
			TsqlProcFormatter tsqlProcFormatter = new TsqlProcFormatter();
			tsqlProcFormatter.Procedure = "[msdb].[dbo].[sp_syscollector_create_collection_set]";
			tsqlProcFormatter.Arguments.Add(new TsqlProcFormatter.SprocArg("name", "Name", required: true, output: false));
			tsqlProcFormatter.Arguments.Add(new TsqlProcFormatter.SprocArg("collection_mode", "CollectionMode", required: true, output: false));
			if (Description != null)
			{
				tsqlProcFormatter.Arguments.Add(new TsqlProcFormatter.SprocArg("description", "Description", required: false, output: false));
			}
			if (TargetName != null)
			{
				tsqlProcFormatter.Arguments.Add(new TsqlProcFormatter.SprocArg("target", "TargetName", required: false, output: false));
			}
			tsqlProcFormatter.Arguments.Add(new TsqlProcFormatter.SprocArg("logging_level", "LoggingLevel", required: false, output: false));
			tsqlProcFormatter.Arguments.Add(new TsqlProcFormatter.SprocArg("days_until_expiration", "DaysUntilExpiration", required: false, output: false));
			if (ProxyName != null)
			{
				tsqlProcFormatter.Arguments.Add(new TsqlProcFormatter.SprocArg("proxy_name", "ProxyName", required: false, output: false));
			}
			if (ScheduleName != null)
			{
				tsqlProcFormatter.Arguments.Add(new TsqlProcFormatter.SprocArg("schedule_name", "ScheduleName", required: false, output: false));
			}
			TsqlProcFormatter.SprocArg sprocArg = new TsqlProcFormatter.SprocArg("collection_set_id", "Id", required: false, output: true);
			tsqlProcFormatter.Arguments.Add(sprocArg);
			tsqlProcFormatter.Arguments.Add(new TsqlProcFormatter.SprocArg("collection_set_uid", "UId", required: false, output: true));
			string result = tsqlProcFormatter.GenerateScript(this, null, null);
			setParamName = sprocArg.outputParamName;
			return result;
		}
	}

	private string GetRenameScript(Key newKey)
	{
		using (tc.GetMethodContext("GetRenameScript"))
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(new TsqlProcFormatter
			{
				Procedure = "[msdb].[dbo].[sp_syscollector_update_collection_set]",
				Arguments = 
				{
					new TsqlProcFormatter.SprocArg("name", "Name", required: true, output: false),
					new TsqlProcFormatter.SprocArg("new_name", required: true)
				}
			}.GenerateScript(this, new List<TsqlProcFormatter.RuntimeArg>
			{
				new TsqlProcFormatter.RuntimeArg(typeof(string), newKey.Name)
			}));
			return stringBuilder.ToString();
		}
	}

	private string GetAlterScript()
	{
		using (tc.GetMethodContext("GetAlterScript"))
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Begin Transaction");
			stringBuilder.AppendLine("Begin Try");
			stringBuilder.AppendLine(GetAlterSetScript());
			foreach (CollectionItem collectionItem in CollectionItems)
			{
				stringBuilder.AppendLine(GetAlterItemScript(collectionItem));
			}
			stringBuilder.AppendLine("Commit Transaction;");
			stringBuilder.AppendLine("End Try");
			stringBuilder.AppendLine("Begin Catch");
			stringBuilder.AppendLine("Rollback Transaction;");
			stringBuilder.AppendLine(CollectorConfigStore.RethrowErrorScript());
			stringBuilder.AppendLine("End Catch;");
			return stringBuilder.ToString();
		}
	}

	private string GetAlterSetScript()
	{
		using MethodTraceContext methodTraceContext = tc.GetMethodContext("GetAlterSetScript");
		TsqlProcFormatter tsqlProcFormatter = new TsqlProcFormatter();
		tsqlProcFormatter.Procedure = "[msdb].[dbo].[sp_syscollector_update_collection_set]";
		tsqlProcFormatter.Arguments.Add(new TsqlProcFormatter.SprocArg("collection_set_id", "Id", required: true, output: false));
		if (base.Properties["Description"].Dirty)
		{
			tsqlProcFormatter.Arguments.Add(new TsqlProcFormatter.SprocArg("description", "Description", required: false, output: false));
		}
		if (base.Properties["TargetName"].Dirty)
		{
			tsqlProcFormatter.Arguments.Add(new TsqlProcFormatter.SprocArg("target", "TargetName", required: false, output: false));
		}
		if (base.Properties["ProxyName"].Dirty)
		{
			tsqlProcFormatter.Arguments.Add(new TsqlProcFormatter.SprocArg("proxy_name", "ProxyName", required: false, output: false));
		}
		if (base.Properties["ScheduleName"].Dirty)
		{
			tsqlProcFormatter.Arguments.Add(new TsqlProcFormatter.SprocArg("schedule_name", "ScheduleName", required: false, output: false));
		}
		if (base.Properties["CollectionMode"].Dirty)
		{
			tsqlProcFormatter.Arguments.Add(new TsqlProcFormatter.SprocArg("collection_mode", "CollectionMode", required: true, output: false));
		}
		if (base.Properties["LoggingLevel"].Dirty)
		{
			tsqlProcFormatter.Arguments.Add(new TsqlProcFormatter.SprocArg("logging_level", "LoggingLevel", required: false, output: false));
		}
		if (base.Properties["DaysUntilExpiration"].Dirty)
		{
			tsqlProcFormatter.Arguments.Add(new TsqlProcFormatter.SprocArg("days_until_expiration", "DaysUntilExpiration", required: false, output: false));
		}
		if (tsqlProcFormatter.Arguments.Count > 1)
		{
			return tsqlProcFormatter.GenerateScript(this, null, null);
		}
		methodTraceContext.TraceVerbose("Nothing to alter in GetAlterSetScript()");
		return string.Empty;
	}

	private string GetAlterItemScript(CollectionItem item)
	{
		using MethodTraceContext methodTraceContext = tc.GetMethodContext("GetAlterItemScript");
		methodTraceContext.TraceParameterIn("item", item.Name);
		return item.State switch
		{
			SfcObjectState.Pending => item.GetCreateScript(null), 
			SfcObjectState.Existing => item.GetAlterScript(), 
			SfcObjectState.ToBeDropped => item.GetDropScript(), 
			_ => null, 
		};
	}

	private string GetDropScript()
	{
		return GetDropStartStopUploadScript("[msdb].[dbo].[sp_syscollector_delete_collection_set]");
	}

	private string GetStartScript()
	{
		return GetDropStartStopUploadScript("[msdb].[dbo].[sp_syscollector_start_collection_set]");
	}

	private string GetStopScript()
	{
		return GetDropStartStopUploadScript("[msdb].[dbo].[sp_syscollector_stop_collection_set]");
	}

	private string GetUploadScript()
	{
		return GetDropStartStopUploadScript("[msdb].[dbo].[sp_syscollector_upload_collection_set]");
	}

	private string GetRunOnceScript()
	{
		return GetDropStartStopUploadScript("[msdb].[dbo].[sp_syscollector_run_collection_set]");
	}

	private string GetCleanupScript()
	{
		if (Parent.IsCleanupProcSupportedOnServer())
		{
			using (MethodTraceContext methodTraceContext = tc.GetMethodContext("GetCleanupScript"))
			{
				string text = "[msdb].[dbo].[sp_syscollector_cleanup_collector]";
				methodTraceContext.TraceParameterIn("procName", text);
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(new TsqlProcFormatter
				{
					Procedure = text,
					Arguments = 
					{
						new TsqlProcFormatter.SprocArg("collection_set_id", "Id", required: true, output: false)
					}
				}.GenerateScript(this));
				return stringBuilder.ToString();
			}
		}
		return CreateRemoveScript();
	}

	private string CreateRemoveScript()
	{
		using (tc.GetMethodContext("CreateCleanupScript"))
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("USE MSDB\r\nGO\r\n\r\nDECLARE @TranCounter INT\r\nSET @TranCounter = @@TRANCOUNT\r\nIF (@TranCounter > 0)\r\n    SAVE TRANSACTION tran_cleanup_collection_set\r\nELSE\r\n    BEGIN TRANSACTION\r\nBEGIN TRY\r\n-- changing isolation level to repeatable to avoid any conflicts that may happen\r\n-- while running this stored procedure and sp_syscollector_start_collection_set concurrently\r\nSET TRANSACTION ISOLATION LEVEL REPEATABLE READ");
			stringBuilder.AppendLine("IF (NOT (ISNULL(IS_MEMBER(N'dc_admin'), 0) = 1) AND NOT (ISNULL(IS_MEMBER(N'db_owner'), 0) = 1))\r\nBEGIN\r\n    REVERT\r\n    RAISERROR(14677, -1, -1, 'dc_admin')\r\nEND");
			stringBuilder.AppendLine("ALTER TABLE dbo.syscollector_collection_sets_internal NOCHECK CONSTRAINT FK_syscollector_collection_sets_collection_sysjobs\r\nALTER TABLE dbo.syscollector_collection_sets_internal NOCHECK CONSTRAINT FK_syscollector_collection_sets_upload_sysjobs");
			stringBuilder.AppendFormat("DECLARE @job_id uniqueidentifier\r\nDECLARE datacollector_jobs_cursor CURSOR LOCAL \r\nFOR\r\n    SELECT collection_job_id AS job_id FROM syscollector_collection_sets\r\n    WHERE collection_job_id IS NOT NULL \r\n    AND collection_set_id = {0}\r\n    UNION\r\n    SELECT upload_job_id AS job_id FROM syscollector_collection_sets\r\n    WHERE upload_job_id IS NOT NULL\r\n    AND collection_set_id = {0}\r\nOPEN datacollector_jobs_cursor\r\nFETCH NEXT FROM datacollector_jobs_cursor INTO @job_id\r\nWHILE (@@fetch_status = 0)\r\nBEGIN\r\n    IF EXISTS ( SELECT COUNT(job_id) FROM sysjobs WHERE job_id = @job_id )\r\n    BEGIN\r\n        DECLARE @job_name sysname\r\n        SELECT @job_name = name from sysjobs WHERE job_id = @job_id\r\n        PRINT 'Removing job '+ @job_name\r\n        EXEC dbo.sp_delete_job @job_id=@job_id, @delete_unused_schedule=0\r\n    END\r\n    FETCH NEXT FROM datacollector_jobs_cursor INTO @job_id\r\nEND\r\nCLOSE datacollector_jobs_cursor\r\nDEALLOCATE datacollector_jobs_cursor", Id);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("ALTER TABLE dbo.syscollector_collection_sets_internal CHECK CONSTRAINT FK_syscollector_collection_sets_collection_sysjobs\r\nALTER TABLE dbo.syscollector_collection_sets_internal CHECK CONSTRAINT FK_syscollector_collection_sets_upload_sysjobs");
			stringBuilder.AppendLine("EXEC('DISABLE TRIGGER syscollector_collection_set_is_running_update_trigger ON syscollector_collection_sets_internal')");
			stringBuilder.AppendFormat("UPDATE syscollector_collection_sets_internal\r\nSET is_running = 0, \r\n    collection_job_id = NULL, \r\n    upload_job_id = NULL\r\nWHERE collection_set_id = {0}", Id);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("EXEC('ENABLE TRIGGER syscollector_collection_set_is_running_update_trigger ON syscollector_collection_sets_internal')");
			stringBuilder.AppendLine("DECLARE @counter INT\r\nSELECT @counter= COUNT(is_running) \r\nFROM syscollector_collection_sets_internal \r\nWHERE is_running = 1\r\nIF (@counter = 0)  \r\nBEGIN\r\n    UPDATE syscollector_config_store_internal\r\n    SET parameter_value = 0\r\n    WHERE parameter_name IN ('CollectorEnabled');\r\n\r\n    UPDATE syscollector_config_store_internal\r\n    SET parameter_value = NULL\r\n    WHERE parameter_name IN ( 'MDWDatabase', 'MDWInstance' )\r\nEND");
			stringBuilder.AppendFormat("DELETE FROM syscollector_execution_log_internal\r\nWHERE collection_set_id = {0}", Id);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("IF (@TranCounter = 0)\r\nBEGIN\r\n    COMMIT TRANSACTION\r\nEND\r\nEND TRY\r\nBEGIN CATCH\r\n    IF (@TranCounter = 0 OR XACT_STATE() = -1)\r\n        ROLLBACK TRANSACTION\r\n    ELSE IF (XACT_STATE() = 1)\r\n        ROLLBACK TRANSACTION tran_cleanup_collection_set\r\n    DECLARE @ErrorMessage   NVARCHAR(4000);\r\n    DECLARE @ErrorSeverity  INT;\r\n    DECLARE @ErrorState     INT;\r\n    DECLARE @ErrorNumber    INT;\r\n    DECLARE @ErrorLine      INT;\r\n    DECLARE @ErrorProcedure NVARCHAR(200);\r\n    SELECT @ErrorLine = ERROR_LINE(),\r\n            @ErrorSeverity = ERROR_SEVERITY(),\r\n            @ErrorState = ERROR_STATE(),\r\n            @ErrorNumber = ERROR_NUMBER(),\r\n            @ErrorMessage = ERROR_MESSAGE(),\r\n            @ErrorProcedure = ISNULL(ERROR_PROCEDURE(), '-');\r\n    RAISERROR (14684, @ErrorSeverity, -1 , @ErrorNumber, @ErrorSeverity, @ErrorState, @ErrorProcedure, @ErrorLine, @ErrorMessage);\r\nEND CATCH");
			return stringBuilder.ToString();
		}
	}

	private string GetDropStartStopUploadScript(string procName)
	{
		using MethodTraceContext methodTraceContext = tc.GetMethodContext("GetDropStartStopUploadScript");
		methodTraceContext.TraceParameterIn("procName", procName);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine(new TsqlProcFormatter
		{
			Procedure = procName,
			Arguments = 
			{
				new TsqlProcFormatter.SprocArg("collection_set_id", "Id", required: true, output: false)
			}
		}.GenerateScript(this));
		return stringBuilder.ToString();
	}

	private void CheckNewName(string newName)
	{
		if (newName == null)
		{
			throw new CollectorException(ExceptionTemplatesSR.NameNotNull);
		}
		Helpers.CheckPropertyValueLength(newName, 128, "Name");
	}

	[Conditional("DEBUG")]
	private void AssertCorrectNumberOfTables(DataSet ds)
	{
		int num = 0;
		foreach (CollectionItem collectionItem in CollectionItems)
		{
			if (collectionItem.Id == 0)
			{
				num++;
			}
		}
	}

	public DataTable EnumCollectionSetExecutionHistory()
	{
		return EnumCollectionSetExecutionHistory(0L);
	}

	public DataTable EnumCollectionSetExecutionHistory(long parentLogId)
	{
		using MethodTraceContext methodTraceContext = tc.GetMethodContext("EnumCollectionSetExecutionHistory");
		string text = $"{Parent.IdentityKey.GetUrnFragment()}/CollectionSet[@Id={Id}]/CollectionSetExecutionHistory[@ParentLogId={parentLogId}]";
		methodTraceContext.TraceVerbose("Running SFC query: {0}", text);
		SfcQueryExpression query = new SfcQueryExpression(text);
		string[] fields = new string[17]
		{
			"LogId", "ParentLogId", "Name", "Status", "RuntimeExecutionMode", "StartTime", "LastIterationTime", "FinishTime", "Duration", "FailureMessage",
			"Operator", "PackageExecutionId", "CollectionSetId", "CollectionSetName", "CollectionItemId", "CollectionItemName", "PackageId"
		};
		OrderBy[] orderByFields = new OrderBy[1]
		{
			new OrderBy("LogId", OrderBy.Direction.Desc)
		};
		return Parent.SfcObjectQuery.ExecuteDataTable(query, fields, orderByFields);
	}

	public DataTable EnumCollectionSetExecutionHistoryDetail(long logId)
	{
		using MethodTraceContext methodTraceContext = tc.GetMethodContext("EnumCollectionSetExecutionHistoryDetail");
		string text = $"{Parent.IdentityKey.GetUrnFragment()}/CollectionSet/CollectionSetExecutionHistoryDetail[@LogId={logId}]";
		methodTraceContext.TraceVerbose("Running SFC query: {0}", text);
		SfcQueryExpression query = new SfcQueryExpression(text);
		string[] fields = new string[7] { "LogId", "Source", "Event", "Message", "StartTime", "FinishTime", "DataCode" };
		OrderBy[] orderByFields = new OrderBy[1]
		{
			new OrderBy("LogId", OrderBy.Direction.Desc)
		};
		return Parent.SfcObjectQuery.ExecuteDataTable(query, fields, orderByFields);
	}

	protected override SfcKey CreateIdentityKey()
	{
		Key result = null;
		if (Name != null)
		{
			result = new Key(Name);
		}
		return result;
	}

	public static SfcObjectFactory GetObjectFactory()
	{
		return ObjectFactory.Instance;
	}

	protected override ISfcCollection GetChildCollection(string elementType)
	{
		if (elementType == "CollectionItem")
		{
			return collectionItems;
		}
		throw new CollectorException(ExceptionTemplatesSR.NoSuchCollection(elementType));
	}

	public DateTime GetLastUploadTime()
	{
		using MethodTraceContext methodTraceContext = tc.GetMethodContext("GetLastUploadTime");
		string text = $"{Parent.IdentityKey.GetUrnFragment()}/CollectionSet[@Id={Id}]/CollectionSetLastUploadTime";
		methodTraceContext.TraceVerbose("Running SFC query: {0}", text);
		SfcQueryExpression query = new SfcQueryExpression(text);
		string[] fields = new string[3] { "Status", "RuntimeExecutionMode", "FinishTime" };
		OrderBy[] orderByFields = new OrderBy[1]
		{
			new OrderBy("FinishTime", OrderBy.Direction.Desc)
		};
		DataTable dataTable = Parent.SfcObjectQuery.ExecuteDataTable(query, fields, orderByFields);
		if (dataTable.Rows.Count == 0)
		{
			return DateTime.MinValue;
		}
		return (DateTime)dataTable.Rows[0][2];
	}
}
