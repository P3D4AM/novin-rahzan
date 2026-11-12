using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Diagnostics.STrace;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;

namespace Microsoft.SqlServer.Management.Dmf;

public sealed class EvaluationHistory : SfcInstance
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
			return string.Format("{0}[@ID='{1}']", "EvaluationHistory", ID);
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
			return new EvaluationHistory();
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

	private static TraceContext traceContext = TraceContext.GetTraceContext("DMF", "EvaluationHistory");

	internal const string typeName = "EvaluationHistory";

	private ConnectionEvaluationHistoryCollection evaluationHistories;

	[SfcProperty(SfcPropertyFlags.None)]
	public string PolicyName
	{
		get
		{
			string result = string.Empty;
			if (Parent != null)
			{
				result = Parent.Name;
			}
			return result;
		}
		internal set
		{
			traceContext.TraceVerbose("Setting PolicyName to: {0}", value);
			base.Properties["PolicyName"].Value = value;
		}
	}

	[SfcProperty(SfcPropertyFlags.None)]
	public DateTime StartDate
	{
		get
		{
			object value = base.Properties["StartDate"].Value;
			if (value == null)
			{
				return DateTime.MinValue;
			}
			return (DateTime)value;
		}
		internal set
		{
			traceContext.TraceVerbose("Setting StartDate to: {0}", value);
			base.Properties["StartDate"].Value = value;
		}
	}

	[SfcProperty(SfcPropertyFlags.None)]
	public DateTime EndDate
	{
		get
		{
			object value = base.Properties["EndDate"].Value;
			if (value == null)
			{
				return DateTime.MinValue;
			}
			return (DateTime)value;
		}
		internal set
		{
			traceContext.TraceVerbose("Setting EndDate to: {0}", value);
			base.Properties["EndDate"].Value = value;
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

	[SfcObject(SfcObjectRelationship.ParentObject, SfcObjectCardinality.One)]
	public new Policy Parent
	{
		get
		{
			return (Policy)base.Parent;
		}
		internal set
		{
			traceContext.TraceVerbose("Setting Parent to: {0}", value);
			base.Parent = value;
		}
	}

	[SfcObject(SfcContainerRelationship.ChildContainer, SfcContainerCardinality.ZeroToAny, typeof(ConnectionEvaluationHistory))]
	public ConnectionEvaluationHistoryCollection ConnectionEvaluationHistories
	{
		get
		{
			if (evaluationHistories == null)
			{
				evaluationHistories = new ConnectionEvaluationHistoryCollection(this);
			}
			return evaluationHistories;
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

	public EvaluationHistory()
	{
	}

	internal EvaluationHistory(Policy parent)
	{
		Parent = parent;
	}

	protected override ISfcCollection GetChildCollection(string elementType)
	{
		if (elementType == "ConnectionEvaluationHistory")
		{
			return ConnectionEvaluationHistories;
		}
		throw traceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.NoSuchCollection(elementType)));
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	public override void Discover(ISfcDependencyDiscoveryObjectSink sink)
	{
		if (sink.Action == SfcDependencyAction.Serialize)
		{
			ConnectionEvaluationHistoryCollection connectionEvaluationHistories = ConnectionEvaluationHistories;
			if (connectionEvaluationHistories != null)
			{
				sink.Add(SfcDependencyDirection.Inbound, connectionEvaluationHistories.GetEnumerator(), SfcTypeRelation.RequiredChild, discovered: false);
			}
		}
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
