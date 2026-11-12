using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlCreateLoginStatement : SqlDdlStatement
{
	private IMutableLogin boundLogin;

	private readonly SqlIdentifier name;

	internal sealed override string StatementPhrase => "CREATE LOGIN";

	internal sealed override bool IsCreateStatement => true;

	internal sealed override bool IsAlterStatement => false;

	internal sealed override bool IsDropStatement => false;

	internal sealed override DdlTargetObjectType TargetObjectType => DdlTargetObjectType.Login;

	internal abstract LoginType MetadataLoginType { get; }

	public ILogin BoundLogin => boundLogin;

	public override IMetadataObject BoundObject => BoundLogin;

	public SqlIdentifier Name => name;

	internal IAsymmetricKey BindAsymmetricKey(BatchBindingContext bindingContext, SqlIdentifier asymmetricKeyId)
	{
		LexicalScope lexicalScope = GetLexicalScope();
		IAsymmetricKey asymmetricKey = bindingContext.FindServerAsymmetricKey(asymmetricKeyId.Value, lexicalScope);
		if (asymmetricKey != null)
		{
			bindingContext.SetBoundObject(asymmetricKeyId, asymmetricKey);
			base.BindingState |= BindingState.BindSucceeded;
		}
		else
		{
			asymmetricKeyId.ReportBindError(SR.CouldNotFindAsymmetricKey(asymmetricKeyId.Value), ErrorFlags.UnresolvedReference);
			base.BindingState |= BindingState.BindFailed;
		}
		return asymmetricKey;
	}

	internal ICredential BindCredential(BatchBindingContext bindingContext, SqlIdentifier credentialId)
	{
		LexicalScope lexicalScope = GetLexicalScope();
		ICredential credential = bindingContext.FindCredential(credentialId.Value, lexicalScope);
		if (credential != null)
		{
			bindingContext.SetBoundObject(credentialId, credential);
			base.BindingState |= BindingState.BindSucceeded;
		}
		else
		{
			credentialId.ReportBindError(SR.CouldNotFindCredential(credentialId.Value), ErrorFlags.UnresolvedReference);
			base.BindingState |= BindingState.BindFailed;
		}
		return credential;
	}

	internal ICertificate BindCertificate(BatchBindingContext bindingContext, SqlIdentifier certificateId)
	{
		LexicalScope lexicalScope = GetLexicalScope();
		ICertificate certificate = bindingContext.FindServerCertificate(certificateId.Value, lexicalScope);
		if (certificate != null)
		{
			bindingContext.SetBoundObject(certificateId, certificate);
			base.BindingState |= BindingState.BindSucceeded;
		}
		else
		{
			certificateId.ReportBindError(SR.CouldNotFindCertificate(certificateId.Value), ErrorFlags.UnresolvedReference);
			base.BindingState |= BindingState.BindFailed;
		}
		return certificate;
	}

	internal IDatabase BindDatabase(BatchBindingContext bindingContext, SqlIdentifier databaseId)
	{
		DatabaseEx databaseEx = bindingContext.FindDatabase(databaseId.Value);
		if (databaseEx != null)
		{
			bindingContext.SetBoundObject(databaseId, databaseEx.ProxyObject);
			base.BindingState |= BindingState.BindSucceeded;
		}
		else
		{
			databaseId.ReportBindError(SR.DatabaseNotExist(databaseId.Value), ErrorFlags.UnresolvedReference);
			base.BindingState |= BindingState.BindFailed;
		}
		return databaseEx?.ProxyObject;
	}

	internal abstract void BindLogin(BatchBindingContext bindingContext, IMutableLogin login, bool setBuildProperties);

	internal override void InitiateBindStatement(BatchBindingContext bindingContext, StatementBindMode bindMode)
	{
		ServerEx server = bindingContext.GetServer();
		ILoginFactory login = bindingContext.MetadataFactory.Login;
		switch (MetadataLoginType)
		{
		case LoginType.AsymmetricKey:
			boundLogin = login.CreateAsymmetricKeyLogin(server.ProxyObject, name.Value);
			break;
		case LoginType.Certificate:
			boundLogin = login.CreateCertificateLogin(server.ProxyObject, name.Value);
			break;
		case LoginType.Sql:
			boundLogin = login.CreateSqlLogin(server.ProxyObject, name.Value);
			break;
		case LoginType.Windows:
			boundLogin = login.CreateWindowsLogin(server.ProxyObject, name.Value);
			break;
		}
		base.BindingState |= BindingState.BindSucceeded;
		LexicalScope lexicalScope = GetLexicalScope();
		if (bindingContext.FindLogin(name.Value, lexicalScope) != null)
		{
			Name.ReportBindError(SR.LoginExist(Name.Value));
			base.BindingState |= BindingState.BindFailed;
			return;
		}
		if (!name.IsEmpty)
		{
			server.Logins.Create(boundLogin, this, bindMode);
		}
		base.BindingState |= BindingState.BindSucceeded;
	}

	internal sealed override void FinalizeBindStatement(BatchBindingContext bindingContext, StatementBindMode bindMode)
	{
		bool setBuildProperties = bindMode == StatementBindMode.OutOfOrder;
		BindLogin(bindingContext, boundLogin, setBuildProperties);
	}

	internal override void Unbind()
	{
		boundLogin = null;
		base.Unbind();
	}

	internal SqlCreateLoginStatement(SqlIdentifier name)
	{
		this.name = name;
	}
}
