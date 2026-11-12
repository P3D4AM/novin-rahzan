using System;
using Microsoft.SqlServer.Management.SqlParser.Common;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class Database : ServerOwnedObject, IMutableDatabase, IDatabase, IServerOwnedObject, IDatabaseObject, IMetadataObject, IMutableServerOwnedObject, IMutableDatabaseObject, IMutableMetadataObject
{
	private readonly CollationInfo m_collationInfo;

	private readonly ApplicationRoleCollection m_applicationRoles;

	private readonly AsymmetricKeyCollection m_asymmetricKeys;

	private readonly CertificateCollection m_certificates;

	private readonly DatabaseRoleCollection m_databaseRoles;

	private readonly SchemaCollection m_schemas;

	private readonly DatabaseDdlTriggerCollection m_triggers;

	private readonly UserCollection m_users;

	private string m_defaultSchemaName;

	private IUser m_owner;

	private DatabaseCompatibilityLevel m_compatibilityLevel;

	public IMutableMetadataCollection<IApplicationRole> ApplicationRoles => m_applicationRoles;

	public IMutableMetadataCollection<IAsymmetricKey> AsymmetricKeys => m_asymmetricKeys;

	public IMutableMetadataCollection<ICertificate> Certificates => m_certificates;

	public DatabaseCompatibilityLevel CompatibilityLevel
	{
		get
		{
			return m_compatibilityLevel;
		}
		set
		{
			m_compatibilityLevel = value;
		}
	}

	public string DefaultSchemaName
	{
		get
		{
			return m_defaultSchemaName;
		}
		set
		{
			m_defaultSchemaName = value;
		}
	}

	public IUser Owner
	{
		get
		{
			return m_owner;
		}
		set
		{
			m_owner = value;
		}
	}

	public IMutableMetadataCollection<IDatabaseRole> Roles => m_databaseRoles;

	public IMutableMetadataCollection<ISchema> Schemas => m_schemas;

	public IMutableMetadataCollection<IDatabaseDdlTrigger> Triggers => m_triggers;

	public IMutableMetadataCollection<IUser> Users => m_users;

	IMetadataCollection<IApplicationRole> IDatabase.ApplicationRoles => m_applicationRoles;

	IMetadataCollection<IAsymmetricKey> IDatabase.AsymmetricKeys => m_asymmetricKeys;

	IMetadataCollection<ICertificate> IDatabase.Certificates => m_certificates;

	public CollationInfo CollationInfo => m_collationInfo;

	IMetadataCollection<IDatabaseRole> IDatabase.Roles => m_databaseRoles;

	IMetadataCollection<ISchema> IDatabase.Schemas => m_schemas;

	IMetadataCollection<IDatabaseDdlTrigger> IDatabase.Triggers => m_triggers;

	IMetadataCollection<IUser> IDatabase.Users => m_users;

	DatabaseCompatibilityLevel IDatabase.CompatibilityLevel => m_compatibilityLevel;

	public Database(IServer server, string name, CollationInfo collationInfo)
		: base(server, name)
	{
		m_collationInfo = collationInfo;
		m_applicationRoles = new ApplicationRoleCollection(collationInfo);
		m_asymmetricKeys = new AsymmetricKeyCollection(collationInfo);
		m_certificates = new CertificateCollection(collationInfo);
		m_databaseRoles = new DatabaseRoleCollection(collationInfo);
		m_schemas = new SchemaCollection(collationInfo);
		m_triggers = new DatabaseDdlTriggerCollection(collationInfo);
		m_users = new UserCollection(collationInfo);
	}

	public Database(IDatabase other)
		: base(other)
	{
		m_collationInfo = other.CollationInfo;
		m_applicationRoles = new ApplicationRoleCollection(other.ApplicationRoles.Count, m_collationInfo);
		m_applicationRoles.AddRange(other.ApplicationRoles);
		m_asymmetricKeys = new AsymmetricKeyCollection(other.AsymmetricKeys.Count, m_collationInfo);
		m_asymmetricKeys.AddRange(other.AsymmetricKeys);
		m_certificates = new CertificateCollection(other.Certificates.Count, m_collationInfo);
		m_certificates.AddRange(other.Certificates);
		m_databaseRoles = new DatabaseRoleCollection(other.Roles.Count, m_collationInfo);
		m_databaseRoles.AddRange(other.Roles);
		m_schemas = new SchemaCollection(other.Schemas.Count, m_collationInfo);
		m_schemas.AddRange(other.Schemas);
		m_triggers = new DatabaseDdlTriggerCollection(other.Triggers.Count, m_collationInfo);
		m_triggers.AddRange(other.Triggers);
		m_users = new UserCollection(other.Users.Count, m_collationInfo);
		m_users.AddRange(other.Users);
		m_defaultSchemaName = other.DefaultSchemaName;
		m_owner = other.Owner;
		m_compatibilityLevel = other.CompatibilityLevel;
	}

	public override T Accept<T>(IServerOwnedObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}
}
