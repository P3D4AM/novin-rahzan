using System;
using System.Collections.Generic;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlCreateUserFromLoginStatement : SqlCreateUserStatement
{
	private readonly SqlIdentifier loginName;

	internal override bool IsVisitable => true;

	internal sealed override UserType MetadataUserType => UserType.SqlLogin;

	public SqlIdentifier LoginName => loginName;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return base.Name;
			yield return loginName;
			if (base.DefaultSchema != null)
			{
				yield return base.DefaultSchema;
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
		if (loginName != null)
		{
			ILogin login = BindLogin(bindingContext, loginName);
			user.Login = login;
		}
	}

	internal override IMetadataCollection<IMetadataObject> GetCompletionList(LexLocation caretLocation, CompletionContext completionContext)
	{
		IMetadataCollection<IMetadataObject> result = null;
		SqlCodeObject sqlCodeObject = FindChildContainingLocation(ref caretLocation);
		if (sqlCodeObject == LoginName)
		{
			result = sqlCodeObject.GetCompletionList(caretLocation, CompletionContext.Logins);
		}
		return result;
	}

	internal SqlCreateUserFromLoginStatement(SqlIdentifier userName, SqlIdentifier loginName, SqlIdentifier defaultSchema)
		: base(userName, defaultSchema)
	{
		this.loginName = loginName;
	}
}
