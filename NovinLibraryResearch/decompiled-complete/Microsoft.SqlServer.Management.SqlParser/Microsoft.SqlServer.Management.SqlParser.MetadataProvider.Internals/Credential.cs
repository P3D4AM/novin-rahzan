using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class Credential : ServerOwnedObject, IMutableCredential, ICredential, IServerOwnedObject, IDatabaseObject, IMetadataObject, IMutableServerOwnedObject, IMutableDatabaseObject, IMutableMetadataObject
{
	public Credential(IServer server, string name)
		: base(server, name)
	{
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
