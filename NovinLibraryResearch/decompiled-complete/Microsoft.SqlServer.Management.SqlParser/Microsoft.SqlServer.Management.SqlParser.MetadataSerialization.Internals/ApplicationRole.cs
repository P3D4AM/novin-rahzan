using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class ApplicationRole : IApplicationRole, IDatabasePrincipal, IDatabaseOwnedObject, IDatabaseObject, IMetadataObject
{
	private readonly DeserializationModel model;

	private readonly int database;

	private readonly int defaultSchema;

	private readonly bool isSystemObject;

	private readonly int memberOfRoles;

	private readonly string name;

	private readonly int permissions;

	ISchema IApplicationRole.DefaultSchema => model.GetObjectById<ISchema>(defaultSchema);

	IMetadataCollection<IDatabasePermission> IDatabasePrincipal.Permissions => model.GetCollectionById<IDatabasePermission>(permissions);

	IMetadataCollection<IDatabaseRole> IDatabasePrincipal.MemberOfRoles => model.GetCollectionById<IDatabaseRole>(memberOfRoles);

	IDatabase IDatabaseOwnedObject.Database => model.GetObjectById<IDatabase>(database);

	IDatabaseObject IDatabaseObject.Parent => ((IDatabaseOwnedObject)this).Database;

	bool IDatabaseObject.IsSystemObject => isSystemObject;

	string IMetadataObject.Name => name;

	public ApplicationRole(DeserializationModel model, int database, int defaultSchema, bool isSystemObject, int memberOfRoles, string name, int permissions)
	{
		this.model = model;
		this.database = database;
		this.defaultSchema = defaultSchema;
		this.isSystemObject = isSystemObject;
		this.memberOfRoles = memberOfRoles;
		this.name = name;
		this.permissions = permissions;
	}

	public ApplicationRole(DeserializationModel model, object[] values)
		: this(model, (int)values[0], (int)values[1], (bool)values[2], (int)values[3], (string)values[4], (int)values[5])
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
