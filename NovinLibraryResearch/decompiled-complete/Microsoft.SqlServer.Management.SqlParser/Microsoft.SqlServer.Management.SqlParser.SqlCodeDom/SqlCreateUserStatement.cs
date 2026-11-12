using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlCreateUserStatement : SqlDdlStatement
{
	private IMutableUser boundUser;

	private readonly SqlIdentifier name;

	private readonly SqlIdentifier defaultSchema;

	internal sealed override string StatementPhrase => "CREATE USER";

	internal sealed override bool IsCreateStatement => true;

	internal sealed override bool IsAlterStatement => false;

	internal sealed override bool IsDropStatement => false;

	internal sealed override DdlTargetObjectType TargetObjectType => DdlTargetObjectType.User;

	internal abstract UserType MetadataUserType { get; }

	public IUser BoundUser => boundUser;

	public override IMetadataObject BoundObject => BoundUser;

	public SqlIdentifier Name => name;

	public SqlIdentifier DefaultSchema => defaultSchema;

	internal IAsymmetricKey BindAsymmetricKey(BatchBindingContext bindingContext, SqlIdentifier asymmetricKeyId)
	{
		LexicalScope lexicalScope = GetLexicalScope();
		IAsymmetricKey asymmetricKey = bindingContext.FindConnectedDatabaseAsymmetricKey(asymmetricKeyId.Value, lexicalScope);
		if (asymmetricKey != null)
		{
			bindingContext.SetBoundObject(asymmetricKeyId, asymmetricKey);
			base.BindingState |= BindingState.BindSucceeded;
		}
		else
		{
			asymmetricKeyId.ReportBindError(SR.CouldNotFindAsymmetricKey(asymmetricKeyId.Value), ErrorFlags.UnresolvedDatabaseObjectReference);
			base.BindingState |= BindingState.BindFailed;
		}
		return asymmetricKey;
	}

	internal ICertificate BindCertificate(BatchBindingContext bindingContext, SqlIdentifier certificateId)
	{
		LexicalScope lexicalScope = GetLexicalScope();
		ICertificate certificate = bindingContext.FindConnectedDatabaseCertificate(certificateId.Value, lexicalScope);
		if (certificate != null)
		{
			bindingContext.SetBoundObject(certificateId, certificate);
			base.BindingState |= BindingState.BindSucceeded;
		}
		else
		{
			certificateId.ReportBindError(SR.CouldNotFindCertificate(certificateId.Value), ErrorFlags.UnresolvedDatabaseObjectReference);
			base.BindingState |= BindingState.BindFailed;
		}
		return certificate;
	}

	internal ILogin BindLogin(BatchBindingContext bindingContext, SqlIdentifier loginId)
	{
		LexicalScope lexicalScope = GetLexicalScope();
		ILogin login = bindingContext.FindLogin(loginId.Value, lexicalScope);
		if (login != null)
		{
			bindingContext.SetBoundObject(loginId, login);
		}
		base.BindingState |= BindingState.BindSucceeded;
		return login;
	}

	internal abstract void BindUser(BatchBindingContext bindingContext, IMutableUser user);

	internal sealed override void InitiateBindStatement(BatchBindingContext bindingContext, StatementBindMode bindMode)
	{
		DatabaseEx connectedDatabase = bindingContext.ConnectedDatabase;
		IUserFactory user = bindingContext.MetadataFactory.User;
		switch (MetadataUserType)
		{
		case UserType.AsymmetricKey:
			boundUser = user.CreateAsymmetricKeyUser(connectedDatabase.ProxyObject, name.Value);
			break;
		case UserType.Certificate:
			boundUser = user.CreateCertificateUser(connectedDatabase.ProxyObject, name.Value);
			break;
		case UserType.NoLogin:
			boundUser = user.CreateNoLoginUser(connectedDatabase.ProxyObject, name.Value);
			break;
		case UserType.SqlLogin:
			boundUser = user.CreateSqlLoginUser(connectedDatabase.ProxyObject, name.Value);
			break;
		case UserType.External:
			boundUser = user.CreateExternalProviderUser(connectedDatabase.ProxyObject, name.Value);
			break;
		}
		LexicalScope lexicalScope = GetLexicalScope();
		if (bindingContext.FindUser(name.Value, lexicalScope) != null)
		{
			name.ReportBindError(SR.UserExist(name.Value));
			base.BindingState |= BindingState.BindFailed;
			return;
		}
		if (!name.IsEmpty)
		{
			connectedDatabase.Users.Create(boundUser, this, bindMode);
		}
		base.BindingState |= BindingState.BindSucceeded;
	}

	internal sealed override void FinalizeBindStatement(BatchBindingContext bindingContext, StatementBindMode bindMode)
	{
		BindUser(bindingContext, boundUser);
		if (!SqlIdentifier.IsNullOrEmpty(defaultSchema))
		{
			SchemaEx schemaEx = bindingContext.FindSchema(null, defaultSchema.Value, GetLexicalScope());
			if (schemaEx != null)
			{
				boundUser.DefaultSchema = schemaEx.ProxyObject;
				bindingContext.SetBoundObject(defaultSchema, schemaEx.ProxyObject);
			}
		}
	}

	internal override void Unbind()
	{
		boundUser = null;
		base.Unbind();
	}

	internal SqlCreateUserStatement(SqlIdentifier name, SqlIdentifier defaultSchema)
	{
		this.name = name;
		this.defaultSchema = defaultSchema;
	}

	internal static SqlCreateUserStatement CreateFromFactory(CreateUserStatementFactory factory, SqlIdentifier userName, SqlIdentifier defaultSchema)
	{
		return factory.Create(userName, defaultSchema);
	}

	internal static SqlCreateUserStatement CreateFromFactory(CreateUserStatementFactory factory, SqlIdentifier userName, IEnumerable<SqlCreateUserOption> createUserOptions)
	{
		SqlCreateUserOption sqlCreateUserOption = null;
		SqlCreateUserOption sqlCreateUserOption2 = null;
		SqlCreateUserOption sqlCreateUserOption3 = null;
		if (createUserOptions != null)
		{
			foreach (SqlCreateUserOption createUserOption in createUserOptions)
			{
				switch (createUserOption.Type)
				{
				case SqlCreateUserOptionType.DefaultLanguage:
					if (sqlCreateUserOption2 != null)
					{
						createUserOption.Value.ReportSyntaxError();
					}
					else
					{
						sqlCreateUserOption2 = createUserOption;
					}
					break;
				case SqlCreateUserOptionType.DefaultSchema:
					if (sqlCreateUserOption != null)
					{
						createUserOption.Value.ReportSyntaxError();
					}
					else
					{
						sqlCreateUserOption = createUserOption;
					}
					break;
				case SqlCreateUserOptionType.Password:
					if (sqlCreateUserOption3 != null)
					{
						createUserOption.Value.ReportSyntaxError();
					}
					else
					{
						sqlCreateUserOption3 = createUserOption;
					}
					break;
				}
			}
		}
		return factory.Create(userName, sqlCreateUserOption?.Value, sqlCreateUserOption2?.Value, sqlCreateUserOption3?.Value.Value);
	}
}
