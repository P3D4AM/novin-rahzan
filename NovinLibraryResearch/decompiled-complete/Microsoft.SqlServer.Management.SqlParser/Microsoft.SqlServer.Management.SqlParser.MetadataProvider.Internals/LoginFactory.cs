using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class LoginFactory : ObjectFactoryBase<LoginFactory>, ILoginFactory
{
	public IMutableLogin CreateAsymmetricKeyLogin(IServer server, string name)
	{
		if (server == null)
		{
			throw new ArgumentNullException("server");
		}
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return new Login(server, name, LoginType.AsymmetricKey);
	}

	public IMutableLogin CreateCertificateLogin(IServer server, string name)
	{
		if (server == null)
		{
			throw new ArgumentNullException("server");
		}
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return new Login(server, name, LoginType.Certificate);
	}

	public IMutableLogin CreateSqlLogin(IServer server, string name)
	{
		if (server == null)
		{
			throw new ArgumentNullException("server");
		}
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return new Login(server, name, LoginType.Sql);
	}

	public IMutableLogin CreateWindowsLogin(IServer server, string name)
	{
		if (server == null)
		{
			throw new ArgumentNullException("server");
		}
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return new Login(server, name, LoginType.Windows);
	}

	public IMutablePassword CreatePassword()
	{
		return new Password();
	}
}
