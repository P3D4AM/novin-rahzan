using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;

namespace Microsoft.SqlServer.Management.XEvent;

public sealed class EventField : SfcInstance
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
			return string.Format(CultureInfo.InvariantCulture, "{0}[@Name='{1}']", "EventField", SfcSecureString.EscapeSquote(Name));
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
			return new EventField();
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

	public const string TypeTypeName = "EventField";

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
			throw new XEventException(ExceptionTemplates.CannotSetNameForExistingObject);
		}
	}

	[SfcProperty(Data = true)]
	public object Value
	{
		get
		{
			object value = base.Properties["Value"].Value;
			if (DBNull.Value == value)
			{
				return null;
			}
			return value;
		}
		set
		{
			base.Properties["Value"].Value = value;
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

	[SfcIgnore]
	public new SfcObjectState State => base.State;

	public EventField()
	{
	}

	public EventField(Event parent, EventColumnInfo eventColumnInfo)
	{
		using MethodTraceContext methodTraceContext = TraceHelper.TraceContext.GetMethodContext("Constructor");
		if (parent == null)
		{
			methodTraceContext.TraceError("Parameter parent is null.");
			throw new ArgumentNullException("parent");
		}
		if (eventColumnInfo == null)
		{
			methodTraceContext.TraceError("Parameter eventColumnInfo is null.");
			throw new ArgumentNullException("eventColumnInfo");
		}
		string text = eventColumnInfo.Parent.Parent.Name + "." + eventColumnInfo.Parent.Name;
		if (parent.Name != text)
		{
			methodTraceContext.TraceError("Parameter eventColumnInfo can't be used with parameter parent.");
			throw new XEventException(ExceptionTemplates.InvalidParameter("eventColumnInfo"));
		}
		Parent = parent;
		Name = eventColumnInfo.Name;
		Description = eventColumnInfo.Description;
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

	public static SfcTypeMetadata GetTypeMetadata()
	{
		return TypeMetadata.Instance;
	}
}
