using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class DatabasePermission : IMutableDatabasePermission, IDatabasePermission, IMetadataObject
{
	private readonly string name;

	private readonly IDatabasePrincipal databasePrincipal;

	private readonly IMetadataObject targetObject;

	private readonly DatabasePermissionType permissionType;

	private readonly IDatabasePrincipal grantor;

	private PermissionState permissionState;

	public PermissionState PermissionState
	{
		get
		{
			return permissionState;
		}
		set
		{
			permissionState = value;
		}
	}

	public IDatabasePrincipal DatabasePrincipal => databasePrincipal;

	public DatabasePermissionType PermissionType => permissionType;

	public IMetadataObject TargetObject => targetObject;

	public IDatabasePrincipal Grantor => grantor;

	public string Name => name;

	public DatabasePermission(IDatabasePrincipal databasePrincipal, IMetadataObject targetObject, DatabasePermissionType permissionType, IDatabasePrincipal grantor)
	{
		name = Guid.NewGuid().ToString();
		this.databasePrincipal = databasePrincipal;
		this.targetObject = targetObject;
		this.permissionType = permissionType;
		this.grantor = grantor;
	}

	public T Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}
}
