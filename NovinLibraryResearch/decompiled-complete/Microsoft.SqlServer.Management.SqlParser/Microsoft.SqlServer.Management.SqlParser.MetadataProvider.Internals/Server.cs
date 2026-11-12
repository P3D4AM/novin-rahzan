using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class Server : IMutableServer, IServer, IDatabaseObject, IMetadataObject, IMutableDatabaseObject, IMutableMetadataObject
{
	private readonly string m_name;

	private readonly CollationInfo m_collationInfo;

	private readonly DatabaseCollection m_databases;

	private readonly CredentialCollection m_credentials;

	private readonly LoginCollection m_logins;

	private readonly ServerDdlTriggerCollection m_triggers;

	private bool m_isSystemObject;

	public IMutableMetadataCollection<IDatabase> Databases => m_databases;

	public IMutableMetadataCollection<ICredential> Credentials => m_credentials;

	public IMutableMetadataCollection<ILogin> Logins => m_logins;

	public IMutableMetadataCollection<IServerDdlTrigger> Triggers => m_triggers;

	public CollationInfo CollationInfo => m_collationInfo;

	IMetadataCollection<IDatabase> IServer.Databases => m_databases;

	IMetadataCollection<ICredential> IServer.Credentials => m_credentials;

	IMetadataCollection<ILogin> IServer.Logins => m_logins;

	IMetadataCollection<IServerDdlTrigger> IServer.Triggers => m_triggers;

	public IDatabaseObject Parent => null;

	bool IDatabaseObject.IsSystemObject => m_isSystemObject;

	public bool IsVolatile => false;

	public bool IsSystemObject
	{
		get
		{
			return m_isSystemObject;
		}
		set
		{
			m_isSystemObject = value;
		}
	}

	public string Name => m_name;

	public Server(string name, CollationInfo collationInfo)
	{
		m_name = name;
		m_collationInfo = collationInfo;
		m_databases = new DatabaseCollection(collationInfo);
		m_credentials = new CredentialCollection(collationInfo);
		m_logins = new LoginCollection(collationInfo);
		m_triggers = new ServerDdlTriggerCollection(collationInfo);
	}

	public Server(IServer otherServer)
		: this(otherServer.Name, otherServer.CollationInfo)
	{
		m_databases.AddRange(otherServer.Databases);
		m_credentials.AddRange(otherServer.Credentials);
		m_logins.AddRange(otherServer.Logins);
		m_triggers.AddRange(otherServer.Triggers);
	}

	public T Accept<T>(IDatabaseObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}

	public T Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		return Accept((IDatabaseObjectVisitor<T>)visitor);
	}
}
