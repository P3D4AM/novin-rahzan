using System;
using System.Globalization;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;

namespace Microsoft.SqlServer.Management.XEvent;

public sealed class XEStore : BaseXEStore, ISfcDomain, ISfcDomainLite, ISfcHasConnection
{
	public sealed class ServerKey : DomainRootKey
	{
		public ServerKey()
			: base(null)
		{
		}

		internal ServerKey(ISfcDomain root)
			: base(root)
		{
		}

		public override int GetHashCode()
		{
			return GetType().GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return this == obj;
		}

		public new static bool Equals(object obj1, object obj2)
		{
			return obj1 as ServerKey == obj2 as ServerKey;
		}

		public override bool Equals(SfcKey key)
		{
			return this == key;
		}

		public static bool operator ==(object obj, ServerKey rightOperand)
		{
			if (obj == null || obj is ServerKey)
			{
				return (ServerKey)obj == rightOperand;
			}
			return false;
		}

		public static bool operator ==(ServerKey leftOperand, object obj)
		{
			if (obj == null || obj is ServerKey)
			{
				return leftOperand == (ServerKey)obj;
			}
			return false;
		}

		public static bool operator ==(ServerKey leftOperand, ServerKey rightOperand)
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

		public static bool operator !=(object obj, ServerKey rightOperand)
		{
			return !(obj == rightOperand);
		}

		public static bool operator !=(ServerKey leftOperand, object obj)
		{
			return !(leftOperand == obj);
		}

		public static bool operator !=(ServerKey leftOperand, ServerKey rightOperand)
		{
			return !(leftOperand == rightOperand);
		}

		private bool IsEqual(ServerKey other)
		{
			return base.Domain == other.Domain;
		}

		public override string GetUrnFragment()
		{
			if (base.Domain.ConnectionContext.Mode == SfcConnectionContextMode.Offline)
			{
				return "XEStore";
			}
			return string.Format(CultureInfo.InvariantCulture, "{0}[@Name='{1}']", "XEStore", base.Domain.DomainInstanceName);
		}
	}

	public const string TypeTypeName = "XEStore";

	private const string NameSpace = "http://schemas.microsoft.com/sqlserver/2008/07/extendedeventsconfig";

	private const string DomainName = "XEvent";

	private SfcConnectionContext context;

	[SfcIgnore]
	public override SfcKey IdentityKey => (ServerKey)base.AbstractIdentityKey;

	SfcConnectionContext ISfcHasConnection.ConnectionContext
	{
		get
		{
			if (context == null)
			{
				context = new SfcConnectionContext(this);
			}
			return context;
		}
	}

	[SfcIgnore]
	string ISfcDomainLite.DomainName => "XEvent";

	[SfcIgnore]
	string ISfcDomainLite.DomainInstanceName
	{
		get
		{
			if (((ISfcHasConnection)this).ConnectionContext.Mode == SfcConnectionContextMode.Offline)
			{
				return "XEvent";
			}
			return base.StoreProvider.DomainInstanceName;
		}
	}

	internal XEStore()
	{
	}

	public XEStore(SqlStoreConnection connection)
		: base(connection)
	{
	}

	public override string ToString()
	{
		return string.Format(CultureInfo.InvariantCulture, "{0} (Server='{1}')", "XEStore", (base.SfcConnection != null) ? SfcSecureString.EscapeSquote(base.SfcConnection.ServerInstance) : "");
	}

	protected override SfcKey CreateIdentityKey()
	{
		return new ServerKey(this);
	}

	ISfcConnection ISfcHasConnection.GetConnection()
	{
		return base.SfcConnection;
	}

	void ISfcHasConnection.SetConnection(ISfcConnection connection)
	{
		SetConnection(connection);
	}

	ISfcConnection ISfcHasConnection.GetConnection(SfcObjectQueryMode mode)
	{
		return base.StoreProvider.GetConnection(mode);
	}

	Type ISfcDomain.GetType(string typeName)
	{
		return typeName switch
		{
			"XEStore" => typeof(XEStore), 
			"Package" => typeof(Package), 
			"EventInfo" => typeof(EventInfo), 
			"EventColumnInfo" => typeof(EventColumnInfo), 
			"DataEventColumnInfo" => typeof(DataEventColumnInfo), 
			"ReadOnlyEventColumnInfo" => typeof(ReadOnlyEventColumnInfo), 
			"ActionInfo" => typeof(ActionInfo), 
			"TargetInfo" => typeof(TargetInfo), 
			"TargetColumnInfo" => typeof(TargetColumnInfo), 
			"PredSourceInfo" => typeof(PredSourceInfo), 
			"PredCompareInfo" => typeof(PredCompareInfo), 
			"TypeInfo" => typeof(TypeInfo), 
			"MapInfo" => typeof(MapInfo), 
			"MapValueInfo" => typeof(MapValueInfo), 
			"Session" => typeof(Session), 
			"Event" => typeof(Event), 
			"Action" => typeof(Action), 
			"EventField" => typeof(EventField), 
			"Target" => typeof(Target), 
			"TargetField" => typeof(TargetField), 
			_ => null, 
		};
	}

	SfcKey ISfcDomain.GetKey(IUrnFragment urnFragment)
	{
		return urnFragment.Name switch
		{
			"XEStore" => new ServerKey(this), 
			"Package" => new Package.Key(urnFragment.FieldDictionary), 
			"EventInfo" => new EventInfo.Key(urnFragment.FieldDictionary), 
			"EventColumnInfo" => new EventColumnInfo.Key(urnFragment.FieldDictionary), 
			"DataEventColumnInfo" => new DataEventColumnInfo.Key(urnFragment.FieldDictionary), 
			"ReadOnlyEventColumnInfo" => new ReadOnlyEventColumnInfo.Key(urnFragment.FieldDictionary), 
			"ActionInfo" => new ActionInfo.Key(urnFragment.FieldDictionary), 
			"TargetInfo" => new TargetInfo.Key(urnFragment.FieldDictionary), 
			"TargetColumnInfo" => new TargetColumnInfo.Key(urnFragment.FieldDictionary), 
			"PredSourceInfo" => new PredSourceInfo.Key(urnFragment.FieldDictionary), 
			"PredCompareInfo" => new PredCompareInfo.Key(urnFragment.FieldDictionary), 
			"TypeInfo" => new TypeInfo.Key(urnFragment.FieldDictionary), 
			"MapInfo" => new MapInfo.Key(urnFragment.FieldDictionary), 
			"MapValueInfo" => new MapValueInfo.Key(urnFragment.FieldDictionary), 
			"Session" => new Session.Key(urnFragment.FieldDictionary), 
			"Event" => new Event.Key(urnFragment.FieldDictionary), 
			"Action" => new Action.Key(urnFragment.FieldDictionary), 
			"EventField" => new EventField.Key(urnFragment.FieldDictionary), 
			"Target" => new Target.Key(urnFragment.FieldDictionary), 
			"TargetField" => new TargetField.Key(urnFragment.FieldDictionary), 
			_ => throw new XEventException(ExceptionTemplates.UnsupportedKey(urnFragment.Name)), 
		};
	}

	ISfcExecutionEngine ISfcDomain.GetExecutionEngine()
	{
		return base.ExecutionEngine;
	}

	SfcTypeMetadata ISfcDomain.GetTypeMetadata(string typeName)
	{
		switch (typeName)
		{
		case "Event":
			return Event.GetTypeMetadata();
		case "Target":
			return Target.GetTypeMetadata();
		case "Action":
			return Action.GetTypeMetadata();
		case "EventField":
			return EventField.GetTypeMetadata();
		case "TargetField":
			return TargetField.GetTypeMetadata();
		case "Session":
		case "XEStore":
			return null;
		default:
			TraceHelper.TraceContext.TraceError("Unknown typeName.");
			throw new XEventException(ExceptionTemplates.InvalidParameter("typeName"));
		}
	}

	bool ISfcDomain.UseSfcStateManagement()
	{
		return true;
	}

	int ISfcDomainLite.GetLogicalVersion()
	{
		return 3;
	}

	protected override IXEStoreProvider GetStoreProvider()
	{
		return new ServerXEStoreProvider(this);
	}

	protected override ISessionProvider GetSessionProivder(Session session)
	{
		return new ServerSessionProvider(session);
	}

	protected override ITargetProvider GetTargetProvider(Target target)
	{
		return new ServerTargetProvider(target);
	}

	protected override IEventProvider GetEventProvider(Event xevent)
	{
		return new ServerEventProvider(xevent);
	}
}
