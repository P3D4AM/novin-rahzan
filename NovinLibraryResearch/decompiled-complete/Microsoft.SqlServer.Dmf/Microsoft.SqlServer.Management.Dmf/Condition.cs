using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Xml;
using Microsoft.SqlServer.Diagnostics.STrace;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Facets;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Server;

namespace Microsoft.SqlServer.Management.Dmf;

public sealed class Condition : SfcInstance, ISfcCreatable, ICreatable, ISfcDroppable, IDroppable, ISfcAlterable, IAlterable, ISfcRenamable, IRenamable, ISfcValidate
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
			if (other == null)
			{
				throw traceContext.TraceThrow(new ArgumentNullException("other"));
			}
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
			return string.Format("{0}[@Name='{1}']", "Condition", SfcSecureString.EscapeSquote(Name));
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
			return new Condition();
		}
	}

	internal delegate void ConditionEvaluationEventHandler(Condition sender, ConditionEvaluationEventArgs args);

	private static TraceContext traceContext;

	private static readonly SfcTsqlProcFormatter scriptDropAction;

	private static readonly SfcTsqlProcFormatter scriptCreateAction;

	private static readonly SfcTsqlProcFormatter scriptAlterAction;

	private static readonly SfcTsqlProcFormatter scriptRenameAction;

	private ExpressionNode expressionNode;

	private bool m_bStatesInitialized;

	internal const string typeName = "Condition";

	private string cachedFacet;

	private Hashtable facetProperties;

	private string validatedFacet = string.Empty;

	[SfcIgnore]
	public ExpressionNode ExpressionNode
	{
		get
		{
			if (expressionNode == null)
			{
				string expression = Expression;
				if (!string.IsNullOrEmpty(expression))
				{
					try
					{
						expressionNode = ExpressionNode.Deserialize(expression);
					}
					catch (XmlException innerException)
					{
						throw traceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.InvalidOrMissingXMLReader, innerException));
					}
				}
			}
			return expressionNode;
		}
		set
		{
			traceContext.TraceVerbose("Setting ExpressionNode to: {0}", value);
			expressionNode = value;
			if (expressionNode == null)
			{
				base.Properties["Expression"].Value = null;
			}
			else
			{
				base.Properties["Expression"].Value = ExpressionNode.SerializeNode(expressionNode);
			}
		}
	}

	[SfcProperty(SfcPropertyFlags.Computed)]
	public bool IsEnumerable
	{
		get
		{
			if (ExpressionNode != null)
			{
				return ExpressionNode.FilterNodeCompatible;
			}
			return false;
		}
	}

	[SfcProperty(SfcPropertyFlags.Required)]
	internal string Expression
	{
		get
		{
			return base.Properties["Expression"].Value as string;
		}
		set
		{
			traceContext.TraceVerbose("Setting Expression to: {0}", value);
			base.Properties["Expression"].Value = value;
		}
	}

	public bool HasScript
	{
		get
		{
			if (ExpressionNode == null)
			{
				return false;
			}
			return ExpressionNode.HasScript;
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

	[SfcProperty(SfcPropertyFlags.None)]
	public string Description
	{
		get
		{
			return (string)base.Properties["Description"].Value;
		}
		set
		{
			traceContext.TraceVerbose("Setting Description to: {0}", value);
			base.Properties["Description"].Value = value;
		}
	}

	[SfcProperty(SfcPropertyFlags.Data)]
	public DateTime CreateDate
	{
		get
		{
			object value = base.Properties["CreateDate"].Value;
			if (value == null)
			{
				return DateTime.MinValue;
			}
			return (DateTime)value;
		}
	}

	[SfcProperty(SfcPropertyFlags.Data)]
	public string CreatedBy => (string)base.Properties["CreatedBy"].Value;

	[SfcProperty(SfcPropertyFlags.Data)]
	public DateTime DateModified
	{
		get
		{
			object value = base.Properties["DateModified"].Value;
			if (value == null)
			{
				return DateTime.MinValue;
			}
			return (DateTime)value;
		}
	}

	[SfcProperty(SfcPropertyFlags.Data)]
	public string ModifiedBy => (string)base.Properties["ModifiedBy"].Value;

	[SfcProperty(SfcPropertyFlags.Required)]
	public string Facet
	{
		get
		{
			if (cachedFacet == null)
			{
				cachedFacet = (string)base.Properties["Facet"].Value;
			}
			return (string)base.Properties["Facet"].Value;
		}
		set
		{
			traceContext.TraceVerbose("Setting Facet to: {0}", value);
			if (cachedFacet == null)
			{
				cachedFacet = (string)base.Properties["Facet"].Value;
				if (string.IsNullOrEmpty(cachedFacet))
				{
					cachedFacet = value;
				}
			}
			base.Properties["Facet"].Value = value;
		}
	}

	internal NameConditionType NameConditionType
	{
		get
		{
			if (ExpressionNode == null)
			{
				return NameConditionType.None;
			}
			return ExpressionNode.NameConditionType;
		}
	}

	internal string ObjectName
	{
		get
		{
			if (ExpressionNode == null)
			{
				return null;
			}
			return ExpressionNode.ObjectName;
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

	internal bool IsConfigurable
	{
		get
		{
			if (string.IsNullOrEmpty(Facet) || ExpressionNode == null)
			{
				return false;
			}
			List<ConfigurationItem> list = new List<ConfigurationItem>();
			try
			{
				ExpressionNode.AnalyzeForConfiguration(list);
				Type facetType = FacetRepository.GetFacetType(Facet);
				foreach (ConfigurationItem item in list)
				{
					if (!FacetRepository.IsPropertyConfigurable(facetType, item.Property))
					{
						return false;
					}
				}
				return true;
			}
			catch (ExpressionNodeNotConfigurableException)
			{
				return false;
			}
			catch (ExpressionNodeNotConfigurableOperatorException)
			{
				return false;
			}
		}
	}

	internal event ConditionEvaluationEventHandler EvaluateCondition;

	static Condition()
	{
		traceContext = TraceContext.GetTraceContext("DMF", "Condition");
		scriptDropAction = new SfcTsqlProcFormatter();
		scriptCreateAction = new SfcTsqlProcFormatter();
		scriptAlterAction = new SfcTsqlProcFormatter();
		scriptRenameAction = new SfcTsqlProcFormatter();
		scriptCreateAction.Procedure = "msdb.dbo.sp_syspolicy_add_condition";
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("name", "Name", required: true, output: false));
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("description", "Description", required: false, output: false));
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("facet", "Facet", required: true, output: false));
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("expression", "Expression", required: true, output: false));
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("is_name_condition", required: true));
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("obj_name", required: true));
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("condition_id", "ID", required: false, output: true));
		scriptAlterAction.Procedure = "msdb.dbo.sp_syspolicy_update_condition";
		scriptAlterAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("condition_id", "ID", required: true, output: false));
		scriptAlterAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("description", "Description", required: false, output: false));
		scriptAlterAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("facet", "Facet", required: false, output: false));
		scriptAlterAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("expression", "Expression", required: false, output: false));
		scriptAlterAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("is_name_condition", required: true));
		scriptAlterAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("obj_name", required: true));
		scriptDropAction.Procedure = "msdb.dbo.sp_syspolicy_delete_condition";
		scriptDropAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("condition_id", "ID", required: true, output: false));
		scriptRenameAction.Procedure = "msdb.dbo.sp_syspolicy_rename_condition";
		scriptRenameAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("name", "Name", required: true, output: false));
		scriptRenameAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("new_name", required: true));
	}

	public Condition()
	{
		Description = string.Empty;
	}

	public Condition(PolicyStore parent, string name)
	{
		traceContext.TraceMethodEnter("Condition");
		SetName(name);
		Parent = parent;
		Description = string.Empty;
		traceContext.TraceMethodExit("Condition");
	}

	public AutomatedPolicyEvaluationMode GetSupportedEvaluationMode()
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("GetSupportedEvaluationMode", TraceEventType.Information);
		AutomatedPolicyEvaluationMode automatedPolicyEvaluationMode = AutomatedPolicyEvaluationMode.None;
		if (!string.IsNullOrEmpty(Facet))
		{
			Type facetType = FacetRepository.GetFacetType(Facet);
			automatedPolicyEvaluationMode |= FacetRepository.GetFacetEvaluationMode(facetType);
			if (HasScript)
			{
				automatedPolicyEvaluationMode &= AutomatedPolicyEvaluationMode.CheckOnSchedule;
			}
		}
		methodTraceContext.TraceParameterOut("returnVal", automatedPolicyEvaluationMode);
		return automatedPolicyEvaluationMode;
	}

	public AutomatedPolicyEvaluationMode GetSupportedEvaluationModeOnDomain(string domain)
	{
		if (string.Equals(domain, "Utility", StringComparison.Ordinal))
		{
			return AutomatedPolicyEvaluationMode.CheckOnSchedule;
		}
		return GetSupportedEvaluationMode();
	}

	protected override void InitializeUIPropertyState()
	{
		if (!m_bStatesInitialized)
		{
			m_bStatesInitialized = true;
			UpdateUIPropertyState();
		}
	}

	protected override void UpdateUIPropertyState()
	{
		if (m_bStatesInitialized)
		{
			SfcProperty sfcProperty = base.Properties["Name"];
			if (base.State == SfcObjectState.Pending)
			{
				sfcProperty.Enabled = true;
			}
			else
			{
				sfcProperty.Enabled = false;
			}
		}
	}

	public void Create()
	{
		traceContext.TraceMethodEnter("Create");
		Validate(ValidationMethod.Create);
		CreateImpl();
		traceContext.TraceMethodExit("Create");
	}

	public ISfcScript ScriptCreate()
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("ScriptCreate", TraceEventType.Information);
		List<SfcTsqlProcFormatter.RuntimeArg> list = new List<SfcTsqlProcFormatter.RuntimeArg>(2);
		list.Add(new SfcTsqlProcFormatter.RuntimeArg(typeof(short), (short)NameConditionType));
		list.Add(new SfcTsqlProcFormatter.RuntimeArg(typeof(string), ObjectName));
		string text = scriptCreateAction.GenerateScript(this, list);
		methodTraceContext.TraceVerbose("Script generated: " + text);
		return new SfcTSqlScript(text);
	}

	protected override void PostCreate(object executionResult)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("PostCreate");
		methodTraceContext.TraceParameters(executionResult);
		if (GetDomain().ConnectionContext.Mode != 0)
		{
			base.Properties["ID"].Value = executionResult;
		}
	}

	public void Alter()
	{
		traceContext.TraceMethodEnter("Alter");
		Validate(ValidationMethod.Alter);
		AlterImpl();
		traceContext.TraceMethodExit("Alter");
	}

	internal void AlterNoValidation()
	{
		AlterImpl();
	}

	public ISfcScript ScriptAlter()
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("ScriptAlter", TraceEventType.Information);
		List<SfcTsqlProcFormatter.RuntimeArg> list = new List<SfcTsqlProcFormatter.RuntimeArg>(2);
		list.Add(new SfcTsqlProcFormatter.RuntimeArg(typeof(short), (short)NameConditionType));
		list.Add(new SfcTsqlProcFormatter.RuntimeArg(typeof(string), ObjectName));
		string text = scriptAlterAction.GenerateScript(this, list);
		methodTraceContext.TraceVerbose("Script generated: " + text);
		return new SfcTSqlScript(text);
	}

	public void Drop()
	{
		DropImpl();
	}

	public ISfcScript ScriptDrop()
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("ScriptDrop", TraceEventType.Information);
		string text = scriptDropAction.GenerateScript(this);
		methodTraceContext.TraceVerbose("Script generated: " + text);
		return new SfcTSqlScript(text);
	}

	public void Rename(string name)
	{
		traceContext.TraceMethodEnter("Rename");
		traceContext.TraceParameters(name);
		Validate(ValidationMethod.Rename);
		RenameImpl(new Key(name));
		traceContext.TraceMethodExit("Rename");
	}

	void ISfcRenamable.Rename(SfcKey key)
	{
		RenameImpl(key);
	}

	ISfcScript ISfcRenamable.ScriptRename(SfcKey key)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("ISfcRenamable.ScriptRename");
		methodTraceContext.TraceParameters(key);
		List<SfcTsqlProcFormatter.RuntimeArg> list = new List<SfcTsqlProcFormatter.RuntimeArg>();
		Key key2 = key as Key;
		list.Add(new SfcTsqlProcFormatter.RuntimeArg(key2.Name.GetType(), key2.Name));
		string text = scriptRenameAction.GenerateScript(this, list);
		methodTraceContext.TraceVerbose("Script generated: " + text);
		return new SfcTSqlScript(text);
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	public override void Discover(ISfcDependencyDiscoveryObjectSink sink)
	{
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	public ReadOnlyCollection<Policy> EnumDependentPolicies()
	{
		List<Policy> list = new List<Policy>();
		foreach (Policy policy in Parent.Policies)
		{
			if (policy.Condition == Name)
			{
				list.Add(policy);
			}
		}
		return new ReadOnlyCollection<Policy>(list);
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

	[STraceConfiguration(SkipAutoTrace = true)]
	protected override ISfcCollection GetChildCollection(string elementType)
	{
		throw traceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.NoSuchCollection(elementType)));
	}

	internal bool CanBeConfigured(ref string message)
	{
		traceContext.TraceMethodEnter("CanBeConfigured");
		traceContext.TraceParameters(message);
		if (string.IsNullOrEmpty(Facet) || ExpressionNode == null)
		{
			traceContext.TraceParameterOut("returnVal", false);
			return false;
		}
		List<ConfigurationItem> list = new List<ConfigurationItem>();
		try
		{
			ExpressionNode.AnalyzeForConfiguration(list);
			Type facetType = FacetRepository.GetFacetType(Facet);
			foreach (ConfigurationItem item in list)
			{
				if (!FacetRepository.IsPropertyConfigurable(facetType, item.Property))
				{
					message = ExceptionTemplatesSR.PropertyCannotBeSet(item.Property);
					traceContext.TraceParameterOut("returnVal", false);
					return false;
				}
			}
			traceContext.TraceParameterOut("returnVal", true);
			return true;
		}
		catch (ExpressionNodeNotConfigurableException ex)
		{
			traceContext.TraceCatch(ex);
			message = ex.Message;
			traceContext.TraceParameterOut("returnVal", false);
			return false;
		}
		catch (ExpressionNodeNotConfigurableOperatorException ex2)
		{
			traceContext.TraceCatch(ex2);
			message = ex2.Message;
			traceContext.TraceParameterOut("returnVal", false);
			return false;
		}
	}

	internal void Configure(object target)
	{
		traceContext.TraceMethodEnter("Configure");
		traceContext.TraceParameters(target);
		FacetEvaluationContext facetEvaluationContext = FacetEvaluationContext.GetFacetEvaluationContext(Facet, target);
		List<ConfigurationItem> list = new List<ConfigurationItem>();
		ExpressionNode.AnalyzeForConfiguration(list);
		foreach (ConfigurationItem item in list)
		{
			facetEvaluationContext.SetPropertyValue(item.Property, item.DesiredValue);
		}
		facetEvaluationContext.Alter();
		facetEvaluationContext.Refresh();
		traceContext.TraceMethodExit("Configure");
	}

	internal string ProduceConfigureScript(object target)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("ProduceConfigureScript");
		methodTraceContext.TraceParameters(target);
		string empty = string.Empty;
		if (!(target is SqlSmoObject))
		{
			return "/*  " + ExceptionTemplatesSR.CannotProduceScript + "  */";
		}
		empty = "/*  Scripting: '" + SfcSqlPathUtilities.ConvertUrnToPath(((SqlSmoObject)target).Urn) + "'  */ ";
		FacetEvaluationContext facetEvaluationContext = FacetEvaluationContext.GetFacetEvaluationContext(Facet, target);
		if ((bool)ExpressionNode.Evaluate(facetEvaluationContext))
		{
			return empty + "\n/*  " + ExceptionTemplatesSR.NoConfigureScriptForCompliantObject + "  */";
		}
		List<ConfigurationItem> list = new List<ConfigurationItem>();
		ExpressionNode.AnalyzeForConfiguration(list);
		foreach (ConfigurationItem item in list)
		{
			facetEvaluationContext.SetPropertyValue(item.Property, item.DesiredValue);
		}
		StringBuilder stringBuilder = new StringBuilder();
		ServerConnection connectionContext = ((SqlSmoObject)target).GetServerObject().ConnectionContext;
		SqlExecutionModes sqlExecutionModes = connectionContext.SqlExecutionModes;
		connectionContext.SqlExecutionModes = SqlExecutionModes.CaptureSql;
		try
		{
			stringBuilder.AppendLine(empty);
			facetEvaluationContext.Alter();
			StringEnumerator enumerator2 = connectionContext.CapturedSql.Text.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					string current2 = enumerator2.Current;
					stringBuilder.AppendLine(current2);
				}
			}
			finally
			{
				if (enumerator2 is IDisposable disposable)
				{
					disposable.Dispose();
				}
			}
		}
		finally
		{
			connectionContext.SqlExecutionModes = sqlExecutionModes;
			connectionContext.CapturedSql.Clear();
			facetEvaluationContext.Refresh();
		}
		return stringBuilder.ToString();
	}

	internal bool Evaluate(object target, AdHocPolicyEvaluationMode evaluationMode)
	{
		FacetEvaluationContext facetEvaluationContext = null;
		Exception ex = null;
		bool flag = false;
		bool isConfigurable = false;
		string message = string.Empty;
		try
		{
			facetEvaluationContext = FacetEvaluationContext.GetFacetEvaluationContext(Facet, target);
			flag = (bool)ExpressionNode.Evaluate(facetEvaluationContext, evaluationMode == AdHocPolicyEvaluationMode.CheckSqlScriptAsProxy);
		}
		catch (Exception ex2)
		{
			traceContext.TraceCatch(ex2);
			ex = ex2;
		}
		if (this.EvaluateCondition != null)
		{
			if (!flag && !SqlContext.IsAvailable)
			{
				isConfigurable = CanBeConfigured(ref message);
			}
			this.EvaluateCondition(this, new ConditionEvaluationEventArgs(facetEvaluationContext, Facet, target, flag, isConfigurable, message, ex));
		}
		if (ex != null)
		{
			throw traceContext.TraceThrow(ex);
		}
		return flag;
	}

	internal static List<SfcInstanceSerializedData> UpgradeInstance(List<SfcInstanceSerializedData> sfcInstanceData, int fileVersion)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("UpgradeInstance");
		methodTraceContext.TraceParameters(sfcInstanceData, fileVersion);
		List<SfcInstanceSerializedData> list = new List<SfcInstanceSerializedData>(sfcInstanceData);
		if (fileVersion < 3)
		{
			foreach (SfcInstanceSerializedData sfcInstanceDatum in sfcInstanceData)
			{
				if (sfcInstanceDatum.Name == "IsNameCondition" || sfcInstanceDatum.Name == "ObjectName")
				{
					list.Remove(sfcInstanceDatum);
				}
			}
		}
		methodTraceContext.TraceParameterOut("returnVal", list);
		return list;
	}

	private void RefreshFacetProperties()
	{
		if (!string.IsNullOrEmpty(Facet) && Facet != validatedFacet)
		{
			validatedFacet = Facet;
			if (facetProperties == null)
			{
				facetProperties = new Hashtable();
			}
			else
			{
				facetProperties.Clear();
			}
			PropertyInfo[] array = FacetRepository.GetFacetProperties(FacetRepository.GetFacetType(Facet));
			foreach (PropertyInfo propertyInfo in array)
			{
				facetProperties.Add(propertyInfo.Name, null);
			}
		}
	}

	private void Validate(string validationMode, bool throwOnFirst, ValidationState validationState)
	{
		ValidateProperties(validationMode, throwOnFirst, validationState);
		ValidateExpression(validationMode, throwOnFirst, validationState);
		ValidateReferences(validationMode, throwOnFirst, validationState);
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
				throw methodTraceContext.TraceThrow(new ObjectValidationException(ExceptionTemplatesSR.Condition, Name, ex2));
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
		if (ExpressionNode == null)
		{
			Exception ex4 = new SfcPropertyNotSetException("ExpressionNode");
			if (throwOnFirst)
			{
				throw methodTraceContext.TraceThrow(new ObjectValidationException(ExceptionTemplatesSR.Condition, Name, ex4));
			}
			validationState.AddError(ex4, "ExpressionNode");
		}
	}

	private void ValidateExpression(string validationMode, bool throwOnFirst, ValidationState validationState)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("ValidateExpression");
		methodTraceContext.TraceParameters(validationMode, throwOnFirst, validationState);
		if (ExpressionNode == null || string.IsNullOrEmpty(Facet))
		{
			return;
		}
		RefreshFacetProperties();
		foreach (string item in ExpressionNode.EnumAttributes())
		{
			traceContext.DebugAssert(facetProperties != null);
			if (!facetProperties.Contains(item))
			{
				Exception ex = new ConflictingPropertyValuesException(validationMode, ExceptionTemplatesSR.ManagementFacet, Facet, ExceptionTemplatesSR.Property, item);
				if (throwOnFirst)
				{
					throw methodTraceContext.TraceThrow(new ObjectValidationException(ExceptionTemplatesSR.Condition, Name, ex));
				}
				validationState.AddError(ex, "ExpressionNode");
			}
		}
	}

	private void ValidateReferences(string validationMode, bool throwOnFirst, ValidationState validationState)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("ValidateReferences");
		methodTraceContext.TraceParameters(validationMode, throwOnFirst, validationState);
		if (!(validationMode == ValidationMethod.Alter))
		{
			return;
		}
		if (Facet != cachedFacet || HasScript)
		{
			foreach (Policy policy in Parent.Policies)
			{
				if (!policy.DependsOnCondition(Name))
				{
					continue;
				}
				if (Facet != cachedFacet)
				{
					Exception ex = new DmfException(ExceptionTemplatesSR.CannotChangeFacet);
					if (throwOnFirst)
					{
						throw methodTraceContext.TraceThrow(new ObjectValidationException(ExceptionTemplatesSR.Condition, Name, ex));
					}
					validationState.AddError(ex, "Facet");
					return;
				}
				if ((policy.AutomatedPolicyEvaluationMode == AutomatedPolicyEvaluationMode.CheckOnChanges || policy.AutomatedPolicyEvaluationMode == AutomatedPolicyEvaluationMode.Enforce) && HasScript)
				{
					Exception ex2 = new DmfException(ExceptionTemplatesSR.ReferencedConditionsCannotContainScript);
					if (throwOnFirst)
					{
						throw methodTraceContext.TraceThrow(new ObjectValidationException(ExceptionTemplatesSR.Condition, Name, ex2));
					}
					validationState.AddError(ex2, "Facet");
					return;
				}
			}
		}
		foreach (ObjectSet objectSet in Parent.ObjectSets)
		{
			if (!objectSet.EnumReferencedConditionNames().Contains(Name))
			{
				continue;
			}
			if (Facet != cachedFacet)
			{
				Exception ex3 = new DmfException(ExceptionTemplatesSR.CannotChangeFacet);
				if (throwOnFirst)
				{
					throw methodTraceContext.TraceThrow(new ObjectValidationException(ExceptionTemplatesSR.Condition, Name, ex3));
				}
				validationState.AddError(ex3, "Facet");
				break;
			}
			if (!IsEnumerable)
			{
				Exception ex4 = new DmfException(ExceptionTemplatesSR.ConditionCannotBeUsedForFiltering(Name));
				if (throwOnFirst)
				{
					throw methodTraceContext.TraceThrow(new ObjectValidationException(ExceptionTemplatesSR.Condition, Name, ex4));
				}
				validationState.AddError(ex4, "Facet");
				break;
			}
		}
	}

	public void Validate(string mode)
	{
		if (Parent == null)
		{
			throw new SfcMissingParentException();
		}
		Validate(mode, throwOnFirst: true, null);
	}

	ValidationState ISfcValidate.Validate(string validationMethod, params object[] arguments)
	{
		ValidationState validationState = new ValidationState();
		Validate(validationMethod, throwOnFirst: false, validationState);
		return validationState;
	}

	internal void ValidateDeserialized(Policy policy)
	{
		traceContext.TraceMethodEnter("ValidateDeserialized");
		traceContext.TraceParameters(policy);
		ValidateProperties(ValidationMethod.Alter, throwOnFirst: true, null);
		ValidateExpression(ValidationMethod.Alter, throwOnFirst: true, null);
		ValidateReferencesDeserialized(policy);
		traceContext.TraceMethodExit("ValidateDeserialized");
	}

	private void ValidateReferencesDeserialized(Policy policy)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("ValidateReferencesDeserialized");
		methodTraceContext.TraceParameters(policy);
		if (Facet != cachedFacet || HasScript)
		{
			foreach (Policy policy2 in Parent.Policies)
			{
				if (policy2.DependsOnCondition(Name))
				{
					if (Facet != cachedFacet && policy2 != policy)
					{
						Exception innerException = new DmfException(ExceptionTemplatesSR.CannotChangeFacet);
						throw methodTraceContext.TraceThrow(new ObjectValidationException(ExceptionTemplatesSR.Condition, Name, innerException));
					}
					if ((policy2.AutomatedPolicyEvaluationMode == AutomatedPolicyEvaluationMode.CheckOnChanges || policy2.AutomatedPolicyEvaluationMode == AutomatedPolicyEvaluationMode.Enforce) && HasScript)
					{
						Exception innerException2 = new DmfException(ExceptionTemplatesSR.ReferencedConditionsCannotContainScript);
						throw methodTraceContext.TraceThrow(new ObjectValidationException(ExceptionTemplatesSR.Condition, Name, innerException2));
					}
				}
			}
		}
		foreach (ObjectSet objectSet in Parent.ObjectSets)
		{
			if (!(objectSet.Name == policy.ObjectSet) && objectSet.EnumReferencedConditionNames().Contains(Name))
			{
				if (Facet != cachedFacet)
				{
					Exception innerException3 = new DmfException(ExceptionTemplatesSR.CannotChangeFacet);
					throw methodTraceContext.TraceThrow(new ObjectValidationException(ExceptionTemplatesSR.Condition, Name, innerException3));
				}
				if (!IsEnumerable)
				{
					Exception innerException4 = new DmfException(ExceptionTemplatesSR.ConditionCannotBeUsedForFiltering(Name));
					throw methodTraceContext.TraceThrow(new ObjectValidationException(ExceptionTemplatesSR.Condition, Name, innerException4));
				}
			}
		}
	}
}
