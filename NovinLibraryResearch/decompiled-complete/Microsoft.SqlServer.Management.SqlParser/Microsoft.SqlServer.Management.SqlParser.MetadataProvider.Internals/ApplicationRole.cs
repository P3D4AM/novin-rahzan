using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class ApplicationRole : DatabasePrincipal, IMutableApplicationRole, IApplicationRole, IDatabasePrincipal, IDatabaseOwnedObject, IDatabaseObject, IMetadataObject, IMutableDatabasePrincipal, IMutableDatabaseOwnedObject, IMutableDatabaseObject, IMutableMetadataObject
{
	private ISchema defaultSchema;

	public ISchema DefaultSchema
	{
		get
		{
			return defaultSchema;
		}
		set
		{
			defaultSchema = value;
		}
	}

	public ApplicationRole(IDatabase database, string name)
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
