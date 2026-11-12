using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Common;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.Binder;

internal sealed class DatabaseEx : DatabaseObjectEx<IDatabase>, IDatabase, IServerOwnedObject, IDatabaseObject, IMetadataObject
{
	private enum SystemDatabaseName : sbyte
	{
		None,
		Master,
		Model,
		Msdb,
		TempDb
	}

	private abstract class SchemaExCache : ObjectExCache<ISchema, SchemaEx>
	{
		protected readonly DatabaseEx m_database;

		protected SchemaExCache(DatabaseEx database)
			: base(8)
		{
			m_database = database;
		}
	}

	private sealed class ImmutableSchemaExCache : SchemaExCache
	{
		public ImmutableSchemaExCache(DatabaseEx database)
			: base(database)
		{
		}

		protected override SchemaEx CreateMetadataObjectEx(ISchema proxyObject)
		{
			return new SchemaEx(m_database, proxyObject);
		}
	}

	private sealed class MutableSchemaExCache : SchemaExCache
	{
		private readonly IMutableDatabase m_proxyDatabase;

		public MutableSchemaExCache(DatabaseEx database, IMutableDatabase proxyDatabase)
			: base(database)
		{
			m_proxyDatabase = proxyDatabase;
		}

		protected override SchemaEx CreateMetadataObjectEx(ISchema proxyObject)
		{
			IMutableSchema mutableSchema = m_database.Schemas.AsMutableObject(proxyObject);
			if (mutableSchema == null)
			{
				mutableSchema = m_database.Server.MetadataFactory.Schema.Create(proxyObject);
				m_database.Schemas.ReplaceWithMutableObject(proxyObject, mutableSchema);
			}
			return new SchemaEx(m_database, mutableSchema);
		}
	}

	private static class Y2kCatalogViewFactory
	{
		private class CatalogViewNames
		{
			public readonly List<string> DatabaseOnly;

			public readonly List<string> ServerAndDatabase;

			public CatalogViewNames(CollationInfo collationInfo)
			{
				string[] server = Config.Y2kCatalogViews.Server;
				string[] database = Config.Y2kCatalogViews.Database;
				DatabaseOnly = new List<string>(database.Length);
				DatabaseOnly.AddRange(database);
				DatabaseOnly.Sort(collationInfo.Comparer);
				ServerAndDatabase = new List<string>(server.Length + database.Length);
				ServerAndDatabase.AddRange(server);
				ServerAndDatabase.AddRange(database);
				ServerAndDatabase.Sort(collationInfo.Comparer);
			}
		}

		private static readonly Dictionary<CollationInfo, CatalogViewNames> catalogViewLookup = new Dictionary<CollationInfo, CatalogViewNames>(4);

		public static List<string> GetNameList(DatabaseEx database)
		{
			CollationInfo collationInfo = database.CollationInfo;
			_ = Config.Y2kCatalogViews.Server;
			_ = Config.Y2kCatalogViews.Database;
			if (!catalogViewLookup.TryGetValue(collationInfo, out var value))
			{
				lock (catalogViewLookup)
				{
					if (!catalogViewLookup.TryGetValue(collationInfo, out value))
					{
						value = new CatalogViewNames(collationInfo);
						catalogViewLookup[collationInfo] = value;
					}
				}
			}
			if (!database.IsMaster)
			{
				return value.DatabaseOnly;
			}
			return value.ServerAndDatabase;
		}
	}

	private readonly ServerEx m_parent;

	private readonly SystemDatabaseName m_systemDbName;

	private readonly SchemaExCache m_schemaCache;

	private SchemaEx m_defaultSchema;

	private SchemaEx m_dboSchema;

	private SchemaEx m_sysSchema;

	private SchemaEx m_unnamedSchema;

	private SchemaEx m_emptySchema;

	private List<string> m_y2kCatalogViewNames;

	private readonly IScopedMetadataCollection<IApplicationRole, IMutableApplicationRole> m_applicationRoles;

	private readonly IScopedMetadataCollection<IDatabaseRole, IMutableDatabaseRole> m_databaseRoles;

	private readonly IScopedMetadataCollection<ISchema, IMutableSchema> m_schemas;

	private readonly IScopedMetadataCollection<IDatabaseDdlTrigger, IMutableDatabaseDdlTrigger> m_triggers;

	private readonly IScopedMetadataCollection<IUser, IMutableUser> m_users;

	public ServerEx Server => m_parent;

	public IScopedMetadataCollection<IApplicationRole, IMutableApplicationRole> ApplicationRoles => m_applicationRoles;

	public IScopedMetadataCollection<IDatabaseRole, IMutableDatabaseRole> Roles => m_databaseRoles;

	public IScopedMetadataCollection<ISchema, IMutableSchema> Schemas => m_schemas;

	public IScopedMetadataCollection<IDatabaseDdlTrigger, IMutableDatabaseDdlTrigger> Triggers => m_triggers;

	public IScopedMetadataCollection<IUser, IMutableUser> Users => m_users;

	public bool IsMaster => m_systemDbName == SystemDatabaseName.Master;

	public bool IsTempDb => m_systemDbName == SystemDatabaseName.TempDb;

	public SchemaEx DefaultSchema
	{
		get
		{
			if (m_defaultSchema == null)
			{
				string defaultSchemaName = DefaultSchemaName;
				if (DefaultSchemaName != null)
				{
					m_defaultSchema = FindSchema(defaultSchemaName);
				}
				if (m_defaultSchema == null)
				{
					m_defaultSchema = EmptySchema;
				}
			}
			return m_defaultSchema;
		}
	}

	public SchemaEx DboSchema
	{
		get
		{
			if (m_dboSchema == null)
			{
				m_dboSchema = FindSchema("dbo");
				if (m_dboSchema == null)
				{
					ISchema proxySchema = m_parent.MetadataFactory.Schema.CreateEmptySchema(this, "dbo", isSystemObject: true);
					m_dboSchema = new SchemaEx(this, proxySchema);
				}
			}
			return m_dboSchema;
		}
	}

	public SchemaEx SysSchema
	{
		get
		{
			if (m_sysSchema == null)
			{
				m_sysSchema = FindSchema("sys");
				if (m_sysSchema == null)
				{
					ISchema proxySchema = m_parent.MetadataFactory.Schema.CreateEmptySchema(this, "sys", isSystemObject: true);
					m_sysSchema = new SchemaEx(this, proxySchema);
				}
			}
			return m_sysSchema;
		}
	}

	public SchemaEx UnnamedSchema
	{
		get
		{
			if (m_unnamedSchema == null)
			{
				if (m_proxyObject.Schemas.Count > 0)
				{
					m_unnamedSchema = SchemaEx.CreateUnnamedSchema(this);
				}
				else
				{
					m_unnamedSchema = EmptySchema;
				}
			}
			return m_unnamedSchema;
		}
	}

	public SchemaEx EmptySchema
	{
		get
		{
			if (m_emptySchema == null)
			{
				m_emptySchema = SchemaEx.CreateEmptySchema(this);
				m_schemaCache.Add(m_emptySchema);
			}
			return m_emptySchema;
		}
	}

	IServer IServerOwnedObject.Server => Server;

	IMetadataCollection<IApplicationRole> IDatabase.ApplicationRoles => m_applicationRoles.GlobalMetadata;

	public IMetadataCollection<IAsymmetricKey> AsymmetricKeys => m_proxyObject.AsymmetricKeys;

	public IMetadataCollection<ICertificate> Certificates => m_proxyObject.Certificates;

	public CollationInfo CollationInfo => m_proxyObject.CollationInfo;

	DatabaseCompatibilityLevel IDatabase.CompatibilityLevel => m_proxyObject.CompatibilityLevel;

	public string DefaultSchemaName => m_proxyObject.DefaultSchemaName;

	public IUser Owner => m_proxyObject.Owner;

	IMetadataCollection<IDatabaseRole> IDatabase.Roles => m_databaseRoles.GlobalMetadata;

	IMetadataCollection<ISchema> IDatabase.Schemas => m_schemas.GlobalMetadata;

	IMetadataCollection<IDatabaseDdlTrigger> IDatabase.Triggers => m_triggers.GlobalMetadata;

	IMetadataCollection<IUser> IDatabase.Users => m_users.GlobalMetadata;

	public DatabaseEx(ServerEx parent, IDatabase proxyDatabase)
		: this(parent, proxyDatabase, isMutable: false)
	{
		CollationInfo collationInfo = proxyDatabase.CollationInfo;
		m_applicationRoles = ScopedMetadataCollection<IApplicationRole, IMutableApplicationRole>.CreateImmutableCollection(proxyDatabase.ApplicationRoles, collationInfo);
		m_databaseRoles = ScopedMetadataCollection<IDatabaseRole, IMutableDatabaseRole>.CreateImmutableCollection(proxyDatabase.Roles, collationInfo);
		m_schemas = ScopedMetadataCollection<ISchema, IMutableSchema>.CreateImmutableCollection(proxyDatabase.Schemas, collationInfo);
		m_triggers = ScopedMetadataCollection<IDatabaseDdlTrigger, IMutableDatabaseDdlTrigger>.CreateImmutableCollection(proxyDatabase.Triggers, collationInfo);
		m_users = ScopedMetadataCollection<IUser, IMutableUser>.CreateImmutableCollection(proxyDatabase.Users, collationInfo);
		m_schemaCache = new ImmutableSchemaExCache(this);
	}

	public DatabaseEx(ServerEx parent, IMutableDatabase proxyDatabase)
		: this(parent, proxyDatabase, isMutable: true)
	{
		CollationInfo collationInfo = proxyDatabase.CollationInfo;
		m_applicationRoles = ScopedMetadataCollection<IApplicationRole, IMutableApplicationRole>.CreateMutableCollection(proxyDatabase.ApplicationRoles, collationInfo);
		m_databaseRoles = ScopedMetadataCollection<IDatabaseRole, IMutableDatabaseRole>.CreateMutableCollection(proxyDatabase.Roles, collationInfo);
		m_schemas = ScopedMetadataCollection<ISchema, IMutableSchema>.CreateMutableCollection(proxyDatabase.Schemas, collationInfo);
		m_triggers = ScopedMetadataCollection<IDatabaseDdlTrigger, IMutableDatabaseDdlTrigger>.CreateMutableCollection(proxyDatabase.Triggers, collationInfo);
		m_users = ScopedMetadataCollection<IUser, IMutableUser>.CreateMutableCollection(proxyDatabase.Users, collationInfo);
		m_schemaCache = new MutableSchemaExCache(this, proxyDatabase);
	}

	private DatabaseEx(ServerEx parent, IDatabase proxyDatabase, bool isMutable)
		: base(proxyDatabase)
	{
		m_parent = parent;
		IEqualityComparer<string> equalityComparer = parent.CollationInfo.EqualityComparer;
		string name = proxyDatabase.Name;
		if (equalityComparer.Equals(name, "master"))
		{
			m_systemDbName = SystemDatabaseName.Master;
		}
		else if (equalityComparer.Equals(name, "model"))
		{
			m_systemDbName = SystemDatabaseName.Model;
		}
		else if (equalityComparer.Equals(name, "msdb"))
		{
			m_systemDbName = SystemDatabaseName.Msdb;
		}
		else if (equalityComparer.Equals(name, "tempdb"))
		{
			m_systemDbName = SystemDatabaseName.TempDb;
		}
		else
		{
			m_systemDbName = SystemDatabaseName.None;
		}
		m_y2kCatalogViewNames = Y2kCatalogViewFactory.GetNameList(this);
	}

	public SchemaEx FindSchema(string name)
	{
		return FindSchema(name, null);
	}

	public SchemaEx FindSchema(string name, LexicalScope lexicalScope)
	{
		ISchema schema = Schemas[name, lexicalScope];
		if (schema == null)
		{
			return null;
		}
		return GetOrCreateSchemaEx(schema);
	}

	public SchemaEx GetOrCreateSchemaEx(ISchema schema)
	{
		return m_schemaCache[schema];
	}

	public SchemaEx SchemaToSchemaEx(ISchema schema)
	{
		return null;
	}

	public SchemaEx MakeUnnamedSchema(SchemaEx defaultSchema)
	{
		SchemaEx result = ((defaultSchema != DefaultSchema) ? SchemaEx.CreateUnnamedSchema(this, defaultSchema) : UnnamedSchema);
		if (defaultSchema != null)
		{
			return SchemaEx.CreateUnnamedSchema(this, defaultSchema);
		}
		return result;
	}

	public SchemaEx MakeTempTableSchema()
	{
		SchemaEx schemaEx = SchemaEx.CreateTempTableSchema(this);
		m_schemaCache.Add(schemaEx);
		return schemaEx;
	}

	public bool IsY2KCatalogViewName(string name)
	{
		return m_y2kCatalogViewNames.BinarySearch(name, CollationInfo.Comparer) >= 0;
	}

	public bool HasMSReservedNamePrefix(string name)
	{
		if (!CollationInfo.IsPrefix(name, "sp_"))
		{
			if (IsMaster)
			{
				if (!CollationInfo.IsPrefix(name, "fn_"))
				{
					return CollationInfo.IsPrefix(name, "xp_");
				}
				return true;
			}
			return false;
		}
		return true;
	}

	public IDatabasePrincipal FindDatabasePrincipal(string name, LexicalScope lexicalScope)
	{
		IDatabasePrincipal databasePrincipal = m_users[name, lexicalScope];
		if (databasePrincipal != null)
		{
			return databasePrincipal;
		}
		databasePrincipal = m_databaseRoles[name, lexicalScope];
		if (databasePrincipal != null)
		{
			return databasePrincipal;
		}
		return m_applicationRoles[name, lexicalScope];
	}

	public T Accept<T>(IServerOwnedObjectVisitor<T> visitor)
	{
		return m_proxyObject.Accept(visitor);
	}
}
