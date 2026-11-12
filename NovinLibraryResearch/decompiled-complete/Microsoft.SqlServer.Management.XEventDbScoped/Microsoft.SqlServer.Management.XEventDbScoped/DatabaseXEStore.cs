using System;
using System.Globalization;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;
using Microsoft.SqlServer.Management.XEvent;

namespace Microsoft.SqlServer.Management.XEventDbScoped;

public sealed class DatabaseXEStore : BaseXEStore, ISfcDomain, ISfcDomainLite, ISfcHasConnection
{
	private sealed class DatabaseKey : NamedDomainKey<DatabaseXEStore>
	{
		private string serverName;

		protected override string UrnName => "DatabaseXEStore";

		public DatabaseKey()
			: base((ISfcDomain)null)
		{
		}

		public DatabaseKey(ISfcDomain domain)
			: base(domain)
		{
		}

		public DatabaseKey(ISfcDomain domain, string name)
			: base(domain, name)
		{
		}

		public DatabaseKey(ISfcDomain domain, string name, string serverName)
			: base(domain, name)
		{
			this.serverName = serverName;
		}

		public override string GetUrnFragment()
		{
			if (base.Domain.ConnectionContext.Mode == SfcConnectionContextMode.Offline)
			{
				return "DatabaseXEStore";
			}
			return "DatabaseXEStore[@Name='" + SfcTsqlProcFormatter.SqlString(base.Name) + "' and @ServerName='" + SfcTsqlProcFormatter.SqlString(serverName) + "']";
		}
	}

	public const string TypeTypeName = "DatabaseXEStore";

	private const string NameSpace = "http://schemas.microsoft.com/sqlserver/2008/07/extendedeventsconfig";

	private const string DomainName = "DatabaseXEvent";

	private string name;

	private SfcConnectionContext context;

	[SfcIgnore]
	public override SfcKey IdentityKey => base.AbstractIdentityKey;

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
	string ISfcDomainLite.DomainName => "DatabaseXEvent";

	[SfcIgnore]
	string ISfcDomainLite.DomainInstanceName
	{
		get
		{
			_ = ((ISfcHasConnection)this).ConnectionContext.Mode;
			return Name;
		}
	}

	public override string Name
	{
		get
		{
			return name;
		}
		protected set
		{
			if (name != null)
			{
				throw new XEventException(ExceptionTemplates.CannotSetNameForExistingObject);
			}
			if (string.IsNullOrEmpty(value))
			{
				throw new XEventException(ExceptionTemplates.NameNullEmpty);
			}
			name = value;
		}
	}

	[SfcProperty(Data = true)]
	public override string ServerName => base.StoreProvider.DomainInstanceName;

	public DatabaseXEStore(SqlStoreConnection connection, string name)
		: this()
	{
		if (connection == null)
		{
			throw new ArgumentNullException("connection");
		}
		if (string.IsNullOrEmpty(name))
		{
			throw new XEventException(ExceptionTemplates.NameNullEmpty);
		}
		this.name = name;
		base.OriginalConnection = connection;
		InitConnection();
	}

	public DatabaseXEStore()
	{
	}

	public DatabaseXEStore(SqlStoreConnection connection)
		: this(connection, connection.ServerConnection.CurrentDatabase)
	{
	}

	public override string ToString()
	{
		return string.Format(CultureInfo.InvariantCulture, "{0} (Server='{1}' and Name='{2}')", "DatabaseXEStore", ServerName, Name, (base.SfcConnection != null) ? SfcSecureString.EscapeSquote(base.SfcConnection.ServerInstance) : string.Empty);
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
			"DatabaseXEStore" => typeof(DatabaseXEStore), 
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
			"Action" => typeof(Microsoft.SqlServer.Management.XEvent.Action), 
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
			"DatabaseXEStore" => new DatabaseKey(this), 
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
			"Action" => new Microsoft.SqlServer.Management.XEvent.Action.Key(urnFragment.FieldDictionary), 
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
			return Microsoft.SqlServer.Management.XEvent.Action.GetTypeMetadata();
		case "EventField":
			return EventField.GetTypeMetadata();
		case "TargetField":
			return TargetField.GetTypeMetadata();
		case "Session":
		case "DatabaseXEStore":
			return null;
		default:
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

	protected override SfcKey CreateIdentityKey()
	{
		return new DatabaseKey(this, name, ServerName);
	}

	protected override IXEStoreProvider GetStoreProvider()
	{
		return new DatabaseXEStoreProvider(this);
	}

	protected override ISessionProvider GetSessionProivder(Session session)
	{
		return new DatabaseSessionProvider(session);
	}

	protected override ITargetProvider GetTargetProvider(Target target)
	{
		return new DatabaseTargetProvider(target);
	}

	protected override IEventProvider GetEventProvider(Event xevent)
	{
		return new DatabaseEventProvider(xevent);
	}
}
