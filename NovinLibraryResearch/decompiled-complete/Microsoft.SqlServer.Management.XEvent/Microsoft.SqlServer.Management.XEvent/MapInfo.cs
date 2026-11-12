using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;

namespace Microsoft.SqlServer.Management.XEvent;

public sealed class MapInfo : SfcInstance, IXEObjectInfo
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
			return string.Format(CultureInfo.InvariantCulture, "{0}[@Name='{1}']", "MapInfo", SfcSecureString.EscapeSquote(Name));
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
			return new MapInfo();
		}
	}

	public const string TypeTypeName = "MapInfo";

	private MapValueInfoCollection m_MapValues;

	[SfcObject(SfcObjectRelationship.ParentObject, SfcObjectCardinality.One)]
	public new Package Parent
	{
		get
		{
			return (Package)base.Parent;
		}
		set
		{
			base.Parent = value;
		}
	}

	[SfcIgnore]
	public Key IdentityKey => (Key)base.AbstractIdentityKey;

	[SfcObject(SfcContainerRelationship.ChildContainer, SfcContainerCardinality.ZeroToAny, typeof(MapValueInfo))]
	public MapValueInfoCollection MapValueInfoSet
	{
		get
		{
			if (m_MapValues == null)
			{
				m_MapValues = new MapValueInfoCollection(this);
			}
			return m_MapValues;
		}
	}

	[SfcProperty(SfcPropertyFlags.Required | SfcPropertyFlags.ReadOnlyAfterCreation)]
	[SfcKey(0)]
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

	internal MapInfo()
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
			result = new Key(Name);
		}
		return result;
	}

	protected override ISfcCollection GetChildCollection(string elementType)
	{
		if (elementType == "MapValueInfo")
		{
			return MapValueInfoSet;
		}
		TraceHelper.TraceContext.TraceError("No such collection for type {0}", elementType);
		throw new XEventException(ExceptionTemplates.NoSuchCollection(elementType));
	}

	public static SfcObjectFactory GetObjectFactory()
	{
		return ObjectFactory.Instance;
	}
}
