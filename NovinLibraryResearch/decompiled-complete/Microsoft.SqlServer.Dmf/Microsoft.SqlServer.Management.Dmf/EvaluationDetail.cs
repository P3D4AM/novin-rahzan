using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Diagnostics.STrace;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;

namespace Microsoft.SqlServer.Management.Dmf;

public sealed class EvaluationDetail : SfcInstance
{
	public sealed class Key : SfcKey
	{
		private long id;

		public long ID
		{
			get
			{
				return id;
			}
			set
			{
				id = value;
			}
		}

		public Key()
		{
		}

		public Key(Key other)
		{
			ID = other.ID;
		}

		public Key(long id)
		{
			ID = id;
		}

		internal Key(Dictionary<string, object> filedDict)
		{
			ID = Convert.ToInt64(filedDict["ID"]);
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
			return ID.GetHashCode();
		}

		private bool IsEqual(Key key)
		{
			return ID == key.ID;
		}

		public override string GetUrnFragment()
		{
			return string.Format("{0}[@ID='{1}']", "EvaluationDetail", ID);
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
			return new EvaluationDetail();
		}
	}

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
			throw traceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.UnsupportedCrudDependencyAction(depAction.ToString())));
		}
	}

	private static TraceContext traceContext = TraceContext.GetTraceContext("DMF", "EvaluationDetail");

	internal const string typeName = "EvaluationDetail";

	[SfcProperty(SfcPropertyFlags.Data)]
	public int HistoryId
	{
		get
		{
			object value = base.Properties["HistoryId"].Value;
			if (value == null)
			{
				return 0;
			}
			return (int)value;
		}
	}

	[SfcProperty(SfcPropertyFlags.None)]
	public DateTime EvaluationDate
	{
		get
		{
			object value = base.Properties["EvaluationDate"].Value;
			if (value == null)
			{
				return DateTime.MinValue;
			}
			return (DateTime)value;
		}
		internal set
		{
			traceContext.TraceVerbose("Setting EvaluationDate to: {0}", value);
			base.Properties["EvaluationDate"].Value = value;
		}
	}

	[SfcProperty(SfcPropertyFlags.None)]
	public string Exception
	{
		get
		{
			object value = base.Properties["Exception"].Value;
			if (value == null)
			{
				return string.Empty;
			}
			return (string)value;
		}
		internal set
		{
			traceContext.TraceVerbose("Setting Exception to: {0}", value);
			base.Properties["Exception"].Value = value;
		}
	}

	[SfcProperty(SfcPropertyFlags.None)]
	public string ResultDetail
	{
		get
		{
			object value = base.Properties["ResultDetail"].Value;
			if (value == null)
			{
				return string.Empty;
			}
			return (string)value;
		}
		internal set
		{
			traceContext.TraceVerbose("Setting ResultDetail to: {0}", value);
			base.Properties["ResultDetail"].Value = value;
		}
	}

	[SfcProperty(SfcPropertyFlags.None)]
	public string TargetQueryExpression
	{
		get
		{
			object value = base.Properties["TargetQueryExpression"].Value;
			if (value == null)
			{
				return string.Empty;
			}
			return (string)value;
		}
		internal set
		{
			traceContext.TraceVerbose("Setting TargetQueryExpression to: {0}", value);
			base.Properties["TargetQueryExpression"].Value = value;
		}
	}

	[SfcObject(SfcObjectRelationship.ParentObject, SfcObjectCardinality.One)]
	public new ConnectionEvaluationHistory Parent
	{
		get
		{
			return (ConnectionEvaluationHistory)base.Parent;
		}
		internal set
		{
			traceContext.TraceVerbose("Setting Parent to: {0}", value);
			base.Parent = value;
		}
	}

	[SfcProperty(SfcPropertyFlags.None)]
	[SfcKey(0)]
	public long ID
	{
		get
		{
			object value = base.Properties["ID"].Value;
			if (value == null)
			{
				return 0L;
			}
			return (long)value;
		}
		internal set
		{
			traceContext.TraceVerbose("Setting ID to: {0}", value);
			base.Properties["ID"].Value = value;
		}
	}

	[SfcIgnore]
	public Key IdentityKey => (Key)base.AbstractIdentityKey;

	[SfcProperty(SfcPropertyFlags.None)]
	public bool Result
	{
		get
		{
			object value = base.Properties["Result"].Value;
			if (value == null)
			{
				return false;
			}
			return (bool)value;
		}
		internal set
		{
			traceContext.TraceVerbose("Setting Result to: {0}", value);
			base.Properties["Result"].Value = value;
		}
	}

	public EvaluationDetail()
	{
	}

	internal EvaluationDetail(ConnectionEvaluationHistory parent)
	{
		Parent = parent;
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	protected override ISfcCollection GetChildCollection(string elementType)
	{
		throw traceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.NoSuchCollection(elementType)));
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	public static SfcObjectFactory GetObjectFactory()
	{
		return ObjectFactory.Instance;
	}

	protected override SfcKey CreateIdentityKey()
	{
		try
		{
			return new Key(ID);
		}
		catch (NullReferenceException)
		{
			traceContext.TraceError("Caught a general Exception of type NullReferenceException");
			return null;
		}
	}

	internal static SfcTypeMetadata GetTypeMetadata()
	{
		return TypeMetadata.Instance;
	}
}
