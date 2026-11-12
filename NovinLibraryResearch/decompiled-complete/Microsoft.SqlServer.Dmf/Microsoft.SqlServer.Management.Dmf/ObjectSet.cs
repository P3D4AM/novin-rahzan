using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using Microsoft.SqlServer.Diagnostics.STrace;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Facets;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Server;

namespace Microsoft.SqlServer.Management.Dmf;

public sealed class ObjectSet : SfcInstance, ISfcCreatable, ICreatable, ISfcDroppable, IDroppable, ISfcAlterable, IAlterable, ISfcValidate
{
	public sealed class Key : SfcKey
	{
		private string keyName;

		public string Name => keyName;

		public Key()
		{
		}

		public Key(Key other)
		{
			keyName = other.Name;
		}

		public Key(string name)
		{
			keyName = name;
		}

		internal Key(Dictionary<string, object> filedDict)
		{
			keyName = (string)filedDict["Name"];
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

		public override int GetHashCode()
		{
			return Name.GetHashCode();
		}

		private bool IsEqual(Key key)
		{
			return string.CompareOrdinal(Name, key.Name) == 0;
		}

		public override string GetUrnFragment()
		{
			return string.Format("{0}[@Name='{1}']", "ObjectSet", SfcSecureString.EscapeSquote(Name));
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
			return new ObjectSet();
		}
	}

	private static TraceContext traceContext;

	private static readonly SfcTsqlProcFormatter scriptCreateAction;

	private static readonly SfcTsqlProcFormatter scriptDropAction;

	private TargetSetCollection m_TargetSets;

	internal const string typeName = "ObjectSet";

	[SfcObject(SfcContainerRelationship.ChildContainer, SfcContainerCardinality.ZeroToAny, typeof(TargetSet))]
	public TargetSetCollection TargetSets
	{
		get
		{
			if (m_TargetSets == null)
			{
				ServerComparer customComparer = null;
				if (base.State != SfcObjectState.Pending && Parent != null)
				{
					customComparer = new ServerComparer((Parent.SqlStoreConnection == null) ? null : Parent.SqlStoreConnection.ServerConnection, "msdb");
				}
				m_TargetSets = new TargetSetCollection(this, customComparer);
			}
			return m_TargetSets;
		}
	}

	internal bool TargetsDatabaseObjects
	{
		get
		{
			foreach (TargetSet targetSet in TargetSets)
			{
				if (!targetSet.TargetsDatabaseObjects)
				{
					return false;
				}
			}
			return true;
		}
	}

	internal bool HasScript
	{
		get
		{
			foreach (TargetSet targetSet in TargetSets)
			{
				foreach (TargetSetLevel level in targetSet.Levels)
				{
					if (!string.IsNullOrEmpty(level.Condition) && Parent.Conditions[level.Condition].HasScript)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	internal string RootLevel
	{
		get
		{
			using (IEnumerator<TargetSet> enumerator = TargetSets.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current.RootLevel;
				}
			}
			return string.Empty;
		}
	}

	[SfcObject(SfcObjectRelationship.ParentObject, SfcObjectCardinality.One)]
	public new PolicyStore Parent
	{
		get
		{
			return (PolicyStore)base.Parent;
		}
		set
		{
			traceContext.TraceVerbose("Setting Parent to: {0}", value);
			base.Parent = value;
		}
	}

	[SfcIgnore]
	public Key IdentityKey => (Key)base.AbstractIdentityKey;

	[SfcProperty(SfcPropertyFlags.Required | SfcPropertyFlags.ReadOnlyAfterCreation)]
	[SfcKey(0)]
	public string Name => (string)base.Properties["Name"].Value;

	[SfcProperty(SfcPropertyFlags.Data)]
	public int ID
	{
		get
		{
			object value = base.Properties["ID"].Value;
			if (value == null)
			{
				return 0;
			}
			return (int)value;
		}
	}

	[SfcProperty(SfcPropertyFlags.Required)]
	public string Facet
	{
		get
		{
			return (string)base.Properties["Facet"].Value;
		}
		set
		{
			traceContext.TraceVerbose("Setting Facet to: {0}", value);
			if (!string.IsNullOrEmpty(Facet))
			{
				throw traceContext.TraceThrow(new Exception("Cannot update the Facet for a given ObjectSet. Drop and create a new one"));
			}
			base.Properties["Facet"].Value = value;
			PopulateTargetSetsBasedOnFacet(this, FacetRepository.GetFacetType(value));
			if (TargetSets.Count != 1)
			{
				return;
			}
			using IEnumerator<TargetSet> enumerator = TargetSets.GetEnumerator();
			if (enumerator.MoveNext())
			{
				enumerator.Current.Enabled = true;
			}
		}
	}

	[SfcProperty(SfcPropertyFlags.Data)]
	public bool IsSystemObject
	{
		get
		{
			object value = base.Properties["IsSystemObject"].Value;
			if (value == null)
			{
				return false;
			}
			return (bool)value;
		}
	}

	static ObjectSet()
	{
		traceContext = TraceContext.GetTraceContext("DMF", "ObjectSet");
		scriptCreateAction = new SfcTsqlProcFormatter();
		scriptDropAction = new SfcTsqlProcFormatter();
		scriptCreateAction.Procedure = "msdb.dbo.sp_syspolicy_add_object_set";
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("object_set_name", "Name", required: true, output: false));
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("facet", "Facet", required: true, output: false));
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("object_set_id", "ID", required: false, output: true));
		scriptDropAction.Procedure = "msdb.dbo.sp_syspolicy_delete_object_set";
		scriptDropAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("object_set_id", "ID", required: true, output: false));
	}

	public ObjectSet()
	{
	}

	public ObjectSet(PolicyStore parent, string name)
	{
		traceContext.TraceMethodEnter("ObjectSet");
		SetName(name);
		Parent = parent;
		traceContext.TraceMethodExit("ObjectSet");
	}

	private static void PopulateTargetSetBasedOnType(ObjectSet objectSet, Type type)
	{
		traceContext.TraceMethodEnter("PopulateTargetSetBasedOnType");
		traceContext.TraceParameters(objectSet, type);
		foreach (string item in SfcMetadataDiscovery.GetUrnSkeletonsFromType(type))
		{
			TargetSet targetSet = new TargetSet(objectSet, item);
			targetSet.Enabled = false;
			objectSet.TargetSets.Add(targetSet);
		}
		traceContext.TraceMethodExit("PopulateTargetSetBasedOnType");
	}

	private static void PopulateTargetSetsBasedOnFacet(ObjectSet objectSet, Type facetType)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("PopulateTargetSetsBasedOnFacet");
		methodTraceContext.TraceParameters(objectSet, facetType);
		if (!FacetRepository.IsRegisteredFacet(facetType))
		{
			throw methodTraceContext.TraceThrow(new NullFacetException(facetType.Name));
		}
		foreach (Type facetSupportedType in FacetRepository.GetFacetSupportedTypes(facetType))
		{
			PopulateTargetSetBasedOnType(objectSet, facetSupportedType);
		}
	}

	private static void PopulateTargetSetsBasedOnFacet(ObjectSet objectSet, Type facetType, SfcDomainInfo domainInfo)
	{
		if (!FacetRepository.IsRegisteredFacet(facetType))
		{
			throw new NullFacetException(facetType.Name);
		}
		object obj = Activator.CreateInstance(domainInfo.RootType);
		if (obj is ISfcDomain2)
		{
			foreach (Type facetSupportedType in FacetRepository.GetFacetSupportedTypes(facetType))
			{
				foreach (string item in ((ISfcDomain2)obj).GetUrnSkeletonsFromType(facetSupportedType))
				{
					TargetSet targetSet = new TargetSet(objectSet, item);
					targetSet.Enabled = false;
					objectSet.TargetSets.Add(targetSet);
				}
			}
			return;
		}
		foreach (Type facetSupportedType2 in FacetRepository.GetFacetSupportedTypes(facetType))
		{
			PopulateTargetSetBasedOnType(objectSet, facetSupportedType2);
		}
	}

	public IEnumerable CalculateTargets(ISfcConnection targetConnection)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("CalculateTargets", TraceEventType.Information);
		methodTraceContext.TraceParameters(targetConnection);
		SfcObjectQuery targetSetQuery = Utils.GetQueryFromDomainInfo(GetTargetDomain(), targetConnection);
		foreach (TargetSet targetSet in TargetSets)
		{
			if (!targetSet.Enabled)
			{
				continue;
			}
			SfcQueryExpression query = new SfcQueryExpression(targetSet.GetFilter());
			IEnumerable enumerable = targetSetQuery.ExecuteIterator(query, null, null);
			foreach (object item in enumerable)
			{
				yield return item;
			}
		}
	}

	internal SfcDomainInfo GetTargetDomain()
	{
		using (IEnumerator<TargetSet> enumerator = TargetSets.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				return Utils.GetDomainFromUrnSkeleton((string)enumerator.Current.Properties["TargetTypeSkeleton"].Value);
			}
		}
		traceContext.DebugAssert(test: false, "TargetSet must include at least one type");
		return null;
	}

	public static IEnumerable CalculateTargets(ISfcConnection targetConnection, SfcQueryExpression sfcQueryExpression)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("CalculateTargets", TraceEventType.Information);
		methodTraceContext.TraceParameters(targetConnection, sfcQueryExpression);
		IEnumerable enumerable = new SfcObjectQuery(new Microsoft.SqlServer.Management.Smo.Server(Utils.GetSqlStoreConnection(targetConnection, "ObjectSet.CalculateTargets").ServerConnection)).ExecuteIterator(sfcQueryExpression, null, null);
		foreach (object item in enumerable)
		{
			yield return item;
		}
	}

	public IEnumerable CalculateTargets(SqlStoreConnection sqlStoreConnection, string policyCategory)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("CalculateTargets", TraceEventType.Information);
		methodTraceContext.TraceParameters(sqlStoreConnection, policyCategory);
		if (sqlStoreConnection == null)
		{
			throw methodTraceContext.TraceThrow(new ArgumentNullException("sqlStoreConnection"));
		}
		PolicyCategory pc = null;
		if (!string.IsNullOrEmpty(policyCategory) && sqlStoreConnection.ServerVersion.Major >= 10)
		{
			if (Parent.SqlStoreConnection == null || Parent.SqlStoreConnection != sqlStoreConnection)
			{
				PolicyStore policyStore = new PolicyStore(sqlStoreConnection);
				if (policyStore.PolicyCategories.Contains(policyCategory))
				{
					pc = policyStore.PolicyCategories[policyCategory];
				}
			}
			else
			{
				if (!Parent.PolicyCategories.Contains(policyCategory))
				{
					throw methodTraceContext.TraceThrow(new MissingObjectException(ExceptionTemplatesSR.Category, policyCategory));
				}
				pc = Parent.PolicyCategories[policyCategory];
			}
		}
		SfcDomainInfo domainInfo = GetTargetDomain();
		SfcObjectQuery policyQuery = Utils.GetQueryFromDomainInfo(domainInfo, sqlStoreConnection);
		Microsoft.SqlServer.Management.Smo.Server server = null;
		foreach (TargetSet targetSet in TargetSets)
		{
			if (!targetSet.Enabled)
			{
				continue;
			}
			SfcQueryExpression sfcQueryExpression;
			if (Utils.IsSmoDomain(domainInfo))
			{
				server = new Microsoft.SqlServer.Management.Smo.Server(sqlStoreConnection.ServerConnection);
				sfcQueryExpression = GetAdjustedFilter(targetSet, server, pc);
			}
			else
			{
				sfcQueryExpression = new SfcQueryExpression(targetSet.GetFilter());
			}
			if (sfcQueryExpression == null)
			{
				continue;
			}
			string[] fields = null;
			if (Utils.IsSmoDomain(domainInfo) && !targetSet.TargetsDatabases)
			{
				traceContext.DebugAssert(server != null, "Server object shouldn't be null");
				fields = GetNonExpensiveProps(sfcQueryExpression, server);
			}
			foreach (object item in policyQuery.ExecuteIterator(sfcQueryExpression, fields, null))
			{
				yield return item;
			}
		}
	}

	private static string[] GetNonExpensiveProps(SfcQueryExpression filter, Microsoft.SqlServer.Management.Smo.Server server)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("GetNonExpensiveProps");
		methodTraceContext.TraceParameters(filter, server);
		RequestObjectInfo requestObjectInfo = new RequestObjectInfo(filter.ToString(), RequestObjectInfo.Flags.Properties);
		ObjectInfo objectInfo = new Enumerator().Process(server.ConnectionContext, requestObjectInfo);
		List<string> list = new List<string>(objectInfo.Properties.Length);
		ObjectProperty[] properties = objectInfo.Properties;
		foreach (ObjectProperty objectProperty in properties)
		{
			if (!objectProperty.Expensive)
			{
				list.Add(objectProperty.Name);
			}
		}
		string[] array = list.ToArray();
		methodTraceContext.TraceParameterOut("returnVal", (array.Length != 0) ? array : null);
		return (array.Length != 0) ? array : null;
	}

	internal static void CalculateTargets(SqlStoreConnection targetConnection, SfcQueryExpression sfcQueryExpression, Condition condition, AdHocPolicyEvaluationMode evaluationMode, out object[] conforming, out TargetEvaluation[] violating)
	{
		conforming = null;
		violating = null;
		SfcDomainInfo sfcDomainInfo = SfcRegistration.Domains[sfcQueryExpression.Expression[0].Name];
		traceContext.DebugAssert(sfcDomainInfo != null, "domain can't be null");
		CalculateTargets(Utils.GetQueryFromDomainInfo(sfcDomainInfo, targetConnection).ExecuteIterator(sfcQueryExpression, null, null), condition, evaluationMode, out conforming, out violating);
	}

	internal static void CalculateTargets(IEnumerable objectSet, Condition condition, AdHocPolicyEvaluationMode evaluationMode, out object[] conforming, out TargetEvaluation[] violating)
	{
		traceContext.TraceMethodEnter("CalculateTargets");
		traceContext.TraceParameters(objectSet, condition, evaluationMode);
		conforming = null;
		violating = null;
		ArrayList arrayList = new ArrayList();
		ArrayList arrayList2 = new ArrayList();
		foreach (object item in objectSet)
		{
			if (condition.Evaluate(item, evaluationMode))
			{
				arrayList.Add(item);
			}
			else
			{
				arrayList2.Add(new TargetEvaluation(item, condition.ExpressionNode.DeepClone()));
			}
		}
		if (arrayList.Count > 0)
		{
			conforming = arrayList.ToArray();
		}
		if (arrayList2.Count > 0)
		{
			violating = (TargetEvaluation[])arrayList2.ToArray(typeof(TargetEvaluation));
		}
		traceContext.TraceParameterOut("conforming", conforming);
		traceContext.TraceParameterOut("violating", violating);
		traceContext.TraceMethodExit("CalculateTargets");
	}

	public void CalculateTargets(SqlStoreConnection targetConnection, Condition condition, AdHocPolicyEvaluationMode evaluationMode, string policyCategory, out object[] conforming, out TargetEvaluation[] violating)
	{
		conforming = null;
		violating = null;
		if (condition == null || condition.ExpressionNode == null)
		{
			throw traceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.ConditionIsNull));
		}
		if (Facet != condition.Facet)
		{
			throw traceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.ObjectSetAndConditionFacetMismatch(Name, Facet, condition.Name, condition.Facet)));
		}
		CalculateTargets(CalculateTargets(targetConnection, policyCategory), condition, evaluationMode, out conforming, out violating);
	}

	internal bool IsEventingFilter()
	{
		foreach (TargetSet targetSet in TargetSets)
		{
			if (!targetSet.IsEventingFilter())
			{
				return false;
			}
		}
		return true;
	}

	internal SfcQueryExpression GetAdjustedFilter(TargetSet ts, Microsoft.SqlServer.Management.Smo.Server server, PolicyCategory pc)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("GetAdjustedFilter");
		methodTraceContext.TraceParameters(ts, server, pc);
		ExpressionNode expressionNode = null;
		Dictionary<string, ExpressionNode> dictionary = new Dictionary<string, ExpressionNode>();
		bool targetsDatabaseObjects = ts.TargetsDatabaseObjects;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		bool flag6 = false;
		SfcQueryExpression sfcQueryExpression = new SfcQueryExpression(ts.GetFilter());
		if (ts.RootLevel == ts.TargetTypeSkeleton)
		{
			methodTraceContext.TraceParameterOut("returnVal", sfcQueryExpression);
			return sfcQueryExpression;
		}
		if (targetsDatabaseObjects)
		{
			TargetSetLevel level = ts.GetLevel(TargetSet.DatabaseLevel);
			traceContext.DebugAssert(level != null, "Null Database Level in TargetSet targeting databases");
			if (!string.IsNullOrEmpty(level.Condition))
			{
				expressionNode = Parent.Conditions[level.Condition].ExpressionNode;
			}
			if (pc != null && !pc.MandateDatabaseSubscriptions)
			{
				if (expressionNode != null && expressionNode.NameConditionType == NameConditionType.Equal)
				{
					foreach (string item in pc.Parent.PolicyCategorySubscriptions.EnumCategorySubscribers(pc.Name, "DATABASE"))
					{
						if (expressionNode.ObjectName == item)
						{
							flag2 = true;
							break;
						}
					}
					if (!flag2)
					{
						methodTraceContext.TraceParameterOut("returnVal", null);
						return null;
					}
				}
				if (!flag2)
				{
					ExpressionNode expressionNode2 = null;
					int num = 0;
					foreach (string item2 in pc.Parent.PolicyCategorySubscriptions.EnumCategorySubscribers(pc.Name, "DATABASE"))
					{
						num++;
						ExpressionNode expressionNode3 = new ExpressionNodeOperator(OperatorType.EQ, new ExpressionNodeAttribute("Name"), new ExpressionNodeConstant(item2));
						expressionNode2 = ((expressionNode2 != null) ? new ExpressionNodeOperator(OperatorType.OR, expressionNode3, expressionNode2) : expressionNode3);
					}
					if (expressionNode2 == null)
					{
						methodTraceContext.TraceParameterOut("returnVal", null);
						return null;
					}
					if (expressionNode == null)
					{
						dictionary.Add(TargetSet.DatabaseLevel, expressionNode2);
						flag2 = num == 1;
					}
					else
					{
						ExpressionNode value = new ExpressionNodeOperator(OperatorType.AND, new ExpressionNodeGroup(expressionNode), new ExpressionNodeGroup(expressionNode2));
						dictionary.Add(TargetSet.DatabaseLevel, value);
					}
				}
			}
		}
		ExpressionNode expressionNode4 = null;
		ExpressionNode expressionNode5 = null;
		if (targetsDatabaseObjects && ts.TargetsDatabases)
		{
			flag = true;
			expressionNode4 = (dictionary.ContainsKey(TargetSet.DatabaseLevel) ? dictionary[TargetSet.DatabaseLevel] : expressionNode);
			expressionNode5 = expressionNode4;
		}
		else
		{
			TargetSetLevel level2 = ts.GetLevel(ts.TargetTypeSkeleton);
			if (!string.IsNullOrEmpty(level2.Condition))
			{
				expressionNode4 = Parent.Conditions[level2.Condition].ExpressionNode;
			}
			if (targetsDatabaseObjects)
			{
				expressionNode5 = ((!dictionary.ContainsKey(TargetSet.DatabaseLevel)) ? expressionNode : dictionary[TargetSet.DatabaseLevel]);
			}
		}
		if (!flag2 && expressionNode5 != null)
		{
			flag2 = expressionNode5.NameConditionType == NameConditionType.Equal;
		}
		if (expressionNode4 != null)
		{
			if (!(targetsDatabaseObjects && flag && flag2))
			{
				int num2 = 0;
				foreach (AttributeOperatorPair item3 in expressionNode4.EnumAttributeOperatorPairs())
				{
					if (item3.Attribute == "IsSystemObject")
					{
						flag5 = true;
					}
					else if (item3.Attribute == "IsAccessible")
					{
						flag6 = true;
					}
					else if ((item3.Attribute == "Name" && item3.OpType == OperatorType.EQ) || (item3.Attribute == "ID" && item3.OpType == OperatorType.EQ))
					{
						flag3 = true;
						num2++;
					}
				}
				flag4 = !(flag5 || flag3);
				if (targetsDatabaseObjects && flag && !flag2 && num2 == 1)
				{
					flag2 = true;
				}
			}
		}
		else
		{
			flag4 = true;
		}
		if (targetsDatabaseObjects && !flag && !flag2 && expressionNode5 != null)
		{
			int num3 = 0;
			foreach (AttributeOperatorPair item4 in expressionNode5.EnumAttributeOperatorPairs())
			{
				if (item4.Attribute == "IsAccessible")
				{
					flag6 = true;
				}
				else if ((item4.Attribute == "Name" && item4.OpType == OperatorType.EQ) || (item4.Attribute == "ID" && item4.OpType == OperatorType.EQ))
				{
					num3++;
				}
			}
			flag2 = num3 == 1;
		}
		if (flag4)
		{
			bool flag7 = false;
			RequestObjectInfo requestObjectInfo = new RequestObjectInfo(sfcQueryExpression.ToString(), RequestObjectInfo.Flags.Properties);
			ObjectProperty[] properties = new Enumerator().Process(server.ConnectionContext, requestObjectInfo).Properties;
			for (int i = 0; i < properties.Length; i++)
			{
				if (properties[i].Name == "IsSystemObject")
				{
					flag7 = true;
					break;
				}
			}
			if (flag7)
			{
				ExpressionNode expressionNode6 = new ExpressionNodeOperator(OperatorType.EQ, new ExpressionNodeAttribute("IsSystemObject"), ExpressionNode.ConstructNode(false));
				if (dictionary.ContainsKey(ts.TargetTypeSkeleton))
				{
					dictionary.Remove(ts.TargetTypeSkeleton);
				}
				if (expressionNode4 == null)
				{
					dictionary.Add(ts.TargetTypeSkeleton, expressionNode6);
				}
				else
				{
					ExpressionNode value2 = new ExpressionNodeOperator(OperatorType.AND, expressionNode6, new ExpressionNodeGroup(expressionNode4));
					dictionary.Add(ts.TargetTypeSkeleton, value2);
				}
			}
		}
		if (targetsDatabaseObjects && !flag2 && !flag6)
		{
			ExpressionNode expressionNode7 = null;
			ExpressionNode expressionNode8 = new ExpressionNodeOperator(OperatorType.EQ, new ExpressionNodeAttribute("IsAccessible"), ExpressionNode.ConstructNode(true));
			if (dictionary.ContainsKey(TargetSet.DatabaseLevel))
			{
				expressionNode7 = dictionary[TargetSet.DatabaseLevel];
				dictionary.Remove(TargetSet.DatabaseLevel);
			}
			else
			{
				expressionNode7 = expressionNode5;
			}
			if (expressionNode7 != null)
			{
				ExpressionNode value3 = new ExpressionNodeOperator(OperatorType.AND, expressionNode8, new ExpressionNodeGroup(expressionNode7));
				dictionary.Add(TargetSet.DatabaseLevel, value3);
			}
			else
			{
				dictionary.Add(TargetSet.DatabaseLevel, expressionNode8);
			}
		}
		return new SfcQueryExpression(ts.GetFilterWithNodeReplacement(dictionary));
	}

	internal static SfcQueryExpression AdjustForSystem(Version ver, Policy policy, SfcQueryExpression targetQueryExpression)
	{
		if (!SqlContext.IsAvailable)
		{
			throw new DmfException(ExceptionTemplatesSR.OnlyInSqlClr);
		}
		SfcQueryExpression result = targetQueryExpression;
		ServerVersion version = new ServerVersion(ver.Major, ver.Minor);
		ObjectSet objectSet = policy.Parent.ObjectSets[policy.ObjectSet];
		TargetSet targetSet = null;
		traceContext.DebugAssert(1 == objectSet.TargetSets.Count, "Only 1 TS expected");
		using (IEnumerator<TargetSet> enumerator = objectSet.TargetSets.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				targetSet = enumerator.Current;
			}
		}
		if (targetSet.TargetsDatabases)
		{
			TargetSetLevel level = targetSet.GetLevel(TargetSet.DatabaseLevel);
			if (!string.IsNullOrEmpty(level.Condition) && policy.Parent.Conditions[level.Condition].ExpressionNode.NameConditionType == NameConditionType.Equal)
			{
				return targetQueryExpression;
			}
		}
		RequestObjectInfo requestObjectInfo = new RequestObjectInfo(targetQueryExpression.ToString(), RequestObjectInfo.Flags.Properties);
		ObjectProperty[] properties = new Enumerator().Process(version, requestObjectInfo).Properties;
		for (int i = 0; i < properties.Length; i++)
		{
			if (properties[i].Name == "IsSystemObject")
			{
				ExpressionNode right = new ExpressionNodeOperator(OperatorType.EQ, new ExpressionNodeAttribute("IsSystemObject"), ExpressionNode.ConstructNode(false));
				ExpressionNode node = ExpressionNode.ConvertFromFilterNode(targetQueryExpression.Expression[targetQueryExpression.Expression.Length - 1].Filter);
				ExpressionNode expressionNode = new ExpressionNodeOperator(OperatorType.AND, new ExpressionNodeGroup(node), right);
				targetQueryExpression.Expression[targetQueryExpression.Expression.Length - 1].Filter = expressionNode.ConvertToFilterNode();
				result = new SfcQueryExpression(targetQueryExpression.Expression.ToString());
				break;
			}
		}
		return result;
	}

	internal bool DependsOnCondition(string conditionName)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("DependsOnCondition");
		methodTraceContext.TraceParameters(conditionName);
		foreach (TargetSet targetSet in TargetSets)
		{
			foreach (TargetSetLevel level in targetSet.Levels)
			{
				if (level.Condition == conditionName)
				{
					methodTraceContext.TraceParameterOut("returnVal", true);
					return true;
				}
			}
		}
		methodTraceContext.TraceParameterOut("returnVal", false);
		return false;
	}

	internal ReadOnlyCollection<Policy> EnumDependentPolicies()
	{
		List<Policy> list = new List<Policy>();
		foreach (Policy policy in Parent.Policies)
		{
			if (policy.ObjectSet == Name)
			{
				list.Add(policy);
			}
		}
		return list.AsReadOnly();
	}

	internal ReadOnlyCollection<string> EnumReferencedConditionNames()
	{
		List<string> list = new List<string>();
		foreach (TargetSet targetSet in TargetSets)
		{
			foreach (TargetSetLevel level in targetSet.Levels)
			{
				if (!string.IsNullOrEmpty(level.Condition) && !list.Contains(level.Condition))
				{
					list.Add(level.Condition);
				}
			}
		}
		return list.AsReadOnly();
	}

	public static string GenerateUniqueName(Policy policy)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("GenerateUniqueName");
		methodTraceContext.TraceParameters(policy);
		if (policy.Parent == null)
		{
			methodTraceContext.TraceParameterOut("returnVal", null);
			return null;
		}
		int num = 1;
		string text = ((!string.IsNullOrEmpty(policy.Name)) ? ((policy.Name.Length > 112) ? (policy.Name.Substring(0, 112) + "_ObjectSet") : (policy.Name + "_ObjectSet")) : "policy_ObjectSet");
		string text2 = text;
		while (policy.Parent.ObjectSets.Contains(text2))
		{
			text2 = text + "_" + num.ToString("#", CultureInfo.InvariantCulture);
			num++;
		}
		methodTraceContext.TraceParameterOut("returnVal", text2);
		return text2;
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	public override void Discover(ISfcDependencyDiscoveryObjectSink sink)
	{
		if (sink == null)
		{
			throw new ArgumentNullException("sink");
		}
		if (sink.Action == SfcDependencyAction.Serialize)
		{
			TargetSetCollection targetSets = TargetSets;
			if (targetSets != null)
			{
				sink.Add(SfcDependencyDirection.Inbound, targetSets.GetEnumerator(), SfcTypeRelation.RequiredChild, discovered: false);
			}
		}
	}

	public ISfcScript ScriptCreate()
	{
		SfcTSqlScript sfcTSqlScript = new SfcTSqlScript();
		List<SfcTsqlProcFormatter.RuntimeArg> list = new List<SfcTsqlProcFormatter.RuntimeArg>();
		list.Add(new SfcTsqlProcFormatter.RuntimeArg(Name.GetType(), Name));
		list.Add(new SfcTsqlProcFormatter.RuntimeArg(Facet.GetType(), Facet));
		list.Add(new SfcTsqlProcFormatter.RuntimeArg(typeof(int), 0));
		sfcTSqlScript.AddBatch(scriptCreateAction.GenerateScript(this, list));
		bool declareArguments = true;
		foreach (TargetSet targetSet in TargetSets)
		{
			sfcTSqlScript.AddBatch(targetSet.ScriptCreate(declareArguments).ToString());
			declareArguments = false;
		}
		return sfcTSqlScript;
	}

	public void Create()
	{
		traceContext.TraceMethodEnter("Create");
		Validate(ValidationMethod.Create);
		CreateImpl();
		TargetSets.Refresh();
		traceContext.TraceMethodExit("Create");
	}

	protected override void PostCreate(object executionResult)
	{
		traceContext.TraceMethodEnter("PostCreate");
		traceContext.TraceParameters(executionResult);
		base.Properties["ID"].Value = executionResult;
		traceContext.TraceMethodExit("PostCreate");
	}

	internal ISfcScript ScriptCreateWithDependencies(string policyCondition)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("ScriptCreateWithDependencies");
		methodTraceContext.TraceParameters(policyCondition);
		SfcTSqlScript sfcTSqlScript = new SfcTSqlScript();
		foreach (string item in EnumReferencedConditionNames())
		{
			if (item != policyCondition)
			{
				sfcTSqlScript.AddBatch(Parent.Conditions[item].ScriptCreate().ToString());
			}
		}
		sfcTSqlScript.AddBatch(ScriptCreate().ToString());
		methodTraceContext.TraceParameterOut("returnVal", sfcTSqlScript);
		return sfcTSqlScript;
	}

	public ISfcScript ScriptDrop()
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("ScriptDrop", TraceEventType.Information);
		string text = scriptDropAction.GenerateScript(this);
		methodTraceContext.TraceVerbose("Script generated: " + text);
		SfcTSqlScript sfcTSqlScript = new SfcTSqlScript(text);
		methodTraceContext.TraceParameterOut("returnVal", sfcTSqlScript);
		return sfcTSqlScript;
	}

	public void Drop()
	{
		DropImpl();
	}

	public ISfcScript ScriptAlter()
	{
		SfcTSqlScript sfcTSqlScript = new SfcTSqlScript();
		foreach (TargetSet targetSet in TargetSets)
		{
			sfcTSqlScript.AddBatch(targetSet.ScriptAlter().ToString());
		}
		return sfcTSqlScript;
	}

	public void Alter()
	{
		traceContext.TraceMethodEnter("Alter");
		Validate(ValidationMethod.Alter);
		AlterImpl();
		traceContext.TraceMethodExit("Alter");
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	public static SfcObjectFactory GetObjectFactory()
	{
		return ObjectFactory.Instance;
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

	private void SetName(string name)
	{
		traceContext.TraceMethodEnter("SetName");
		traceContext.TraceParameters(name);
		base.Properties["Name"].Value = name;
		traceContext.TraceMethodExit("SetName");
	}

	public void SetFacetWithDomain(string facet, string domain)
	{
		if (string.IsNullOrEmpty(domain))
		{
			throw new ArgumentException(ExceptionTemplatesSR.ArgumentNullOrEmpty("domain"));
		}
		if (!string.IsNullOrEmpty(Facet))
		{
			throw new DmfException(ExceptionTemplatesSR.CannotChangeFacetForObjectSet);
		}
		SfcDomainInfo domainInfo = SfcRegistration.Domains[domain];
		PopulateTargetSetsBasedOnFacet(this, FacetRepository.GetFacetType(facet), domainInfo);
		base.Properties["Facet"].Value = facet;
		if (TargetSets.Count != 1)
		{
			return;
		}
		using IEnumerator<TargetSet> enumerator = TargetSets.GetEnumerator();
		if (enumerator.MoveNext())
		{
			enumerator.Current.Enabled = true;
		}
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	protected override ISfcCollection GetChildCollection(string elementType)
	{
		if (elementType == "TargetSet")
		{
			return TargetSets;
		}
		throw traceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.NoSuchCollection(elementType)));
	}

	private void Validate(string validationMode, bool throwOnFirst, ValidationState validationState)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("Validate");
		methodTraceContext.TraceParameters(validationMode, throwOnFirst, validationState);
		ValidateProperties(validationMode, throwOnFirst, validationState);
		ValidateTargetSets(validationMode, throwOnFirst, validationState);
	}

	private void ValidateProperties(string validationMode, bool throwOnFirst, ValidationState validationState)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("ValidateProperties");
		methodTraceContext.TraceParameters(validationMode, throwOnFirst, validationState);
		if (string.IsNullOrEmpty(Name))
		{
			Exception ex = new SfcPropertyNotSetException("Name");
			if (throwOnFirst)
			{
				throw methodTraceContext.TraceThrow(new ObjectValidationException(ExceptionTemplatesSR.Condition, Name, ex));
			}
			validationState.AddError(ex, "Name");
		}
		if (validationMode == ValidationMethod.Rename)
		{
			return;
		}
		if (string.IsNullOrEmpty(Facet))
		{
			Exception ex2 = new SfcPropertyNotSetException("Facet");
			if (throwOnFirst)
			{
				throw methodTraceContext.TraceThrow(new ObjectValidationException(ExceptionTemplatesSR.ManagementFacet, Name, ex2));
			}
			validationState.AddError(ex2, "Facet");
		}
		else if (!PolicyStore.Facets.Contains(Facet))
		{
			Exception ex3 = new MissingObjectException(ExceptionTemplatesSR.ManagementFacet, Facet);
			if (throwOnFirst)
			{
				throw methodTraceContext.TraceThrow(new ObjectValidationException(ExceptionTemplatesSR.Condition, Name, ex3));
			}
			validationState.AddError(ex3, "Facet");
		}
	}

	private void ValidateTargetSets(string validationMode, bool throwOnFirst, ValidationState validationState)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("ValidateTargetSets");
		methodTraceContext.TraceParameters(validationMode, throwOnFirst, validationState);
		if (validationMode == ValidationMethod.Rename)
		{
			return;
		}
		foreach (TargetSet targetSet in TargetSets)
		{
			if (string.CompareOrdinal(RootLevel, targetSet.RootLevel) != 0)
			{
				Exception ex = new DmfException(ExceptionTemplatesSR.InvalidUrnSkeleton(targetSet.TargetTypeSkeleton));
				if (throwOnFirst)
				{
					throw new ObjectValidationException(GetType().Name, Name, ex);
				}
				validationState.AddError(ex, "TargetSets");
			}
		}
		ObjectSet objectSet = new ObjectSet(Parent, Name);
		if (string.CompareOrdinal(RootLevel, "Utility") != 0)
		{
			objectSet.Facet = Facet;
		}
		else
		{
			objectSet.SetFacetWithDomain(Facet, "Utility");
		}
		if (TargetSets.Count != objectSet.TargetSets.Count)
		{
			Exception ex2 = new TargetSetCountMismatchException(Name, Facet);
			if (throwOnFirst)
			{
				throw methodTraceContext.TraceThrow(new ObjectValidationException(GetType().Name, Name, ex2));
			}
			validationState.AddError(ex2, "TargetSets");
		}
		bool flag = false;
		foreach (TargetSet targetSet2 in TargetSets)
		{
			flag |= targetSet2.Enabled;
			if (!objectSet.TargetSets.Contains(targetSet2.TargetTypeSkeleton))
			{
				Exception ex3 = new UnsupportedTargetSetForFacetException(targetSet2.TargetTypeSkeleton, Name, Facet);
				if (throwOnFirst)
				{
					throw methodTraceContext.TraceThrow(new ObjectValidationException(GetType().Name, Name, ex3));
				}
				validationState.AddError(ex3, "TargetSets");
			}
			foreach (TargetSetLevel level in targetSet2.Levels)
			{
				if (string.IsNullOrEmpty(level.Condition))
				{
					continue;
				}
				Condition condition = Parent.Conditions[level.Condition];
				if (condition == null)
				{
					throw new MissingObjectException(ExceptionTemplatesSR.Condition, level.Condition);
				}
				if (!condition.IsEnumerable)
				{
					Exception ex4 = new DmfException(ExceptionTemplatesSR.ConditionCannotBeUsedForFiltering(level.Condition));
					if (throwOnFirst)
					{
						throw methodTraceContext.TraceThrow(new ObjectValidationException(GetType().Name, Name, ex4));
					}
					validationState.AddError(ex4, "TargetSets");
				}
				if (!FacetRepository.GetFacetSupportedTypes(FacetRepository.GetFacetType(condition.Facet)).Contains(level.TargetType))
				{
					Exception ex5 = new ConflictingPropertyValuesException(validationMode, ExceptionTemplatesSR.ManagementFacet, condition.Facet, ExceptionTemplatesSR.ObjectType, level.TargetType.ToString());
					if (throwOnFirst)
					{
						throw methodTraceContext.TraceThrow(new ObjectValidationException(ExceptionTemplatesSR.Condition, "TargetSets", ex5));
					}
					validationState.AddError(ex5, "TargetSets");
				}
			}
		}
		if (!flag)
		{
			Exception ex6 = new NoTargetSetEnabledException(Name);
			if (throwOnFirst)
			{
				throw methodTraceContext.TraceThrow(new ObjectValidationException(GetType().Name, Name, ex6));
			}
			validationState.AddError(ex6, "TargetSets");
		}
	}

	public void Validate(string mode)
	{
		Validate(mode, throwOnFirst: true, null);
	}

	ValidationState ISfcValidate.Validate(string validationMethod, params object[] arguments)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("ISfcValidate.Validate");
		methodTraceContext.TraceParameters(validationMethod, arguments);
		ValidationState validationState = new ValidationState();
		Validate(validationMethod, throwOnFirst: false, validationState);
		methodTraceContext.TraceParameterOut("returnVal", validationState);
		return validationState;
	}
}
