using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class DatabaseRole : IDatabaseRole, IDatabasePrincipal, IDatabaseOwnedObject, IDatabaseObject, IMetadataObject
{
	private readonly DeserializationModel model;

	private readonly int database;

	private readonly bool isFixedRole;

	private readonly bool isSystemObject;

	private readonly int memberOfRoles;

	private readonly string name;

	private readonly int owner;

	private readonly int permissions;

	bool IDatabaseRole.IsFixedRole => isFixedRole;

	IDatabasePrincipal IDatabaseRole.Owner => model.GetObjectById<IDatabasePrincipal>(owner);

	IMetadataCollection<IDatabasePermission> IDatabasePrincipal.Permissions => model.GetCollectionById<IDatabasePermission>(permissions);

	IMetadataCollection<IDatabaseRole> IDatabasePrincipal.MemberOfRoles => model.GetCollectionById<IDatabaseRole>(memberOfRoles);

	IDatabase IDatabaseOwnedObject.Database => model.GetObjectById<IDatabase>(database);

	IDatabaseObject IDatabaseObject.Parent => ((IDatabaseOwnedObject)this).Database;

	bool IDatabaseObject.IsSystemObject => isSystemObject;

	string IMetadataObject.Name => name;

	public DatabaseRole(DeserializationModel model, int database, bool isFixedRole, bool isSystemObject, int memberOfRoles, string name, int owner, int permissions)
	{
		this.model = model;
		this.database = database;
		this.isFixedRole = isFixedRole;
		this.isSystemObject = isSystemObject;
		this.memberOfRoles = memberOfRoles;
		this.name = name;
		this.owner = owner;
		this.permissions = permissions;
	}

	public DatabaseRole(DeserializationModel model, object[] values)
		: this(model, (int)values[0], (bool)values[1], (bool)values[2], (int)values[3], (string)values[4], (int)values[5], (int)values[6])
	{
	}

	T IDatabaseOwnedObject.Accept<T>(IDatabaseOwnedObjectVisitor<T> visitor)
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
		return ((IDatabaseOwnedObject)this).Accept((IDatabaseOwnedObjectVisitor<T>)visitor);
	}

	T IMetadataObject.Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return ((IDatabaseOwnedObject)this).Accept((IDatabaseOwnedObjectVisitor<T>)visitor);
	}
}
