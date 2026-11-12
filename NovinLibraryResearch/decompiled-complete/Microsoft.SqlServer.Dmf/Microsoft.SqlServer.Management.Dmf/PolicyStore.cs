using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Xml;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Diagnostics.STrace;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Facets;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.Dmf;

[TypeConverter(typeof(LocalizableTypeConverter))]
public sealed class PolicyStore : SfcInstance, ISfcAlterable, IAlterable, ISfcDomain, ISfcDomainLite, ISfcHasConnection, ISfcSerializableUpgrade, ISfcValidate
{
	[Flags]
	public enum EnumerationMode
	{
		NonSystemOnly = 1,
		SystemOnly = 2,
		All = 3
	}

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
			if (base.Domain.ConnectionContext.Mode == SfcConnectionContextMode.Offline)
			{
				return "PolicyStore";
			}
			return string.Format("{0}[@Name='{1}']", "PolicyStore", base.Domain.DomainInstanceName);
		}
	}

	private static TraceContext traceContext;

	internal static readonly Version KatmaiVersion;

	internal static readonly Version KilimanjaroVersion;

	private SfcObjectQuery objectQuery;

	private SfcObjectQuery smoObjectQuery;

	private static readonly SfcTsqlProcFormatter scriptAlterAction;

	private static readonly SfcTsqlProcFormatter purgeHealthStateAction;

	private static readonly SfcTsqlProcFormatter markSystemObjectAction;

	private const string DomainName = "DMF";

	private const string PropNameEnabled = "Enabled";

	private const string PropNameHistoryRetentionInDays = "HistoryRetentionInDays";

	private const string PropNameLogOnSuccess = "LogOnSuccess";

	private SfcConnectionContext m_context;

	private SqlStoreConnection m_connection;

	private ConditionCollection m_Conditions;

	private ObjectSetCollection m_ObjectSets;

	private PolicyCollection m_Policies;

	private PolicyCategoryCollection m_PolicyCategories;

	private static FacetInfoCollection m_facets;

	private PolicyCategorySubscriptionCollection m_CategorySubscriptions;

	private const int nameColIndex = 0;

	private const int resultColIndex = 1;

	private const int lastRunDateColIndex = 2;

	private const string policyHealthQuery = "select p.name as [Name], \r\n                            case when exists (select * from msdb.dbo.syspolicy_system_health_state h where h.policy_id = p.policy_id and h.target_query_expression_with_id like '{0}%' ESCAPE '\\' ) then 1 else 0 end as [PolicyHealthState],\r\n                            (select top 1 end_date from msdb.dbo.syspolicy_policy_execution_history h2 where h2.policy_id = p.policy_id ) as [LastExecutionTime]\r\n                        from msdb.dbo.syspolicy_policies p";

	private const string aggregatedStateQuery = "SELECT CASE WHEN EXISTS (SELECT * FROM msdb.dbo.syspolicy_system_health_state s WHERE s.target_query_expression_with_id LIKE @target_path_mask ESCAPE '\\')  THEN 1 ELSE 0 END";

	internal const string typeName = "PolicyStore";

	[SfcIgnore]
	public Key IdentityKey => (Key)base.AbstractIdentityKey;

	SfcConnectionContext ISfcHasConnection.ConnectionContext
	{
		get
		{
			if (m_context == null)
			{
				m_context = new SfcConnectionContext(this);
			}
			return m_context;
		}
	}

	[SfcIgnore]
	public SqlStoreConnection SqlStoreConnection
	{
		get
		{
			return m_connection;
		}
		set
		{
			traceContext.TraceVerbose("Setting SqlStoreConnection to: {0}", value);
			m_connection = value;
			MarkRootAsConnected();
		}
	}

	[SfcObject(SfcContainerRelationship.ChildContainer, SfcContainerCardinality.ZeroToAny, typeof(Condition))]
	public ConditionCollection Conditions
	{
		get
		{
			if (m_Conditions == null)
			{
				m_Conditions = new ConditionCollection(this);
			}
			return m_Conditions;
		}
	}

	[SfcObject(SfcContainerRelationship.ChildContainer, SfcContainerCardinality.ZeroToAny, typeof(ObjectSet))]
	public ObjectSetCollection ObjectSets
	{
		get
		{
			if (m_ObjectSets == null)
			{
				m_ObjectSets = new ObjectSetCollection(this);
			}
			return m_ObjectSets;
		}
	}

	[SfcObject(SfcContainerRelationship.ChildContainer, SfcContainerCardinality.ZeroToAny, typeof(Policy))]
	public PolicyCollection Policies
	{
		get
		{
			if (m_Policies == null)
			{
				m_Policies = new PolicyCollection(this, new ServerComparer((SqlStoreConnection == null) ? null : SqlStoreConnection.ServerConnection, "msdb"));
			}
			return m_Policies;
		}
	}

	[SfcObject(SfcContainerRelationship.ChildContainer, SfcContainerCardinality.ZeroToAny, typeof(PolicyCategory))]
	public PolicyCategoryCollection PolicyCategories
	{
		get
		{
			if (m_PolicyCategories == null)
			{
				m_PolicyCategories = new PolicyCategoryCollection(this, new ServerComparer((SqlStoreConnection == null) ? null : SqlStoreConnection.ServerConnection, "msdb"));
			}
			return m_PolicyCategories;
		}
	}

	[SfcIgnore]
	public static FacetInfoCollection Facets
	{
		get
		{
			if (m_facets == null)
			{
				m_facets = new FacetInfoCollection();
			}
			if (m_facets.Count != FacetRepository.RegisteredFacetsCount)
			{
				m_facets.Clear();
				List<FacetInfo> list = new List<FacetInfo>();
				foreach (object registeredFacet in FacetRepository.RegisteredFacets)
				{
					list.Add(new FacetInfo((Type)registeredFacet));
				}
				list.Sort();
				foreach (FacetInfo item in list)
				{
					m_facets.Add(item);
				}
			}
			return m_facets;
		}
	}

	[SfcObject(SfcContainerRelationship.ChildContainer, SfcContainerCardinality.ZeroToAny, typeof(PolicyCategorySubscription))]
	public PolicyCategorySubscriptionCollection PolicyCategorySubscriptions
	{
		get
		{
			if (m_CategorySubscriptions == null)
			{
				m_CategorySubscriptions = new PolicyCategorySubscriptionCollection(this);
			}
			return m_CategorySubscriptions;
		}
	}

	[SfcIgnore]
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
	internal SfcObjectQuery SmoObjectQuery
	{
		get
		{
			if (smoObjectQuery == null)
			{
				smoObjectQuery = new SfcObjectQuery(new Microsoft.SqlServer.Management.Smo.Server(SqlStoreConnection.ServerConnection));
			}
			return smoObjectQuery;
		}
	}

	[SfcIgnore]
	string ISfcDomainLite.DomainName => "DMF";

	[SfcIgnore]
	string ISfcDomainLite.DomainInstanceName
	{
		get
		{
			if (((ISfcHasConnection)this).ConnectionContext.Mode == SfcConnectionContextMode.Offline)
			{
				return "DMF";
			}
			return m_connection.ServerConnection.TrueName;
		}
	}

	public string Name
	{
		get
		{
			if (SqlStoreConnection == null)
			{
				return null;
			}
			return SqlStoreConnection.ServerInstance;
		}
	}

	[SfcProperty(Data = true)]
	public bool Enabled
	{
		get
		{
			return (bool)base.Properties["Enabled"].Value;
		}
		set
		{
			traceContext.TraceVerbose("Setting Enabled to: {0}", value);
			base.Properties["Enabled"].Value = value;
		}
	}

	[SfcProperty(Data = true)]
	public int HistoryRetentionInDays
	{
		get
		{
			return (int)base.Properties["HistoryRetentionInDays"].Value;
		}
		set
		{
			traceContext.TraceVerbose("Setting HistoryRetentionInDays to: {0}", value);
			base.Properties["HistoryRetentionInDays"].Value = value;
		}
	}

	[SfcProperty(Data = true)]
	public bool LogOnSuccess
	{
		get
		{
			return (bool)base.Properties["LogOnSuccess"].Value;
		}
		set
		{
			traceContext.TraceVerbose("Setting LogOnSuccess to: {0}", value);
			base.Properties["LogOnSuccess"].Value = value;
		}
	}

	static PolicyStore()
	{
		traceContext = TraceContext.GetTraceContext("DMF", "PolicyStore");
		KatmaiVersion = new Version(10, 0);
		KilimanjaroVersion = new Version(10, 50);
		scriptAlterAction = new SfcTsqlProcFormatter();
		purgeHealthStateAction = new SfcTsqlProcFormatter();
		markSystemObjectAction = new SfcTsqlProcFormatter();
		scriptAlterAction.Procedure = "msdb.dbo.sp_syspolicy_configure";
		scriptAlterAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("name", required: true));
		scriptAlterAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("value", required: true));
		purgeHealthStateAction.Procedure = "msdb.dbo.sp_syspolicy_purge_health_state";
		purgeHealthStateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("target_tree_root_with_id", required: false));
		markSystemObjectAction.Procedure = "msdb.dbo.sp_syspolicy_mark_system";
		markSystemObjectAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("type", required: true));
		markSystemObjectAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("object_id", required: true));
		markSystemObjectAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("marker", required: true));
	}

	public PolicyStore()
	{
	}

	public PolicyStore(SfcConnection connection)
	{
		traceContext.TraceMethodEnter("PolicyStore");
		traceContext.DebugAssert(connection is SqlStoreConnection);
		SqlStoreConnection = (SqlStoreConnection)connection;
		MarkRootAsConnected();
		traceContext.TraceMethodExit("PolicyStore");
	}

	public void Alter()
	{
		traceContext.TraceMethodEnter("Alter");
		Validate();
		AlterImpl();
		traceContext.TraceMethodExit("Alter");
	}

	public ISfcScript ScriptAlter()
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("ScriptAlter", TraceEventType.Information);
		SfcTSqlScript sfcTSqlScript = new SfcTSqlScript();
		List<SfcTsqlProcFormatter.RuntimeArg> list = new List<SfcTsqlProcFormatter.RuntimeArg>();
		list.Add(new SfcTsqlProcFormatter.RuntimeArg(Enabled.GetType(), "Enabled"));
		list.Add(new SfcTsqlProcFormatter.RuntimeArg(typeof(int), Enabled ? 1 : 0));
		sfcTSqlScript.AddBatch(scriptAlterAction.GenerateScript(this, list));
		list = new List<SfcTsqlProcFormatter.RuntimeArg>();
		list.Add(new SfcTsqlProcFormatter.RuntimeArg("HistoryRetentionInDays".GetType(), "HistoryRetentionInDays"));
		list.Add(new SfcTsqlProcFormatter.RuntimeArg(typeof(int), HistoryRetentionInDays));
		sfcTSqlScript.AddBatch(scriptAlterAction.GenerateScript(this, list));
		list = new List<SfcTsqlProcFormatter.RuntimeArg>();
		list.Add(new SfcTsqlProcFormatter.RuntimeArg("LogOnSuccess".GetType(), "LogOnSuccess"));
		list.Add(new SfcTsqlProcFormatter.RuntimeArg(typeof(int), LogOnSuccess ? 1 : 0));
		sfcTSqlScript.AddBatch(scriptAlterAction.GenerateScript(this, list));
		methodTraceContext.TraceParameterOut("returnVal", sfcTSqlScript);
		return sfcTSqlScript;
	}

	public override string ToString()
	{
		return string.Format(CultureInfo.InvariantCulture, "{0} (Smo.Server='{1}')", "PolicyStore", (SqlStoreConnection != null && SqlStoreConnection.ServerConnection != null) ? SfcSecureString.EscapeSquote(SqlStoreConnection.ServerConnection.TrueName) : "");
	}

	protected override SfcKey CreateIdentityKey()
	{
		return new Key(this);
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	protected override ISfcCollection GetChildCollection(string elementType)
	{
		return elementType switch
		{
			"Condition" => Conditions, 
			"Policy" => Policies, 
			"PolicyCategorySubscription" => PolicyCategorySubscriptions, 
			"PolicyCategory" => PolicyCategories, 
			"ObjectSet" => ObjectSets, 
			_ => throw traceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.NoSuchCollection(elementType))), 
		};
	}

	ISfcConnection ISfcHasConnection.GetConnection()
	{
		return SqlStoreConnection;
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	void ISfcHasConnection.SetConnection(ISfcConnection connection)
	{
		if (connection is ServerConnection)
		{
			SqlStoreConnection = new SqlStoreConnection(((ServerConnection)connection).SqlConnectionObject);
			return;
		}
		if (connection is SqlStoreConnection)
		{
			SqlStoreConnection = (SqlStoreConnection)connection;
			return;
		}
		throw traceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.UnsupportedConnectionType(connection.GetType().FullName)));
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	ISfcConnection ISfcHasConnection.GetConnection(SfcObjectQueryMode mode)
	{
		switch (mode)
		{
		case SfcObjectQueryMode.SingleActiveQuery:
			return SqlStoreConnection;
		case SfcObjectQueryMode.MultipleActiveQueries:
			if (SqlStoreConnection.ServerConnection.MultipleActiveResultSets)
			{
				return SqlStoreConnection;
			}
			return null;
		default:
			return null;
		}
	}

	public static FacetInfoCollection EnumDomainFacets(params string[] args)
	{
		if (args == null || args.Length == 0)
		{
			throw new ArgumentException(ExceptionTemplatesSR.ArgumentNullOrEmpty("args"));
		}
		FacetInfoCollection facetInfoCollection = new FacetInfoCollection();
		List<string> list = new List<string>(args);
		List<FacetInfo> list2 = new List<FacetInfo>();
		foreach (Type registeredFacet in FacetRepository.RegisteredFacets)
		{
			string registeredDomainForType = SfcRegistration.GetRegisteredDomainForType(registeredFacet.FullName, throwOnUnregisteredDomain: false);
			if (!string.IsNullOrEmpty(registeredDomainForType))
			{
				string namespaceQualifier = SfcRegistration.Domains[registeredDomainForType].NamespaceQualifier;
				if (!list.Contains(namespaceQualifier))
				{
					continue;
				}
			}
			else if (!list.Contains(null))
			{
				continue;
			}
			list2.Add(new FacetInfo(registeredFacet));
		}
		list2.Sort();
		foreach (FacetInfo item in list2)
		{
			facetInfoCollection.Add(item);
		}
		return facetInfoCollection;
	}

	public static FacetInfoCollection EnumRootFacets(Type rootType)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("EnumRootFacets", TraceEventType.Information);
		methodTraceContext.TraceParameters(rootType);
		List<FacetInfo> list = new List<FacetInfo>();
		foreach (Type rootFacet in FacetRepository.GetRootFacets(rootType))
		{
			list.Add(new FacetInfo(rootFacet));
		}
		list.Sort();
		FacetInfoCollection facetInfoCollection = new FacetInfoCollection();
		foreach (FacetInfo item in list)
		{
			facetInfoCollection.Add(item);
		}
		methodTraceContext.TraceParameterOut("returnVal", facetInfoCollection);
		return facetInfoCollection;
	}

	public StringCollection EnumConditionsOnFacet(string facet)
	{
		StringCollection stringCollection = new StringCollection();
		if (string.IsNullOrEmpty(facet) || null == FacetRepository.GetFacetType(facet))
		{
			return stringCollection;
		}
		List<string> list = new List<string>();
		foreach (Condition condition in Conditions)
		{
			if (condition.Facet == facet)
			{
				list.Add(condition.Name);
			}
		}
		list.Sort();
		stringCollection.AddRange(list.ToArray());
		return stringCollection;
	}

	public StringCollection EnumConditionsOnFacet(string facet, EnumerationMode enumerationMode)
	{
		if (enumerationMode == EnumerationMode.All)
		{
			return EnumConditionsOnFacet(facet);
		}
		StringCollection stringCollection = new StringCollection();
		if (string.IsNullOrEmpty(facet) || null == FacetRepository.GetFacetType(facet))
		{
			return stringCollection;
		}
		List<string> list = new List<string>();
		foreach (Condition condition in Conditions)
		{
			if (!(condition.Facet == facet))
			{
				continue;
			}
			switch (enumerationMode)
			{
			case EnumerationMode.NonSystemOnly:
				if (condition.IsSystemObject)
				{
					continue;
				}
				break;
			case EnumerationMode.SystemOnly:
				if (!condition.IsSystemObject)
				{
					continue;
				}
				break;
			}
			list.Add(condition.Name);
		}
		list.Sort();
		stringCollection.AddRange(list.ToArray());
		return stringCollection;
	}

	public StringCollection EnumRootConditions(Type rootType)
	{
		StringCollection stringCollection = new StringCollection();
		List<Type> rootFacets = FacetRepository.GetRootFacets(rootType);
		foreach (Condition condition in Conditions)
		{
			if (rootFacets.Contains(FacetRepository.GetFacetType(condition.Facet)))
			{
				stringCollection.Add(condition.Name);
			}
		}
		return stringCollection;
	}

	public StringCollection EnumPoliciesOnFacet(string facet)
	{
		StringCollection stringCollection = new StringCollection();
		if (string.IsNullOrEmpty(facet) || null == FacetRepository.GetFacetType(facet))
		{
			return stringCollection;
		}
		StringCollection stringCollection2 = EnumConditionsOnFacet(facet);
		if (stringCollection2.Count == 0)
		{
			return stringCollection;
		}
		foreach (Policy policy in Policies)
		{
			if (stringCollection2.Contains(policy.Condition))
			{
				stringCollection.Add(policy.Name);
			}
		}
		return stringCollection;
	}

	public StringCollection EnumPoliciesOnFacet(string facet, EnumerationMode enumerationMode)
	{
		if (enumerationMode == EnumerationMode.All)
		{
			return EnumPoliciesOnFacet(facet);
		}
		StringCollection stringCollection = new StringCollection();
		if (string.IsNullOrEmpty(facet) || null == FacetRepository.GetFacetType(facet))
		{
			return stringCollection;
		}
		StringCollection stringCollection2 = EnumConditionsOnFacet(facet);
		if (stringCollection2.Count == 0)
		{
			return stringCollection;
		}
		foreach (Policy policy in Policies)
		{
			switch (enumerationMode)
			{
			case EnumerationMode.NonSystemOnly:
				if (policy.IsSystemObject)
				{
					continue;
				}
				break;
			case EnumerationMode.SystemOnly:
				if (!policy.IsSystemObject)
				{
					continue;
				}
				break;
			}
			if (stringCollection2.Contains(policy.Condition))
			{
				stringCollection.Add(policy.Name);
			}
		}
		return stringCollection;
	}

	public StringCollection EnumTargetSetConditions(Type type)
	{
		StringCollection stringCollection = new StringCollection();
		if (null == type || !FacetRepository.IsRegisteredFacet(type))
		{
			return stringCollection;
		}
		string name = type.Name;
		foreach (Condition condition in Conditions)
		{
			if (condition.IsEnumerable && string.Equals(name, condition.Facet, StringComparison.Ordinal))
			{
				stringCollection.Add(condition.Name);
			}
		}
		return stringCollection;
	}

	public StringCollection EnumTargetSetConditions(Type type, EnumerationMode enumerationMode)
	{
		if (enumerationMode == EnumerationMode.All)
		{
			return EnumTargetSetConditions(type);
		}
		StringCollection stringCollection = new StringCollection();
		if (null == type || !FacetRepository.IsRegisteredFacet(type))
		{
			return stringCollection;
		}
		string name = type.Name;
		foreach (Condition condition in Conditions)
		{
			switch (enumerationMode)
			{
			case EnumerationMode.NonSystemOnly:
				if (condition.IsSystemObject)
				{
					continue;
				}
				break;
			case EnumerationMode.SystemOnly:
				if (!condition.IsSystemObject)
				{
					continue;
				}
				break;
			}
			if (condition.IsEnumerable && string.Equals(name, condition.Facet, StringComparison.Ordinal))
			{
				stringCollection.Add(condition.Name);
			}
		}
		return stringCollection;
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	Type ISfcDomain.GetType(string typeName)
	{
		return typeName switch
		{
			"PolicyStore" => typeof(PolicyStore), 
			"Policy" => typeof(Policy), 
			"PolicyCategorySubscription" => typeof(PolicyCategorySubscription), 
			"PolicyCategory" => typeof(PolicyCategory), 
			"ObjectSet" => typeof(ObjectSet), 
			"TargetSet" => typeof(TargetSet), 
			"Condition" => typeof(Condition), 
			"TargetSetLevel" => typeof(TargetSetLevel), 
			"EvaluationHistory" => typeof(EvaluationHistory), 
			"ConnectionEvaluationHistory" => typeof(ConnectionEvaluationHistory), 
			"EvaluationDetail" => typeof(EvaluationDetail), 
			_ => null, 
		};
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	SfcKey ISfcDomain.GetKey(IUrnFragment urnFragment)
	{
		return urnFragment.Name switch
		{
			"PolicyStore" => new Key(this), 
			"Policy" => new Policy.Key(urnFragment.FieldDictionary), 
			"PolicyCategorySubscription" => new PolicyCategorySubscription.Key(urnFragment.FieldDictionary), 
			"PolicyCategory" => new PolicyCategory.Key(urnFragment.FieldDictionary), 
			"ObjectSet" => new ObjectSet.Key(urnFragment.FieldDictionary), 
			"TargetSet" => new TargetSet.Key(urnFragment.FieldDictionary), 
			"Condition" => new Condition.Key(urnFragment.FieldDictionary), 
			"TargetSetLevel" => new TargetSetLevel.Key(urnFragment.FieldDictionary), 
			"EvaluationHistory" => new EvaluationHistory.Key(urnFragment.FieldDictionary), 
			"ConnectionEvaluationHistory" => new ConnectionEvaluationHistory.Key(urnFragment.FieldDictionary), 
			"EvaluationDetail" => new EvaluationDetail.Key(urnFragment.FieldDictionary), 
			_ => throw traceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.UnsupportedKey(urnFragment.Name))), 
		};
	}

	ISfcExecutionEngine ISfcDomain.GetExecutionEngine()
	{
		return new SfcTSqlExecutionEngine(SqlStoreConnection.ServerConnection);
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	SfcTypeMetadata ISfcDomain.GetTypeMetadata(string typeName)
	{
		if (typeName == "TargetSetLevel")
		{
			return TargetSetLevel.GetTypeMetadata();
		}
		return null;
	}

	bool ISfcDomain.UseSfcStateManagement()
	{
		return true;
	}

	int ISfcDomainLite.GetLogicalVersion()
	{
		return 3;
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	public override void Discover(ISfcDependencyDiscoveryObjectSink sink)
	{
	}

	public DataTable EnumApplicablePolicies(SfcQueryExpression target)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("EnumApplicablePolicies", TraceEventType.Information);
		methodTraceContext.TraceParameters(target);
		DataTable dataTable = new DataTable();
		dataTable.Locale = CultureInfo.InvariantCulture;
		dataTable.Columns.Add("Name", typeof(string));
		dataTable.Columns.Add("PolicyCategory", typeof(string));
		dataTable.Columns.Add("Effective", typeof(bool));
		dataTable.Columns.Add("AutomatedPolicyEvaluationMode", typeof(AutomatedPolicyEvaluationMode));
		dataTable.Columns.Add("PolicyEffectiveState", typeof(PolicyEffectiveState));
		dataTable.Columns.Add("PolicyHealthState", typeof(PolicyHealthState));
		dataTable.Columns.Add("LastExecutionTime", typeof(DateTime));
		try
		{
			if (target == null)
			{
				throw methodTraceContext.TraceThrow(new ArgumentNullException("target"));
			}
			DataTable policiesHealthStateTable = GetPoliciesHealthStateTable(target);
			Policies.Refresh();
			foreach (DataRow row in policiesHealthStateTable.Rows)
			{
				Policy policy = Policies[row[0].ToString()];
				bool flag = false;
				if (policy == null)
				{
					continue;
				}
				PolicyHealthState policyHealthState = (Convert.ToBoolean(row[1]) ? PolicyHealthState.Critical : PolicyHealthState.Unknown);
				PolicyEffectiveState policyEffectiveState = PolicyEffectiveState.None;
				if (IsPolicyApplicableForGivenTarget(policy, target))
				{
					flag = true;
					if (IsTargetInCategory(policy.PolicyCategory, target))
					{
						policyEffectiveState |= PolicyEffectiveState.InCategory;
					}
					if (policy.IsInTargetSet(policy.Parent.SqlStoreConnection, target, checkSqlScriptAsProxy: false))
					{
						policyEffectiveState |= PolicyEffectiveState.InFilter;
					}
					if (policy.Enabled)
					{
						policyEffectiveState |= PolicyEffectiveState.Enabled;
					}
				}
				bool flag2 = policyEffectiveState == (PolicyEffectiveState.Enabled | PolicyEffectiveState.InFilter | PolicyEffectiveState.InCategory);
				object obj = row[2];
				if (!(obj is DBNull))
				{
					obj = (DateTime)row[2];
				}
				if (flag)
				{
					dataTable.Rows.Add(policy.Name, policy.PolicyCategory, flag2, policy.AutomatedPolicyEvaluationMode, policyEffectiveState, policyHealthState, obj);
				}
			}
		}
		catch (Exception ex)
		{
			methodTraceContext.TraceCatch(ex);
			if (!Utils.ShouldProcessException(ex))
			{
				throw;
			}
			throw traceContext.TraceThrow(new FailedOperationException(ExceptionTemplatesSR.EnumApplicablePolicies, string.Empty, GetType().Name, ex));
		}
		methodTraceContext.TraceParameterOut("returnVal", dataTable);
		return dataTable;
	}

	private DataTable GetPoliciesHealthStateTable(SfcQueryExpression target)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("GetPoliciesHealthStateTable");
		methodTraceContext.TraceParameters(target);
		if (!(Policy.GetOneTarget(SqlStoreConnection, target) is SqlSmoObject sqlSmoObject))
		{
			throw methodTraceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.OnlyOneTarget(target.ToString())));
		}
		string pattern = sqlSmoObject.UrnOnlyId;
		string sqlCommand = $"select p.name as [Name], \r\n                            case when exists (select * from msdb.dbo.syspolicy_system_health_state h where h.policy_id = p.policy_id and h.target_query_expression_with_id like '{SfcTsqlProcFormatter.EscapeString(EscapeLikePattern(pattern), '\'')}%' ESCAPE '\\' ) then 1 else 0 end as [PolicyHealthState],\r\n                            (select top 1 end_date from msdb.dbo.syspolicy_policy_execution_history h2 where h2.policy_id = p.policy_id ) as [LastExecutionTime]\r\n                        from msdb.dbo.syspolicy_policies p";
		DataTable dataTable = SqlStoreConnection.ServerConnection.ExecuteWithResults(sqlCommand).Tables[0];
		methodTraceContext.TraceParameterOut("returnVal", dataTable);
		return dataTable;
	}

	public PolicyHealthState GetAggregatedHealthState(SfcQueryExpression target)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("GetAggregatedHealthState", TraceEventType.Information);
		methodTraceContext.TraceParameters(target);
		if (!Enabled)
		{
			methodTraceContext.TraceParameterOut("returnVal", PolicyHealthState.Unknown);
			return PolicyHealthState.Unknown;
		}
		try
		{
			if (target == null)
			{
				throw methodTraceContext.TraceThrow(new ArgumentNullException("target"));
			}
			if (!target.ToString().StartsWith("Server", StringComparison.Ordinal))
			{
				methodTraceContext.TraceParameterOut("returnVal", PolicyHealthState.Unknown);
				return PolicyHealthState.Unknown;
			}
			RequestObjectInfo requestObjectInfo = new RequestObjectInfo(target.ToString(), RequestObjectInfo.Flags.Properties);
			ObjectInfo objectInfo = new Enumerator().Process(SqlStoreConnection.ServerConnection, requestObjectInfo);
			bool flag = false;
			ObjectProperty[] properties = objectInfo.Properties;
			foreach (ObjectProperty objectProperty in properties)
			{
				flag |= objectProperty.Name == "PolicyHealthState";
			}
			if (!flag)
			{
				methodTraceContext.TraceParameterOut("returnVal", PolicyHealthState.Unknown);
				return PolicyHealthState.Unknown;
			}
			SfcObjectQuery sfcObjectQuery = new SfcObjectQuery(new Microsoft.SqlServer.Management.Smo.Server(SqlStoreConnection.ServerConnection));
			if (target.Expression[target.Expression.Length - 1].Filter == null)
			{
				string text = string.Empty;
				if (target.Expression.Length == 1 && target.Expression[0].Name == "Server")
				{
					text = "Server";
				}
				else
				{
					SfcQueryExpression query = new SfcQueryExpression(new Urn(target.ToString()).Parent.ToString());
					foreach (SqlSmoObject item in sfcObjectQuery.ExecuteIterator(query, null, null))
					{
						if (text.Length > 0)
						{
							return GetAggregatedHealthStateWithFilter(target, sfcObjectQuery);
						}
						string name = target.Expression[target.Expression.Length - 1].Name;
						text = string.Concat(item.UrnOnlyId, "/", name);
						if (name == "User")
						{
							text += "[";
						}
					}
					if (text.Length == 0)
					{
						methodTraceContext.TraceParameterOut("returnVal", PolicyHealthState.Unknown);
						return PolicyHealthState.Unknown;
					}
				}
				SqlCommand obj = new SqlCommand("SELECT CASE WHEN EXISTS (SELECT * FROM msdb.dbo.syspolicy_system_health_state s WHERE s.target_query_expression_with_id LIKE @target_path_mask ESCAPE '\\')  THEN 1 ELSE 0 END")
				{
					Connection = SqlStoreConnection.ServerConnection.SqlConnectionObject
				};
				SqlParameter value = new SqlParameter("@target_path_mask", SfcTsqlProcFormatter.EscapeString(EscapeLikePattern(text), '\'') + "%");
				obj.Parameters.Add(value);
				return (PolicyHealthState)obj.ExecuteScalar();
			}
			return GetAggregatedHealthStateWithFilter(target, sfcObjectQuery);
		}
		catch (Exception ex)
		{
			methodTraceContext.TraceCatch(ex);
			if (!Utils.ShouldProcessException(ex))
			{
				throw;
			}
			throw traceContext.TraceThrow(new FailedOperationException(ExceptionTemplatesSR.GetAggregatedHealthState, string.Empty, GetType().Name, ex));
		}
	}

	private PolicyHealthState GetAggregatedHealthStateWithFilter(SfcQueryExpression target, SfcObjectQuery policyQuery)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("GetAggregatedHealthStateWithFilter");
		methodTraceContext.TraceParameters(target, policyQuery);
		if (target.ToString().Contains("PolicyHealthState"))
		{
			throw new ArgumentException("target", ExceptionTemplatesSR.UnsupportedTqeProp(target.ToString(), "PolicyHealthState"));
		}
		if (target.Expression.Length == 1 && target.Expression[0].Name == "Server")
		{
			DataTable dataTable = policyQuery.ExecuteDataTable(target, new string[1] { "PolicyHealthState" }, null);
			traceContext.DebugAssert(dataTable.Rows.Count == 1);
			traceContext.DebugAssert(dataTable.Columns.Count == 1);
			return (PolicyHealthState)dataTable.Rows[0][0];
		}
		FilterNode filter = target.Expression[target.Expression.Length - 1].Filter;
		FilterNode filterNode = new FilterNodeOperator(FilterNodeOperator.Type.EQ, new FilterNodeAttribute("PolicyHealthState"), new FilterNodeConstant("1", FilterNodeConstant.ObjectType.Number));
		if (filter == null)
		{
			target.Expression[target.Expression.Length - 1].Filter = filterNode;
		}
		else
		{
			target.Expression[target.Expression.Length - 1].Filter = new FilterNodeOperator(FilterNodeOperator.Type.And, filterNode, new FilterNodeGroup(filter));
		}
		target = new SfcQueryExpression(target.Expression.ToString());
		IEnumerator enumerator = policyQuery.ExecuteIterator(target, new string[1] { "PolicyHealthState" }, null).GetEnumerator();
		try
		{
			if (enumerator.MoveNext())
			{
				_ = enumerator.Current;
				methodTraceContext.TraceParameterOut("returnVal", PolicyHealthState.Critical);
				return PolicyHealthState.Critical;
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
		methodTraceContext.TraceParameterOut("returnVal", PolicyHealthState.Unknown);
		return PolicyHealthState.Unknown;
	}

	private string EscapeLikePattern(string pattern)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("EscapeLikePattern");
		methodTraceContext.TraceParameters(pattern);
		StringBuilder stringBuilder = new StringBuilder(pattern.Length * 2);
		foreach (char c in pattern)
		{
			if (c == '[' || c == ']' || c == '?' || c == '\\')
			{
				stringBuilder.Append('\\');
			}
			stringBuilder.Append(c);
		}
		return stringBuilder.ToString();
	}

	public ReadOnlyCollection<PolicyCategoryInformation> EnumPolicyCategories()
	{
		List<PolicyCategoryInformation> list = new List<PolicyCategoryInformation>(PolicyCategories.Count);
		foreach (PolicyCategory policyCategory in PolicyCategories)
		{
			list.Add(new PolicyCategoryInformation(policyCategory));
		}
		list.Sort(PolicyCategoryInformation.CompareByCategoryIDPolicyName);
		return new ReadOnlyCollection<PolicyCategoryInformation>(list);
	}

	public ReadOnlyCollection<PolicyCategoryInformation> EnumApplicablePolicyCategories(SfcQueryExpression target)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("EnumApplicablePolicyCategories", TraceEventType.Information);
		methodTraceContext.TraceParameters(target);
		List<PolicyCategoryInformation> list = new List<PolicyCategoryInformation>();
		string sqlCommand = "select pc.policy_category_id, pc.name as PCName, p.name as PName\r\n                                   from msdb.dbo.syspolicy_policy_categories pc FULL OUTER JOIN msdb.dbo.syspolicy_policies p \r\n                                   on pc.policy_category_id = p.policy_category_id\r\n                                   group by pc.policy_category_id, p.name, pc.name\r\n                                   union  \r\n                                   select null, null, null";
		foreach (DataRow row in SqlStoreConnection.ServerConnection.ExecuteWithResults(sqlCommand).Tables[0].Rows)
		{
			bool targetSubscribes = false;
			string text = row["PCName"] as string;
			Policy policy = null;
			if (row["PName"] != DBNull.Value)
			{
				policy = Policies[row["PName"] as string];
			}
			if (text == null || (target != null && IsTargetInCategory(text, target)))
			{
				targetSubscribes = true;
			}
			list.Add(new PolicyCategoryInformation((text == null) ? null : PolicyCategories[text], policy, targetSubscribes));
		}
		list.Sort(PolicyCategoryInformation.CompareByCategoryIDPolicyName);
		return new ReadOnlyCollection<PolicyCategoryInformation>(list);
	}

	internal bool IsTargetInCategory(string policyCategory, SfcQueryExpression target)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("IsTargetInCategory");
		methodTraceContext.TraceParameters(policyCategory, target);
		if (string.IsNullOrEmpty(policyCategory))
		{
			methodTraceContext.TraceParameterOut("returnVal", true);
			return true;
		}
		if (target == null)
		{
			throw methodTraceContext.TraceThrow(new ArgumentNullException("targetFilter"));
		}
		PolicyCategory policyCategory2 = PolicyCategories[policyCategory];
		if (policyCategory2 == null)
		{
			throw methodTraceContext.TraceThrow(new MissingObjectException(ExceptionTemplatesSR.Category, policyCategory));
		}
		if (policyCategory2.MandateDatabaseSubscriptions)
		{
			methodTraceContext.TraceParameterOut("returnVal", true);
			return true;
		}
		if (!target.ExpressionSkeleton.StartsWith(TargetSet.DatabaseLevel, StringComparison.Ordinal))
		{
			methodTraceContext.TraceParameterOut("returnVal", true);
			return true;
		}
		if (target.Expression[1].Filter == null)
		{
			methodTraceContext.TraceParameterOut("returnVal", true);
			return true;
		}
		ExpressionNode expressionNode = ExpressionNode.ConvertFromFilterNode(target.Expression[1].Filter);
		foreach (PolicyCategorySubscription policyCategorySubscription in PolicyCategorySubscriptions)
		{
			if (policyCategorySubscription.PolicyCategory == policyCategory && policyCategorySubscription.TargetType == "DATABASE" && policyCategorySubscription.Target == expressionNode.ObjectName)
			{
				methodTraceContext.TraceParameterOut("returnVal", true);
				return true;
			}
		}
		methodTraceContext.TraceParameterOut("returnVal", false);
		return false;
	}

	public PolicyCategorySubscription SubscribeToPolicyCategory(SfcQueryExpression target, string policyCategory)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("SubscribeToPolicyCategory", TraceEventType.Information);
		methodTraceContext.TraceParameters(target, policyCategory);
		if (target == null)
		{
			throw methodTraceContext.TraceThrow(new ArgumentNullException("target"));
		}
		string[] fields = new string[1] { "Urn" };
		if (SmoObjectQuery.ExecuteDataTable(target, fields, null).Rows.Count == 0)
		{
			throw methodTraceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.OnlyOneTarget(target.ToString())));
		}
		PolicyCategorySubscription policyCategorySubscription = new PolicyCategorySubscription(this, target, policyCategory);
		policyCategorySubscription.Create();
		methodTraceContext.TraceParameterOut("returnVal", policyCategorySubscription);
		return policyCategorySubscription;
	}

	public void UnsubscribeFromPolicyCategory(SfcQueryExpression target, string policyCategory)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("UnsubscribeFromPolicyCategory", TraceEventType.Information);
		methodTraceContext.TraceParameters(target, policyCategory);
		if (target == null)
		{
			throw methodTraceContext.TraceThrow(new ArgumentNullException("target"));
		}
		PolicyCategorySubscription policyCategorySubscription = new PolicyCategorySubscription(this, target, policyCategory);
		foreach (PolicyCategorySubscription policyCategorySubscription2 in PolicyCategorySubscriptions)
		{
			if (policyCategorySubscription.TargetType == policyCategorySubscription2.TargetType && policyCategorySubscription.Target == policyCategorySubscription2.Target && policyCategorySubscription.PolicyCategory == policyCategorySubscription2.PolicyCategory)
			{
				policyCategorySubscription2.Drop();
				return;
			}
		}
		throw methodTraceContext.TraceThrow(new DmfException(string.Format(ExceptionTemplatesSR.PolicyCategoryNotSubscribed, target, policyCategory)));
	}

	private object GetTarget(SfcQueryExpression target)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("GetTarget");
		methodTraceContext.TraceParameters(target);
		IEnumerable enumerable = null;
		string text = target.ToString();
		string[] fields = null;
		enumerable = ((!text.StartsWith("Server", StringComparison.Ordinal) || (text.Length != 6 && text[6] != '[' && text[6] != '/')) ? SfcObjectQuery.ExecuteIterator(target, fields, null) : SmoObjectQuery.ExecuteIterator(target, fields, null));
		object obj = null;
		foreach (object item in enumerable)
		{
			if (obj != null)
			{
				throw methodTraceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.OnlyOneTarget(target.ToString())));
			}
			obj = item;
		}
		if (obj == null)
		{
			throw methodTraceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.OnlyOneTarget(target.ToString())));
		}
		methodTraceContext.TraceParameterOut("returnVal", obj);
		return obj;
	}

	private PolicyStore CreateVirtualStore()
	{
		return new PolicyStore();
	}

	internal Policy GeneratePolicyFromFacet(object target, string facetName, string policyName, string conditionName, List<ObjectToValidate> objectsToValidate)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("GeneratePolicyFromFacet");
		methodTraceContext.TraceParameters(target, facetName, policyName, conditionName, objectsToValidate);
		Type facetType = FacetRepository.GetFacetType(facetName);
		if (null == facetType)
		{
			throw methodTraceContext.TraceThrow(new NullFacetException(facetName));
		}
		if (objectsToValidate != null)
		{
			if (Policies.Contains(policyName))
			{
				throw methodTraceContext.TraceThrow(new ObjectAlreadyExistsException(ExceptionTemplatesSR.Policy, policyName));
			}
			if (Conditions.Contains(conditionName))
			{
				throw methodTraceContext.TraceThrow(new ObjectAlreadyExistsException(ExceptionTemplatesSR.Condition, conditionName));
			}
		}
		FacetEvaluationContext facetEvaluationContext = FacetEvaluationContext.GetFacetEvaluationContext(facetType, target);
		Policy policy = new Policy(this, policyName);
		Condition condition = new Condition(this, conditionName);
		ObjectSet objectSet = null;
		condition.Facet = facetName;
		if (target is SqlSmoObject || target is SfcInstance)
		{
			objectSet = new ObjectSet(this, ObjectSet.GenerateUniqueName(policy));
			objectSet.Facet = facetName;
			policy.ObjectSet = objectSet.Name;
		}
		policy.Condition = condition.Name;
		ExpressionNode expressionNode = null;
		PropertyInfo[] facetProperties = FacetRepository.GetFacetProperties(facetType);
		PropertyInfoNameComparer comparer = new PropertyInfoNameComparer();
		Array.Sort(facetProperties, comparer);
		PropertyInfo[] array = facetProperties;
		foreach (PropertyInfo propertyInfo in array)
		{
			string name = propertyInfo.Name;
			try
			{
				object obj = facetEvaluationContext.GetPropertyValue(name);
				if (obj == null)
				{
					continue;
				}
				try
				{
					if (obj.GetType() != propertyInfo.PropertyType)
					{
						obj = ((!propertyInfo.PropertyType.IsEnum) ? Convert.ChangeType(obj, propertyInfo.PropertyType, CultureInfo.InvariantCulture) : Enum.Parse(propertyInfo.PropertyType, obj.ToString()));
					}
				}
				catch (InvalidCastException ex)
				{
					methodTraceContext.TraceCatch(ex);
					traceContext.DebugAssert(test: true, ex.Message);
					obj = null;
				}
				if (obj != null)
				{
					ExpressionNode expressionNode2 = new ExpressionNodeOperator(OperatorType.EQ, new ExpressionNodeAttribute(name, facetType), ExpressionNode.ConstructNode(obj));
					expressionNode = ((expressionNode != null) ? new ExpressionNodeOperator(OperatorType.AND, expressionNode, expressionNode2) : expressionNode2);
				}
			}
			catch (NonRetrievablePropertyException)
			{
				methodTraceContext.TraceError("Caught a general Exception of type NonRetrievablePropertyException");
			}
			catch (MissingPropertyException)
			{
				methodTraceContext.TraceError("Caught a general Exception of type MissingPropertyException");
			}
		}
		condition.ExpressionNode = expressionNode;
		Conditions.Add(condition);
		objectsToValidate?.Add(new ObjectToValidate(condition, typeof(Condition), ValidationMethod.Create));
		if (objectSet != null)
		{
			ObjectSets.Add(objectSet);
			objectsToValidate?.Add(new ObjectToValidate(objectSet, typeof(ObjectSet), ValidationMethod.Create));
		}
		Policies.Add(policy);
		objectsToValidate?.Add(new ObjectToValidate(policy, typeof(Policy), ValidationMethod.Create));
		SfcQueryExpression sfcQueryExpression = null;
		if (target is SqlSmoObject)
		{
			sfcQueryExpression = new SfcQueryExpression((target as SqlSmoObject).Urn);
		}
		else if (target is SfcInstance)
		{
			sfcQueryExpression = new SfcQueryExpression((target as SfcInstance).Urn);
		}
		if (sfcQueryExpression != null && objectSet != null)
		{
			if (!objectSet.TargetSets.Contains(sfcQueryExpression.ExpressionSkeleton))
			{
				throw traceContext.TraceThrow(new ConflictingPropertyValuesException(ValidationMethod.Create, ExceptionTemplatesSR.ManagementFacet, facetName, ExceptionTemplatesSR.TargetSet, sfcQueryExpression.ExpressionSkeleton));
			}
			objectSet.TargetSets[sfcQueryExpression.ExpressionSkeleton].Enabled = true;
		}
		((ISfcCollection)Conditions).Initialized = true;
		((ISfcCollection)ObjectSets).Initialized = true;
		((ISfcCollection)Policies).Initialized = true;
		methodTraceContext.TraceParameterOut("returnVal", policy);
		return policy;
	}

	private void DoCreatePolicyFromFacet(object target, string facetName, string policyName, string conditionName, XmlWriter writer, bool commit)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("DoCreatePolicyFromFacet", TraceEventType.Information);
		PolicyStore policyStore = (commit ? this : CreateVirtualStore());
		List<ObjectToValidate> objectsToValidate = new List<ObjectToValidate>();
		try
		{
			Policy policy = policyStore.GeneratePolicyFromFacet(target, facetName, policyName, conditionName, objectsToValidate);
			ObjectToValidate.ValidateObjects(objectsToValidate);
			if (commit)
			{
				ObjectToValidate.CommitObjects(this, objectsToValidate);
				return;
			}
			methodTraceContext.Assert(writer != null, "Create policy from facet requested without a valid xml writer.");
			policy.Serialize(writer);
		}
		catch (Exception ex)
		{
			methodTraceContext.TraceCatch(ex);
			if (!Utils.ShouldProcessException(ex))
			{
				throw;
			}
			if (commit)
			{
				ObjectToValidate.RestoreCollections(this, objectsToValidate);
			}
			throw;
		}
	}

	public void CreatePolicyFromFacet(SfcQueryExpression target, string facetName, string policyName, string conditionName)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("CreatePolicyFromFacet", TraceEventType.Information);
		methodTraceContext.TraceParameters(target, facetName, policyName, conditionName);
		if (target == null)
		{
			throw methodTraceContext.TraceThrow(new ArgumentNullException("target"));
		}
		if (string.IsNullOrEmpty(facetName))
		{
			throw methodTraceContext.TraceThrow(new ArgumentException(ExceptionTemplatesSR.ArgumentNullOrEmpty("facetName")));
		}
		if (string.IsNullOrEmpty(policyName))
		{
			throw methodTraceContext.TraceThrow(new ArgumentException(ExceptionTemplatesSR.ArgumentNullOrEmpty("policyName")));
		}
		if (string.IsNullOrEmpty(conditionName))
		{
			throw methodTraceContext.TraceThrow(new ArgumentException(ExceptionTemplatesSR.ArgumentNullOrEmpty("conditionName")));
		}
		object target2 = GetTarget(target);
		DoCreatePolicyFromFacet(target2, facetName, policyName, conditionName, null, commit: true);
	}

	public void CreatePolicyFromFacet(SfcQueryExpression target, string facetName, string policyName, string conditionName, XmlWriter writer)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("CreatePolicyFromFacet", TraceEventType.Information);
		if (target == null)
		{
			throw methodTraceContext.TraceThrow(new ArgumentNullException("target"));
		}
		object target2 = GetTarget(target);
		CreatePolicyFromFacet(target2, facetName, policyName, conditionName, writer);
	}

	public void CreatePolicyFromFacet(object target, string facetName, string policyName, string conditionName, XmlWriter writer)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("CreatePolicyFromFacet", TraceEventType.Information);
		methodTraceContext.TraceParameters(target, facetName, policyName, conditionName, writer);
		if (target == null)
		{
			throw methodTraceContext.TraceThrow(new ArgumentNullException("target"));
		}
		if (string.IsNullOrEmpty(facetName))
		{
			throw methodTraceContext.TraceThrow(new ArgumentException(ExceptionTemplatesSR.ArgumentNullOrEmpty("facetName")));
		}
		if (string.IsNullOrEmpty(policyName))
		{
			throw methodTraceContext.TraceThrow(new ArgumentException(ExceptionTemplatesSR.ArgumentNullOrEmpty("policyName")));
		}
		if (string.IsNullOrEmpty(conditionName))
		{
			throw methodTraceContext.TraceThrow(new ArgumentException(ExceptionTemplatesSR.ArgumentNullOrEmpty("conditionName")));
		}
		if (writer == null)
		{
			throw methodTraceContext.TraceThrow(new ArgumentNullException("writer"));
		}
		DoCreatePolicyFromFacet(target, facetName, policyName, conditionName, writer, commit: false);
	}

	public Policy ImportPolicy(XmlReader xmlReader, ImportPolicyEnabledState importEnabledState, bool overwriteExistingPolicy, bool overwriteExistingCondition)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("ImportPolicy", TraceEventType.Information);
		methodTraceContext.TraceParameters(importEnabledState, overwriteExistingPolicy, overwriteExistingCondition);
		Policy policy = DeserializePolicy(xmlReader, overwriteExistingPolicy, overwriteExistingCondition, import: true, importEnabledState);
		methodTraceContext.TraceParameterOut("returnVal", policy);
		return policy;
	}

	public Policy DeserializePolicy(XmlReader xmlReader, bool overwriteExistingPolicy, bool overwriteExistingCondition)
	{
		return DeserializePolicy(xmlReader, overwriteExistingPolicy, overwriteExistingCondition, import: false, ImportPolicyEnabledState.Unchanged);
	}

	internal Policy DeserializePolicy(XmlReader xmlReader, bool overwriteExistingPolicy, bool overwriteExistingCondition, bool import, ImportPolicyEnabledState importEnabledState)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("DeserializePolicy");
		methodTraceContext.TraceParameters(xmlReader, overwriteExistingPolicy, overwriteExistingCondition, import, importEnabledState);
		if (xmlReader == null)
		{
			throw methodTraceContext.TraceThrow(new ArgumentNullException("xmlReader"));
		}
		List<ObjectToValidate> objectsToValidate = new List<ObjectToValidate>();
		try
		{
			Policy policy = InstantiateObjectsFromReader(xmlReader, overwriteExistingPolicy, overwriteExistingCondition, objectsToValidate);
			if (policy == null)
			{
				throw methodTraceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.PolicyImportFileDoesNotHaveAnyPolicy));
			}
			if (import)
			{
				switch (importEnabledState)
				{
				case ImportPolicyEnabledState.Enable:
					policy.Enabled = true;
					break;
				case ImportPolicyEnabledState.Disable:
					policy.Enabled = false;
					break;
				}
				if (policy.AutomatedPolicyEvaluationMode == AutomatedPolicyEvaluationMode.CheckOnSchedule)
				{
					PolicyScheduleHelper.FixPolicySchedule(policy);
				}
				else
				{
					policy.Schedule = null;
					policy.ScheduleUid = Guid.Empty;
				}
			}
			ObjectToValidate.ValidateObjects(objectsToValidate);
			if (import)
			{
				ObjectToValidate.CommitObjects(this, objectsToValidate);
			}
			methodTraceContext.TraceParameterOut("returnVal", policy);
			return policy;
		}
		catch (Exception ex)
		{
			methodTraceContext.TraceCatch(ex);
			if (!Utils.ShouldProcessException(ex))
			{
				throw;
			}
			ObjectToValidate.RestoreCollections(this, objectsToValidate);
			if (ex is SfcSerializationException)
			{
				throw methodTraceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.SinglePolicyDeserializationFailed, ex));
			}
			throw;
		}
	}

	internal Policy InstantiateObjectsFromReader(XmlReader xmlReader, bool overwriteExistingPolicy, bool overwriteExistingCondition, List<ObjectToValidate> objectsToValidate)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("InstantiateObjectsFromReader");
		methodTraceContext.TraceParameters(xmlReader, overwriteExistingPolicy, overwriteExistingCondition, objectsToValidate);
		ReadOnlyCollection<string> readOnlyCollection = null;
		Policy policy = null;
		SfcSerializer sfcSerializer = new SfcSerializer();
		object obj = null;
		obj = ((((ISfcHasConnection)this).ConnectionContext.Mode != 0) ? sfcSerializer.Deserialize(xmlReader) : sfcSerializer.Deserialize(xmlReader, SfcObjectState.Existing));
		List<object> unParentedReferences = sfcSerializer.UnParentedReferences;
		Policy policy2 = null;
		ObjectSet objectSet = null;
		string value = null;
		if (obj is Policy)
		{
			Policy policy3 = (Policy)obj;
			if (policy != null)
			{
				throw traceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.SinglePolicyExpected((policy != null && policy.Name != null) ? policy.Name : ExceptionTemplatesSR.UnknownPolicy, (policy3 != null && policy3.Name != null) ? policy3.Name : ExceptionTemplatesSR.UnknownPolicy)));
			}
			policy = policy3;
			policy.Parent = this;
			if (Policies.Contains(policy.IdentityKey))
			{
				if (!overwriteExistingPolicy)
				{
					throw methodTraceContext.TraceThrow(new ObjectAlreadyExistsException(ExceptionTemplatesSR.Policy, policy.Name));
				}
				policy2 = Policies[policy.IdentityKey];
				value = policy2.ObjectSet;
				Utils.ReplaceSfcProperties(policy2, policy);
				policy = policy2;
				objectsToValidate.Add(new ObjectToValidate(policy2, typeof(Policy), ValidationMethod.Alter));
			}
			else
			{
				Policies.Add(policy);
				objectsToValidate.Add(new ObjectToValidate(policy, typeof(Policy), ValidationMethod.Create));
			}
		}
		foreach (SfcInstance item in (IEnumerable<object>)unParentedReferences)
		{
			if (item is ObjectSet)
			{
				ObjectSet objectSet2 = (ObjectSet)item;
				if (string.Compare(objectSet2.Name, policy.ObjectSet, StringComparison.Ordinal) != 0)
				{
					throw methodTraceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.ReferencedObjectExpected(ExceptionTemplatesSR.ObjectSet, objectSet2.Name)));
				}
				objectSet2.Parent = this;
				objectSet = ObjectSets[objectSet2.IdentityKey];
				if (objectSet != null)
				{
					if (!overwriteExistingPolicy)
					{
						throw methodTraceContext.TraceThrow(new ObjectAlreadyExistsException(ExceptionTemplatesSR.ObjectSet, objectSet.Name));
					}
					if (string.IsNullOrEmpty(value))
					{
						throw methodTraceContext.TraceThrow(new ObjectAlreadyExistsException(ExceptionTemplatesSR.ObjectSet, objectSet.Name));
					}
					ObjectSet obj2 = objectSet;
					objectsToValidate.Add(new ObjectToValidate(obj2, typeof(ObjectSet), ObjectToValidate.Drop));
					objectSet = objectSet2;
					objectsToValidate.Add(new ObjectToValidate(objectSet2, typeof(ObjectSet), ValidationMethod.Create));
					readOnlyCollection = objectSet.EnumReferencedConditionNames();
				}
				else
				{
					ObjectSets.Add(objectSet2);
					objectsToValidate.Add(new ObjectToValidate(objectSet2, typeof(ObjectSet), ValidationMethod.Create));
					readOnlyCollection = objectSet2.EnumReferencedConditionNames();
				}
			}
			if (item is Condition)
			{
				Condition condition = (Condition)item;
				if (condition.Name != policy.Condition && condition.Name != policy.RootCondition && readOnlyCollection != null && !readOnlyCollection.Contains(condition.Name))
				{
					throw methodTraceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.ReferencedObjectExpected(ExceptionTemplatesSR.Condition, condition.Name)));
				}
				Condition condition2 = Conditions[condition.IdentityKey];
				if (condition2 != null)
				{
					if ((condition2.Facet != condition.Facet || !condition2.ExpressionNode.Equals(condition.ExpressionNode)) && !overwriteExistingCondition)
					{
						throw methodTraceContext.TraceThrow(new ObjectAlreadyExistsException(ExceptionTemplatesSR.Condition, condition.Name));
					}
					if (overwriteExistingCondition)
					{
						Utils.ReplaceSfcProperties(condition2, condition);
					}
					condition = condition2;
					objectsToValidate.Add(new ObjectToValidate(condition2, typeof(Condition), ValidationMethod.Alter));
				}
				else
				{
					condition.Parent = this;
					Conditions.Add(condition);
					objectsToValidate.Add(new ObjectToValidate(condition, typeof(Condition), ValidationMethod.Create));
				}
			}
			else if (item is PolicyCategory)
			{
				PolicyCategory policyCategory = (PolicyCategory)item;
				policyCategory.Parent = this;
				if (!PolicyCategories.Contains(policyCategory.Name))
				{
					policyCategory.MandateDatabaseSubscriptions = true;
					PolicyCategories.Add(policyCategory);
					objectsToValidate.Add(new ObjectToValidate(policyCategory, typeof(PolicyCategory), ValidationMethod.Create));
				}
			}
		}
		((ISfcCollection)Conditions).Initialized = true;
		((ISfcCollection)ObjectSets).Initialized = true;
		((ISfcCollection)Policies).Initialized = true;
		methodTraceContext.TraceParameterOut("returnVal", policy);
		return policy;
	}

	private bool IsPolicyApplicableForGivenTarget(Policy policy, SfcQueryExpression target)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("IsPolicyApplicableForGivenTarget");
		methodTraceContext.TraceParameters(policy, target);
		string strA = ((!target.ToString().StartsWith("Server", StringComparison.Ordinal)) ? target.GetLeafTypeName() : Utils.GetTypeFromUrnSkeleton(new Urn(target.ToString())).Name);
		foreach (Type targetType in new FacetInfo(Conditions[policy.Condition].Facet).TargetTypes)
		{
			if (string.Compare(strA, targetType.Name, StringComparison.OrdinalIgnoreCase) == 0)
			{
				methodTraceContext.TraceParameterOut("returnVal", true);
				return true;
			}
		}
		methodTraceContext.TraceParameterOut("returnVal", false);
		return false;
	}

	public void RepairPolicyAutomation()
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("RepairPolicyAutomation", TraceEventType.Information);
		try
		{
			SqlStoreConnection.ServerConnection.ExecuteNonQuery("EXEC msdb.dbo.sp_syspolicy_repair_policy_automation");
		}
		catch (Exception ex)
		{
			methodTraceContext.TraceCatch(ex);
			SqlSmoObject.FilterException(ex);
			throw methodTraceContext.TraceThrow(new FailedOperationException(ExceptionTemplatesSR.ConsistencyRepair, Name, GetType().Name, ex));
		}
	}

	public void EraseSystemHealthPhantomRecords()
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("EraseSystemHealthPhantomRecords", TraceEventType.Information);
		int columnIndex = 0;
		int columnIndex2 = 1;
		try
		{
			List<long> list = new List<long>();
			DataSet dataSet = SqlStoreConnection.ServerConnection.ExecuteWithResults("SELECT health_state_id, target_query_expression_with_id FROM msdb.dbo.syspolicy_system_health_state");
			if (dataSet.Tables.Count == 0)
			{
				return;
			}
			SfcObjectQuery sfcObjectQuery = new SfcObjectQuery(new Microsoft.SqlServer.Management.Smo.Server(SqlStoreConnection.ServerConnection));
			foreach (DataRow row in dataSet.Tables[0].Rows)
			{
				if (row[columnIndex2] != null)
				{
					DataTable dataTable = sfcObjectQuery.ExecuteDataTable(new SfcQueryExpression((string)row[columnIndex2]), new string[1] { "Urn" }, null);
					if (dataTable == null || dataTable.Rows.Count == 0)
					{
						list.Add((long)row[columnIndex]);
					}
				}
			}
			using SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = SqlStoreConnection.ServerConnection.SqlConnectionObject;
			sqlCommand.CommandText = "DELETE FROM msdb.dbo.syspolicy_system_health_state WHERE health_state_id = @health_state_id_param;";
			SqlParameter sqlParameter = new SqlParameter("@health_state_id_param", SqlDbType.BigInt);
			sqlParameter.SqlDbType = SqlDbType.BigInt;
			sqlCommand.Parameters.Add(sqlParameter);
			foreach (long item in list)
			{
				sqlCommand.Parameters[0].Value = item;
				sqlCommand.ExecuteNonQuery();
			}
		}
		catch (Exception ex)
		{
			methodTraceContext.TraceCatch(ex);
			SqlSmoObject.FilterException(ex);
			throw methodTraceContext.TraceThrow(new FailedOperationException(ExceptionTemplatesSR.EraseSystemHealthPhantomRecords, Name, GetType().Name, ex));
		}
	}

	public UpgradeSession StartSerializationUpgrade()
	{
		return new PolicyStoreUpgradeSession();
	}

	public void PurgeHealthState()
	{
		PurgeHealthState(null);
	}

	public void PurgeHealthState(SfcQueryExpression targetTreeRoot)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("PurgeHealthState", TraceEventType.Information);
		methodTraceContext.TraceParameters(targetTreeRoot);
		try
		{
			string text = string.Empty;
			if (targetTreeRoot != null)
			{
				SfcObjectQuery sfcObjectQuery = new SfcObjectQuery(new Microsoft.SqlServer.Management.Smo.Server(SqlStoreConnection.ServerConnection));
				string text2 = string.Empty;
				foreach (SqlSmoObject item in sfcObjectQuery.ExecuteIterator(targetTreeRoot, null, null))
				{
					if (text2.Length > 0)
					{
						throw methodTraceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.OnlyOneTarget(targetTreeRoot.ToString())));
					}
					text2 = item.UrnOnlyId;
				}
				if (!string.IsNullOrEmpty(text2))
				{
					List<SfcTsqlProcFormatter.RuntimeArg> list = new List<SfcTsqlProcFormatter.RuntimeArg>();
					list.Add(new SfcTsqlProcFormatter.RuntimeArg(typeof(string), text2));
					text = purgeHealthStateAction.GenerateScript(this, list);
				}
			}
			else
			{
				text = "EXEC " + purgeHealthStateAction.Procedure;
			}
			if (!string.IsNullOrEmpty(text))
			{
				SqlStoreConnection.ServerConnection.ExecuteNonQuery(text, ExecutionTypes.NoCommands);
			}
		}
		catch (Exception ex)
		{
			methodTraceContext.TraceCatch(ex);
			if (!Utils.ShouldProcessException(ex))
			{
				throw;
			}
			throw methodTraceContext.TraceThrow(new FailedOperationException(ExceptionTemplatesSR.PurgeHealthState, Name, GetType().Name, ex));
		}
	}

	public void MarkSystemObject(object obj, bool marker)
	{
		if (obj == null)
		{
			throw new ArgumentNullException("obj");
		}
		if (SqlStoreConnection.ServerVersion < KilimanjaroVersion)
		{
			throw new DmfException(ExceptionTemplatesSR.VersionNotSupported(SqlStoreConnection.ServerVersion.ToString(), KilimanjaroVersion.ToString()));
		}
		int iD;
		string value;
		string name;
		if (obj.GetType() == typeof(Policy))
		{
			Policy policy = (Policy)obj;
			policy.Refresh();
			if (policy.IsSystemObject == marker)
			{
				return;
			}
			iD = policy.ID;
			value = "POLICY";
			name = policy.Name;
		}
		else if (obj.GetType() == typeof(Condition))
		{
			Condition condition = (Condition)obj;
			condition.Refresh();
			if (condition.IsSystemObject == marker)
			{
				return;
			}
			iD = condition.ID;
			value = "CONDITION";
			name = condition.Name;
		}
		else
		{
			if (!(obj.GetType() == typeof(ObjectSet)))
			{
				throw new DmfException(ExceptionTemplatesSR.CannotMarkSystemObject(obj.GetType().Name));
			}
			ObjectSet objectSet = (ObjectSet)obj;
			objectSet.Refresh();
			if (objectSet.IsSystemObject == marker)
			{
				return;
			}
			iD = objectSet.ID;
			value = "OBJECTSET";
			name = objectSet.Name;
		}
		try
		{
			List<SfcTsqlProcFormatter.RuntimeArg> list = new List<SfcTsqlProcFormatter.RuntimeArg>();
			list.Add(new SfcTsqlProcFormatter.RuntimeArg(typeof(string), value));
			list.Add(new SfcTsqlProcFormatter.RuntimeArg(typeof(int), iD));
			list.Add(new SfcTsqlProcFormatter.RuntimeArg(typeof(int), marker ? 1 : 0));
			string text = markSystemObjectAction.GenerateScript(this, list);
			if (!string.IsNullOrEmpty(text))
			{
				SqlStoreConnection.ServerConnection.ExecuteNonQuery(text, ExecutionTypes.NoCommands);
			}
			((SfcInstance)obj).Refresh();
		}
		catch (Exception ex)
		{
			if (!Utils.ShouldProcessException(ex))
			{
				throw;
			}
			throw new FailedOperationException(ExceptionTemplatesSR.MarkSystemObject, name, obj.GetType().Name, ex);
		}
	}

	private void Validate(string validationMode, bool throwOnFirst, ValidationState validationState)
	{
		ValidateProperties(validationMode, throwOnFirst, validationState);
	}

	private void ValidateProperties(string validationMode, bool throwOnFirst, ValidationState validationState)
	{
		if (HistoryRetentionInDays < 0)
		{
			Exception ex = new DmfException(ExceptionTemplatesSR.ValuePositive);
			if (throwOnFirst)
			{
				throw traceContext.TraceThrow(new ObjectValidationException(ExceptionTemplatesSR.PolicyStore, Name, ex));
			}
			validationState.AddError(ex, "HistoryRetentionInDays");
		}
	}

	ValidationState ISfcValidate.Validate(string validationMethod, params object[] arguments)
	{
		ValidationState validationState = new ValidationState();
		Validate(validationMethod, throwOnFirst: false, validationState);
		return validationState;
	}
}
