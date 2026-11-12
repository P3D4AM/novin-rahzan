using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class Login : ServerOwnedObject, IMutableLogin, ILogin, IServerOwnedObject, IDatabaseObject, IMetadataObject, IMutableServerOwnedObject, IMutableDatabaseObject, IMutableMetadataObject
{
	private readonly LoginType m_loginType;

	private IAsymmetricKey m_asymmetricKey;

	private ICertificate m_certificate;

	private ICredential m_credential;

	private IDatabase m_defaultDatabase;

	private IPassword m_password;

	private string m_language;

	private byte[] m_sid;

	public LoginType LoginType => m_loginType;

	public IAsymmetricKey AsymmetricKey
	{
		get
		{
			return m_asymmetricKey;
		}
		set
		{
			m_asymmetricKey = value;
		}
	}

	public ICertificate Certificate
	{
		get
		{
			return m_certificate;
		}
		set
		{
			m_certificate = value;
		}
	}

	public ICredential Credential
	{
		get
		{
			return m_credential;
		}
		set
		{
			m_credential = value;
		}
	}

	public IDatabase DefaultDatabase
	{
		get
		{
			return m_defaultDatabase;
		}
		set
		{
			m_defaultDatabase = value;
		}
	}

	public string Language
	{
		get
		{
			return m_language;
		}
		set
		{
			m_language = value;
		}
	}

	public IPassword Password
	{
		get
		{
			return m_password;
		}
		set
		{
			m_password = value;
		}
	}

	public byte[] Sid
	{
		get
		{
			return m_sid;
		}
		set
		{
			m_sid = value;
		}
	}

	public Login(IServer server, string name, LoginType loginType)
		: base(server, name)
	{
		m_loginType = loginType;
	}

	public override T Accept<T>(IServerOwnedObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}
}
