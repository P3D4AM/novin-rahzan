using System;
using Microsoft.SqlServer.Management.SqlParser.Common;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class EmptyDatabase : IDatabase, IServerOwnedObject, IDatabaseObject, IMetadataObject
{
	private readonly IServer m_server;

	private readonly string m_name;

	private readonly CollationInfo m_collationInfo;

	private readonly bool m_isSystemObject;

	public IMetadataCollection<IApplicationRole> ApplicationRoles => Collection<IApplicationRole>.Empty;

	public IMetadataCollection<IAsymmetricKey> AsymmetricKeys => Collection<IAsymmetricKey>.Empty;

	public IMetadataCollection<ICertificate> Certificates => Collection<ICertificate>.Empty;

	public CollationInfo CollationInfo => m_collationInfo;

	public string DefaultSchemaName => null;

	public IUser Owner => null;

	public IMetadataCollection<IDatabaseRole> Roles => Collection<IDatabaseRole>.Empty;

	public IMetadataCollection<ISchema> Schemas => Collection<ISchema>.Empty;

	public IMetadataCollection<IDatabaseDdlTrigger> Triggers => Collection<IDatabaseDdlTrigger>.Empty;

	public IMetadataCollection<IUser> Users => Collection<IUser>.Empty;

	DatabaseCompatibilityLevel IDatabase.CompatibilityLevel => DatabaseCompatibilityLevel.Current;

	public IServer Server => m_server;

	public IDatabaseObject Parent => m_server;

	public bool IsSystemObject => m_isSystemObject;

	public string Name => m_name;

	public EmptyDatabase(IServer server, string name, CollationInfo collationInfo, bool isSystemObject)
	{
		m_server = server;
		m_name = name;
		m_collationInfo = collationInfo;
		m_isSystemObject = isSystemObject;
	}

	public T Accept<T>(IServerOwnedObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}

	public T Accept<T>(IDatabaseObjectVisitor<T> visitor)
	{
		return Accept((IServerOwnedObjectVisitor<T>)visitor);
	}

	public T Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		return Accept((IServerOwnedObjectVisitor<T>)visitor);
	}
}
