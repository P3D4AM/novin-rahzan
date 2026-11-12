using System.Linq;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

namespace Microsoft.SqlServer.Management.SqlParser.Binder;

internal sealed class ServerEx : DatabaseObjectEx<IServer>, IServer, IDatabaseObject, IMetadataObject
{
	private abstract class DatabaseExCache : ObjectExCache<IDatabase, DatabaseEx>
	{
		protected readonly ServerEx m_server;

		protected DatabaseExCache(ServerEx server)
			: base(8)
		{
			m_server = server;
		}
	}

	private sealed class ImmutableDatabaseExCache : DatabaseExCache
	{
		public ImmutableDatabaseExCache(ServerEx server)
			: base(server)
		{
		}

		protected override DatabaseEx CreateMetadataObjectEx(IDatabase proxyObject)
		{
			return new DatabaseEx(m_server, proxyObject);
		}
	}

	private sealed class MutableDatabaseExCache : DatabaseExCache
	{
		private readonly IMutableServer m_proxyServer;

		public MutableDatabaseExCache(ServerEx server, IMutableServer proxyServer)
			: base(server)
		{
			m_proxyServer = proxyServer;
		}

		protected override DatabaseEx CreateMetadataObjectEx(IDatabase proxyObject)
		{
			IMutableDatabase mutableDatabase = m_server.Databases.AsMutableObject(proxyObject);
			if (mutableDatabase == null)
			{
				mutableDatabase = m_server.m_metadataFactory.Database.Create(proxyObject);
				m_server.Databases.ReplaceWithMutableObject(proxyObject, mutableDatabase);
			}
			return new DatabaseEx(m_server, mutableDatabase);
		}
	}

	private readonly ISystemDataTypeLookup m_systemDataTypeLookup;

	private readonly IBuiltInFunctionLookup m_builtInFunctionLookup;

	private readonly ICollationLookup m_collationLookup;

	private readonly IMetadataFactory m_metadataFactory;

	private readonly DatabaseExCache m_databaseCache;

	private readonly IScopedMetadataCollection<IDatabase, IMutableDatabase> m_databases;

	private readonly IScopedMetadataCollection<ILogin, IMutableLogin> m_logins;

	private readonly IScopedMetadataCollection<IServerDdlTrigger, IMutableServerDdlTrigger> m_triggers;

	private readonly IMetadataCollection<IMetadataObject> m_systemDataTypes;

	private SchemaEx m_tempTableSchema;

	private DatabaseEx m_tempDatabase;

	private readonly Duplicator m_metadataDuplicator;

	public DatabaseEx this[string databaseName]
	{
		get
		{
			IDatabase database = Databases.GlobalMetadata[databaseName];
			if (database == null)
			{
				return null;
			}
			return GetOrCreateDatabaseEx(database);
		}
	}

	public ISystemDataTypeLookup SystemDataTypeLookup => m_systemDataTypeLookup;

	public IBuiltInFunctionLookup BuiltInFunctionLookup => m_builtInFunctionLookup;

	public ICollationLookup CollationLookup => m_collationLookup;

	public IMetadataFactory MetadataFactory => m_metadataFactory;

	public Duplicator MetadataDuplicator => m_metadataDuplicator;

	public SchemaEx TempTableSchema => m_tempTableSchema;

	public IScopedMetadataCollection<IDatabase, IMutableDatabase> Databases => m_databases;

	public IScopedMetadataCollection<ILogin, IMutableLogin> Logins => m_logins;

	public IScopedMetadataCollection<IServerDdlTrigger, IMutableServerDdlTrigger> Triggers => m_triggers;

	public IMetadataCollection<IMetadataObject> SystemDataTypes => m_systemDataTypes;

	private DatabaseEx TempDatabase
	{
		get
		{
			if (m_tempDatabase == null)
			{
				m_tempDatabase = this["tempdb"];
				if (m_tempDatabase == null)
				{
					IDatabase proxyDatabase = m_metadataFactory.Database.CreateEmptyDatabase(this, "tempdb", CollationInfo, isSystemObject: true);
					m_tempDatabase = new DatabaseEx(this, proxyDatabase);
				}
			}
			return m_tempDatabase;
		}
	}

	public CollationInfo CollationInfo => m_proxyObject.CollationInfo;

	IMetadataCollection<IDatabase> IServer.Databases => m_databases.GlobalMetadata;

	public IMetadataCollection<ICredential> Credentials => m_proxyObject.Credentials;

	IMetadataCollection<ILogin> IServer.Logins => m_logins.GlobalMetadata;

	IMetadataCollection<IServerDdlTrigger> IServer.Triggers => m_triggers.GlobalMetadata;

	public ServerEx(IServer proxyServer, ISystemDataTypeLookup systemDataTypeLookup, IBuiltInFunctionLookup builtInFunctionLookup, ICollationLookup collationLookup, IMetadataFactory metadataFactory)
		: this(proxyServer, systemDataTypeLookup, builtInFunctionLookup, collationLookup, metadataFactory, isMutable: false)
	{
		CollationInfo collationInfo = proxyServer.CollationInfo;
		m_databases = ScopedMetadataCollection<IDatabase, IMutableDatabase>.CreateImmutableCollection(proxyServer.Databases, collationInfo);
		m_logins = ScopedMetadataCollection<ILogin, IMutableLogin>.CreateImmutableCollection(proxyServer.Logins, collationInfo);
		m_triggers = ScopedMetadataCollection<IServerDdlTrigger, IMutableServerDdlTrigger>.CreateImmutableCollection(proxyServer.Triggers, collationInfo);
		m_databaseCache = new ImmutableDatabaseExCache(this);
		CollationInfo @default = CollationInfo.Default;
		IMetadataObject[] items = DataTypeSpec.AllDataTypes.Select((DataTypeSpec dataTypespec) => SystemDataTypeLookup.Find(dataTypespec, isMaximum: false)).ToArray();
		m_systemDataTypes = Collection<IMetadataObject>.CreateOrderedCollection(@default, items);
	}

	public ServerEx(IMutableServer proxyServer, ISystemDataTypeLookup systemDataTypeLookup, IBuiltInFunctionLookup builtInFunctionLookup, ICollationLookup collationLookup, IMetadataFactory metadataFactory)
		: this(proxyServer, systemDataTypeLookup, builtInFunctionLookup, collationLookup, metadataFactory, isMutable: true)
	{
		CollationInfo collationInfo = proxyServer.CollationInfo;
		m_databases = ScopedMetadataCollection<IDatabase, IMutableDatabase>.CreateMutableCollection(proxyServer.Databases, collationInfo);
		m_logins = ScopedMetadataCollection<ILogin, IMutableLogin>.CreateMutableCollection(proxyServer.Logins, collationInfo);
		m_triggers = ScopedMetadataCollection<IServerDdlTrigger, IMutableServerDdlTrigger>.CreateMutableCollection(proxyServer.Triggers, collationInfo);
		m_databaseCache = new MutableDatabaseExCache(this, proxyServer);
	}

	private ServerEx(IServer proxyServer, ISystemDataTypeLookup systemDataTypeLookup, IBuiltInFunctionLookup builtInFunctionLookup, ICollationLookup collationLookup, IMetadataFactory metadataFactory, bool isMutable)
		: base(proxyServer)
	{
		m_systemDataTypeLookup = systemDataTypeLookup;
		m_builtInFunctionLookup = builtInFunctionLookup;
		m_collationLookup = collationLookup;
		m_metadataFactory = metadataFactory;
		m_metadataDuplicator = new Duplicator(metadataFactory);
	}

	public DatabaseEx GetOrCreateDatabaseEx(IDatabase database)
	{
		return m_databaseCache[database];
	}

	public SchemaEx GetOrCreateTempTableSchema()
	{
		if (m_tempTableSchema == null)
		{
			DatabaseEx tempDatabase = TempDatabase;
			m_tempTableSchema = tempDatabase.MakeTempTableSchema();
		}
		return m_tempTableSchema;
	}
}
