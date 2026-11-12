using System;
using Microsoft.SqlServer.Management.SqlParser.Common;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class Database : IDatabase, IServerOwnedObject, IDatabaseObject, IMetadataObject
{
	private readonly DeserializationModel model;

	private readonly int applicationRoles;

	private readonly int asymmetricKeys;

	private readonly int certificates;

	private readonly CollationInfo collationInfo;

	private readonly DatabaseCompatibilityLevel compatibilityLevel;

	private readonly string defaultSchemaName;

	private readonly bool isSystemObject;

	private readonly string name;

	private readonly int owner;

	private readonly int roles;

	private readonly int schemas;

	private readonly int server;

	private readonly int triggers;

	private readonly int users;

	IMetadataCollection<IApplicationRole> IDatabase.ApplicationRoles => model.GetCollectionById<IApplicationRole>(applicationRoles);

	IMetadataCollection<IAsymmetricKey> IDatabase.AsymmetricKeys => model.GetCollectionById<IAsymmetricKey>(asymmetricKeys);

	IMetadataCollection<ICertificate> IDatabase.Certificates => model.GetCollectionById<ICertificate>(certificates);

	CollationInfo IDatabase.CollationInfo => collationInfo;

	DatabaseCompatibilityLevel IDatabase.CompatibilityLevel => compatibilityLevel;

	string IDatabase.DefaultSchemaName => defaultSchemaName;

	IUser IDatabase.Owner => model.GetObjectById<IUser>(owner);

	IMetadataCollection<IDatabaseRole> IDatabase.Roles => model.GetCollectionById<IDatabaseRole>(roles);

	IMetadataCollection<ISchema> IDatabase.Schemas => model.GetCollectionById<ISchema>(schemas);

	IMetadataCollection<IDatabaseDdlTrigger> IDatabase.Triggers => model.GetCollectionById<IDatabaseDdlTrigger>(triggers);

	IMetadataCollection<IUser> IDatabase.Users => model.GetCollectionById<IUser>(users);

	IServer IServerOwnedObject.Server => model.GetObjectById<IServer>(server);

	IDatabaseObject IDatabaseObject.Parent => ((IServerOwnedObject)this).Server;

	bool IDatabaseObject.IsSystemObject => isSystemObject;

	string IMetadataObject.Name => name;

	public Database(DeserializationModel model, int applicationRoles, int asymmetricKeys, int certificates, CollationInfo collationInfo, DatabaseCompatibilityLevel compatibilityLevel, string defaultSchemaName, bool isSystemObject, string name, int owner, int roles, int schemas, int server, int triggers, int users)
	{
		this.model = model;
		this.applicationRoles = applicationRoles;
		this.asymmetricKeys = asymmetricKeys;
		this.certificates = certificates;
		this.collationInfo = collationInfo;
		this.compatibilityLevel = compatibilityLevel;
		this.defaultSchemaName = defaultSchemaName;
		this.isSystemObject = isSystemObject;
		this.name = name;
		this.owner = owner;
		this.roles = roles;
		this.schemas = schemas;
		this.server = server;
		this.triggers = triggers;
		this.users = users;
	}

	public Database(DeserializationModel model, object[] values)
		: this(model, (int)values[0], (int)values[1], (int)values[2], (CollationInfo)values[3], (DatabaseCompatibilityLevel)values[4], (string)values[5], (bool)values[6], (string)values[7], (int)values[8], (int)values[9], (int)values[10], (int)values[11], (int)values[12], (int)values[13])
	{
	}

	T IServerOwnedObject.Accept<T>(IServerOwnedObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}

	T IDatabaseObject.Accept<T>(IDatabaseObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return ((IServerOwnedObject)this).Accept((IServerOwnedObjectVisitor<T>)visitor);
	}

	T IMetadataObject.Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return ((IServerOwnedObject)this).Accept((IServerOwnedObjectVisitor<T>)visitor);
	}
}
