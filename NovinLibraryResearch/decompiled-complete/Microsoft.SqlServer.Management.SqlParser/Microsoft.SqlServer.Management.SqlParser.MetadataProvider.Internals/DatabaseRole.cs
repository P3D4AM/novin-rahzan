using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class DatabaseRole : DatabasePrincipal, IMutableDatabaseRole, IDatabaseRole, IDatabasePrincipal, IDatabaseOwnedObject, IDatabaseObject, IMetadataObject, IMutableDatabasePrincipal, IMutableDatabaseOwnedObject, IMutableDatabaseObject, IMutableMetadataObject
{
	private bool isFixedRole;

	private IDatabasePrincipal owner;

	public bool IsFixedRole
	{
		get
		{
			return isFixedRole;
		}
		set
		{
			isFixedRole = value;
		}
	}

	public IDatabasePrincipal Owner
	{
		get
		{
			return owner;
		}
		set
		{
			owner = value;
		}
	}

	public DatabaseRole(IDatabase database, string name)
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
