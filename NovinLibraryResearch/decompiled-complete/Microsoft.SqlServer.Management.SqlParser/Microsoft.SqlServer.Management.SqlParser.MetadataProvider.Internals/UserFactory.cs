using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class UserFactory : ObjectFactoryBase<UserFactory>, IUserFactory
{
	public IMutableUser CreateAsymmetricKeyUser(IDatabase database, string name)
	{
		if (database == null)
		{
			throw new ArgumentNullException("database");
		}
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return new User(database, name, UserType.AsymmetricKey);
	}

	public IMutableUser CreateCertificateUser(IDatabase database, string name)
	{
		if (database == null)
		{
			throw new ArgumentNullException("database");
		}
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return new User(database, name, UserType.Certificate);
	}

	public IMutableUser CreateNoLoginUser(IDatabase database, string name)
	{
		if (database == null)
		{
			throw new ArgumentNullException("database");
		}
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return new User(database, name, UserType.NoLogin);
	}

	public IMutableUser CreateSqlLoginUser(IDatabase database, string name)
	{
		if (database == null)
		{
			throw new ArgumentNullException("database");
		}
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return new User(database, name, UserType.SqlLogin);
	}

	public IMutableUser CreateExternalProviderUser(IDatabase database, string name)
	{
		if (database == null)
		{
			throw new ArgumentNullException("database");
		}
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return new User(database, name, UserType.External);
	}
}
