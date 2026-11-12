using System;
using System.Collections.Generic;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlCreateLoginWithPasswordStatement : SqlCreateLoginStatement
{
	private readonly LoginOptions options;

	private readonly SqlLoginPassword sqlPassword;

	internal override bool IsVisitable => true;

	internal sealed override LoginType MetadataLoginType => LoginType.Sql;

	public SqlLoginPassword SqlPassword => sqlPassword;

	public SqlLiteralExpression Sid => options.Sid;

	public SqlIdentifier DefaultDatabase => options.DefaultDatabase;

	public SqlIdentifier DefaultLanguage => options.DefaultLanguage;

	public SqlIdentifier Credential => options.Credential;

	public SqlOnOffValue CheckExpiration => options.CheckExpiration;

	public SqlOnOffValue CheckPolicy => options.CheckPolicy;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return base.Name;
			yield return SqlPassword;
			foreach (SqlCodeObject child in options.Children)
			{
				yield return child;
			}
		}
	}

	public override void Accept(ISqlStatementVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override T Accept<T>(ISqlStatementVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}

	public override void Accept<C>(ISqlStatementContextVisitor<C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this, context);
	}

	public override T Accept<T, C>(ISqlStatementContextVisitor<T, C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this, context);
	}

	public override void Accept(ISqlCodeObjectVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override T Accept<T>(ISqlCodeObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}

	public override void Accept<C>(ISqlCodeObjectContextVisitor<C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this, context);
	}

	public override T Accept<T, C>(ISqlCodeObjectContextVisitor<T, C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this, context);
	}

	internal override void BindLogin(BatchBindingContext bindingContext, IMutableLogin login, bool setBuildProperties)
	{
		if (DefaultDatabase != null)
		{
			login.DefaultDatabase = BindDatabase(bindingContext, DefaultDatabase);
		}
		if (DefaultLanguage != null)
		{
			login.Language = DefaultLanguage.Value;
		}
		if (Credential != null)
		{
			login.Credential = BindCredential(bindingContext, Credential);
		}
		if (setBuildProperties)
		{
			login.Sid = null;
			IMutablePassword mutablePassword = bindingContext.MetadataFactory.Login.CreatePassword();
			mutablePassword.Value = null;
			mutablePassword.IsHashed = sqlPassword.IsHashed;
			mutablePassword.MustChange = SqlPassword.MustChange;
			mutablePassword.CheckExpiration = CheckExpiration == SqlOnOffValue.On;
			mutablePassword.CheckPolicy = CheckPolicy == SqlOnOffValue.On || CheckPolicy == SqlOnOffValue.None;
			login.Password = mutablePassword;
		}
	}

	internal override IMetadataCollection<IMetadataObject> GetCompletionList(LexLocation caretLocation, CompletionContext completionContext)
	{
		IMetadataCollection<IMetadataObject> result = null;
		SqlCodeObject sqlCodeObject = FindChildContainingLocation(ref caretLocation);
		if (sqlCodeObject == Credential)
		{
			result = sqlCodeObject.GetCompletionList(caretLocation, CompletionContext.Credentials);
		}
		return result;
	}

	internal SqlCreateLoginWithPasswordStatement(SqlIdentifier loginName, SqlLoginPassword sqlPassword, List<SqlLoginOption> options)
		: base(loginName)
	{
		this.sqlPassword = sqlPassword;
		this.options = new LoginOptions(options);
	}
}
