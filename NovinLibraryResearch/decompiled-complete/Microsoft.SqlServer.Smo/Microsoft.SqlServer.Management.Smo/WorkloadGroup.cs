using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Dmf;
using Microsoft.SqlServer.Management.Facets;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;

namespace Microsoft.SqlServer.Management.Smo;

[StateChangeEvent("CREATE_WORKLOAD_GROUP", "WORKLOADGROUP", "WORKLOAD GROUP")]
[StateChangeEvent("ALTER_WORKLOAD_GROUP", "WORKLOADGROUP", "WORKLOAD GROUP")]
[EvaluationMode(AutomatedPolicyEvaluationMode.Enforce | AutomatedPolicyEvaluationMode.CheckOnChanges | AutomatedPolicyEvaluationMode.CheckOnSchedule)]
[PhysicalFacet]
[SfcElement(SfcElementFlags.Standalone)]
public sealed class WorkloadGroup : ScriptNameObjectBase, ICreatable, IDroppable, IDropIfExists, IAlterable, IScriptable, ISfcSupportsDesignMode
{
	internal class PropertyMetadataProvider : SqlPropertyMetadataProvider
	{
		private static int[] versionCount = new int[10] { 0, 0, 0, 9, 9, 9, 9, 10, 10, 11 };

		private static int[] cloudVersionCount = new int[3];

		private static int sqlDwPropertyCount = 0;

		internal static StaticMetadata[] sqlDwStaticMetadata = new StaticMetadata[0];

		internal static StaticMetadata[] cloudStaticMetadata = new StaticMetadata[0];

		internal static StaticMetadata[] staticMetadata = new StaticMetadata[11]
		{
			new StaticMetadata("GroupMaximumRequests", expensive: false, readOnly: false, typeof(int)),
			new StaticMetadata("ID", expensive: false, readOnly: true, typeof(int)),
			new StaticMetadata("Importance", expensive: false, readOnly: false, typeof(WorkloadGroupImportance)),
			new StaticMetadata("IsSystemObject", expensive: false, readOnly: true, typeof(bool)),
			new StaticMetadata("MaximumDegreeOfParallelism", expensive: false, readOnly: false, typeof(int)),
			new StaticMetadata("PolicyHealthState", expensive: true, readOnly: false, typeof(PolicyHealthState)),
			new StaticMetadata("RequestMaximumCpuTimeInSeconds", expensive: false, readOnly: false, typeof(int)),
			new StaticMetadata("RequestMaximumMemoryGrantPercentage", expensive: false, readOnly: false, typeof(int)),
			new StaticMetadata("RequestMemoryGrantTimeoutInSeconds", expensive: false, readOnly: false, typeof(int)),
			new StaticMetadata("ExternalResourcePoolName", expensive: false, readOnly: false, typeof(string)),
			new StaticMetadata("RequestMaximumMemoryGrantPercentageAsDouble", expensive: false, readOnly: false, typeof(double))
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
				"GroupMaximumRequests" => 0, 
				"ID" => 1, 
				"Importance" => 2, 
				"IsSystemObject" => 3, 
				"MaximumDegreeOfParallelism" => 4, 
				"PolicyHealthState" => 5, 
				"RequestMaximumCpuTimeInSeconds" => 6, 
				"RequestMaximumMemoryGrantPercentage" => 7, 
				"RequestMemoryGrantTimeoutInSeconds" => 8, 
				"ExternalResourcePoolName" => 9, 
				"RequestMaximumMemoryGrantPercentageAsDouble" => 10, 
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

	private const string InternalPoolName = "internal";

	[SfcKey(0)]
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

	public static string UrnSuffix => "WorkloadGroup";

	[SfcObject(SfcObjectRelationship.ParentObject)]
	public ResourcePool Parent
	{
		get
		{
			CheckObjectState();
			return base.ParentColl.ParentInstance as ResourcePool;
		}
		set
		{
			SetParentImpl(value);
		}
	}

	bool ISfcSupportsDesignMode.IsDesignMode => base.IsDesignMode;

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public string ExternalResourcePoolName
	{
		get
		{
			return (string)base.Properties.GetValueWithNullReplacement("ExternalResourcePoolName");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("ExternalResourcePoolName", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public int GroupMaximumRequests
	{
		get
		{
			return (int)base.Properties.GetValueWithNullReplacement("GroupMaximumRequests");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("GroupMaximumRequests", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public int ID => (int)base.Properties.GetValueWithNullReplacement("ID");

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public WorkloadGroupImportance Importance
	{
		get
		{
			return (WorkloadGroupImportance)base.Properties.GetValueWithNullReplacement("Importance");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("Importance", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public bool IsSystemObject => (bool)base.Properties.GetValueWithNullReplacement("IsSystemObject");

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public int MaximumDegreeOfParallelism
	{
		get
		{
			return (int)base.Properties.GetValueWithNullReplacement("MaximumDegreeOfParallelism");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("MaximumDegreeOfParallelism", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public int RequestMaximumCpuTimeInSeconds
	{
		get
		{
			return (int)base.Properties.GetValueWithNullReplacement("RequestMaximumCpuTimeInSeconds");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("RequestMaximumCpuTimeInSeconds", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public int RequestMaximumMemoryGrantPercentage
	{
		get
		{
			return (int)base.Properties.GetValueWithNullReplacement("RequestMaximumMemoryGrantPercentage");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("RequestMaximumMemoryGrantPercentage", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public double RequestMaximumMemoryGrantPercentageAsDouble
	{
		get
		{
			return (double)base.Properties.GetValueWithNullReplacement("RequestMaximumMemoryGrantPercentageAsDouble");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("RequestMaximumMemoryGrantPercentageAsDouble", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public int RequestMemoryGrantTimeoutInSeconds
	{
		get
		{
			return (int)base.Properties.GetValueWithNullReplacement("RequestMemoryGrantTimeoutInSeconds");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("RequestMemoryGrantTimeoutInSeconds", value);
		}
	}

	internal WorkloadGroup(AbstractCollectionBase parentColl, ObjectKeyBase key, SqlSmoState state)
		: base(parentColl, key, state)
	{
		m_comparer = ((ResourcePool)parentColl.ParentInstance).Parent.Parent.StringComparer;
	}

	public void Create()
	{
		CreateImpl();
	}

	public void Alter()
	{
		AlterImpl();
	}

	public void Drop()
	{
		DropImpl();
	}

	public void DropIfExists()
	{
		DropImpl(isDropIfExists: true);
	}

	public StringCollection Script()
	{
		return ScriptImpl();
	}

	public StringCollection Script(ScriptingOptions scriptingOptions)
	{
		return ScriptImpl(scriptingOptions);
	}

	public void MoveToPool(string poolName)
	{
		CheckObjectState(throwIfNotCreated: true);
		MoveToPoolImpl(poolName);
	}

	public StringCollection ScriptMoveToPool(string poolName)
	{
		return new StringCollection { string.Format(SmoApplication.DefaultCulture, "ALTER WORKLOAD GROUP {0} USING {1}", SqlSmoObject.MakeSqlBraket(SqlSmoObject.SqlString(Name)), SqlSmoObject.MakeSqlBraket(SqlSmoObject.SqlString(poolName))) };
	}

	internal override void ScriptCreate(StringCollection queries, ScriptingPreferences sp)
	{
		StringBuilder stringBuilder = new StringBuilder(Globals.INIT_BUFFER_SIZE);
		ScriptIncludeHeaders(stringBuilder, sp, UrnSuffix);
		if (sp.IncludeScripts.ExistenceCheck)
		{
			stringBuilder.AppendFormat(SmoApplication.DefaultCulture, Scripts.INCLUDE_EXISTS_RG_WORKLOADGROUP, "NOT", FormatFullNameForScripting(sp, nameIsIndentifier: false));
			stringBuilder.Append(sp.NewLine);
			stringBuilder.Append(Scripts.BEGIN);
			stringBuilder.Append(sp.NewLine);
		}
		stringBuilder.AppendFormat(SmoApplication.DefaultCulture, "CREATE WORKLOAD GROUP {0}", FormatFullNameForScripting(sp));
		int count = 0;
		GetAllParams(stringBuilder, sp, ref count);
		stringBuilder.AppendFormat(SmoApplication.DefaultCulture, " USING {0}", SqlSmoObject.MakeSqlBraket(Parent.Name));
		if (IsSupportedProperty("ExternalResourcePoolName"))
		{
			int count2 = 0;
			StringBuilder stringBuilder2 = new StringBuilder(Globals.INIT_BUFFER_SIZE);
			GetParameter(stringBuilder2, sp, "ExternalResourcePoolName", "{0}", ref count2);
			if (0 < count2)
			{
				stringBuilder.AppendFormat(SmoApplication.DefaultCulture, ", EXTERNAL {0}", SqlSmoObject.MakeSqlBraket(stringBuilder2.ToString()));
			}
		}
		if (sp.IncludeScripts.ExistenceCheck)
		{
			stringBuilder.Append(sp.NewLine);
			stringBuilder.Append(Scripts.END);
			stringBuilder.Append(sp.NewLine);
		}
		queries.Add(stringBuilder.ToString());
	}

	internal override void ScriptAlter(StringCollection queries, ScriptingPreferences sp)
	{
		StringBuilder stringBuilder = new StringBuilder(Globals.INIT_BUFFER_SIZE);
		ScriptIncludeHeaders(stringBuilder, sp, UrnSuffix);
		stringBuilder.AppendFormat(SmoApplication.DefaultCulture, "ALTER WORKLOAD GROUP {0}", FormatFullNameForScripting(sp));
		int count = 0;
		GetAllParams(stringBuilder, sp, ref count);
		int count2 = 0;
		if (IsSupportedProperty("ExternalResourcePoolName"))
		{
			StringBuilder stringBuilder2 = new StringBuilder(Globals.INIT_BUFFER_SIZE);
			GetParameter(stringBuilder2, sp, "ExternalResourcePoolName", "{0}", ref count2);
			if (0 < count2)
			{
				stringBuilder.AppendFormat(SmoApplication.DefaultCulture, " USING EXTERNAL {0}", SqlSmoObject.MakeSqlBraket(stringBuilder2.ToString()));
			}
		}
		if (0 < count || 0 < count2)
		{
			queries.Add(stringBuilder.ToString());
		}
	}

	internal override void ScriptDrop(StringCollection queries, ScriptingPreferences sp)
	{
		StringBuilder stringBuilder = new StringBuilder(Globals.INIT_BUFFER_SIZE);
		if (sp.IncludeScripts.ExistenceCheck)
		{
			stringBuilder.AppendFormat(SmoApplication.DefaultCulture, Scripts.INCLUDE_EXISTS_RG_WORKLOADGROUP, string.Empty, FormatFullNameForScripting(sp, nameIsIndentifier: false));
			stringBuilder.Append(sp.NewLine);
			stringBuilder.Append(Scripts.BEGIN);
			stringBuilder.Append(sp.NewLine);
		}
		stringBuilder.AppendFormat(SmoApplication.DefaultCulture, "DROP WORKLOAD GROUP {0}", FormatFullNameForScripting(sp));
		if (sp.IncludeScripts.ExistenceCheck)
		{
			stringBuilder.Append(sp.NewLine);
			stringBuilder.Append(Scripts.END);
			stringBuilder.Append(sp.NewLine);
		}
		queries.Add(stringBuilder.ToString());
	}

	protected override void TouchImpl()
	{
		GroupMaximumRequests = GroupMaximumRequests;
		Importance = Importance;
		RequestMaximumCpuTimeInSeconds = RequestMaximumCpuTimeInSeconds;
		if (IsSupportedProperty("RequestMaximumMemoryGrantPercentageAsDouble"))
		{
			RequestMaximumMemoryGrantPercentageAsDouble = RequestMaximumMemoryGrantPercentageAsDouble;
		}
		else
		{
			RequestMaximumMemoryGrantPercentage = RequestMaximumMemoryGrantPercentage;
		}
		RequestMemoryGrantTimeoutInSeconds = RequestMemoryGrantTimeoutInSeconds;
		MaximumDegreeOfParallelism = MaximumDegreeOfParallelism;
	}

	private void InitComparer()
	{
		m_comparer = Parent.Parent.Parent.StringComparer;
	}

	private void GetAllParams(StringBuilder sb, ScriptingPreferences sp, ref int count)
	{
		StringBuilder stringBuilder = new StringBuilder(Globals.INIT_BUFFER_SIZE);
		GetParameter(stringBuilder, sp, "GroupMaximumRequests", "group_max_requests={0}", ref count);
		GetParameter(stringBuilder, sp, "Importance", "importance={0}", ref count);
		GetParameter(stringBuilder, sp, "RequestMaximumCpuTimeInSeconds", "request_max_cpu_time_sec={0}", ref count);
		int num = count;
		if (IsSupportedProperty("RequestMaximumMemoryGrantPercentageAsDouble", sp))
		{
			GetParameter(stringBuilder, sp, "RequestMaximumMemoryGrantPercentageAsDouble", "request_max_memory_grant_percent={0}", ref count);
		}
		if (num == count)
		{
			GetParameter(stringBuilder, sp, "RequestMaximumMemoryGrantPercentage", "request_max_memory_grant_percent={0}", ref count);
		}
		GetParameter(stringBuilder, sp, "RequestMemoryGrantTimeoutInSeconds", "request_memory_grant_timeout_sec={0}", ref count);
		GetParameter(stringBuilder, sp, "MaximumDegreeOfParallelism", "max_dop={0}", ref count);
		if (0 < count)
		{
			sb.AppendFormat(SmoApplication.DefaultCulture, " WITH({0})", stringBuilder.ToString());
		}
	}

	private void MoveToPoolImpl(string poolName)
	{
		try
		{
			string name = Name;
			if (poolName == null)
			{
				throw new SmoException(ExceptionTemplatesImpl.InnerException, new ArgumentNullException("poolName"));
			}
			if (m_comparer == null)
			{
				InitComparer();
			}
			if (m_comparer.Compare(poolName, "internal") == 0)
			{
				string message = string.Format(CultureInfo.InvariantCulture, ExceptionTemplatesImpl.CannotMoveToInternalResourcePool, name);
				throw new SmoException(ExceptionTemplatesImpl.InnerException, new ArgumentException(message, "poolName"));
			}
			ResourcePool resourcePool = Parent.Parent.ResourcePools[poolName];
			if (resourcePool == null)
			{
				string message2 = string.Format(CultureInfo.InvariantCulture, ExceptionTemplatesImpl.ResourcePoolNotExist, poolName);
				throw new SmoException(ExceptionTemplatesImpl.InnerException, new ArgumentException(message2, "poolName"));
			}
			if (m_comparer.Compare(poolName, Parent.Name) == 0)
			{
				string message3 = string.Format(CultureInfo.InvariantCulture, ExceptionTemplatesImpl.CannotMoveToSamePool, poolName);
				throw new SmoException(ExceptionTemplatesImpl.InnerException, new ArgumentException(message3, "poolName"));
			}
			StringCollection queries = ScriptMoveToPool(poolName);
			ExecutionManager.ExecuteNonQuery(queries);
			SetState(SqlSmoState.Pending);
			Parent = resourcePool;
			SetState(SqlSmoState.Existing);
		}
		catch (Exception ex)
		{
			SqlSmoObject.FilterException(ex);
			throw new FailedOperationException(ExceptionTemplatesImpl.MoveToPool, this, ex);
		}
	}

	internal static WorkloadGroup GetWorkloadGroup(Server server, string groupName)
	{
		string path = string.Format(SmoApplication.DefaultCulture, string.Concat(server.Urn, "/ResourceGovernor/ResourcePool/WorkloadGroup[@Name='{0}']"), Urn.EscapeString(groupName));
		IEnumerable enumerable = new SfcObjectQuery(server).ExecuteIterator(new SfcQueryExpression(path), null, null);
		WorkloadGroup workloadGroup = null;
		IEnumerator enumerator = enumerable.GetEnumerator();
		try
		{
			if (enumerator.MoveNext())
			{
				workloadGroup = enumerator.Current as WorkloadGroup;
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
		if (workloadGroup == null)
		{
			throw new SmoException(ExceptionTemplatesImpl.CouldNotFindManagementObject("WorkloadGroup", groupName));
		}
		return workloadGroup;
	}

	public WorkloadGroup()
	{
	}

	public WorkloadGroup(ResourcePool resourcePool, string name)
	{
		ValidateName(name);
		key = new SimpleObjectKey(name);
		Parent = resourcePool;
	}

	internal override SqlPropertyMetadataProvider GetPropertyMetadataProvider()
	{
		return new PropertyMetadataProvider(base.ServerVersion, DatabaseEngineType, DatabaseEngineEdition);
	}
}
