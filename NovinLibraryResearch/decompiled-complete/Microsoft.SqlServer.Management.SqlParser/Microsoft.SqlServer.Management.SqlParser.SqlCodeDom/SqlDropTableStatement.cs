using System;
using System.Collections.Generic;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlDropTableStatement : SqlDropStatement, IDropObjectStatement<ITable, SqlObjectIdentifier>
{
	private SqlObjectIdentifierCollection objects;

	internal override bool IsVisitable => true;

	internal sealed override string StatementPhrase => "DROP TABLE";

	internal sealed override DdlTargetObjectType TargetObjectType => DdlTargetObjectType.Table;

	ISqlCodeCollection<SqlObjectIdentifier> IDropObjectStatement<ITable, SqlObjectIdentifier>.DropObjects => objects;

	public SqlObjectIdentifierCollection Objects => objects;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			foreach (SqlObjectIdentifier @object in objects)
			{
				yield return @object;
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

	internal override void BindStatement(BatchBindingContext bindingContext)
	{
		SqlDropStatement.BindDropObjectStatement(this, DropObjectBinderHelper.GetSchemaOwnedObjectHelper(this), bindingContext);
	}

	ITable IDropObjectStatement<ITable, SqlObjectIdentifier>.FindMetadataObject(BatchBindingContext bindingContext, SqlObjectIdentifier name, LexicalScope lexicalScope)
	{
		return BindUtils.Table.AsTable(bindingContext.FindTableOrView(name.DatabaseName.Value, name.SchemaName.Value, name.ObjectName.Value, lexicalScope));
	}

	void IDropObjectStatement<ITable, SqlObjectIdentifier>.ExecuteDrop(BatchBindingContext bindingContext, ITable dropObject, LexicalScope lexicalScope)
	{
		bindingContext.SchemaToSchemaEx(dropObject.Schema).Tables.Drop(dropObject, lexicalScope);
	}

	internal override IMetadataCollection<IMetadataObject> GetCompletionList(LexLocation caretLocation, CompletionContext completionContext)
	{
		return base.GetCompletionList(caretLocation, CompletionContext.DatabaseQualifiedTables);
	}

	internal SqlDropTableStatement(List<SqlObjectIdentifier> objects)
	{
		this.objects = new SqlObjectIdentifierCollection(objects);
	}
}
