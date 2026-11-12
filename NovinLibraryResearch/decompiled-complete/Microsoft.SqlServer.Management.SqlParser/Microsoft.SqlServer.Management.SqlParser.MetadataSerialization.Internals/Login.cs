using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class Login : ILogin, IServerOwnedObject, IDatabaseObject, IMetadataObject
{
	private readonly DeserializationModel model;

	private readonly int asymmetricKey;

	private readonly int certificate;

	private readonly int credential;

	private readonly int defaultDatabase;

	private readonly bool isSystemObject;

	private readonly string language;

	private readonly LoginType loginType;

	private readonly string name;

	private readonly IPassword password;

	private readonly int server;

	private readonly byte[] sid;

	LoginType ILogin.LoginType => loginType;

	IAsymmetricKey ILogin.AsymmetricKey => model.GetObjectById<IAsymmetricKey>(asymmetricKey);

	ICertificate ILogin.Certificate => model.GetObjectById<ICertificate>(certificate);

	ICredential ILogin.Credential => model.GetObjectById<ICredential>(credential);

	IDatabase ILogin.DefaultDatabase => model.GetObjectById<IDatabase>(defaultDatabase);

	string ILogin.Language => language;

	IPassword ILogin.Password => password;

	byte[] ILogin.Sid => sid;

	IServer IServerOwnedObject.Server => model.GetObjectById<IServer>(server);

	IDatabaseObject IDatabaseObject.Parent => ((IServerOwnedObject)this).Server;

	bool IDatabaseObject.IsSystemObject => isSystemObject;

	string IMetadataObject.Name => name;

	public Login(DeserializationModel model, int asymmetricKey, int certificate, int credential, int defaultDatabase, bool isSystemObject, string language, LoginType loginType, string name, IPassword password, int server, byte[] sid)
	{
		this.model = model;
		this.asymmetricKey = asymmetricKey;
		this.certificate = certificate;
		this.credential = credential;
		this.defaultDatabase = defaultDatabase;
		this.isSystemObject = isSystemObject;
		this.language = language;
		this.loginType = loginType;
		this.name = name;
		this.password = password;
		this.server = server;
		this.sid = sid;
	}

	public Login(DeserializationModel model, object[] values)
		: this(model, (int)values[0], (int)values[1], (int)values[2], (int)values[3], (bool)values[4], (string)values[5], (LoginType)values[6], (string)values[7], (IPassword)values[8], (int)values[9], (byte[])values[10])
	{
	}

	T IServerOwnedObject.Accept<T>(IServerOwnedObjectVisitor<T> visitor)
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
		return ((IServerOwnedObject)this).Accept((IServerOwnedObjectVisitor<T>)visitor);
	}

	T IMetadataObject.Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return ((IServerOwnedObject)this).Accept((IServerOwnedObjectVisitor<T>)visitor);
	}
}
