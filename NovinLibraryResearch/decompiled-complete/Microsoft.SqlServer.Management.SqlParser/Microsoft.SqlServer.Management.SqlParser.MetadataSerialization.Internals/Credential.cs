using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class Credential : ICredential, IServerOwnedObject, IDatabaseObject, IMetadataObject
{
	private readonly DeserializationModel model;

	private readonly bool isSystemObject;

	private readonly string name;

	private readonly int server;

	IServer IServerOwnedObject.Server => model.GetObjectById<IServer>(server);

	IDatabaseObject IDatabaseObject.Parent => ((IServerOwnedObject)this).Server;

	bool IDatabaseObject.IsSystemObject => isSystemObject;

	string IMetadataObject.Name => name;

	public Credential(DeserializationModel model, bool isSystemObject, string name, int server)
	{
		this.model = model;
		this.isSystemObject = isSystemObject;
		this.name = name;
		this.server = server;
	}

	public Credential(DeserializationModel model, object[] values)
		: this(model, (bool)values[0], (string)values[1], (int)values[2])
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
