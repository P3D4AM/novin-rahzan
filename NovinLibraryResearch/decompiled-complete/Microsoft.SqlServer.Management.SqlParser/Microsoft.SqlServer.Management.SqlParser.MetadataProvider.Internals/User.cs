using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class User : DatabasePrincipal, IMutableUser, IUser, IDatabasePrincipal, IDatabaseOwnedObject, IDatabaseObject, IMetadataObject, IMutableDatabasePrincipal, IMutableDatabaseOwnedObject, IMutableDatabaseObject, IMutableMetadataObject
{
	private readonly UserType userType;

	private IAsymmetricKey asymmetricKey;

	private ICertificate certificate;

	private ILogin login;

	private ISchema defaultSchema;

	private string password;

	public UserType UserType => userType;

	public IAsymmetricKey AsymmetricKey
	{
		get
		{
			return asymmetricKey;
		}
		set
		{
			asymmetricKey = value;
		}
	}

	public ICertificate Certificate
	{
		get
		{
			return certificate;
		}
		set
		{
			certificate = value;
		}
	}

	public ILogin Login
	{
		get
		{
			return login;
		}
		set
		{
			login = value;
		}
	}

	public string Password
	{
		get
		{
			return password;
		}
		set
		{
			password = value;
		}
	}

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

	public User(IDatabase database, string name, UserType userType)
		: base(database, name)
	{
		this.userType = userType;
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
