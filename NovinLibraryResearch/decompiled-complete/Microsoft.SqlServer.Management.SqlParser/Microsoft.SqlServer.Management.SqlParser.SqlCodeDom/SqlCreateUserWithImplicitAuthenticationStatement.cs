using System;
using System.Collections.Generic;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlCreateUserWithImplicitAuthenticationStatement : SqlCreateUserStatement
{
	private readonly SqlIdentifier defaultLanguage;

	private readonly string password;

	internal override bool IsVisitable => true;

	internal sealed override UserType MetadataUserType => UserType.SqlLogin;

	public SqlIdentifier DefaultLanguage => defaultLanguage;

	public string Password => password;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return base.Name;
			if (base.DefaultSchema != null && defaultLanguage != null)
			{
				if (base.DefaultSchema.Position.CompareTo(defaultLanguage.Position) <= 0)
				{
					yield return base.DefaultSchema;
					yield return defaultLanguage;
				}
				else
				{
					yield return defaultLanguage;
					yield return base.DefaultSchema;
				}
				yield break;
			}
			if (base.DefaultSchema != null)
			{
				yield return base.DefaultSchema;
			}
			if (defaultLanguage != null)
			{
				yield return defaultLanguage;
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

	internal override void BindUser(BatchBindingContext bindingContext, IMutableUser user)
	{
		if (base.Name != null)
		{
			ILogin login = BindLogin(bindingContext, base.Name);
			user.Login = login;
		}
	}

	internal override IMetadataCollection<IMetadataObject> GetCompletionList(LexLocation caretLocation, CompletionContext completionContext)
	{
		return null;
	}

	internal SqlCreateUserWithImplicitAuthenticationStatement(SqlIdentifier userName, SqlIdentifier defaultSchema, SqlIdentifier defaultLanguage, string password)
		: base(userName, defaultSchema)
	{
		this.defaultLanguage = defaultLanguage;
		this.password = password;
	}
}
