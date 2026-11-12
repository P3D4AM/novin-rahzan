using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Diagnostics.STrace;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Facets;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Agent;
using Microsoft.SqlServer.Server;

namespace Microsoft.SqlServer.Management.Dmf;

public sealed class Policy : SfcInstance, ISfcCreatable, ICreatable, ISfcDroppable, IDroppable, ISfcAlterable, IAlterable, ISfcRenamable, IRenamable, ISfcValidate
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
				throw new ArgumentNullException("other");
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
			return string.Format("{0}[@Name='{1}']", "Policy", SfcSecureString.EscapeSquote(Name));
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
			return new Policy();
		}
	}

	public delegate void PolicyEvaluationStartedEventHandler(object sender, EventArgs e);

	public delegate void ConnectionProcessingStartedEventHandler(object sender, ConnectionProcessingStartedEventArgs e);

	public sealed class ConnectionProcessingStartedEventArgs : EventArgs
	{
		private static TraceContext traceContext = TraceContext.GetTraceContext("DMF", "ConnectionProcessingStartedEventArgs");

		private ISfcConnection connection;

		public ISfcConnection Connection
		{
			get
			{
				return connection;
			}
			set
			{
				traceContext.TraceVerbose("Setting Connection to: {0}", value);
				connection = value;
			}
		}

		internal ConnectionProcessingStartedEventArgs(ISfcConnection connection)
		{
			this.connection = connection;
		}
	}

	public delegate void TargetProcessedEventHandler(object sender, TargetProcessedEventArgs e);

	public sealed class TargetProcessedEventArgs : EventArgs
	{
		private static TraceContext traceContext = TraceContext.GetTraceContext("DMF", "TargetProcessedEventArgs");

		private object targetObject;

		private string targetExpression;

		private bool result;

		private ExpressionNode expressionNode;

		private PolicyEvaluationException exception;

		private string serverName;

		private bool isConfigurable;

		private string configMessage;

		public object TargetObject => targetObject;

		public string TargetExpression
		{
			get
			{
				return targetExpression;
			}
			set
			{
				traceContext.TraceVerbose("Setting TargetExpression to: {0}", value);
				targetExpression = value;
			}
		}

		public bool Result
		{
			get
			{
				return result;
			}
			set
			{
				traceContext.TraceVerbose("Setting Result to: {0}", value);
				result = value;
			}
		}

		public ExpressionNode ExpressionNode
		{
			get
			{
				return expressionNode;
			}
			set
			{
				traceContext.TraceVerbose("Setting ExpressionNode to: {0}", value);
				expressionNode = value;
			}
		}

		public PolicyEvaluationException Exception
		{
			get
			{
				return exception;
			}
			set
			{
				traceContext.TraceVerbose("Setting Exception to: {0}", value);
				exception = value;
			}
		}

		public string ServerName
		{
			get
			{
				return serverName;
			}
			set
			{
				traceContext.TraceVerbose("Setting ServerName to: {0}", value);
				serverName = value;
			}
		}

		public bool IsConfigurable
		{
			get
			{
				return isConfigurable;
			}
			set
			{
				traceContext.TraceVerbose("Setting IsConfigurable to: {0}", value);
				isConfigurable = value;
			}
		}

		public string ConfigurationErrorMessage
		{
			get
			{
				return configMessage;
			}
			set
			{
				traceContext.TraceVerbose("Setting ConfigurationErrorMessage to: {0}", value);
				configMessage = value;
			}
		}

		internal TargetProcessedEventArgs(object targetObject, string targetExpression, bool result, bool isConfigurable, string configMessage, ExpressionNode expressionNode, PolicyEvaluationException exception, string serverName)
		{
			traceContext.TraceMethodEnter("TargetProcessedEventArgs");
			traceContext.TraceParameters(targetObject, targetExpression, result, isConfigurable, configMessage, expressionNode, exception, serverName);
			this.targetObject = targetObject;
			this.targetExpression = targetExpression;
			this.result = result;
			this.isConfigurable = isConfigurable;
			this.configMessage = configMessage;
			this.expressionNode = expressionNode;
			this.exception = exception;
			this.serverName = serverName;
			traceContext.TraceMethodExit("TargetProcessedEventArgs");
		}
	}

	public delegate void ConnectionProcessingFinishedEventHandler(object sender, ConnectionProcessingFinishedEventArgs e);

	public sealed class ConnectionProcessingFinishedEventArgs : EventArgs
	{
		private static TraceContext traceContext = TraceContext.GetTraceContext("DMF", "ConnectionProcessingFinishedEventArgs");

		private bool result;

		private PolicyEvaluationException exception;

		private ISfcConnection connection;

		private int targetsEvaluated;

		private bool rootCheckPassed;

		public bool Result
		{
			get
			{
				return result;
			}
			set
			{
				traceContext.TraceVerbose("Setting Result to: {0}", value);
				result = value;
			}
		}

		public PolicyEvaluationException Exception
		{
			get
			{
				return exception;
			}
			set
			{
				traceContext.TraceVerbose("Setting Exception to: {0}", value);
				exception = value;
			}
		}

		public ISfcConnection Connection
		{
			get
			{
				return connection;
			}
			set
			{
				traceContext.TraceVerbose("Setting Connection to: {0}", value);
				connection = value;
			}
		}

		public int TargetsEvaluated
		{
			get
			{
				return targetsEvaluated;
			}
			set
			{
				targetsEvaluated = value;
			}
		}

		public bool RootCheckPassed
		{
			get
			{
				return rootCheckPassed;
			}
			set
			{
				rootCheckPassed = value;
			}
		}

		internal ConnectionProcessingFinishedEventArgs(bool result, PolicyEvaluationException exception, ISfcConnection connection, int targetsEvaluated, bool rootCheckPassed)
		{
			traceContext.TraceMethodEnter("ConnectionProcessingFinishedEventArgs");
			traceContext.TraceParameters(result, exception, connection);
			this.result = result;
			this.exception = exception;
			this.connection = connection;
			this.targetsEvaluated = targetsEvaluated;
			this.rootCheckPassed = rootCheckPassed;
			traceContext.TraceMethodExit("ConnectionProcessingFinishedEventArgs");
		}

		internal ConnectionProcessingFinishedEventArgs(bool result, PolicyEvaluationException exception, ISfcConnection connection, int targetsEvaluated)
		{
			this.result = result;
			this.exception = exception;
			this.connection = connection;
			this.targetsEvaluated = targetsEvaluated;
			rootCheckPassed = true;
		}
	}

	public delegate void PolicyEvaluationFinishedEventHandler(object sender, PolicyEvaluationFinishedEventArgs e);

	public sealed class PolicyEvaluationFinishedEventArgs : EventArgs
	{
		private static TraceContext traceContext = TraceContext.GetTraceContext("DMF", "PolicyEvaluationFinishedEventArgs");

		private bool result;

		private PolicyEvaluationException exception;

		private EvaluationHistory history;

		public bool Result
		{
			get
			{
				return result;
			}
			set
			{
				traceContext.TraceVerbose("Setting Result to: {0}", value);
				result = value;
			}
		}

		public PolicyEvaluationException Exception
		{
			get
			{
				return exception;
			}
			set
			{
				traceContext.TraceVerbose("Setting Exception to: {0}", value);
				exception = value;
			}
		}

		public EvaluationHistory EvaluationHistory
		{
			get
			{
				return history;
			}
			set
			{
				traceContext.TraceVerbose("Setting EvaluationHistory to: {0}", value);
				history = value;
			}
		}

		public PolicyEvaluationFinishedEventArgs(bool result, PolicyEvaluationException exception)
		{
			traceContext.TraceMethodEnter("PolicyEvaluationFinishedEventArgs");
			traceContext.TraceParameters(result, exception);
			this.result = result;
			this.exception = exception;
			traceContext.TraceMethodExit("PolicyEvaluationFinishedEventArgs");
		}
	}

	private static TraceContext traceContext;

	private static readonly SfcTsqlProcFormatter scriptCreateAction;

	private static readonly SfcTsqlProcFormatter scriptAlterAction;

	private static readonly SfcTsqlProcFormatter scriptDropAction;

	private static readonly SfcTsqlProcFormatter scriptRenameAction;

	internal AutomatedPolicyEvaluationMode execModeInBackend;

	private bool execModeCacheSet;

	private const string EvaluationModePropertyName = "AutomatedPolicyEvaluationMode";

	private bool m_bStatesInitialized;

	private LogPolicyEvents logPolicyEvents;

	private bool raiseConditionEvaluationEvent = true;

	private bool raiseConditionEvaluationEventToWindowsLog = true;

	private const int messageNumberEnforceAutomatic = 34050;

	private const int messageNumberEnforceAdHoc = 34051;

	private const int messageNumberCheckOnChanges = 34053;

	private const int messageNumberCheckOnSchedule = 34052;

	internal const string typeName = "Policy";

	private EvaluationHistoryCollection evaluationHistories;

	private EvaluationHistory hiddenHistory;

	private ConnectionEvaluationHistory hiddenConnectionHistory;

	private int connectionCount = 1;

	private int detailCount = 1;

	public const int HelpTextStringMaxLength = 4000;

	public const int HelpLinkStringMaxLength = 2083;

	private const AutomatedPolicyEvaluationMode EventingModes = AutomatedPolicyEvaluationMode.Enforce | AutomatedPolicyEvaluationMode.CheckOnChanges;

	private bool IsLogAvailable => ID != 0;

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
			if (base.Properties["ID"].Value == null)
			{
				return 0;
			}
			return (int)base.Properties["ID"].Value;
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
	public DateTime CreateDate => (DateTime)base.Properties["CreateDate"].Value;

	[SfcProperty(SfcPropertyFlags.Required)]
	[SfcReference(typeof(Condition), "PolicyStore/Condition[@Name='{0}']", new string[] { "Condition" })]
	public string Condition
	{
		get
		{
			return (string)base.Properties["Condition"].Value;
		}
		set
		{
			traceContext.TraceVerbose("Setting Condition to: {0}", value);
			base.Properties["Condition"].Value = value;
		}
	}

	[SfcProperty(SfcPropertyFlags.None)]
	[SfcReference(typeof(ObjectSet), "PolicyStore/ObjectSet[@Name='{0}']", new string[] { "ObjectSet" })]
	public string ObjectSet
	{
		get
		{
			return (string)base.Properties["ObjectSet"].Value;
		}
		set
		{
			traceContext.TraceVerbose("Setting ObjectSet to: {0}", value);
			base.Properties["ObjectSet"].Value = value;
		}
	}

	[SfcProperty(SfcPropertyFlags.Required)]
	[SfcReference(typeof(Condition), "PolicyStore/Condition[@Name='{0}']", new string[] { "RootCondition" })]
	public string RootCondition
	{
		get
		{
			return (string)base.Properties["RootCondition"].Value;
		}
		set
		{
			traceContext.TraceVerbose("Setting RootCondition to: {0}", value);
			base.Properties["RootCondition"].Value = value;
		}
	}

	[SfcProperty(SfcPropertyFlags.None)]
	[SfcReference(typeof(PolicyCategory), "PolicyStore/PolicyCategory[@Name='{0}']", new string[] { "PolicyCategory" })]
	public string PolicyCategory
	{
		get
		{
			return (string)base.Properties["PolicyCategory"].Value;
		}
		set
		{
			traceContext.TraceVerbose("Setting PolicyCategory to: {0}", value);
			base.Properties["PolicyCategory"].Value = value;
		}
	}

	[SfcProperty(SfcPropertyFlags.Required)]
	public bool Enabled
	{
		get
		{
			object value = base.Properties["Enabled"].Value;
			if (value == null)
			{
				return false;
			}
			return (bool)value;
		}
		set
		{
			traceContext.TraceVerbose("Setting Enabled to: {0}", value);
			base.Properties["Enabled"].Value = value;
		}
	}

	[SfcProperty(SfcPropertyFlags.Required)]
	public AutomatedPolicyEvaluationMode AutomatedPolicyEvaluationMode
	{
		get
		{
			object value = base.Properties["AutomatedPolicyEvaluationMode"].Value;
			if (value == null)
			{
				return AutomatedPolicyEvaluationMode.None;
			}
			return (AutomatedPolicyEvaluationMode)value;
		}
		set
		{
			traceContext.TraceVerbose("Setting AutomatedPolicyEvaluationMode to: {0}", value);
			base.Properties["AutomatedPolicyEvaluationMode"].Value = value;
		}
	}

	[SfcProperty(SfcPropertyFlags.None)]
	public Guid ScheduleUid
	{
		get
		{
			object value = base.Properties["ScheduleUid"].Value;
			if (value == null)
			{
				return Guid.Empty;
			}
			return (Guid)value;
		}
		set
		{
			traceContext.TraceVerbose("Setting ScheduleUid to: {0}", value);
			base.Properties["ScheduleUid"].Value = value;
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
			if (value == null || value is DBNull)
			{
				return DateTime.MinValue;
			}
			return (DateTime)value;
		}
	}

	[SfcProperty(SfcPropertyFlags.Data)]
	public string ModifiedBy
	{
		get
		{
			object value = base.Properties["ModifiedBy"].Value;
			if (value == null)
			{
				return string.Empty;
			}
			return (string)value;
		}
	}

	[SfcProperty(SfcPropertyFlags.Data)]
	public int CategoryId
	{
		get
		{
			object value = base.Properties["CategoryId"].Value;
			if (value == null)
			{
				return 0;
			}
			return (int)value;
		}
	}

	[SfcProperty(SfcPropertyFlags.Required)]
	public string HelpText
	{
		get
		{
			return (string)base.Properties["HelpText"].Value;
		}
		set
		{
			traceContext.TraceVerbose("Setting HelpText to: {0}", value);
			base.Properties["HelpText"].Value = value;
		}
	}

	[SfcProperty(SfcPropertyFlags.Required)]
	public string HelpLink
	{
		get
		{
			return (string)base.Properties["HelpLink"].Value;
		}
		set
		{
			traceContext.TraceVerbose("Setting HelpLink to: {0}", value);
			base.Properties["HelpLink"].Value = value;
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

	[SfcObject(SfcContainerRelationship.ChildContainer, SfcContainerCardinality.ZeroToAny, typeof(EvaluationHistory))]
	[SfcNonSerializable]
	public EvaluationHistoryCollection EvaluationHistories
	{
		get
		{
			if (evaluationHistories == null)
			{
				evaluationHistories = new EvaluationHistoryCollection(this);
			}
			return evaluationHistories;
		}
	}

	public bool HasScript
	{
		get
		{
			bool result = false;
			if (Parent.Conditions[Condition].HasScript || (!string.IsNullOrEmpty(RootCondition) && Parent.Conditions[RootCondition].HasScript) || (!string.IsNullOrEmpty(ObjectSet) && Parent.ObjectSets[ObjectSet].HasScript))
			{
				result = true;
			}
			return result;
		}
	}

	[SfcProperty(SfcPropertyFlags.None)]
	internal DateTime ActiveEndDate
	{
		get
		{
			return (DateTime)base.Properties["ActiveEndDate"].Value;
		}
		set
		{
			traceContext.TraceVerbose("Setting ActiveEndDate to: {0}", value);
			base.Properties["ActiveEndDate"].Value = value;
		}
	}

	[SfcProperty(SfcPropertyFlags.None)]
	internal long ActiveEndTimeOfDay
	{
		get
		{
			return (long)base.Properties["ActiveEndTimeOfDay"].Value;
		}
		set
		{
			traceContext.TraceVerbose("Setting ActiveEndTimeOfDay to: {0}", value);
			base.Properties["ActiveEndTimeOfDay"].Value = value;
		}
	}

	[SfcProperty(SfcPropertyFlags.None)]
	internal DateTime ActiveStartDate
	{
		get
		{
			return (DateTime)base.Properties["ActiveStartDate"].Value;
		}
		set
		{
			traceContext.TraceVerbose("Setting ActiveStartDate to: {0}", value);
			base.Properties["ActiveStartDate"].Value = value;
		}
	}

	[SfcProperty(SfcPropertyFlags.None)]
	internal long ActiveStartTimeOfDay
	{
		get
		{
			return (long)base.Properties["ActiveStartTimeOfDay"].Value;
		}
		set
		{
			traceContext.TraceVerbose("Setting ActiveStartTimeOfDay to: {0}", value);
			base.Properties["ActiveStartTimeOfDay"].Value = value;
		}
	}

	[SfcProperty(SfcPropertyFlags.None)]
	internal int FrequencyInterval
	{
		get
		{
			return (int)base.Properties["FrequencyInterval"].Value;
		}
		set
		{
			traceContext.TraceVerbose("Setting FrequencyInterval to: {0}", value);
			base.Properties["FrequencyInterval"].Value = value;
		}
	}

	[SfcProperty(SfcPropertyFlags.None)]
	internal int FrequencyRecurrenceFactor
	{
		get
		{
			return (int)base.Properties["FrequencyRecurrenceFactor"].Value;
		}
		set
		{
			traceContext.TraceVerbose("Setting FrequencyRecurrenceFactor to: {0}", value);
			base.Properties["FrequencyRecurrenceFactor"].Value = value;
		}
	}

	[SfcProperty(SfcPropertyFlags.None)]
	internal FrequencyRelativeIntervals FrequencyRelativeIntervals
	{
		get
		{
			return (FrequencyRelativeIntervals)base.Properties["FrequencyRelativeIntervals"].Value;
		}
		set
		{
			traceContext.TraceVerbose("Setting FrequencyRelativeIntervals to: {0}", value);
			base.Properties["FrequencyRelativeIntervals"].Value = value;
		}
	}

	[SfcProperty(SfcPropertyFlags.None)]
	internal int FrequencySubDayInterval
	{
		get
		{
			return (int)base.Properties["FrequencySubDayInterval"].Value;
		}
		set
		{
			traceContext.TraceVerbose("Setting FrequencySubDayInterval to: {0}", value);
			base.Properties["FrequencySubDayInterval"].Value = value;
		}
	}

	[SfcProperty(SfcPropertyFlags.None)]
	internal FrequencySubDayTypes FrequencySubDayTypes
	{
		get
		{
			return (FrequencySubDayTypes)base.Properties["FrequencySubDayTypes"].Value;
		}
		set
		{
			traceContext.TraceVerbose("Setting FrequencySubDayTypes to: {0}", value);
			base.Properties["FrequencySubDayTypes"].Value = value;
		}
	}

	[SfcProperty(SfcPropertyFlags.None)]
	internal FrequencyTypes FrequencyTypes
	{
		get
		{
			return (FrequencyTypes)base.Properties["FrequencyTypes"].Value;
		}
		set
		{
			traceContext.TraceVerbose("Setting FrequencyTypes to: {0}", value);
			base.Properties["FrequencyTypes"].Value = value;
		}
	}

	[SfcProperty(SfcPropertyFlags.None)]
	internal string Schedule
	{
		get
		{
			return (string)base.Properties["Schedule"].Value;
		}
		set
		{
			traceContext.TraceVerbose("Setting Schedule to: {0}", value);
			base.Properties["Schedule"].Value = value;
		}
	}

	public event PolicyEvaluationStartedEventHandler PolicyEvaluationStarted;

	public event ConnectionProcessingStartedEventHandler ConnectionProcessingStarted;

	public event TargetProcessedEventHandler TargetProcessed;

	public event ConnectionProcessingFinishedEventHandler ConnectionProcessingFinished;

	public event PolicyEvaluationFinishedEventHandler PolicyEvaluationFinished;

	static Policy()
	{
		traceContext = TraceContext.GetTraceContext("DMF", "Policy");
		scriptCreateAction = new SfcTsqlProcFormatter();
		scriptAlterAction = new SfcTsqlProcFormatter();
		scriptDropAction = new SfcTsqlProcFormatter();
		scriptRenameAction = new SfcTsqlProcFormatter();
		scriptCreateAction.Procedure = "msdb.dbo.sp_syspolicy_add_policy";
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("name", "Name", required: true, output: false));
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("condition_name", "Condition", required: true, output: false));
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("policy_category", "PolicyCategory", required: false, output: false));
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("description", "Description", required: false, output: false));
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("help_text", "HelpText", required: false, output: false));
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("help_link", "HelpLink", required: false, output: false));
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("schedule_uid", "ScheduleUid", required: false, output: false));
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("execution_mode", "AutomatedPolicyEvaluationMode", required: true, output: false));
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("is_enabled", "Enabled", required: false, output: false));
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("policy_id", "ID", required: false, output: true));
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("root_condition_name", "RootCondition", required: false, output: false));
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("object_set", "ObjectSet", required: false, output: false));
		scriptAlterAction.Procedure = "msdb.dbo.sp_syspolicy_update_policy";
		scriptAlterAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("policy_id", "ID", required: true, output: false));
		scriptAlterAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("condition_name", "Condition", required: false, output: false));
		scriptAlterAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("execution_mode", "AutomatedPolicyEvaluationMode", required: false, output: false));
		scriptAlterAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("policy_category", "PolicyCategory", required: false, output: false));
		scriptAlterAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("schedule_uid", "ScheduleUid", required: true, output: false));
		scriptAlterAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("description", "Description", required: false, output: false));
		scriptAlterAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("help_text", "HelpText", required: false, output: false));
		scriptAlterAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("help_link", "HelpLink", required: false, output: false));
		scriptAlterAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("is_enabled", "Enabled", required: false, output: false));
		scriptAlterAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("root_condition_name", "RootCondition", required: false, output: false));
		scriptAlterAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("object_set", "ObjectSet", required: false, output: false));
		scriptDropAction.Procedure = "msdb.dbo.sp_syspolicy_delete_policy";
		scriptDropAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("policy_id", "ID", required: true, output: false));
		scriptRenameAction.Procedure = "msdb.dbo.sp_syspolicy_rename_policy";
		scriptRenameAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("name", "Name", required: true, output: false));
		scriptRenameAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("new_name", required: true));
	}

	public Policy()
	{
		InitializeEventSubscription();
	}

	private void InitializeEventSubscription()
	{
		base.PropertyChanged += PolicyPropertyChangedEventHandler;
	}

	private void PolicyPropertyChangedEventHandler(object sender, PropertyChangedEventArgs e)
	{
		traceContext.TraceMethodEnter("PolicyPropertyChangedEventHandler");
		if (!execModeCacheSet && e.PropertyName == "AutomatedPolicyEvaluationMode" && base.Properties["AutomatedPolicyEvaluationMode"].Retrieved)
		{
			execModeInBackend = AutomatedPolicyEvaluationMode;
			execModeCacheSet = true;
		}
		traceContext.TraceMethodExit("PolicyPropertyChangedEventHandler");
	}

	public Policy(PolicyStore parent, string name)
	{
		traceContext.TraceMethodEnter("Policy");
		Parent = parent;
		SetName(name);
		InitializeEventSubscription();
		traceContext.TraceMethodExit("Policy");
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
		if (!m_bStatesInitialized)
		{
			return;
		}
		SfcProperty sfcProperty = base.Properties["Enabled"];
		SfcProperty sfcProperty2 = base.Properties["Name"];
		SfcProperty sfcProperty3 = base.Properties["AutomatedPolicyEvaluationMode"];
		SfcProperty sfcProperty4 = base.Properties["ScheduleUid"];
		SfcProperty sfcProperty5 = base.Properties["Condition"];
		_ = base.Properties["RootCondition"];
		if (base.State == SfcObjectState.Pending)
		{
			sfcProperty2.Enabled = true;
		}
		else
		{
			sfcProperty2.Enabled = false;
		}
		if (sfcProperty5.IsAvailable && sfcProperty5.Value != null)
		{
			sfcProperty3.Enabled = true;
		}
		else
		{
			sfcProperty3.Enabled = false;
		}
		if (sfcProperty3.IsAvailable && AutomatedPolicyEvaluationMode == AutomatedPolicyEvaluationMode.CheckOnSchedule)
		{
			sfcProperty4.Enabled = true;
		}
		else
		{
			sfcProperty4.Enabled = false;
		}
		if (!string.IsNullOrEmpty(Name) && sfcProperty5.Value != null && AutomatedPolicyEvaluationMode != 0)
		{
			if (AutomatedPolicyEvaluationMode == AutomatedPolicyEvaluationMode.CheckOnSchedule)
			{
				if (sfcProperty4.IsAvailable && ScheduleUid != Guid.Empty)
				{
					sfcProperty.Enabled = true;
				}
				else
				{
					sfcProperty.Enabled = false;
				}
			}
			else
			{
				sfcProperty.Enabled = true;
			}
		}
		else
		{
			sfcProperty.Enabled = false;
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
		string text = scriptCreateAction.GenerateScript(this);
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
		SetCategoryID();
	}

	public ISfcScript ScriptCreateWithDependencies()
	{
		SfcTSqlScript sfcTSqlScript = new SfcTSqlScript();
		if (!string.IsNullOrEmpty(Condition))
		{
			sfcTSqlScript.AddBatch(Parent.Conditions[Condition].ScriptCreate().ToString());
		}
		else
		{
			ScriptCreate();
		}
		if (!string.IsNullOrEmpty(RootCondition) && RootCondition != Condition)
		{
			sfcTSqlScript.AddBatch(Parent.Conditions[RootCondition].ScriptCreate().ToString());
		}
		if (!string.IsNullOrEmpty(ObjectSet))
		{
			sfcTSqlScript.AddBatch(Parent.ObjectSets[ObjectSet].ScriptCreateWithDependencies(Condition).ToString());
		}
		sfcTSqlScript.AddBatch(ScriptCreate().ToString());
		return sfcTSqlScript;
	}

	public ISfcScript ScriptCreateWithObjectSet()
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("ScriptCreateWithObjectSet", TraceEventType.Information);
		SfcTSqlScript sfcTSqlScript = new SfcTSqlScript();
		if (!string.IsNullOrEmpty(ObjectSet))
		{
			sfcTSqlScript.AddBatch(Parent.ObjectSets[ObjectSet].ScriptCreate().ToString());
		}
		sfcTSqlScript.AddBatch(ScriptCreate().ToString());
		methodTraceContext.TraceParameterOut("returnVal", sfcTSqlScript);
		return sfcTSqlScript;
	}

	public void Alter()
	{
		traceContext.TraceMethodEnter("Alter");
		Validate(ValidationMethod.Alter);
		AlterImpl();
		execModeInBackend = AutomatedPolicyEvaluationMode;
		traceContext.TraceMethodExit("Alter");
	}

	public ISfcScript ScriptAlter()
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("ScriptAlter", TraceEventType.Information);
		string text = scriptAlterAction.GenerateScript(this);
		methodTraceContext.TraceVerbose("Script generated: " + text);
		return new SfcTSqlScript(text);
	}

	protected override void PostAlter(object executionResult)
	{
		SetCategoryID();
	}

	public ISfcScript ScriptAlterWithObjectSet()
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("ScriptAlterWithObjectSet", TraceEventType.Information);
		SfcTSqlScript sfcTSqlScript = new SfcTSqlScript();
		if (!string.IsNullOrEmpty(ObjectSet))
		{
			sfcTSqlScript.AddBatch(Parent.ObjectSets[ObjectSet].ScriptAlter().ToString());
		}
		sfcTSqlScript.AddBatch(ScriptAlter().ToString());
		methodTraceContext.TraceParameterOut("returnVal", sfcTSqlScript);
		return sfcTSqlScript;
	}

	private void SetCategoryID()
	{
		if (string.IsNullOrEmpty(PolicyCategory))
		{
			base.Properties["CategoryId"].Value = 0;
			return;
		}
		PolicyCategory policyCategory = Parent.PolicyCategories[PolicyCategory];
		traceContext.DebugAssert(policyCategory != null);
		base.Properties["CategoryId"].Value = policyCategory.ID;
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

	public ISfcScript ScriptDropWithObjectSet()
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("ScriptDropWithObjectSet", TraceEventType.Information);
		SfcTSqlScript sfcTSqlScript = new SfcTSqlScript();
		sfcTSqlScript.AddBatch(ScriptDrop().ToString());
		if (!string.IsNullOrEmpty(ObjectSet))
		{
			sfcTSqlScript.AddBatch(Parent.ObjectSets[ObjectSet].ScriptDrop().ToString());
		}
		methodTraceContext.TraceParameterOut("returnVal", sfcTSqlScript);
		return sfcTSqlScript;
	}

	public void Rename(string name)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("Rename", TraceEventType.Information);
		methodTraceContext.TraceParameters(name);
		if (string.IsNullOrEmpty(name))
		{
			throw methodTraceContext.TraceThrow(new ArgumentException(ExceptionTemplatesSR.ArgumentNullOrEmpty("Name")));
		}
		RenameImpl(new Key(name));
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

	private bool EvaluateRootCondition(SqlStoreConnection sqlStoreConnection, bool checkSqlScriptAsProxy)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("EvaluateRootCondition");
		methodTraceContext.TraceParameters(sqlStoreConnection, checkSqlScriptAsProxy);
		object obj = null;
		if (!string.IsNullOrEmpty(RootCondition))
		{
			Microsoft.SqlServer.Management.Smo.Server server = new Microsoft.SqlServer.Management.Smo.Server(sqlStoreConnection.ServerConnection);
			Condition condition = Parent.Conditions[RootCondition];
			Type facetType = FacetRepository.GetFacetType(condition.Facet);
			List<Type> facetSupportedTypes = FacetRepository.GetFacetSupportedTypes(facetType);
			traceContext.DebugAssert(facetSupportedTypes.Count > 0);
			if (facetSupportedTypes.Count > 1)
			{
				throw methodTraceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.TooManyURNsReturned(RootCondition)));
			}
			if (facetSupportedTypes[0] != typeof(Microsoft.SqlServer.Management.Smo.Server))
			{
				List<string> urnSkeletonsFromType = SfcMetadataDiscovery.GetUrnSkeletonsFromType(facetSupportedTypes[0]);
				if (urnSkeletonsFromType.Count > 1)
				{
					throw methodTraceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.TooManyURNsReturned(RootCondition)));
				}
				obj = server.GetSmoObject(urnSkeletonsFromType[0]);
			}
			else
			{
				obj = server;
			}
			if (obj == null)
			{
				throw methodTraceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.RootConditionFailed(RootCondition)));
			}
			FacetEvaluationContext facetEvaluationContext = FacetEvaluationContext.GetFacetEvaluationContext(facetType, obj);
			bool flag = (bool)condition.ExpressionNode.Evaluate(facetEvaluationContext, checkSqlScriptAsProxy);
			methodTraceContext.TraceParameterOut("returnVal", flag);
			return flag;
		}
		methodTraceContext.TraceParameterOut("returnVal", true);
		return true;
	}

	private bool ProcessViolators(Condition condition, AdHocPolicyEvaluationMode evaluationMode, TargetEvaluation[] violating, LogPolicyEvents logPolicyEvents)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("ProcessViolators");
		methodTraceContext.TraceParameters(condition, evaluationMode, violating, logPolicyEvents);
		if (violating == null || violating.Length == 0)
		{
			methodTraceContext.TraceParameterOut("returnVal", true);
			return true;
		}
		if (evaluationMode == AdHocPolicyEvaluationMode.Check || evaluationMode == AdHocPolicyEvaluationMode.CheckSqlScriptAsProxy)
		{
			return violating.Length == 0;
		}
		traceContext.DebugAssert(evaluationMode == AdHocPolicyEvaluationMode.Configure, "You've already returned the result if you're in Check mode and can't be here other than if you're configuring");
		raiseConditionEvaluationEvent = true;
		bool flag = true;
		bool flag2 = true;
		bool flag3 = false;
		string empty = string.Empty;
		foreach (TargetEvaluation targetEvaluation in violating)
		{
			flag3 = false;
			empty = string.Empty;
			try
			{
				condition.ExpressionNode = targetEvaluation.Result;
				condition.Configure(targetEvaluation.Target);
				flag2 = condition.Evaluate(targetEvaluation.Target, evaluationMode);
				flag = flag && flag2;
				if (!flag2 && !SqlContext.IsAvailable)
				{
					flag3 = condition.CanBeConfigured(ref empty);
				}
				if (logPolicyEvents != null && (!flag || Parent.LogOnSuccess))
				{
					logPolicyEvents.LogPolicyEvaluationDetail(condition, new ConditionEvaluationEventArgs(null, condition.Facet, targetEvaluation.Target, flag2, flag3, empty, null));
				}
				if (!flag)
				{
					throw methodTraceContext.TraceThrow(new ExpressionNodeConfigurationException());
				}
			}
			catch (Exception ex)
			{
				methodTraceContext.TraceCatch(ex);
				flag = false;
				flag2 = false;
				if (!Utils.ShouldProcessException(ex))
				{
					throw;
				}
				Exception innerException = ex;
				if (ex is PropertyReadOnlyException)
				{
					innerException = new NonConfigurableReadOnlyPropertyException(ex.Message, ((PropertyReadOnlyException)ex).PropertyName, ex);
				}
				PolicyEvaluationException exception = new PolicyEvaluationException(ExceptionTemplatesSR.PolicyEvaluationFailed(Name), innerException);
				string empty2 = string.Empty;
				FireTargetProcessed(new TargetProcessedEventArgs(targetEvaluation.Target, SfcSqlPathUtilities.ConvertUrnToPath(SfcUtility.GetUrn(targetEvaluation.Target)), flag2, flag3, empty, targetEvaluation.Result, exception, empty2));
			}
		}
		methodTraceContext.TraceParameterOut("returnVal", flag);
		return flag;
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	public bool Evaluate(AdHocPolicyEvaluationMode evaluationMode, params object[] targetObjects)
	{
		if (targetObjects == null)
		{
			throw traceContext.TraceThrow(new ArgumentNullException("targetObjects"));
		}
		Condition validCondition = GetValidCondition();
		FirePolicyEvaluationStarted();
		validCondition.EvaluateCondition += EvaluateConditionEventHandler;
		try
		{
			bool flag = true;
			PolicyEvaluationException exception = null;
			foreach (object obj in targetObjects)
			{
				int targetsEvaluated = 0;
				FireConnectionProcessingStarted(new ConnectionProcessingStartedEventArgs(null));
				try
				{
					Microsoft.SqlServer.Management.Dmf.ObjectSet.CalculateTargets(new object[1] { obj }, validCondition, evaluationMode, out var conforming, out var violating);
					int num = ((conforming != null) ? conforming.Length : 0);
					int num2 = ((violating != null) ? violating.Length : 0);
					targetsEvaluated = num + num2;
					flag &= ProcessViolators(validCondition, evaluationMode, violating, null);
				}
				catch (Exception ex)
				{
					traceContext.TraceCatch(ex);
					flag = false;
					if (!Utils.ShouldProcessException(ex))
					{
						throw;
					}
					exception = new PolicyEvaluationException(string.Empty, ex);
				}
				FireConnectionProcessingFinished(new ConnectionProcessingFinishedEventArgs(flag, exception, null, targetsEvaluated));
			}
			FirePolicyEvaluationFinished(new PolicyEvaluationFinishedEventArgs(flag, null));
			return flag;
		}
		finally
		{
			validCondition.EvaluateCondition -= EvaluateConditionEventHandler;
		}
	}

	public bool Evaluate(AdHocPolicyEvaluationMode evaluationMode, params ISfcConnection[] targetConnections)
	{
		long historyId = 0L;
		return Evaluate(evaluationMode, ref historyId, targetConnections);
	}

	internal bool Evaluate(AdHocPolicyEvaluationMode evaluationMode, ref long historyId, params ISfcConnection[] targetConnections)
	{
		return EvaluatePolicyUsingConnections(evaluationMode, null, ref historyId, targetConnections);
	}

	public bool Evaluate(AdHocPolicyEvaluationMode evaluationMode, SfcQueryExpression targetQueryExpression, params ISfcConnection[] targetConnections)
	{
		long historyId = 0L;
		return Evaluate(evaluationMode, targetQueryExpression, ref historyId, targetConnections);
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	internal bool Evaluate(AdHocPolicyEvaluationMode evaluationMode, SfcQueryExpression targetQueryExpression, ref long historyId, params ISfcConnection[] targetConnections)
	{
		if (targetQueryExpression == null)
		{
			throw traceContext.TraceThrow(new ArgumentNullException("targetQueryExpression"));
		}
		return EvaluatePolicyUsingConnections(evaluationMode, targetQueryExpression, ref historyId, targetConnections);
	}

	private bool EvaluatePolicyUsingConnections(AdHocPolicyEvaluationMode evaluationMode, SfcQueryExpression targetQueryExpression, ref long historyId, params ISfcConnection[] targetConnections)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("EvaluatePolicyUsingConnections");
		methodTraceContext.TraceParameters(evaluationMode, targetQueryExpression, historyId, targetConnections);
		bool flag = targetQueryExpression == null;
		if (flag)
		{
			raiseConditionEvaluationEventToWindowsLog = false;
		}
		if (targetConnections == null || targetConnections.Length == 0)
		{
			throw methodTraceContext.TraceThrow(new ArgumentNullException("targetConnections"));
		}
		Condition validCondition = GetValidCondition();
		ValidateConnectionCompatibility(validCondition.Facet, targetConnections);
		FirePolicyEvaluationStarted();
		if (evaluationMode == AdHocPolicyEvaluationMode.Configure)
		{
			raiseConditionEvaluationEvent = false;
		}
		validCondition.EvaluateCondition += EvaluateConditionEventHandler;
		try
		{
			bool flag2 = true;
			Exception ex = null;
			foreach (ISfcConnection sfcConnection in targetConnections)
			{
				bool flag3 = true;
				int targetsEvaluated = 0;
				if (base.State == SfcObjectState.Existing && Parent.SqlStoreConnection != null && Parent.SqlStoreConnection.ServerInstance.Equals(sfcConnection.ServerInstance, StringComparison.OrdinalIgnoreCase))
				{
					logPolicyEvents = new LogPolicyEvents(Parent.SqlStoreConnection.ServerConnection);
					logPolicyEvents.LogPolicyEvaluationStart(ID, targetQueryExpression == null);
					historyId = logPolicyEvents.HistoryId;
				}
				bool isOpen = sfcConnection.IsOpen;
				SqlStoreConnection sqlStoreConnection = Utils.GetSqlStoreConnection(sfcConnection, "Policy.Evaluate");
				FireConnectionProcessingStarted(new ConnectionProcessingStartedEventArgs(sfcConnection));
				if (!EvaluateRootCondition(sqlStoreConnection, evaluationMode == AdHocPolicyEvaluationMode.CheckSqlScriptAsProxy))
				{
					FireConnectionProcessingFinished(new ConnectionProcessingFinishedEventArgs(result: true, null, sfcConnection, 0, rootCheckPassed: false));
					continue;
				}
				try
				{
					object[] conforming = null;
					TargetEvaluation[] violating = null;
					if (targetQueryExpression == null)
					{
						if (string.IsNullOrEmpty(ObjectSet))
						{
							throw methodTraceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.ObjectSetIsNull));
						}
						GetValidObjectSet().CalculateTargets(sqlStoreConnection, validCondition, evaluationMode, PolicyCategory, out conforming, out violating);
					}
					else
					{
						Microsoft.SqlServer.Management.Dmf.ObjectSet.CalculateTargets(sqlStoreConnection, targetQueryExpression, validCondition, evaluationMode, out conforming, out violating);
					}
					int num = ((conforming != null) ? conforming.Length : 0);
					int num2 = ((violating != null) ? violating.Length : 0);
					targetsEvaluated = num + num2;
					if (targetQueryExpression != null && Enabled && conforming != null && conforming.Length != 0 && !Parent.LogOnSuccess)
					{
						object[] array = conforming;
						foreach (object target in array)
						{
							if (logPolicyEvents != null)
							{
								logPolicyEvents.LogPolicyEvaluationDetail(null, new ConditionEvaluationEventArgs(null, validCondition.Facet, target, evaluationResult: true, isConfigurable: false, null, null));
							}
						}
					}
					flag3 &= ProcessViolators(validCondition, evaluationMode, violating, logPolicyEvents);
				}
				catch (Exception ex2)
				{
					methodTraceContext.TraceCatch(ex2);
					flag3 = false;
					if (!Utils.ShouldProcessException(ex2))
					{
						throw;
					}
					ex = ex2;
				}
				finally
				{
					try
					{
						sqlStoreConnection.ServerConnection.ExecuteNonQuery("use [master]", ExecutionTypes.NoCommands);
					}
					catch (Exception ex3)
					{
						methodTraceContext.TraceCatch(ex3);
						if (!Utils.ShouldProcessException(ex3))
						{
							throw;
						}
					}
					if (!isOpen && sfcConnection.IsOpen)
					{
						sfcConnection.Disconnect();
					}
				}
				FireConnectionProcessingFinished(new ConnectionProcessingFinishedEventArgs(flag3, (ex == null) ? null : new PolicyEvaluationException(ExceptionTemplatesSR.PolicyEvaluationFailed(Name), ex), sfcConnection, targetsEvaluated));
				if (logPolicyEvents != null)
				{
					logPolicyEvents.LogPolicyEvaluationEnd(flag3, ex);
					logPolicyEvents = null;
				}
				flag2 = flag2 && flag3;
			}
			FirePolicyEvaluationFinished(new PolicyEvaluationFinishedEventArgs(flag2, null));
			if (!flag2 && flag && Parent.SqlStoreConnection != null)
			{
				RaisePolicyResultEvent(null);
			}
			raiseConditionEvaluationEventToWindowsLog = true;
			methodTraceContext.TraceParameterOut("returnVal", flag2);
			return flag2;
		}
		finally
		{
			validCondition.EvaluateCondition -= EvaluateConditionEventHandler;
		}
	}

	internal void EvaluateConditionEventHandler(Condition policyCondition, ConditionEvaluationEventArgs eventArgs)
	{
		traceContext.TraceMethodEnter("EvaluateConditionEventHandler");
		traceContext.TraceParameters(policyCondition, eventArgs);
		if (raiseConditionEvaluationEvent && logPolicyEvents != null && (!eventArgs.EvaluationResult || policyCondition.Parent.LogOnSuccess))
		{
			if (!eventArgs.EvaluationResult && raiseConditionEvaluationEventToWindowsLog)
			{
				RaisePolicyResultEvent(eventArgs.TargetPsPath);
			}
			logPolicyEvents.LogPolicyEvaluationDetail(policyCondition, eventArgs);
		}
		if (raiseConditionEvaluationEvent)
		{
			PolicyEvaluationException exception = null;
			if (!eventArgs.EvaluationResult && eventArgs.EvaluationException != null)
			{
				exception = new PolicyEvaluationException(ExceptionTemplatesSR.PolicyEvaluationFailed(Name), eventArgs.EvaluationException);
			}
			FireTargetProcessed(new TargetProcessedEventArgs(eventArgs.Target, (!string.IsNullOrEmpty(eventArgs.TargetPsPath)) ? eventArgs.TargetPsPath : eventArgs.TargetUrnOnlyId, eventArgs.EvaluationResult, eventArgs.IsConfigurable, eventArgs.ConfigurationErrorMessage, policyCondition.ExpressionNode.DeepClone(), exception, eventArgs.ServerName));
		}
		traceContext.TraceMethodExit("EvaluateConditionEventHandler");
	}

	public string ProduceConfigureScript(object target)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		if (string.IsNullOrEmpty(Condition))
		{
			return string.Empty;
		}
		return Parent.Conditions[Condition].ProduceConfigureScript(target);
	}

	internal static object GetOneTarget(ISfcConnection targetConnection, SfcQueryExpression targetQueryExpression)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("GetOneTarget");
		methodTraceContext.TraceParameters(targetConnection, targetQueryExpression);
		traceContext.DebugAssert(targetConnection != null);
		if (targetQueryExpression == null)
		{
			throw methodTraceContext.TraceThrow(new ArgumentNullException("targetQueryExpression"));
		}
		if (!(targetConnection is SqlStoreConnection sqlStoreConnection))
		{
			throw methodTraceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.UnsupportedObjectType(targetConnection.GetType().Name, "Policy.Execute")));
		}
		SfcObjectQuery sfcObjectQuery = new SfcObjectQuery(new Microsoft.SqlServer.Management.Smo.Server(sqlStoreConnection.ServerConnection));
		object obj = null;
		string[] fields = null;
		foreach (object item in sfcObjectQuery.ExecuteIterator(targetQueryExpression, fields, null))
		{
			if (obj == null)
			{
				obj = item;
				continue;
			}
			throw methodTraceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.OnlyOneTarget(targetQueryExpression.ToString())));
		}
		methodTraceContext.TraceParameterOut("returnVal", obj);
		return obj;
	}

	internal bool IsInTargetSet(SfcConnection targetConnection, SfcQueryExpression targetQE, bool checkSqlScriptAsProxy)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("IsInTargetSet");
		methodTraceContext.TraceParameters(targetConnection, targetQE, checkSqlScriptAsProxy);
		if (string.IsNullOrEmpty(ObjectSet))
		{
			methodTraceContext.TraceParameterOut("returnVal", false);
			return false;
		}
		SqlStoreConnection sqlStoreConnection = Utils.GetSqlStoreConnection(targetConnection, "Policy.Execute");
		if (!EvaluateRootCondition(sqlStoreConnection, checkSqlScriptAsProxy))
		{
			methodTraceContext.TraceParameterOut("returnVal", false);
			return false;
		}
		TargetSet targetSet = Parent.ObjectSets[ObjectSet].TargetSets[targetQE.ExpressionSkeleton];
		if (targetSet == null || !targetSet.Enabled)
		{
			methodTraceContext.TraceParameterOut("returnVal", false);
			return false;
		}
		if (targetSet.RootLevel == targetSet.TargetTypeSkeleton)
		{
			methodTraceContext.TraceParameterOut("returnVal", true);
			return true;
		}
		Urn urn = targetQE.ToString();
		while (null != urn)
		{
			SfcQueryExpression sfcQueryExpression = new SfcQueryExpression(urn.ToString());
			TargetSetLevel level = targetSet.GetLevel(sfcQueryExpression.ExpressionSkeleton);
			if (!string.IsNullOrEmpty(level.Condition))
			{
				object oneTarget = GetOneTarget(targetConnection, sfcQueryExpression);
				if (oneTarget == null)
				{
					methodTraceContext.TraceParameterOut("returnVal", false);
					return false;
				}
				FacetEvaluationContext facetEvaluationContext = FacetEvaluationContext.GetFacetEvaluationContext(level.TargetType, oneTarget);
				if (!(bool)Parent.Conditions[level.Condition].ExpressionNode.Evaluate(facetEvaluationContext, checkSqlScriptAsProxy))
				{
					methodTraceContext.TraceParameterOut("returnVal", false);
					return false;
				}
			}
			urn = urn.Parent;
			if (null != urn && urn.Type == targetSet.RootLevel)
			{
				urn = null;
			}
		}
		methodTraceContext.TraceParameterOut("returnVal", true);
		return true;
	}

	internal void RaisePolicyResultEvent(string targetUri)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("RaisePolicyResultEvent");
		methodTraceContext.TraceParameters(targetUri);
		if (AutomatedPolicyEvaluationMode == AutomatedPolicyEvaluationMode.None || !Enabled)
		{
			return;
		}
		int num = 0;
		string empty = string.Empty;
		num = AutomatedPolicyEvaluationMode switch
		{
			AutomatedPolicyEvaluationMode.Enforce => (!SqlContext.IsAvailable) ? 34051 : 34050, 
			AutomatedPolicyEvaluationMode.CheckOnChanges => 34053, 
			AutomatedPolicyEvaluationMode.CheckOnSchedule => 34052, 
			_ => throw traceContext.TraceThrow(new DmfException(ExceptionTemplatesImpl.UnknownEnumeration(typeof(AutomatedPolicyEvaluationMode).Name))), 
		};
		empty = ((targetUri != null && targetUri.Length != 0) ? ExceptionTemplatesSR.PolicyViolatedTarget(SfcTsqlProcFormatter.EscapeString(Name, '\''), targetUri) : ExceptionTemplatesSR.PolicyViolated(Name));
		try
		{
			Parent.SqlStoreConnection.ServerConnection.ExecuteNonQuery($"RAISERROR ({num}, 16, 1, N'{SfcTsqlProcFormatter.EscapeString(empty, '\'')}') WITH LOG", ExecutionTypes.NoCommands);
		}
		catch (ExecutionFailureException ex)
		{
			methodTraceContext.TraceCatch(ex);
			if (!(ex.InnerException is SqlException ex2) || (ex2.Number != num && ex2.Number != 2778))
			{
				throw;
			}
		}
	}

	public override void Discover(ISfcDependencyDiscoveryObjectSink sink)
	{
		if (sink == null)
		{
			throw traceContext.TraceThrow(new ArgumentNullException("sink"));
		}
		if (sink.Action != SfcDependencyAction.Serialize)
		{
			return;
		}
		if (!string.IsNullOrEmpty(ObjectSet))
		{
			ObjectSet objectSet = Parent.ObjectSets[ObjectSet];
			if (objectSet != null)
			{
				sink.Add(SfcDependencyDirection.Outbound, objectSet, SfcTypeRelation.StrongReference, discovered: false);
			}
		}
		ConditionCollection conditions = Parent.Conditions;
		if (!string.IsNullOrEmpty(Condition) && conditions.Contains(Condition))
		{
			sink.Add(SfcDependencyDirection.Outbound, conditions[Condition], SfcTypeRelation.StrongReference, discovered: false);
		}
		if (!string.IsNullOrEmpty(RootCondition) && conditions.Contains(RootCondition))
		{
			sink.Add(SfcDependencyDirection.Outbound, conditions[RootCondition], SfcTypeRelation.StrongReference, discovered: false);
		}
		if (!string.IsNullOrEmpty(PolicyCategory))
		{
			PolicyCategoryCollection policyCategories = Parent.PolicyCategories;
			if (!string.IsNullOrEmpty(PolicyCategory) && policyCategories.Contains(PolicyCategory))
			{
				sink.Add(SfcDependencyDirection.Outbound, policyCategories[PolicyCategory], SfcTypeRelation.StrongReference, discovered: false);
			}
		}
		UpdateScheduleProperties();
	}

	internal void UpdateScheduleProperties()
	{
		bool flag = false;
		if (Parent != null && ScheduleUid != Guid.Empty)
		{
			JobSchedule jobSchedule = new Microsoft.SqlServer.Management.Smo.Server(Parent.SqlStoreConnection.ServerConnection).JobServer.SharedSchedules[ScheduleUid];
			if (jobSchedule != null)
			{
				flag = true;
				Schedule = jobSchedule.Name;
				ActiveEndDate = jobSchedule.ActiveEndDate;
				ActiveEndTimeOfDay = jobSchedule.ActiveEndTimeOfDay.Ticks;
				ActiveStartDate = jobSchedule.ActiveStartDate;
				ActiveStartTimeOfDay = jobSchedule.ActiveStartTimeOfDay.Ticks;
				FrequencyInterval = jobSchedule.FrequencyInterval;
				FrequencyRecurrenceFactor = jobSchedule.FrequencyRecurrenceFactor;
				FrequencyRelativeIntervals = jobSchedule.FrequencyRelativeIntervals;
				FrequencySubDayInterval = jobSchedule.FrequencySubDayInterval;
				FrequencySubDayTypes = jobSchedule.FrequencySubDayTypes;
				FrequencyTypes = jobSchedule.FrequencyTypes;
			}
		}
		if (!flag)
		{
			if (base.Properties["ActiveEndDate"].IsNull || base.Properties["ActiveEndDate"].Value is DBNull)
			{
				ActiveEndDate = DateTime.MinValue;
			}
			if (base.Properties["ActiveStartDate"].IsNull || base.Properties["ActiveStartDate"].Value is DBNull)
			{
				ActiveStartDate = DateTime.MinValue;
			}
		}
	}

	internal bool DependsOnCondition(string conditionName)
	{
		if (Condition == conditionName || RootCondition == conditionName || (!string.IsNullOrEmpty(ObjectSet) && Parent.ObjectSets[ObjectSet].DependsOnCondition(conditionName)))
		{
			return true;
		}
		return false;
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

	protected override ISfcCollection GetChildCollection(string elementType)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("GetChildCollection");
		methodTraceContext.TraceParameters(elementType);
		if (elementType == "EvaluationHistory")
		{
			methodTraceContext.TraceParameterOut("returnVal", EvaluationHistories);
			return EvaluationHistories;
		}
		throw methodTraceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.NoSuchCollection(elementType)));
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	public bool UsesFacet(string facet)
	{
		Condition condition = Parent.Conditions[Condition];
		if (condition != null)
		{
			return condition.Facet == facet;
		}
		return false;
	}

	internal static List<SfcInstanceSerializedData> UpgradeInstance(List<SfcInstanceSerializedData> sfcInstanceData, int fileVersion)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("UpgradeInstance");
		methodTraceContext.TraceParameters(sfcInstanceData, fileVersion);
		List<SfcInstanceSerializedData> list = new List<SfcInstanceSerializedData>();
		if (fileVersion < 3)
		{
			foreach (SfcInstanceSerializedData sfcInstanceDatum in sfcInstanceData)
			{
				if (sfcInstanceDatum.Name == "AutomatedPolicyExecutionMode")
				{
					string input = (string)sfcInstanceDatum.Value;
					input = new Regex("AutomatedPolicyExecutionMode", RegexOptions.IgnoreCase).Replace(input, "AutomatedPolicyEvaluationMode");
					SfcInstanceSerializedData item = new SfcInstanceSerializedData(SfcSerializedTypes.Property, "AutomatedPolicyEvaluationMode", "AutomatedPolicyEvaluationMode", input);
					list.Add(item);
				}
				else
				{
					list.Add(sfcInstanceDatum);
				}
			}
		}
		methodTraceContext.TraceParameterOut("returnVal", list);
		return list;
	}

	private void PolicyEvaluationStartedHistoryBuilder()
	{
		PolicyStore policyStore = new PolicyStore();
		((ISfcHasConnection)policyStore).ConnectionContext.Mode = SfcConnectionContextMode.Offline;
		Policy policy = new Policy(policyStore, Name);
		hiddenHistory = new EvaluationHistory(policy);
		hiddenHistory.PolicyName = policy.Name;
		connectionCount = 1;
		detailCount = 1;
		hiddenHistory.StartDate = DateTime.Now;
	}

	private void PolicyEvaluationFinishedHistoryBuilder(PolicyEvaluationFinishedEventArgs e)
	{
		traceContext.TraceMethodEnter("PolicyEvaluationFinishedHistoryBuilder");
		traceContext.TraceParameters(e);
		traceContext.DebugAssert(hiddenHistory != null, "The history object is null, but it should have been created on the evaluation start event");
		if (hiddenHistory != null)
		{
			hiddenHistory.EndDate = DateTime.Now;
			hiddenHistory.Exception = string.Empty;
			if (e.Exception != null)
			{
				hiddenHistory.Exception = e.Exception.ToString();
			}
			hiddenHistory.Result = e.Result;
			hiddenHistory.ID = 1L;
		}
		e.EvaluationHistory = hiddenHistory;
		hiddenHistory = null;
		hiddenConnectionHistory = null;
		connectionCount = 1;
		detailCount = 1;
		traceContext.TraceMethodExit("PolicyEvaluationFinishedHistoryBuilder");
	}

	private void ConnectionProcessingStartedHistoryBuilder(ConnectionProcessingStartedEventArgs e)
	{
		traceContext.TraceMethodEnter("ConnectionProcessingStartedHistoryBuilder");
		traceContext.TraceParameters(e);
		hiddenConnectionHistory = new ConnectionEvaluationHistory(hiddenHistory);
		hiddenConnectionHistory.ID = connectionCount++;
		hiddenConnectionHistory.ServerInstance = string.Empty;
		if (e.Connection != null)
		{
			hiddenConnectionHistory.ServerInstance = e.Connection.ServerInstance;
		}
		traceContext.TraceMethodExit("ConnectionProcessingStartedHistoryBuilder");
	}

	private void ConnectionProcessingFinishedHistoryBuilder(ConnectionProcessingFinishedEventArgs e)
	{
		traceContext.TraceMethodEnter("ConnectionProcessingFinishedHistoryBuilder");
		traceContext.TraceParameters(e);
		traceContext.DebugAssert(hiddenConnectionHistory != null, "The history connection object is null, but it should have been created on the connection start event");
		if (hiddenConnectionHistory != null)
		{
			hiddenConnectionHistory.Result = e.Result;
			hiddenConnectionHistory.Exception = string.Empty;
			if (e.Exception != null)
			{
				hiddenConnectionHistory.Exception = e.Exception.ToString();
			}
			hiddenHistory.ConnectionEvaluationHistories.Add(hiddenConnectionHistory);
		}
		hiddenConnectionHistory = null;
		traceContext.TraceMethodExit("ConnectionProcessingFinishedHistoryBuilder");
	}

	private void TargetProcessedHistoryBuilder(TargetProcessedEventArgs e)
	{
		traceContext.TraceMethodEnter("TargetProcessedHistoryBuilder");
		traceContext.TraceParameters(e);
		traceContext.DebugAssert(hiddenConnectionHistory != null, "The history connection object is null, but it should have been created on the connection start event");
		if (hiddenConnectionHistory != null)
		{
			EvaluationDetail evaluationDetail = new EvaluationDetail(hiddenConnectionHistory);
			evaluationDetail.ID = detailCount++;
			evaluationDetail.Result = e.Result;
			evaluationDetail.TargetQueryExpression = string.Empty;
			if (!string.IsNullOrEmpty(e.TargetExpression))
			{
				evaluationDetail.TargetQueryExpression = e.TargetExpression;
			}
			evaluationDetail.Exception = string.Empty;
			if (e.Exception != null)
			{
				evaluationDetail.Exception = e.Exception.ToString();
			}
			evaluationDetail.ResultDetail = string.Empty;
			if (e.ExpressionNode != null)
			{
				evaluationDetail.ResultDetail = ExpressionNode.SerializeNodeWithResult(e.ExpressionNode);
			}
			hiddenConnectionHistory.EvaluationDetails.Add(evaluationDetail);
		}
		traceContext.TraceMethodExit("TargetProcessedHistoryBuilder");
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	private void FireEvent(Delegate del, params object[] args)
	{
		if ((object)del != null)
		{
			try
			{
				del.DynamicInvoke(args);
			}
			catch (Exception ex)
			{
				traceContext.TraceCatch(ex);
				throw traceContext.TraceThrow(new PolicyEvaluationException(ExceptionTemplatesSR.PolicyEvaluationFailedOnDelegate(Name), ex));
			}
		}
	}

	private void FirePolicyEvaluationStarted()
	{
		if (this.PolicyEvaluationFinished != null)
		{
			PolicyEvaluationStartedHistoryBuilder();
		}
		FireEvent(this.PolicyEvaluationStarted, this, EventArgs.Empty);
	}

	private void FirePolicyEvaluationFinished(PolicyEvaluationFinishedEventArgs e)
	{
		if (this.PolicyEvaluationFinished != null)
		{
			PolicyEvaluationFinishedHistoryBuilder(e);
		}
		FireEvent(this.PolicyEvaluationFinished, this, e);
	}

	private void FireConnectionProcessingStarted(ConnectionProcessingStartedEventArgs e)
	{
		if (this.PolicyEvaluationFinished != null)
		{
			ConnectionProcessingStartedHistoryBuilder(e);
		}
		FireEvent(this.ConnectionProcessingStarted, this, e);
	}

	private void FireConnectionProcessingFinished(ConnectionProcessingFinishedEventArgs e)
	{
		if (this.PolicyEvaluationFinished != null)
		{
			ConnectionProcessingFinishedHistoryBuilder(e);
		}
		FireEvent(this.ConnectionProcessingFinished, this, e);
	}

	private void FireTargetProcessed(TargetProcessedEventArgs e)
	{
		if (this.PolicyEvaluationFinished != null)
		{
			TargetProcessedHistoryBuilder(e);
		}
		FireEvent(this.TargetProcessed, this, e);
	}

	private void Validate(string validationMode, bool throwOnFirst, ValidationState validationState)
	{
		ValidateProperties(validationMode, throwOnFirst, validationState);
		ValidateMode(validationMode, throwOnFirst, validationState);
		ValidateCategory(validationMode, throwOnFirst, validationState);
		ValidateObjectSet(validationMode, throwOnFirst, validationState);
		ValidateRootCondition(validationMode, throwOnFirst, validationState);
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
				throw methodTraceContext.TraceThrow(new ObjectValidationException(ExceptionTemplatesSR.Policy, Name, ex));
			}
			validationState.AddError(ex, "Name");
		}
		if (validationMode == ValidationMethod.Rename)
		{
			return;
		}
		if (string.IsNullOrEmpty(Condition))
		{
			Exception ex2 = new SfcPropertyNotSetException("Condition");
			if (throwOnFirst)
			{
				throw methodTraceContext.TraceThrow(new ObjectValidationException(ExceptionTemplatesSR.Policy, Name, ex2));
			}
			validationState.AddError(ex2, "Condition");
		}
		if (HelpText != null && HelpText.Length > 4000)
		{
			Exception ex3 = new StringPropertyTooLongException(ExceptionTemplatesSR.AdditionalHelpText, 4000, HelpText.Length);
			if (throwOnFirst)
			{
				throw methodTraceContext.TraceThrow(new ObjectValidationException(ExceptionTemplatesSR.Policy, HelpText, ex3));
			}
			validationState.AddError(ex3, "HelpText");
		}
		if (HelpLink == null)
		{
			return;
		}
		if (!string.IsNullOrEmpty(HelpLink) && !IsSystemObject)
		{
			Exception ex4 = null;
			try
			{
				if (!Utils.IsValidHelpLink(HelpLink))
				{
					ex4 = new Exception(ExceptionTemplatesSR.InvalidHelpLinkMessage);
				}
			}
			catch (Exception ex5)
			{
				methodTraceContext.TraceCatch(ex5);
				ex4 = ex5;
			}
			if (ex4 != null)
			{
				if (throwOnFirst)
				{
					throw methodTraceContext.TraceThrow(new ObjectValidationException(ExceptionTemplatesSR.Policy, HelpLink, ex4));
				}
				validationState.AddError(ex4, "HelpLink");
			}
		}
		if (HelpLink.Length > 2083)
		{
			Exception ex6 = new StringPropertyTooLongException(ExceptionTemplatesSR.AdditionalHelpLink, 2083, HelpLink.Length);
			if (throwOnFirst)
			{
				throw methodTraceContext.TraceThrow(new ObjectValidationException(ExceptionTemplatesSR.Policy, HelpLink, ex6));
			}
			validationState.AddError(ex6, "HelpLink");
		}
	}

	private void ValidateMode(string validationMode, bool throwOnFirst, ValidationState validationState)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("ValidateMode");
		methodTraceContext.TraceParameters(validationMode, throwOnFirst, validationState);
		if (validationMode == ValidationMethod.Rename)
		{
			return;
		}
		if (AutomatedPolicyEvaluationMode != 0)
		{
			if (AutomatedPolicyEvaluationMode == AutomatedPolicyEvaluationMode.CheckOnSchedule)
			{
				if (Parent == null || ((ISfcHasConnection)Parent).ConnectionContext.Mode == SfcConnectionContextMode.Offline)
				{
					if (string.IsNullOrEmpty(Schedule))
					{
						Exception ex = new SfcPropertyNotSetException("Schedule");
						if (throwOnFirst)
						{
							throw methodTraceContext.TraceThrow(new ObjectValidationException(ExceptionTemplatesSR.Policy, Schedule, ex));
						}
						validationState.AddError(ex, "ScheduleUid");
					}
				}
				else if (Guid.Empty != ScheduleUid)
				{
					if (!throwOnFirst)
					{
						Microsoft.SqlServer.Management.Smo.Server server = new Microsoft.SqlServer.Management.Smo.Server(Parent.SqlStoreConnection.ServerConnection);
						if (server.JobServer.SharedSchedules[ScheduleUid] == null)
						{
							validationState.AddWarning(ExceptionTemplatesSR.ScheduleDoesntExist(ScheduleUid.ToString()), "ScheduleUid");
						}
					}
				}
				else
				{
					Exception ex2 = new MissingJobScheduleException(ExceptionTemplatesSR.MissingJobSchedule);
					if (throwOnFirst)
					{
						throw methodTraceContext.TraceThrow(new ObjectValidationException(ExceptionTemplatesSR.Policy, Name, ex2));
					}
					validationState.AddError(ex2, "ScheduleUid");
				}
			}
			Condition condition = Parent.Conditions[Condition];
			if (condition == null)
			{
				Exception ex3 = new MissingObjectException(ExceptionTemplatesSR.Condition, Condition);
				if (throwOnFirst)
				{
					throw methodTraceContext.TraceThrow(new ObjectValidationException(ExceptionTemplatesSR.Policy, Name, ex3));
				}
				validationState.AddError(ex3, "Condition");
			}
			AutomatedPolicyEvaluationMode supportedEvaluationMode = condition.GetSupportedEvaluationMode();
			if ((supportedEvaluationMode & AutomatedPolicyEvaluationMode) == 0)
			{
				Exception ex4 = new ConflictingPropertyValuesException(validationMode, ExceptionTemplatesSR.Condition, condition.Name, ExceptionTemplatesSR.EvaluationMode, Utils.GetDescriptionForEvaluationMode(AutomatedPolicyEvaluationMode));
				if (throwOnFirst)
				{
					throw methodTraceContext.TraceThrow(new ObjectValidationException(ExceptionTemplatesSR.Policy, Name, ex4));
				}
				validationState.AddError(ex4, "AutomatedPolicyEvaluationMode");
			}
			AutomatedPolicyEvaluationMode automatedPolicyEvaluationMode = AutomatedPolicyEvaluationMode.Enforce | AutomatedPolicyEvaluationMode.CheckOnChanges;
			if (!string.IsNullOrEmpty(RootCondition) && (AutomatedPolicyEvaluationMode & automatedPolicyEvaluationMode) != 0)
			{
				Exception ex5 = new ConflictingPropertyValuesException(validationMode, ExceptionTemplatesSR.RootCondition, RootCondition, ExceptionTemplatesSR.EvaluationMode, Utils.GetDescriptionForEvaluationMode(AutomatedPolicyEvaluationMode));
				if (throwOnFirst)
				{
					throw methodTraceContext.TraceThrow(new ObjectValidationException(ExceptionTemplatesSR.Policy, Name, ex5));
				}
				validationState.AddError(ex5, "AutomatedPolicyEvaluationMode");
			}
			if ((AutomatedPolicyEvaluationMode & (AutomatedPolicyEvaluationMode.Enforce | AutomatedPolicyEvaluationMode.CheckOnChanges)) != 0 && throwOnFirst && !string.IsNullOrEmpty(ObjectSet) && !Parent.ObjectSets[ObjectSet].IsEventingFilter())
			{
				Exception innerException = new ConflictingPropertyValuesException(validationMode, ExceptionTemplatesSR.ObjectSet, ObjectSet, ExceptionTemplatesSR.EvaluationMode, Utils.GetDescriptionForEvaluationMode(AutomatedPolicyEvaluationMode));
				throw methodTraceContext.TraceThrow(new ObjectValidationException(ExceptionTemplatesSR.Policy, Name, innerException));
			}
		}
		else if (Enabled)
		{
			Exception ex6 = new ConflictingPropertyValuesException(validationMode, ExceptionTemplatesSR.EvaluationMode, Utils.GetDescriptionForEvaluationMode(AutomatedPolicyEvaluationMode), ExceptionTemplatesSR.Enabled, Enabled.ToString());
			if (throwOnFirst)
			{
				throw methodTraceContext.TraceThrow(new ObjectValidationException(ExceptionTemplatesSR.Policy, Name, ex6));
			}
			validationState.AddError(ex6, "Enabled");
		}
	}

	private void ValidateCategory(string validationMode, bool throwOnFirst, ValidationState validationState)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("ValidateCategory");
		methodTraceContext.TraceParameters(validationMode, throwOnFirst, validationState);
		if (!string.IsNullOrEmpty(PolicyCategory) && !Parent.PolicyCategories.Contains(PolicyCategory))
		{
			Exception ex = new MissingObjectException("PolicyCategory", PolicyCategory);
			if (throwOnFirst)
			{
				throw methodTraceContext.TraceThrow(new ObjectValidationException("Policy", Name, ex));
			}
			validationState.AddError(ex, "PolicyCategory");
		}
	}

	private void ValidateObjectSet(string validationMode, bool throwOnFirst, ValidationState validationState)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("ValidateObjectSet");
		methodTraceContext.TraceParameters(validationMode, throwOnFirst, validationState);
		if (string.IsNullOrEmpty(ObjectSet))
		{
			if (AutomatedPolicyEvaluationMode != 0)
			{
				Exception ex = new DmfException(ExceptionTemplatesSR.PolicyWithNoFilters);
				if (throwOnFirst)
				{
					throw methodTraceContext.TraceThrow(new ObjectValidationException("Policy", Name, ex));
				}
				validationState.AddError(ex, "ObjectSet");
			}
			return;
		}
		if (!Parent.ObjectSets.Contains(ObjectSet))
		{
			Exception ex2 = new MissingObjectException("ObjectSet", ObjectSet);
			if (throwOnFirst)
			{
				throw methodTraceContext.TraceThrow(new ObjectValidationException("Policy", Name, ex2));
			}
			validationState.AddError(ex2, "ObjectSet");
		}
		if (!string.IsNullOrEmpty(Condition) && Parent.Conditions[Condition].Facet != Parent.ObjectSets[ObjectSet].Facet)
		{
			Exception ex3 = new DmfException(ExceptionTemplatesSR.ObjectSetAndConditionFacetMismatch(ObjectSet, Parent.ObjectSets[ObjectSet].Facet, Condition, Parent.Conditions[Condition].Facet));
			if (throwOnFirst)
			{
				throw methodTraceContext.TraceThrow(new ObjectValidationException(ExceptionTemplatesSR.Policy, Name, ex3));
			}
			validationState.AddError(ex3, "ObjectSet");
		}
		((ISfcCollection)Parent.Policies).Initialized = true;
		ReadOnlyCollection<Policy> readOnlyCollection = Parent.ObjectSets[ObjectSet].EnumDependentPolicies();
		if (readOnlyCollection.Count > 1 || (readOnlyCollection.Count == 1 && !readOnlyCollection.Contains(this)))
		{
			Exception ex4 = new DmfException(ExceptionTemplatesSR.ObjectSetAlreadyReferenced(ObjectSet, readOnlyCollection[0].Name));
			if (throwOnFirst)
			{
				throw methodTraceContext.TraceThrow(new ObjectValidationException(ExceptionTemplatesSR.Policy, Name, ex4));
			}
			validationState.AddError(ex4, "ObjectSet");
		}
	}

	private void ValidateRootCondition(string validationMode, bool throwOnFirst, ValidationState validationState)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("ValidateRootCondition");
		methodTraceContext.TraceParameters(validationMode, throwOnFirst, validationState);
		if (throwOnFirst && !(validationMode == ValidationMethod.Rename) && !string.IsNullOrEmpty(RootCondition))
		{
			Condition condition = Parent.Conditions[RootCondition];
			if (condition == null)
			{
				Exception innerException = new MissingObjectException(ExceptionTemplatesSR.Condition, RootCondition);
				throw methodTraceContext.TraceThrow(new ObjectValidationException(ExceptionTemplatesSR.Policy, Name, innerException));
			}
			Type facetType = FacetRepository.GetFacetType(condition.Facet);
			if (!FacetRepository.IsRootFacet(SfcMetadataDiscovery.GetRootFromType(FacetRepository.GetFacetSupportedTypes(facetType)[0]), facetType))
			{
				Exception innerException2 = new MissingTypeFacetAssociationException(ExceptionTemplatesSR.RootCondition, Parent.Conditions[RootCondition].Facet);
				throw methodTraceContext.TraceThrow(new ObjectValidationException(ExceptionTemplatesSR.Policy, Name, innerException2));
			}
		}
	}

	public void Validate(string mode)
	{
		if (Parent == null)
		{
			throw traceContext.TraceThrow(new SfcMissingParentException());
		}
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

	private Condition GetValidCondition()
	{
		Condition condition = Parent.Conditions[Condition];
		if (condition == null || condition.ExpressionNode == null)
		{
			throw traceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.ConditionIsNull));
		}
		return condition;
	}

	private ObjectSet GetValidObjectSet()
	{
		return Parent.ObjectSets[ObjectSet] ?? throw traceContext.TraceThrow(new MissingObjectException("ObjectSet", ObjectSet));
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	private void ValidateConnectionCompatibility(string facet, params ISfcConnection[] targetConnections)
	{
		if (facet == "ISurfaceAreaConfigurationForAnalysisServer" || facet == "ISurfaceAreaConfigurationForReportingServices")
		{
			throw traceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.IncompatiblePolicyEvaluationMode));
		}
	}
}
