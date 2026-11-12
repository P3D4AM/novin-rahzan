using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.SqlServer.Diagnostics.STrace;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;

namespace Microsoft.SqlServer.Management.Dmf;

public sealed class TargetSet : SfcInstance
{
	private sealed class TypeMetadata : SfcTypeMetadata
	{
		private static readonly TypeMetadata instance;

		public static TypeMetadata Instance => instance;

		static TypeMetadata()
		{
			instance = new TypeMetadata();
		}

		private TypeMetadata()
		{
		}

		public override bool IsCrudActionHandledByParent(SfcDependencyAction depAction)
		{
			if ((uint)(depAction - 2) <= 3u)
			{
				return true;
			}
			throw new DmfException(ExceptionTemplatesSR.UnsupportedCrudDependencyAction(depAction.ToString()));
		}
	}

	public sealed class Key : SfcKey
	{
		private string targetTypeSkeletonValue;

		public string targetTypeSkeleton
		{
			get
			{
				return targetTypeSkeletonValue;
			}
			set
			{
				targetTypeSkeletonValue = value;
			}
		}

		public Key()
		{
		}

		public Key(Key other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			targetTypeSkeleton = other.targetTypeSkeleton;
		}

		public Key(string targetTypeSkeleton)
		{
			this.targetTypeSkeleton = targetTypeSkeleton;
		}

		internal Key(Dictionary<string, object> filedDict)
		{
			targetTypeSkeleton = (string)filedDict["TargetTypeSkeleton"];
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
			return targetTypeSkeleton.GetHashCode();
		}

		private bool IsEqual(Key key)
		{
			return string.Compare(targetTypeSkeleton, key.targetTypeSkeleton, StringComparison.OrdinalIgnoreCase) == 0;
		}

		public override string GetUrnFragment()
		{
			return string.Format("{0}[@TargetTypeSkeleton='{1}']", "TargetSet", Urn.EscapeString(targetTypeSkeleton));
		}
	}

	private class ObjectFactory : SfcObjectFactory
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
			return new TargetSet();
		}
	}

	private static TraceContext traceContext;

	internal static readonly string DatabaseLevel;

	private static readonly SfcTsqlProcFormatter scriptCreateAction;

	private static readonly SfcTsqlProcFormatter scriptDropAction;

	private static readonly SfcTsqlProcFormatter scriptAlterAction;

	private static readonly SfcTsqlProcFormatter scriptCreateReferenceAction;

	private static readonly SfcTsqlProcFormatter scriptAlterReferenceAction;

	private static readonly string smoDatabaseUrnSkeleton;

	private Urn m_urn;

	private TargetSetLevelCollection m_Levels;

	private string rootLevel;

	internal const string typeName = "TargetSet";

	private Type m_type;

	[SfcObject(SfcContainerRelationship.ChildContainer, SfcContainerCardinality.ZeroToAny, typeof(TargetSetLevel))]
	public TargetSetLevelCollection Levels
	{
		get
		{
			if (m_Levels == null)
			{
				ServerComparer customComparer = null;
				SfcConnectionContextMode sfcConnectionContextMode = SfcConnectionContextMode.Offline;
				if (Parent != null && Parent.Parent != null)
				{
					sfcConnectionContextMode = ((ISfcHasConnection)Parent.Parent).ConnectionContext.Mode;
				}
				if (base.State != SfcObjectState.Pending && sfcConnectionContextMode != 0)
				{
					customComparer = new ServerComparer((Parent == null || Parent.Parent == null || Parent.Parent.SqlStoreConnection == null) ? null : Parent.Parent.SqlStoreConnection.ServerConnection, "msdb");
				}
				m_Levels = new TargetSetLevelCollection(this, customComparer);
			}
			return m_Levels;
		}
	}

	public string RootLevel
	{
		get
		{
			if (rootLevel == null)
			{
				if (null == m_urn)
				{
					m_urn = new Urn(TargetTypeSkeleton);
					if (!m_urn.IsValidUrnSkeleton())
					{
						throw traceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.InvalidUrnSkeleton(TargetTypeSkeleton)));
					}
				}
				rootLevel = m_urn.XPathExpression.BlockExpressionSkeleton(0);
			}
			return rootLevel;
		}
	}

	internal bool IsAllDatabasesFilter
	{
		get
		{
			if (string.Compare(DatabaseLevel, TargetTypeSkeleton, StringComparison.OrdinalIgnoreCase) == 0)
			{
				foreach (TargetSetLevel level in Levels)
				{
					if (!string.IsNullOrEmpty(level.Condition))
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}
	}

	internal bool TargetsDatabaseObjects => GetFilter().StartsWith(smoDatabaseUrnSkeleton, StringComparison.OrdinalIgnoreCase);

	internal bool TargetsDatabases => string.Equals(DatabaseLevel, TargetTypeSkeleton, StringComparison.OrdinalIgnoreCase);

	[SfcObject(SfcObjectRelationship.ParentObject, SfcObjectCardinality.One)]
	public new ObjectSet Parent
	{
		get
		{
			return (ObjectSet)base.Parent;
		}
		set
		{
			traceContext.TraceVerbose("Setting Parent to: {0}", value);
			base.Parent = value;
		}
	}

	[SfcIgnore]
	public Key IdentityKey => (Key)base.AbstractIdentityKey;

	[SfcProperty(SfcPropertyFlags.Data)]
	public int ID
	{
		get
		{
			if (base.Properties["ID"].Value == null)
			{
				return 0;
			}
			return (int)base.Properties["ID"].Value;
		}
	}

	[SfcIgnore]
	private int ObjectSetID => Parent.ID;

	[SfcProperty(SfcPropertyFlags.Required)]
	[SfcKey(0)]
	public string TargetTypeSkeleton => (string)base.Properties["TargetTypeSkeleton"].Value;

	[SfcProperty(SfcPropertyFlags.Required)]
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

	[SfcProperty(SfcPropertyFlags.Data)]
	public string TargetType
	{
		get
		{
			if (base.Properties["TargetType"].Value == null && base.Properties["TargetTypeSkeleton"].Value != null)
			{
				base.Properties["TargetType"].Value = GetTargetType(new Urn(base.Properties["TargetTypeSkeleton"].Value as string).XPathExpression.ExpressionSkeleton);
			}
			return (string)base.Properties["TargetType"].Value;
		}
	}

	[SfcProperty(SfcPropertyFlags.Computed)]
	public Type Type
	{
		get
		{
			if (null == m_type)
			{
				m_type = Utils.GetTypeFromUrnSkeleton(TargetTypeSkeleton);
			}
			return m_type;
		}
	}

	static TargetSet()
	{
		traceContext = TraceContext.GetTraceContext("DMF", "TargetSet");
		DatabaseLevel = "Server/Database";
		scriptCreateAction = new SfcTsqlProcFormatter();
		scriptDropAction = new SfcTsqlProcFormatter();
		scriptAlterAction = new SfcTsqlProcFormatter();
		scriptCreateReferenceAction = new SfcTsqlProcFormatter();
		scriptAlterReferenceAction = new SfcTsqlProcFormatter();
		smoDatabaseUrnSkeleton = null;
		scriptCreateAction.Procedure = "msdb.dbo.sp_syspolicy_add_target_set";
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("object_set_name", required: true));
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("type_skeleton", "TargetTypeSkeleton", required: true, output: false));
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("type", "TargetType", required: true, output: false));
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("enabled", "Enabled", required: true, output: false));
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("target_set_id", "ID", required: false, output: true));
		scriptCreateReferenceAction.Procedure = "msdb.dbo.sp_syspolicy_add_target_set_level";
		scriptCreateReferenceAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("target_set_id", required: true));
		scriptCreateReferenceAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("type_skeleton", "TargetTypeSkeleton", required: true, output: false));
		scriptCreateReferenceAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("level_name", "LevelName", required: true, output: false));
		scriptCreateReferenceAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("condition_name", "Condition", required: true, output: false));
		scriptCreateReferenceAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("target_set_level_id", required: true));
		scriptAlterAction.Procedure = "msdb.dbo.sp_syspolicy_update_target_set";
		scriptAlterAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("target_set_id", "ID", required: true, output: false));
		scriptAlterAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("enabled", "Enabled", required: true, output: false));
		scriptAlterReferenceAction.Procedure = "msdb.dbo.sp_syspolicy_update_target_set_level";
		scriptAlterReferenceAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("target_set_id", required: true));
		scriptAlterReferenceAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("type_skeleton", "TargetTypeSkeleton", required: true, output: false));
		scriptAlterReferenceAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("condition_name", "Condition", required: true, output: false));
		scriptDropAction.Procedure = "msdb.dbo.sp_syspolicy_delete_target_set";
		scriptDropAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("target_set_id", "ID", required: true, output: false));
		smoDatabaseUrnSkeleton = "Server/Database";
	}

	internal static SfcTypeMetadata GetTypeMetadata()
	{
		return TypeMetadata.Instance;
	}

	internal ISfcScript ScriptCreate(bool declareArguments)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("ScriptCreate");
		methodTraceContext.TraceParameters(declareArguments);
		SfcTSqlScript sfcTSqlScript = new SfcTSqlScript();
		List<SfcTsqlProcFormatter.RuntimeArg> list = new List<SfcTsqlProcFormatter.RuntimeArg>();
		list.Add(new SfcTsqlProcFormatter.RuntimeArg(Parent.Name.GetType(), Parent.Name));
		List<SfcTsqlProcFormatter.RuntimeArg> list2 = new List<SfcTsqlProcFormatter.RuntimeArg>();
		list2.Add(new SfcTsqlProcFormatter.RuntimeArg(typeof(int), "@target_set_id"));
		list2.Add(new SfcTsqlProcFormatter.RuntimeArg(typeof(int), 0));
		if (base.Properties["TargetTypeSkeleton"].Value != null)
		{
			base.Properties["TargetType"].Value = GetTargetType(new Urn(base.Properties["TargetTypeSkeleton"].Value as string).XPathExpression.ExpressionSkeleton);
		}
		string text = scriptCreateAction.GenerateScript(this, list, declareArguments);
		methodTraceContext.TraceVerbose("Script generated: " + text);
		sfcTSqlScript.AddBatch(text);
		foreach (TargetSetLevel level in Levels)
		{
			string text2 = scriptCreateReferenceAction.GenerateScript(level, list2, declareArguments);
			methodTraceContext.TraceVerbose("Script generated: " + text2);
			sfcTSqlScript.AddBatch(text2);
		}
		methodTraceContext.TraceParameterOut("returnVal", sfcTSqlScript);
		return sfcTSqlScript;
	}

	internal ISfcScript ScriptDrop()
	{
		return new SfcTSqlScript(scriptDropAction.GenerateScript(this));
	}

	internal ISfcScript ScriptAlter()
	{
		SfcTSqlScript sfcTSqlScript = new SfcTSqlScript("");
		sfcTSqlScript.AddBatch(scriptAlterAction.GenerateScript(this));
		List<SfcTsqlProcFormatter.RuntimeArg> list = new List<SfcTsqlProcFormatter.RuntimeArg>();
		list.Add(new SfcTsqlProcFormatter.RuntimeArg(typeof(int), ID));
		foreach (TargetSetLevel level in Levels)
		{
			string batch = scriptAlterReferenceAction.GenerateScript(level, list);
			sfcTSqlScript.AddBatch(batch);
		}
		return sfcTSqlScript;
	}

	public TargetSet()
	{
	}

	public TargetSet(ObjectSet parent, string targetTypeSkeleton)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("TargetSet", TraceEventType.Information);
		methodTraceContext.TraceParameters(parent, targetTypeSkeleton);
		m_urn = new Urn(targetTypeSkeleton);
		if (!m_urn.IsValidUrnSkeleton())
		{
			throw methodTraceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.InvalidUrnSkeleton(targetTypeSkeleton)));
		}
		Parent = parent;
		base.Properties["TargetTypeSkeleton"].Value = targetTypeSkeleton;
		rootLevel = m_urn.XPathExpression.BlockExpressionSkeleton(0);
		for (int i = 1; i < m_urn.XPathExpression.Length; i++)
		{
			Levels.Add(new TargetSetLevel(this, m_urn.XPathExpression.BlockExpressionSkeleton(i)));
		}
		base.Properties["TargetType"].Value = GetTargetType(m_urn.XPathExpression.ExpressionSkeleton);
	}

	private string GetTargetType(string skeleton)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("GetTargetType");
		methodTraceContext.TraceParameters(skeleton);
		Urn urn = new Urn(skeleton);
		string text = urn.Type.ToLowerInvariant() switch
		{
			"userdefinedtype" => "TYPE", 
			"userdefineddatatype" => "TYPE", 
			"userdefinedfunction" => "FUNCTION", 
			"storedprocedure" => "PROCEDURE", 
			"applicationrole" => "APPLICATION ROLE", 
			"role" => urn.Parent.Type.ToLowerInvariant().Equals("database") ? "ROLE" : "SERVER ROLE", 
			_ => urn.Type.ToUpperInvariant(), 
		};
		methodTraceContext.TraceParameterOut("returnVal", text);
		return text;
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	public override void Discover(ISfcDependencyDiscoveryObjectSink sink)
	{
		if (sink == null)
		{
			throw new ArgumentNullException("sink");
		}
		SfcDependencyAction action = sink.Action;
		if ((uint)(action - 1) <= 1u)
		{
			TargetSetLevelCollection levels = Levels;
			if (levels != null)
			{
				sink.Add(SfcDependencyDirection.Inbound, levels.GetEnumerator(), SfcTypeRelation.RequiredChild, discovered: false);
			}
		}
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	public static SfcObjectFactory GetObjectFactory()
	{
		return ObjectFactory.Instance;
	}

	protected override SfcKey CreateIdentityKey()
	{
		Key result = null;
		if (TargetTypeSkeleton != null)
		{
			result = new Key(TargetTypeSkeleton);
		}
		return result;
	}

	protected override ISfcCollection GetChildCollection(string elementType)
	{
		if (elementType == "TargetSetLevel")
		{
			return Levels;
		}
		throw traceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.NoSuchCollection(elementType)));
	}

	public string GetFilter()
	{
		StringBuilder stringBuilder = new StringBuilder(RootLevel);
		foreach (TargetSetLevel item in GetLevelsSorted())
		{
			stringBuilder.Append("/");
			stringBuilder.Append(item.LevelName);
			if (!string.IsNullOrEmpty(item.Condition))
			{
				stringBuilder.Append("[");
				stringBuilder.Append(Parent.Parent.Conditions[item.Condition].ExpressionNode.ToStringForUrn());
				stringBuilder.Append("]");
			}
		}
		return stringBuilder.ToString();
	}

	internal string GetFilterWithNodeReplacement(Dictionary<string, ExpressionNode> adjustments)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("GetFilterWithNodeReplacement");
		methodTraceContext.TraceParameters(adjustments);
		if (adjustments == null || adjustments.Count == 0)
		{
			return GetFilter();
		}
		StringBuilder stringBuilder = new StringBuilder(RootLevel);
		foreach (TargetSetLevel item in GetLevelsSorted())
		{
			stringBuilder.Append("/");
			stringBuilder.Append(item.LevelName);
			if (adjustments.ContainsKey(item.TargetTypeSkeleton))
			{
				stringBuilder.Append("[");
				stringBuilder.Append(adjustments[item.TargetTypeSkeleton].ToStringForUrn());
				stringBuilder.Append("]");
			}
			else if (!string.IsNullOrEmpty(item.Condition))
			{
				stringBuilder.Append("[");
				stringBuilder.Append(Parent.Parent.Conditions[item.Condition].ExpressionNode.ToStringForUrn());
				stringBuilder.Append("]");
			}
		}
		return stringBuilder.ToString();
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	public IList<TargetSetLevel> GetLevelsSorted()
	{
		SortedList<TargetSetLevel, object> sortedList = new SortedList<TargetSetLevel, object>();
		foreach (TargetSetLevel level in Levels)
		{
			sortedList.Add(level, null);
		}
		return sortedList.Keys;
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	public TargetSetLevel GetLevel(string skeleton)
	{
		if (Levels.Contains(skeleton))
		{
			return Levels[skeleton];
		}
		return null;
	}

	public TargetSetLevel SetLevelCondition(TargetSetLevel level, string condition)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("SetLevelCondition", TraceEventType.Information);
		methodTraceContext.TraceParameters(level, condition);
		Levels[level.TargetTypeSkeleton].Condition = condition;
		methodTraceContext.TraceParameterOut("returnVal", Levels[level.TargetTypeSkeleton]);
		return Levels[level.TargetTypeSkeleton];
	}

	internal bool IsEventingFilter()
	{
		foreach (TargetSetLevel level in Levels)
		{
			if (!string.IsNullOrEmpty(level.Condition))
			{
				if (!(level.TargetTypeSkeleton == DatabaseLevel))
				{
					return false;
				}
				if (Parent.Parent.Conditions[level.Condition].NameConditionType == NameConditionType.None)
				{
					return false;
				}
			}
		}
		return true;
	}

	internal TargetSet Clone(ObjectSet parentObjectSet)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("Clone");
		methodTraceContext.TraceParameters(parentObjectSet);
		traceContext.DebugAssert(parentObjectSet != null);
		TargetSet targetSet = new TargetSet(parentObjectSet, TargetTypeSkeleton);
		foreach (TargetSetLevel level in Levels)
		{
			if (!string.IsNullOrEmpty(level.Condition))
			{
				targetSet.SetLevelCondition(targetSet.Levels[level.TargetTypeSkeleton], level.Condition);
			}
		}
		Utils.ReplaceSfcProperties(targetSet, this);
		methodTraceContext.TraceParameterOut("returnVal", targetSet);
		return targetSet;
	}
}
