using System;
using System.Collections.Generic;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public sealed class SqlDropSchemaStatement : SqlDropStatement, IDropObjectStatement<ISchema, SqlIdentifier>
{
	private readonly SqlIdentifier schemaName;

	internal override bool IsVisitable => true;

	internal sealed override string StatementPhrase => "DROP SCHEMA";

	internal sealed override DdlTargetObjectType TargetObjectType => DdlTargetObjectType.Schema;

	ISqlCodeCollection<SqlIdentifier> IDropObjectStatement<ISchema, SqlIdentifier>.DropObjects => new SqlIdentifierCollection(new SqlIdentifier[1] { schemaName });

	public SqlIdentifier SchemaName => schemaName;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return schemaName;
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

	ISchema IDropObjectStatement<ISchema, SqlIdentifier>.FindMetadataObject(BatchBindingContext bindingContext, SqlIdentifier name, LexicalScope lexicalScope)
	{
		return bindingContext.FindSchema(null, name.Value, lexicalScope);
	}

	void IDropObjectStatement<ISchema, SqlIdentifier>.ExecuteDrop(BatchBindingContext bindingContext, ISchema dropObject, LexicalScope lexicalScope)
	{
	}

	internal override IMetadataCollection<IMetadataObject> GetCompletionList(LexLocation caretLocation, CompletionContext completionContext)
	{
		return base.GetCompletionList(caretLocation, CompletionContext.Schemas);
	}

	internal SqlDropSchemaStatement(SqlIdentifier schemaName)
	{
		this.schemaName = schemaName;
	}
}
