using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class Server : IServer, IDatabaseObject, IMetadataObject
{
	private readonly DeserializationModel model;

	private readonly CollationInfo collationInfo;

	private readonly int credentials;

	private readonly int databases;

	private readonly bool isSystemObject;

	private readonly int logins;

	private readonly string name;

	private readonly int triggers;

	CollationInfo IServer.CollationInfo => collationInfo;

	IMetadataCollection<IDatabase> IServer.Databases => model.GetCollectionById<IDatabase>(databases);

	IMetadataCollection<ICredential> IServer.Credentials => model.GetCollectionById<ICredential>(credentials);

	IMetadataCollection<ILogin> IServer.Logins => model.GetCollectionById<ILogin>(logins);

	IMetadataCollection<IServerDdlTrigger> IServer.Triggers => model.GetCollectionById<IServerDdlTrigger>(triggers);

	IDatabaseObject IDatabaseObject.Parent => null;

	bool IDatabaseObject.IsSystemObject => isSystemObject;

	string IMetadataObject.Name => name;

	public Server(DeserializationModel model, CollationInfo collationInfo, int credentials, int databases, bool isSystemObject, int logins, string name, int triggers)
	{
		this.model = model;
		this.collationInfo = collationInfo;
		this.credentials = credentials;
		this.databases = databases;
		this.isSystemObject = isSystemObject;
		this.logins = logins;
		this.name = name;
		this.triggers = triggers;
	}

	public Server(DeserializationModel model, object[] values)
		: this(model, (CollationInfo)values[0], (int)values[1], (int)values[2], (bool)values[3], (int)values[4], (string)values[5], (int)values[6])
	{
	}

	T IDatabaseObject.Accept<T>(IDatabaseObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}

	T IMetadataObject.Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return ((IDatabaseObject)this).Accept((IDatabaseObjectVisitor<T>)visitor);
	}
}
