using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.SqlServer.Diagnostics.STrace;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.Dmf;

public sealed class TargetSetLevel : SfcInstance, IComparable<TargetSetLevel>
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
			return string.Format("{0}[@TargetTypeSkeleton='{1}']", "TargetSetLevel", targetTypeSkeleton);
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
			return new TargetSetLevel();
		}
	}

	private static TraceContext traceContext = TraceContext.GetTraceContext("DMF", "TargetSetLevel");

	private Type m_type;

	internal const string typeName = "TargetSetLevel";

	public Type TargetType
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

	[SfcObject(SfcObjectRelationship.ParentObject, SfcObjectCardinality.One)]
	public new TargetSet Parent
	{
		get
		{
			return (TargetSet)base.Parent;
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
	private int TargetSetID => Parent.ID;

	[SfcProperty(SfcPropertyFlags.Required)]
	[SfcKey(0)]
	public string TargetTypeSkeleton => (string)base.Properties["TargetTypeSkeleton"].Value;

	[SfcProperty(SfcPropertyFlags.Required)]
	public string LevelName => (string)base.Properties["LevelName"].Value;

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

	public TargetSetLevel()
	{
	}

	internal TargetSetLevel(TargetSet parent, string TargetTypeSkeleton)
	{
		traceContext.TraceMethodEnter("TargetSetLevel");
		traceContext.TraceParameters(TargetTypeSkeleton);
		Parent = parent;
		base.Properties["TargetTypeSkeleton"].Value = TargetTypeSkeleton;
		m_type = Utils.GetTypeFromUrnSkeleton(TargetTypeSkeleton);
		Urn urn = new Urn(TargetTypeSkeleton);
		base.Properties["LevelName"].Value = urn.Type;
		Condition = string.Empty;
		traceContext.TraceMethodExit("TargetSetLevel");
	}

	public override bool Equals(object obj)
	{
		if (obj == null || !(obj is TargetSetLevel))
		{
			return false;
		}
		return TargetTypeSkeleton.Equals(((TargetSetLevel)obj).TargetTypeSkeleton);
	}

	public override int GetHashCode()
	{
		return TargetTypeSkeleton.GetHashCode();
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	public int CompareTo(TargetSetLevel level)
	{
		if (level == null)
		{
			throw new ArgumentNullException("level");
		}
		return TargetTypeSkeleton.Length - level.TargetTypeSkeleton.Length;
	}

	internal static SfcTypeMetadata GetTypeMetadata()
	{
		return TypeMetadata.Instance;
	}

	public static PropertyInfo[] GetTypeFilterProperties(string skeleton)
	{
		return SmoDmfAdapter.GetTypeFilterProperties(skeleton);
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
		throw traceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.NoSuchCollection(elementType)));
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	public override void Discover(ISfcDependencyDiscoveryObjectSink sink)
	{
		if (sink == null)
		{
			throw traceContext.TraceThrow(new ArgumentNullException("sink"));
		}
		if (sink.Action == SfcDependencyAction.Serialize)
		{
			ConditionCollection conditions = Parent.Parent.Parent.Conditions;
			if (!string.IsNullOrEmpty(Condition) && conditions.Contains(Condition))
			{
				sink.Add(SfcDependencyDirection.Outbound, conditions[Condition], SfcTypeRelation.StrongReference, discovered: false);
			}
		}
	}
}
