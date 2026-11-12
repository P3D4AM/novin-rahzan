using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;

namespace Microsoft.SqlServer.Management.XEvent;

public sealed class Session : SfcInstance, ISfcCreatable, ICreatable, ISfcAlterable, IAlterable, ISfcDroppable, IDroppable, ISfcValidate, ISfcDiscoverObject
{
	[Flags]
	private enum EventCapabilities
	{
		None = 0,
		Private = 1,
		NoBlock = 0x400
	}

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

		public Key(Dictionary<string, object> filedDict)
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
			return string.Format(CultureInfo.InvariantCulture, "{0}[@Name='{1}']", "Session", SfcSecureString.EscapeSquote(Name));
		}
	}

	internal sealed class ObjectFactory : SfcObjectFactory
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
			return new Session();
		}
	}

	public enum EventRetentionModeEnum
	{
		AllowSingleEventLoss,
		AllowMultipleEventLoss,
		NoEventLoss
	}

	public enum MemoryPartitionModeEnum
	{
		None,
		PerNode,
		PerCpu
	}

	public const string MaxMemoryProperty = "MaxMemory";

	public const string EventRetentionModeProperty = "EventRetentionMode";

	public const string MaxDispatchLatencyProperty = "MaxDispatchLatency";

	public const string MaxEventSizeProperty = "MaxEventSize";

	public const string MemoryPartitionModeProperty = "MemoryPartitionMode";

	public const string TrackCausalityProperty = "TrackCausality";

	public const string AutoStartProperty = "AutoStart";

	public const string TypeTypeName = "Session";

	private ISessionProvider sessionProvider;

	private bool statesInitialized;

	private EventCollection events;

	private TargetCollection targets;

	public const int InfiniteDispatchLatency = 0;

	public const int DefaultDispatchLatency = 30;

	public const int DefaultMaxMemory = 4096;

	public static readonly DateTime NotStarted = DateTime.MinValue;

	private ISessionProvider ProviderImpl
	{
		get
		{
			if (sessionProvider == null)
			{
				sessionProvider = Parent.GetSessionProviderInternal(this);
			}
			return sessionProvider;
		}
	}

	[SfcObject(SfcObjectRelationship.ParentObject, SfcObjectCardinality.One)]
	public new BaseXEStore Parent
	{
		get
		{
			return (BaseXEStore)base.Parent;
		}
		set
		{
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
			object value = base.Properties["ID"].Value;
			if (value == null)
			{
				return -1;
			}
			return (int)value;
		}
	}

	[SfcProperty(SfcPropertyFlags.Required | SfcPropertyFlags.ReadOnlyAfterCreation)]
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
				if (string.IsNullOrEmpty(value))
				{
					throw new XEventException(ExceptionTemplates.NameNullEmpty);
				}
				base.Properties["Name"].Value = value;
				return;
			}
			throw new XEventException(ExceptionTemplates.CannotSetNameForExistingObject + ExceptionTemplates.ChangeNameForExistingSession);
		}
	}

	[SfcObject(SfcContainerRelationship.ChildContainer, SfcContainerCardinality.ZeroToAny, typeof(Event))]
	public EventCollection Events
	{
		get
		{
			if (events == null)
			{
				events = new EventCollection(this);
			}
			((ISfcCollection)events).EnsureInitialized();
			return events;
		}
	}

	[SfcObject(SfcContainerRelationship.ChildContainer, SfcContainerCardinality.ZeroToAny, typeof(Target))]
	public TargetCollection Targets
	{
		get
		{
			if (targets == null)
			{
				targets = new TargetCollection(this);
			}
			((ISfcCollection)targets).EnsureInitialized();
			return targets;
		}
	}

	[SfcProperty(Data = true)]
	public bool IsRunning
	{
		get
		{
			object value = base.Properties["IsRunning"].Value;
			if (value == null)
			{
				return false;
			}
			return Convert.ToBoolean(value, CultureInfo.InvariantCulture);
		}
		private set
		{
			base.Properties["IsRunning"].Value = value;
		}
	}

	[SfcProperty(Data = true)]
	public EventRetentionModeEnum EventRetentionMode
	{
		get
		{
			object value = base.Properties["EventRetentionMode"].Value;
			if (value == null)
			{
				return EventRetentionModeEnum.AllowSingleEventLoss;
			}
			switch (value as string)
			{
			case "S":
				return EventRetentionModeEnum.AllowSingleEventLoss;
			case "M":
				return EventRetentionModeEnum.AllowMultipleEventLoss;
			case "N":
				return EventRetentionModeEnum.NoEventLoss;
			default:
				TraceHelper.TraceContext.TraceCriticalError("Unknown EventRetentionMode");
				throw new XEventException(ExceptionTemplates.InvalidParameter("EventRetentionMode"));
			}
		}
		set
		{
			string value2 = value switch
			{
				EventRetentionModeEnum.AllowSingleEventLoss => "S", 
				EventRetentionModeEnum.AllowMultipleEventLoss => "M", 
				EventRetentionModeEnum.NoEventLoss => "N", 
				_ => throw new XEventException(ExceptionTemplates.InvalidParameter("EventRetentionMode")), 
			};
			base.Properties["EventRetentionMode"].Value = value2;
		}
	}

	[SfcProperty(Data = true)]
	public int MaxDispatchLatency
	{
		get
		{
			object value = base.Properties["MaxDispatchLatency"].Value;
			if (value == null)
			{
				return 30;
			}
			return (int)value / 1000;
		}
		set
		{
			base.Properties["MaxDispatchLatency"].Value = value * 1000;
		}
	}

	[SfcProperty(Data = true)]
	public int MaxMemory
	{
		get
		{
			object value = base.Properties["MaxMemory"].Value;
			if (value == null)
			{
				return 4096;
			}
			return (int)value;
		}
		set
		{
			base.Properties["MaxMemory"].Value = value;
		}
	}

	[SfcProperty(Data = true)]
	public int MaxEventSize
	{
		get
		{
			object value = base.Properties["MaxEventSize"].Value;
			if (value == null)
			{
				return 0;
			}
			return (int)value;
		}
		set
		{
			base.Properties["MaxEventSize"].Value = value;
		}
	}

	[SfcProperty(Data = true)]
	public MemoryPartitionModeEnum MemoryPartitionMode
	{
		get
		{
			object value = base.Properties["MemoryPartitionMode"].Value;
			if (value == null)
			{
				return MemoryPartitionModeEnum.None;
			}
			switch (value as string)
			{
			case "G":
				return MemoryPartitionModeEnum.None;
			case "N":
				return MemoryPartitionModeEnum.PerNode;
			case "C":
				return MemoryPartitionModeEnum.PerCpu;
			default:
				TraceHelper.TraceContext.TraceCriticalError("Unrecognized MemoryPartitionMode");
				throw new XEventException(ExceptionTemplates.InvalidParameter("MemoryPartitionMode"));
			}
		}
		set
		{
			string value2 = value switch
			{
				MemoryPartitionModeEnum.None => "G", 
				MemoryPartitionModeEnum.PerNode => "N", 
				MemoryPartitionModeEnum.PerCpu => "C", 
				_ => throw new XEventException(ExceptionTemplates.InvalidParameter("MemoryPartitionMode")), 
			};
			base.Properties["MemoryPartitionMode"].Value = value2;
		}
	}

	[SfcProperty(SfcPropertyFlags.Required)]
	public bool TrackCausality
	{
		get
		{
			object value = base.Properties["TrackCausality"].Value;
			if (value == null)
			{
				return false;
			}
			return Convert.ToBoolean(value, CultureInfo.InvariantCulture);
		}
		set
		{
			base.Properties["TrackCausality"].Value = (value ? 1 : 0);
		}
	}

	[SfcProperty(SfcPropertyFlags.Required)]
	public bool AutoStart
	{
		get
		{
			object value = base.Properties["AutoStart"].Value;
			if (value == null)
			{
				return false;
			}
			return Convert.ToBoolean(value, CultureInfo.InvariantCulture);
		}
		set
		{
			base.Properties["AutoStart"].Value = (value ? 1 : 0);
		}
	}

	[SfcProperty(Data = true)]
	public DateTime StartTime
	{
		get
		{
			object value = base.Properties["StartTime"].Value;
			if (value == null)
			{
				return NotStarted;
			}
			return (DateTime)value;
		}
	}

	[SfcIgnore]
	public new SfcObjectState State => base.State;

	public Session()
	{
	}

	public Session(BaseXEStore parent, string name)
	{
		using MethodTraceContext methodTraceContext = TraceHelper.TraceContext.GetMethodContext("constructor");
		methodTraceContext.TraceParameterIn("parent", parent);
		methodTraceContext.TraceParameterIn("name", name);
		base.Parent = parent;
		Name = name;
	}

	protected override void InitializeUIPropertyState()
	{
		if (!statesInitialized)
		{
			statesInitialized = true;
			UpdateUIPropertyState();
		}
	}

	protected override void UpdateUIPropertyState()
	{
		if (statesInitialized)
		{
			SfcProperty sfcProperty = base.Properties["Name"];
			if (State == SfcObjectState.Pending)
			{
				sfcProperty.Enabled = true;
			}
			else
			{
				sfcProperty.Enabled = false;
			}
		}
	}

	private void ValidateName(bool throwOnFirst, ValidationState validationState)
	{
		if (string.IsNullOrEmpty(Name))
		{
			TraceHelper.TraceContext.TraceError("Name can not be null or empty.");
			Exception ex = new XEventException(ExceptionTemplates.NameNullEmpty);
			if (throwOnFirst)
			{
				throw ex;
			}
			validationState.AddError(ex, "Name");
		}
	}

	private void Validate(string methodName, bool throwOnFirst, ValidationState validationState)
	{
		ValidateName(throwOnFirst, validationState);
		Exception ex = null;
		if (EventRetentionMode == EventRetentionModeEnum.NoEventLoss)
		{
			foreach (Event @event in Events)
			{
				if (XEUtils.ToBeCreated(@event.State) && ((uint)Parent.ObjectInfoSet.Get<EventInfo>(@event.ModuleID, @event.Name).Capabilities & 0x400u) != 0)
				{
					ex = new XEventException(ExceptionTemplates.NoBlockingEventNotAllowedInNoEventLossSession(Name, @event.Name));
					if (throwOnFirst)
					{
						throw ex;
					}
					validationState.AddError(ex, "Events");
				}
			}
		}
		if (methodName == ValidationMethod.Create && Events.Count == 0)
		{
			TraceHelper.TraceContext.TraceError("Cannot create a session without any event.");
			ex = new XEventException(ExceptionTemplates.NewSessionMustContainEvent);
			if (throwOnFirst)
			{
				throw ex;
			}
			validationState.AddError(ex, "EventCount");
		}
		if (methodName == ValidationMethod.Alter && State != SfcObjectState.Existing)
		{
			ex = new XEventException(ExceptionTemplates.InvalidState(State, SfcObjectState.Existing));
			if (throwOnFirst)
			{
				throw ex;
			}
			validationState.AddError(ex, "State");
		}
	}

	public void Validate(string methodName)
	{
		using MethodTraceContext methodTraceContext = TraceHelper.TraceContext.GetMethodContext("Validate(string)");
		methodTraceContext.TraceParameterIn("methodName", methodName);
		if (methodName != ValidationMethod.Create && methodName != ValidationMethod.Alter)
		{
			methodTraceContext.TraceError("Invalid parameter.");
			throw new XEventException(ExceptionTemplates.InvalidParameter(methodName));
		}
		if (Parent == null)
		{
			methodTraceContext.TraceError("Parent can't be null.");
			throw new XEventException(ExceptionTemplates.ParentNull);
		}
		Validate(methodName, throwOnFirst: true, null);
	}

	public ValidationState Validate(string methodName, params object[] arguments)
	{
		using MethodTraceContext methodTraceContext = TraceHelper.TraceContext.GetMethodContext("Validate");
		methodTraceContext.TraceParameterIn("methodName", methodName);
		methodTraceContext.TraceParameterIn("arguments", arguments);
		ValidationState validationState = new ValidationState();
		Validate(methodName, throwOnFirst: false, validationState);
		return validationState;
	}

	public void Create()
	{
		TraceHelper.TraceContext.TraceMethodEnter("Create");
		Validate(ValidationMethod.Create);
		CreateImpl();
		TraceHelper.TraceContext.TraceMethodExit("Create");
	}

	public ISfcScript ScriptCreate()
	{
		Validate(ValidationMethod.Create);
		return ProviderImpl.GetCreateScript();
	}

	public void Alter()
	{
		TraceHelper.TraceContext.TraceMethodEnter("Alter");
		Validate(ValidationMethod.Alter);
		if (IsDirty())
		{
			ProviderImpl.ValidateAlter();
			AlterImpl();
			TraceHelper.TraceContext.TraceMethodExit("Alter");
		}
	}

	public ISfcScript ScriptAlter()
	{
		Validate(ValidationMethod.Alter);
		return ProviderImpl.GetAlterScript();
	}

	public void Drop()
	{
		TraceHelper.TraceContext.TraceMethodEnter("Drop");
		ValidateName(throwOnFirst: true, null);
		DropImpl();
		TraceHelper.TraceContext.TraceMethodExit("Drop");
	}

	public ISfcScript ScriptDrop()
	{
		ValidateName(throwOnFirst: true, null);
		return ProviderImpl.GetDropScript();
	}

	public void Start()
	{
		ValidateName(throwOnFirst: true, null);
		ProviderImpl.Start();
		IsRunning = true;
		SfcApplication.Events.OnObjectAltered(this, new SfcObjectAlteredEventArgs(base.Urn, this));
	}

	public void Stop()
	{
		ValidateName(throwOnFirst: true, null);
		ProviderImpl.Stop();
		IsRunning = false;
		SfcApplication.Events.OnObjectAltered(this, new SfcObjectAlteredEventArgs(base.Urn, this));
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

	protected override ISfcCollection GetChildCollection(string elementType)
	{
		if (!(elementType == "Event"))
		{
			if (elementType == "Target")
			{
				return Targets;
			}
			TraceHelper.TraceContext.TraceError("No such collection for type {0}", elementType);
			throw new XEventException(ExceptionTemplates.NoSuchCollection(elementType));
		}
		return Events;
	}

	public static SfcObjectFactory GetObjectFactory()
	{
		return ObjectFactory.Instance;
	}

	public Event AddEvent(EventInfo eventInfo)
	{
		Event @event = new Event(this, eventInfo);
		Events.Add(@event);
		return @event;
	}

	public Event AddEvent(string eventName)
	{
		Event @event = new Event(this, eventName);
		Events.Add(@event);
		return @event;
	}

	public bool RemoveEvent(Event evt)
	{
		if (evt == null)
		{
			throw new ArgumentNullException("evt");
		}
		return Events.Remove(evt);
	}

	public Target AddTarget(TargetInfo targetInfo)
	{
		Target target = new Target(this, targetInfo);
		Targets.Add(target);
		return target;
	}

	public Target AddTarget(string targetName)
	{
		Target target = new Target(this, targetName);
		Targets.Add(target);
		return target;
	}

	public bool RemoveTarget(Target target)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		return Targets.Remove(target);
	}

	private bool IsDirty()
	{
		foreach (Event @event in Events)
		{
			if (@event.State == SfcObjectState.Pending || @event.State == SfcObjectState.ToBeDropped || (@event.State == SfcObjectState.Existing && ((ISessionObject)@event).IsDirty()) || @event.State == SfcObjectState.Recreate)
			{
				return true;
			}
		}
		foreach (Target target in Targets)
		{
			if (target.State == SfcObjectState.Pending || target.State == SfcObjectState.ToBeDropped || (target.State == SfcObjectState.Existing && ((ISessionObject)target).IsDirty()) || target.State == SfcObjectState.Recreate)
			{
				return true;
			}
		}
		foreach (SfcProperty property in base.Properties)
		{
			if (property.Dirty)
			{
				return true;
			}
		}
		return false;
	}

	void ISfcDiscoverObject.Discover(ISfcDependencyDiscoveryObjectSink sink)
	{
		if (sink == null)
		{
			throw new ArgumentNullException("sink");
		}
		switch (sink.Action)
		{
		case SfcDependencyAction.Create:
			if (Events != null)
			{
				foreach (Event @event in Events)
				{
					if (@event.State != SfcObjectState.ToBeDropped)
					{
						sink.Add(SfcDependencyDirection.Inbound, @event, SfcTypeRelation.RequiredChild, discovered: false);
					}
				}
			}
			if (Targets == null)
			{
				break;
			}
			{
				foreach (Target target in Targets)
				{
					if (target.State != SfcObjectState.ToBeDropped)
					{
						sink.Add(SfcDependencyDirection.Inbound, target, SfcTypeRelation.RequiredChild, discovered: false);
					}
				}
				break;
			}
		case SfcDependencyAction.Alter:
			if (Events != null)
			{
				sink.Add(SfcDependencyDirection.Inbound, Events.GetEnumerator(), SfcTypeRelation.RequiredChild, discovered: false);
			}
			if (Targets != null)
			{
				sink.Add(SfcDependencyDirection.Inbound, Targets.GetEnumerator(), SfcTypeRelation.RequiredChild, discovered: false);
			}
			break;
		case SfcDependencyAction.Drop:
		case SfcDependencyAction.Rename:
		case SfcDependencyAction.Move:
			break;
		default:
			TraceHelper.TraceContext.TraceError("Unknown sink.Action.");
			throw new XEventException(ExceptionTemplates.InvalidParameter("sink.Action"));
		}
	}
}
