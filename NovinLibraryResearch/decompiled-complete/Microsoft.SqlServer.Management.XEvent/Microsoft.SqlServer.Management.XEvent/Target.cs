using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;

namespace Microsoft.SqlServer.Management.XEvent;

public sealed class Target : SfcInstance, ISfcDiscoverObject, ISessionObject
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
			return string.Format(CultureInfo.InvariantCulture, "{0}[@Name='{1}']", "Target", SfcSecureString.EscapeSquote(Name));
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
			return new Target();
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

	public const string TypeTypeName = "Target";

	private ITargetProvider targetProvider;

	internal string eventModuleID;

	private TargetFieldCollection targetFields;

	private ITargetProvider ProviderImpl
	{
		get
		{
			if (targetProvider == null)
			{
				targetProvider = Parent.Parent.GetTargetProviderInternal(this);
			}
			return targetProvider;
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

	[SfcObject(SfcContainerRelationship.ChildContainer, SfcContainerCardinality.ZeroToAny, typeof(TargetField))]
	public TargetFieldCollection TargetFields
	{
		get
		{
			if (targetFields == null)
			{
				targetFields = new TargetFieldCollection(this);
				if (SfcObjectState.Pending == State)
				{
					Name.Substring(Name.LastIndexOf('.') + 1);
					foreach (TargetColumnInfo item in Parent.Parent.ObjectInfoSet.Get<TargetInfo>(ModuleID, Name).TargetColumnInfoSet)
					{
						if (!targetFields.Contains(item.Name))
						{
							TargetField obj = new TargetField(this, item);
							targetFields.Add(obj);
						}
					}
				}
			}
			return targetFields;
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

	public string ScriptName
	{
		get
		{
			if (Parent.Parent.ObjectInfoSet.GetAll<TargetInfo>(Name).Count > 1)
			{
				return string.Format(CultureInfo.InvariantCulture, "[{0}].{1}", ModuleID, Name);
			}
			return Name;
		}
	}

	[SfcIgnore]
	public new SfcObjectState State => base.State;

	public Target()
	{
	}

	public Target(Session parent, string name)
		: this(parent, parent.Parent.ObjectInfoSet.Get<TargetInfo>(name))
	{
	}

	public Target(Session parent, TargetInfo targetInfo)
	{
		using MethodTraceContext methodTraceContext = TraceHelper.TraceContext.GetMethodContext("Constructor");
		methodTraceContext.TraceParameterIn("parent", parent);
		methodTraceContext.TraceParameterIn("targetInfo", targetInfo);
		Parent = parent;
		SetTargetInfo(targetInfo);
	}

	public void SetTargetInfo(TargetInfo targetInfo)
	{
		using MethodTraceContext methodTraceContext = TraceHelper.TraceContext.GetMethodContext("SetTargetInfo");
		methodTraceContext.TraceParameterIn("targetInfo", targetInfo);
		if (SfcObjectState.Pending != State)
		{
			methodTraceContext.TraceError("set targetInfo for an target not in pending state");
			throw new XEventException(ExceptionTemplates.CannotSetTargetInfoForExistingTarget);
		}
		if (targetInfo == null)
		{
			methodTraceContext.TraceError("targetInfo is null.");
			throw new ArgumentNullException("targetInfo");
		}
		Name = string.Format(CultureInfo.InvariantCulture, "{0}.{1}", targetInfo.Parent.Name, targetInfo.Name);
		SetPackage(targetInfo.Parent.Name);
		ModuleID = targetInfo.Parent.ModuleID;
		Description = targetInfo.Description;
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
		if (elementType == "TargetField")
		{
			return TargetFields;
		}
		TraceHelper.TraceContext.TraceError("No such collection for type {0}", elementType);
		throw new XEventException(ExceptionTemplates.NoSuchCollection(elementType));
	}

	public string GetTargetData()
	{
		return ProviderImpl.GetTargetData();
	}

	public static SfcObjectFactory GetObjectFactory()
	{
		return ObjectFactory.Instance;
	}

	string ISessionObject.GetCreateScript()
	{
		return ProviderImpl.GetCreateScript();
	}

	public bool HasCustomizableField()
	{
		foreach (TargetField targetField in TargetFields)
		{
			if (targetField.Value != null)
			{
				return true;
			}
		}
		return false;
	}

	string ISessionObject.GetDropScript()
	{
		return string.Format(CultureInfo.InvariantCulture, "DROP TARGET {0}", ScriptName);
	}

	bool ISessionObject.IsDirty()
	{
		foreach (TargetField targetField in TargetFields)
		{
			if (targetField.Properties["Value"].Dirty)
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
			if (TargetFields == null)
			{
				break;
			}
			{
				foreach (TargetField targetField in TargetFields)
				{
					if (targetField.State != SfcObjectState.ToBeDropped)
					{
						sink.Add(SfcDependencyDirection.Inbound, targetField, SfcTypeRelation.RequiredChild, discovered: false);
					}
				}
				break;
			}
		case SfcDependencyAction.Drop:
		case SfcDependencyAction.Alter:
			if (TargetFields != null)
			{
				sink.Add(SfcDependencyDirection.Inbound, TargetFields.GetEnumerator(), SfcTypeRelation.RequiredChild, discovered: false);
			}
			break;
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
