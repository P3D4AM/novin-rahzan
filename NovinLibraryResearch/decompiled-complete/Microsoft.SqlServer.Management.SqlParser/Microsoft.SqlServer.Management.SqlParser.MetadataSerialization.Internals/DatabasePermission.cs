using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class DatabasePermission : IDatabasePermission, IMetadataObject
{
	private readonly DeserializationModel model;

	private readonly int databasePrincipal;

	private readonly int grantor;

	private readonly PermissionState permissionState;

	private readonly DatabasePermissionType permissionType;

	private readonly int targetObject;

	private readonly string name = Guid.NewGuid().ToString();

	IDatabasePrincipal IDatabasePermission.DatabasePrincipal => model.GetObjectById<IDatabasePrincipal>(databasePrincipal);

	DatabasePermissionType IDatabasePermission.PermissionType => permissionType;

	IMetadataObject IDatabasePermission.TargetObject => model.GetObjectById<IMetadataObject>(targetObject);

	PermissionState IDatabasePermission.PermissionState => permissionState;

	IDatabasePrincipal IDatabasePermission.Grantor => model.GetObjectById<IDatabasePrincipal>(grantor);

	string IMetadataObject.Name => name;

	public DatabasePermission(DeserializationModel model, int databasePrincipal, int grantor, PermissionState permissionState, DatabasePermissionType permissionType, int targetObject)
	{
		this.model = model;
		this.databasePrincipal = databasePrincipal;
		this.grantor = grantor;
		this.permissionState = permissionState;
		this.permissionType = permissionType;
		this.targetObject = targetObject;
	}

	public DatabasePermission(DeserializationModel model, object[] values)
		: this(model, (int)values[0], (int)values[1], (PermissionState)values[2], (DatabasePermissionType)values[3], (int)values[4])
	{
	}

	T IMetadataObject.Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}
}
