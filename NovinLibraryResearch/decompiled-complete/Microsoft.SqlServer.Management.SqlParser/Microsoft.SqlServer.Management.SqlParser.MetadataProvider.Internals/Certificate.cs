using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class Certificate : DatabaseOwnedObject, IMutableCertificate, ICertificate, IDatabaseOwnedObject, IDatabaseObject, IMetadataObject, IMutableDatabaseOwnedObject, IMutableDatabaseObject, IMutableMetadataObject
{
	public Certificate(IDatabase database, string name)
		: base(database, name)
	{
	}

	public override T Accept<T>(IDatabaseOwnedObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}
}
