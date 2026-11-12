using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;

namespace Microsoft.SqlServer.Management.XEvent;

public sealed class Action : SfcInstance
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
			return string.Format(CultureInfo.InvariantCulture, "{0}[@Name='{1}']", "Action", SfcSecureString.EscapeSquote(Name));
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
			return new Action();
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

	public const string TypeTypeName = "Action";

	[SfcObject(SfcObjectRelationship.ParentObject, SfcObjectCardinality.One)]
	public new Event Parent
	{
		get
		{
			return (Event)base.Parent;
		}
		set
		{
			base.Parent = value;
		}
	}

	[SfcIgnore]
	public Key IdentityKey => (Key)base.AbstractIdentityKey;

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

	[SfcIgnore]
	public new SfcObjectState State => base.State;

	public Action()
	{
	}

	public Action(Event parent, string name)
		: this(parent, parent.Parent.Parent.ObjectInfoSet.Get<ActionInfo>(name))
	{
	}

	public Action(Event parent, ActionInfo actionInfo)
	{
		using MethodTraceContext methodTraceContext = TraceHelper.TraceContext.GetMethodContext("Constructor");
		methodTraceContext.TraceParameterIn("parent", parent);
		methodTraceContext.TraceParameterIn("actionInfo", actionInfo);
		Parent = parent;
		SetActionInfo(actionInfo);
	}

	public void SetActionInfo(ActionInfo actionInfo)
	{
		using MethodTraceContext methodTraceContext = TraceHelper.TraceContext.GetMethodContext("SetActionInfo");
		methodTraceContext.TraceParameterIn("actionInfo", actionInfo);
		if (SfcObjectState.Pending != State)
		{
			methodTraceContext.TraceError("set actionInfo for an action not in pending state");
			throw new XEventException(ExceptionTemplates.CannotSetActionInfoForExistingAction);
		}
		if (actionInfo == null)
		{
			methodTraceContext.TraceError("actionInfo is null.");
			throw new ArgumentNullException("actionInfo");
		}
		Name = string.Format(CultureInfo.InvariantCulture, "{0}.{1}", actionInfo.Parent.Name, actionInfo.Name);
		SetPackage(actionInfo.Parent.Name);
		ModuleID = actionInfo.Parent.ModuleID;
		Description = actionInfo.Description;
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
		TraceHelper.TraceContext.TraceError("No such collection for type {0}", elementType);
		throw new XEventException(ExceptionTemplates.NoSuchCollection(elementType));
	}

	public static SfcObjectFactory GetObjectFactory()
	{
		return ObjectFactory.Instance;
	}

	public string GetScriptCreate()
	{
		if (Parent.Parent.Parent.ObjectInfoSet.GetAll<ActionInfo>(Name).Count > 1)
		{
			return string.Format(CultureInfo.InvariantCulture, "[{0}].{1}", ModuleID, Name);
		}
		return Name;
	}

	public static SfcTypeMetadata GetTypeMetadata()
	{
		return TypeMetadata.Instance;
	}
}
