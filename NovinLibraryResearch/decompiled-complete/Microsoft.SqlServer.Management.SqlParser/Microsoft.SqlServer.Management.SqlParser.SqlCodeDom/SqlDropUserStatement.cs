using System;
using System.Collections.Generic;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlDropUserStatement : SqlDropStatement, IDropObjectStatement<IUser, SqlIdentifier>
{
	private SqlIdentifier userName;

	internal override bool IsVisitable => true;

	internal sealed override string StatementPhrase => "DROP USER";

	internal sealed override DdlTargetObjectType TargetObjectType => DdlTargetObjectType.User;

	ISqlCodeCollection<SqlIdentifier> IDropObjectStatement<IUser, SqlIdentifier>.DropObjects => new SqlIdentifierCollection(new SqlIdentifier[1] { userName });

	public SqlIdentifier UserName => userName;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return userName;
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

	IUser IDropObjectStatement<IUser, SqlIdentifier>.FindMetadataObject(BatchBindingContext bindingContext, SqlIdentifier name, LexicalScope lexicalScope)
	{
		return bindingContext.FindUser(userName.Value, lexicalScope);
	}

	void IDropObjectStatement<IUser, SqlIdentifier>.ExecuteDrop(BatchBindingContext bindingContext, IUser dropObject, LexicalScope lexicalScope)
	{
		bindingContext.DatabaseToDatabaseEx(dropObject.Database).Users.Drop(dropObject, lexicalScope);
	}

	internal override IMetadataCollection<IMetadataObject> GetCompletionList(LexLocation caretLocation, CompletionContext completionContext)
	{
		return base.GetCompletionList(caretLocation, CompletionContext.Users);
	}

	internal SqlDropUserStatement(SqlIdentifier userName)
	{
		this.userName = userName;
	}
}
