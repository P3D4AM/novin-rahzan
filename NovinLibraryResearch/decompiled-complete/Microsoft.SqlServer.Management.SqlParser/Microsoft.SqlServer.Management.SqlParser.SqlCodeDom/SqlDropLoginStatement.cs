using System;
using System.Collections.Generic;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlDropLoginStatement : SqlDropStatement, IDropObjectStatement<ILogin, SqlIdentifier>
{
	private SqlIdentifier loginName;

	internal override bool IsVisitable => true;

	internal sealed override string StatementPhrase => "DROP LOGIN";

	internal sealed override DdlTargetObjectType TargetObjectType => DdlTargetObjectType.Login;

	ISqlCodeCollection<SqlIdentifier> IDropObjectStatement<ILogin, SqlIdentifier>.DropObjects => new SqlIdentifierCollection(new SqlIdentifier[1] { loginName });

	public SqlIdentifier LoginName => loginName;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return loginName;
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

	internal override void BindStatement(BatchBindingContext bindingContext)
	{
		SqlDropStatement.BindDropObjectStatement(this, DropObjectBinderHelper.GetStandardHelper(this), bindingContext);
	}

	ILogin IDropObjectStatement<ILogin, SqlIdentifier>.FindMetadataObject(BatchBindingContext bindingContext, SqlIdentifier name, LexicalScope lexicalScope)
	{
		return bindingContext.FindLogin(name.Value, lexicalScope);
	}

	void IDropObjectStatement<ILogin, SqlIdentifier>.ExecuteDrop(BatchBindingContext bindingContext, ILogin dropObject, LexicalScope lexicalScope)
	{
		bindingContext.GetServer().Logins.Drop(dropObject, lexicalScope);
	}

	internal override IMetadataCollection<IMetadataObject> GetCompletionList(LexLocation caretLocation, CompletionContext completionContext)
	{
		return base.GetCompletionList(caretLocation, CompletionContext.Logins);
	}

	internal SqlDropLoginStatement(SqlIdentifier loginName)
	{
		this.loginName = loginName;
	}
}
