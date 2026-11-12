using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;

namespace Microsoft.SqlServer.Management.XEvent;

public sealed class Event : SfcInstance, ISfcDiscoverObject, ISessionObject
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
			return string.Format(CultureInfo.InvariantCulture, "{0}[@Name='{1}']", "Event", SfcSecureString.EscapeSquote(Name));
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
			return new Event();
		}
	}

	private class TypeMetadata : SfcTypeMetadata
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
			using MethodTraceContext methodTraceContext = TraceHelper.TraceContext.GetMethodContext("TypeMetadata.IsCrudActionHandledByParent");
			switch (depAction)
			{
			case SfcDependencyAction.Create:
			case SfcDependencyAction.Drop:
			case SfcDependencyAction.Alter:
				return true;
			case SfcDependencyAction.Rename:
			case SfcDependencyAction.Move:
				return false;
			default:
				methodTraceContext.TraceError("Unknown depAction.");
				throw new XEventException(ExceptionTemplates.InvalidParameter("depAction"));
			}
		}
	}

	public const string TypeTypeName = "Event";

	private IEventProvider eventProvider;

	private EventFieldCollection eventFields;

	private ActionCollection actions;

	private bool isPredicateXmlDirty = true;

	private PredExpr predicate;

	private bool isPredicateDirty;

	private IEventProvider ProviderImpl
	{
		get
		{
			if (eventProvider == null)
			{
				eventProvider = Parent.Parent.GetEventProviderInternal(this);
			}
			return eventProvider;
		}
	}

	[SfcObject(SfcObjectRelationship.ParentObject, SfcObjectCardinality.One)]
	public new Session Parent
	{
		get
		{
			return (Session)base.Parent;
		}
		set
		{
			base.Parent = value;
		}
	}

	[SfcIgnore]
	public Key IdentityKey => (Key)base.AbstractIdentityKey;

	[SfcObject(SfcContainerRelationship.ChildContainer, SfcContainerCardinality.ZeroToAny, typeof(EventField))]
	public EventFieldCollection EventFields
	{
		get
		{
			if (eventFields == null)
			{
				eventFields = new EventFieldCollection(this);
				if (SfcObjectState.Pending == State)
				{
					foreach (EventColumnInfo item in Parent.Parent.ObjectInfoSet.Get<EventInfo>(ModuleID, Name).EventColumnInfoSet)
					{
						if (!eventFields.Contains(item.Name))
						{
							EventField obj = new EventField(this, item);
							eventFields.Add(obj);
						}
					}
				}
			}
			return eventFields;
		}
	}

	[SfcObject(SfcContainerRelationship.ChildContainer, SfcContainerCardinality.ZeroToAny, typeof(Action))]
	public ActionCollection Actions
	{
		get
		{
			if (actions == null)
			{
				actions = new ActionCollection(this);
			}
			((ISfcCollection)actions).EnsureInitialized();
			return actions;
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
			throw new XEventException(ExceptionTemplates.CannotSetNameForExistingObject);
		}
	}

	[SfcProperty(SfcPropertyFlags.Required)]
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

	[SfcProperty(SfcPropertyFlags.Required)]
	public Guid ModuleID
	{
		get
		{
			object value = base.Properties["ModuleID"].Value;
			if (value is string g)
			{
				Guid guid = new Guid(g);
				base.Properties["ModuleID"].Value = guid;
				return guid;
			}
			return (Guid)value;
		}
		private set
		{
			base.Properties["ModuleID"].Value = value;
		}
	}

	[SfcProperty(SfcPropertyFlags.Required)]
	public string PackageName => (string)base.Properties["PackageName"].Value;

	[SfcProperty(SfcPropertyFlags.Required)]
	public string PredicateExpression
	{
		get
		{
			if (!isPredicateDirty)
			{
				object value = base.Properties["PredicateExpression"].Value;
				if (DBNull.Value == value)
				{
					return null;
				}
				return value as string;
			}
			if (predicate != null)
			{
				return Parent.Parent.FormatPredicateExpression(predicate);
			}
			return null;
		}
		set
		{
			CheckNullObject("PredicateExpression", value);
			base.Properties["PredicateExpression"].Value = value;
		}
	}

	[SfcProperty(SfcPropertyFlags.Data)]
	internal string PredicateXml
	{
		get
		{
			if (State == SfcObjectState.Existing && isPredicateXmlDirty)
			{
				Refresh();
				isPredicateXmlDirty = false;
			}
			object value = base.Properties["PredicateXml"].Value;
			if (DBNull.Value == value)
			{
				return null;
			}
			return value as string;
		}
	}

	[SfcProperty(Data = true)]
	public string Description
	{
		get
		{
			object value = base.Properties["Description"].Value;
			if (DBNull.Value == value)
			{
				return null;
			}
			return value as string;
		}
		private set
		{
			base.Properties["Description"].Value = value;
		}
	}

	public PredExpr Predicate
	{
		get
		{
			if (predicate == null && !isPredicateDirty)
			{
				predicate = PredExpr.ParsePredicateXml(Parent.Parent, PredicateXml);
			}
			return predicate;
		}
		set
		{
			predicate = value;
			isPredicateDirty = true;
		}
	}

	public string ScriptName
	{
		get
		{
			if (Parent.Parent.ObjectInfoSet.GetAll<EventInfo>(Name).Count > 1)
			{
				return string.Format(CultureInfo.InvariantCulture, "[{0}].{1}", ModuleID, Name);
			}
			return Name;
		}
	}

	[SfcIgnore]
	public new SfcObjectState State => base.State;

	public Event()
	{
	}

	public Event(Session parent, string name)
		: this(parent, parent.Parent.ObjectInfoSet.Get<EventInfo>(name))
	{
	}

	public Event(Session parent, EventInfo eventInfo)
	{
		using MethodTraceContext methodTraceContext = TraceHelper.TraceContext.GetMethodContext("Constructor");
		methodTraceContext.TraceParameterIn("parent", parent);
		methodTraceContext.TraceParameterIn("eventInfo", eventInfo);
		Parent = parent;
		SetEventInfo(eventInfo);
	}

	public void SetEventInfo(EventInfo eventInfo)
	{
		using MethodTraceContext methodTraceContext = TraceHelper.TraceContext.GetMethodContext("SetEventInfo");
		methodTraceContext.TraceParameterIn("eventInfo", eventInfo);
		if (SfcObjectState.Pending != State)
		{
			methodTraceContext.TraceError("set eventinfo for an event not in pending state");
			throw new XEventException(ExceptionTemplates.CannotSetEventInfoForExistingEvent);
		}
		if (eventInfo == null)
		{
			methodTraceContext.TraceError("eventInfo is null.");
			throw new ArgumentNullException("eventInfo");
		}
		Name = string.Format(CultureInfo.InvariantCulture, "{0}.{1}", eventInfo.Parent.Name, eventInfo.Name);
		SetPackage(eventInfo.Parent.Name);
		ModuleID = eventInfo.Parent.ModuleID;
		Description = eventInfo.Description;
	}

	private void SetPackage(string packageName)
	{
		base.Properties["PackageName"].Value = packageName;
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
		if (!(elementType == "EventField"))
		{
			if (elementType == "Action")
			{
				return Actions;
			}
			TraceHelper.TraceContext.TraceError("No such collection for type {0}", elementType);
			throw new XEventException(ExceptionTemplates.NoSuchCollection(elementType));
		}
		return EventFields;
	}

	public static SfcObjectFactory GetObjectFactory()
	{
		return ObjectFactory.Instance;
	}

	protected override void PostCreate(object executionResult)
	{
		base.PostCreate(executionResult);
		ResetPredicate();
	}

	protected override void PostAlter(object executionResult)
	{
		base.PostAlter(executionResult);
		ResetPredicate();
	}

	private void ResetPredicate()
	{
		isPredicateDirty = false;
		isPredicateXmlDirty = true;
	}

	private void CheckNullObject(string objectName, object obj)
	{
		if (obj == null)
		{
			TraceHelper.TraceContext.TraceError(string.Format(CultureInfo.InvariantCulture, "{0} value is null.", objectName));
			throw new ArgumentNullException(objectName);
		}
	}

	public Action AddAction(ActionInfo actionInfo)
	{
		using MethodTraceContext methodTraceContext = TraceHelper.TraceContext.GetMethodContext("AddAction:ActionInfo");
		if (actionInfo == null)
		{
			methodTraceContext.TraceError("actionInfo is null.");
			throw new ArgumentNullException("actionInfo");
		}
		Action action = new Action(this, actionInfo);
		Actions.Add(action);
		return action;
	}

	public Action AddAction(string actionName)
	{
		using MethodTraceContext methodTraceContext = TraceHelper.TraceContext.GetMethodContext("AddAction:ActionName");
		if (actionName == null || actionName.Trim().Length == 0)
		{
			methodTraceContext.TraceError("actionName is null or empty string.");
			throw new XEventException(ExceptionTemplates.NameNullEmpty);
		}
		Action action = new Action(this, actionName);
		Actions.Add(action);
		return action;
	}

	public bool RemoveAction(Action action)
	{
		using (MethodTraceContext methodTraceContext = TraceHelper.TraceContext.GetMethodContext("RemoveAction"))
		{
			if (action == null)
			{
				methodTraceContext.TraceError("actionInfo is null.");
				throw new ArgumentNullException("actionInfo");
			}
		}
		return Actions.Remove(action);
	}

	string ISessionObject.GetCreateScript()
	{
		return ProviderImpl.GetCreateScript();
	}

	public bool HasCustomizableField()
	{
		foreach (EventField eventField in EventFields)
		{
			if (eventField.Value != null)
			{
				return true;
			}
		}
		return false;
	}

	string ISessionObject.GetDropScript()
	{
		return ProviderImpl.GetDropScript();
	}

	bool ISessionObject.IsDirty()
	{
		foreach (Action action in Actions)
		{
			if (action.State == SfcObjectState.Pending || action.State == SfcObjectState.ToBeDropped)
			{
				return true;
			}
		}
		foreach (EventField eventField in EventFields)
		{
			if (eventField.Properties["Value"].Dirty)
			{
				return true;
			}
		}
		if (isPredicateDirty)
		{
			return true;
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
			if (Actions != null)
			{
				foreach (Action action in Actions)
				{
					if (action.State != SfcObjectState.ToBeDropped)
					{
						sink.Add(SfcDependencyDirection.Inbound, action, SfcTypeRelation.RequiredChild, discovered: false);
					}
				}
			}
			if (EventFields == null)
			{
				break;
			}
			{
				foreach (EventField eventField in EventFields)
				{
					if (eventField.State != SfcObjectState.ToBeDropped)
					{
						sink.Add(SfcDependencyDirection.Inbound, eventField, SfcTypeRelation.RequiredChild, discovered: false);
					}
				}
				break;
			}
		case SfcDependencyAction.Alter:
			if (Actions != null)
			{
				sink.Add(SfcDependencyDirection.Inbound, Actions.GetEnumerator(), SfcTypeRelation.RequiredChild, discovered: false);
			}
			if (EventFields != null)
			{
				sink.Add(SfcDependencyDirection.Inbound, EventFields.GetEnumerator(), SfcTypeRelation.RequiredChild, discovered: false);
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

	public static SfcTypeMetadata GetTypeMetadata()
	{
		return TypeMetadata.Instance;
	}
}
