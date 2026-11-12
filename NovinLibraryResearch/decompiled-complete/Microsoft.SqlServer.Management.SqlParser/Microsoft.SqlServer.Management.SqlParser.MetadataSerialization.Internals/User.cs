using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class User : IUser, IDatabasePrincipal, IDatabaseOwnedObject, IDatabaseObject, IMetadataObject
{
	private readonly DeserializationModel model;

	private readonly int asymmetricKey;

	private readonly int certificate;

	private readonly int database;

	private readonly int defaultSchema;

	private readonly bool isSystemObject;

	private readonly int login;

	private readonly int memberOfRoles;

	private readonly string name;

	private readonly string password;

	private readonly int permissions;

	private readonly UserType userType;

	UserType IUser.UserType => userType;

	IAsymmetricKey IUser.AsymmetricKey => model.GetObjectById<IAsymmetricKey>(asymmetricKey);

	ICertificate IUser.Certificate => model.GetObjectById<ICertificate>(certificate);

	ILogin IUser.Login => model.GetObjectById<ILogin>(login);

	string IUser.Password => password;

	ISchema IUser.DefaultSchema => model.GetObjectById<ISchema>(defaultSchema);

	IMetadataCollection<IDatabasePermission> IDatabasePrincipal.Permissions => model.GetCollectionById<IDatabasePermission>(permissions);

	IMetadataCollection<IDatabaseRole> IDatabasePrincipal.MemberOfRoles => model.GetCollectionById<IDatabaseRole>(memberOfRoles);

	IDatabase IDatabaseOwnedObject.Database => model.GetObjectById<IDatabase>(database);

	IDatabaseObject IDatabaseObject.Parent => ((IDatabaseOwnedObject)this).Database;

	bool IDatabaseObject.IsSystemObject => isSystemObject;

	string IMetadataObject.Name => name;

	public User(DeserializationModel model, int asymmetricKey, int certificate, int database, int defaultSchema, bool isSystemObject, int login, int memberOfRoles, string name, string password, int permissions, UserType userType)
	{
		this.model = model;
		this.asymmetricKey = asymmetricKey;
		this.certificate = certificate;
		this.database = database;
		this.defaultSchema = defaultSchema;
		this.isSystemObject = isSystemObject;
		this.login = login;
		this.memberOfRoles = memberOfRoles;
		this.name = name;
		this.password = password;
		this.permissions = permissions;
		this.userType = userType;
	}

	public User(DeserializationModel model, object[] values)
		: this(model, (int)values[0], (int)values[1], (int)values[2], (int)values[3], (bool)values[4], (int)values[5], (int)values[6], (string)values[7], (string)values[8], (int)values[9], (UserType)values[10])
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
