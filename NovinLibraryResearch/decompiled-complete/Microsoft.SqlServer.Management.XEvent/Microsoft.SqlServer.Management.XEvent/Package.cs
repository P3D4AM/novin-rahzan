using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;

namespace Microsoft.SqlServer.Management.XEvent;

public sealed class Package : SfcInstance
{
	public sealed class Key : SfcKey
	{
		private string Name { get; set; }

		private string ModuleID { get; set; }

		public Key()
		{
		}

		public Key(Key other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			Name = other.Name;
			ModuleID = other.ModuleID;
		}

		public Key(string guid, string name)
		{
			Name = name;
			ModuleID = guid;
		}

		public Key(Dictionary<string, object> filedDict)
		{
			Name = (string)filedDict["Name"];
			ModuleID = (string)filedDict["ModuleID"];
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
			return Name.GetHashCode() ^ ModuleID.GetHashCode();
		}

		private bool IsEqual(Key key)
		{
			if (string.CompareOrdinal(Name, key.Name) == 0)
			{
				return string.CompareOrdinal(ModuleID.ToUpperInvariant(), key.ModuleID.ToUpperInvariant()) == 0;
			}
			return false;
		}

		public override string GetUrnFragment()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}[@Name='{1}' and @ModuleID='{2}']", "Package", SfcSecureString.EscapeSquote(Name), ModuleID.ToUpperInvariant());
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
			return new Package();
		}
	}

	public const string TypeTypeName = "Package";

	private EventInfoCollection eventInfoSet;

	private ActionInfoCollection actionInfoSet;

	private TargetInfoCollection targetInfoSet;

	private TypeInfoCollection typeInfoSet;

	private MapInfoCollection mapInfoSet;

	private PredSourceInfoCollection predSourceInfoSet;

	private PredCompareInfoCollection predCompareInfoSet;

	[SfcIgnore]
	public Key IdentityKey => (Key)base.AbstractIdentityKey;

	[SfcObject(SfcContainerRelationship.ChildContainer, SfcContainerCardinality.ZeroToAny, typeof(EventInfo))]
	public EventInfoCollection EventInfoSet
	{
		get
		{
			if (eventInfoSet == null)
			{
				eventInfoSet = new EventInfoCollection(this);
			}
			return eventInfoSet;
		}
	}

	[SfcObject(SfcContainerRelationship.ChildContainer, SfcContainerCardinality.ZeroToAny, typeof(ActionInfo))]
	public ActionInfoCollection ActionInfoSet
	{
		get
		{
			if (actionInfoSet == null)
			{
				actionInfoSet = new ActionInfoCollection(this);
			}
			return actionInfoSet;
		}
	}

	[SfcObject(SfcContainerRelationship.ChildContainer, SfcContainerCardinality.ZeroToAny, typeof(TargetInfo))]
	public TargetInfoCollection TargetInfoSet
	{
		get
		{
			if (targetInfoSet == null)
			{
				targetInfoSet = new TargetInfoCollection(this);
			}
			return targetInfoSet;
		}
	}

	[SfcObject(SfcContainerRelationship.ChildContainer, SfcContainerCardinality.ZeroToAny, typeof(TypeInfo))]
	public TypeInfoCollection TypeInfoSet
	{
		get
		{
			if (typeInfoSet == null)
			{
				typeInfoSet = new TypeInfoCollection(this);
			}
			return typeInfoSet;
		}
	}

	[SfcObject(SfcContainerRelationship.ChildContainer, SfcContainerCardinality.ZeroToAny, typeof(MapInfo))]
	public MapInfoCollection MapInfoSet
	{
		get
		{
			if (mapInfoSet == null)
			{
				mapInfoSet = new MapInfoCollection(this);
			}
			return mapInfoSet;
		}
	}

	[SfcObject(SfcContainerRelationship.ChildContainer, SfcContainerCardinality.ZeroToAny, typeof(PredSourceInfo))]
	public PredSourceInfoCollection PredSourceInfoSet
	{
		get
		{
			if (predSourceInfoSet == null)
			{
				predSourceInfoSet = new PredSourceInfoCollection(this);
			}
			return predSourceInfoSet;
		}
	}

	[SfcObject(SfcContainerRelationship.ChildContainer, SfcContainerCardinality.ZeroToAny, typeof(PredCompareInfo))]
	public PredCompareInfoCollection PredCompareInfoSet
	{
		get
		{
			if (predCompareInfoSet == null)
			{
				predCompareInfoSet = new PredCompareInfoCollection(this);
			}
			return predCompareInfoSet;
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

	[SfcProperty(SfcPropertyFlags.Required)]
	public Guid ID => (Guid)base.Properties["ID"].Value;

	[SfcProperty(SfcPropertyFlags.Required | SfcPropertyFlags.ReadOnlyAfterCreation)]
	[SfcKey(1)]
	public string Name => (string)base.Properties["Name"].Value;

	[SfcProperty(SfcPropertyFlags.Required)]
	public string Description => base.Properties["Description"].Value as string;

	[SfcProperty(SfcPropertyFlags.Required)]
	public int Capabilities => (int)base.Properties["Capabilities"].Value;

	[SfcProperty(SfcPropertyFlags.Required)]
	public string CapabilitiesDesc
	{
		get
		{
			object value = base.Properties["CapabilitiesDesc"].Value;
			if (DBNull.Value == value)
			{
				return null;
			}
			return value as string;
		}
	}

	[SfcProperty(SfcPropertyFlags.Required | SfcPropertyFlags.ReadOnlyAfterCreation)]
	[SfcKey(0)]
	public Guid ModuleID => (Guid)base.Properties["ModuleID"].Value;

	[SfcProperty(Data = true)]
	public byte[] ModuleAddress => (byte[])base.Properties["ModuleAddress"].Value;

	internal Package()
	{
	}

	private void SetName(string name)
	{
		base.Properties["Name"].Value = name;
	}

	protected override SfcKey CreateIdentityKey()
	{
		Key result = null;
		if (Name != null)
		{
			result = new Key(ModuleID.ToString(), Name);
		}
		return result;
	}

	protected override ISfcCollection GetChildCollection(string elementType)
	{
		switch (elementType)
		{
		case "EventInfo":
			return EventInfoSet;
		case "ActionInfo":
			return ActionInfoSet;
		case "TargetInfo":
			return TargetInfoSet;
		case "TypeInfo":
			return TypeInfoSet;
		case "PredSourceInfo":
			return PredSourceInfoSet;
		case "PredCompareInfo":
			return PredCompareInfoSet;
		case "MapInfo":
			return MapInfoSet;
		default:
			TraceHelper.TraceContext.TraceError("No such collection for type {0}", elementType);
			throw new XEventException(ExceptionTemplates.NoSuchCollection(elementType));
		}
	}

	internal IXEObjectInfoCollection<T> GetChildCollection<T>() where T : SfcInstance, IXEObjectInfo
	{
		return GetChildCollection(typeof(T).Name) as IXEObjectInfoCollection<T>;
	}

	public static SfcObjectFactory GetObjectFactory()
	{
		return ObjectFactory.Instance;
	}
}
