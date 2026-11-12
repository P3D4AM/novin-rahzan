using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlDropSynonymStatement : SqlDropStatement, IDropObjectStatement<ISynonym, SqlObjectIdentifier>
{
	private SqlObjectIdentifierCollection objects;

	internal override bool IsVisitable => true;

	internal sealed override string StatementPhrase => "DROP SYNONYM";

	internal sealed override DdlTargetObjectType TargetObjectType => DdlTargetObjectType.Synonym;

	ISqlCodeCollection<SqlObjectIdentifier> IDropObjectStatement<ISynonym, SqlObjectIdentifier>.DropObjects => objects;

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

	ISynonym IDropObjectStatement<ISynonym, SqlObjectIdentifier>.FindMetadataObject(BatchBindingContext bindingContext, SqlObjectIdentifier name, LexicalScope lexicalScope)
	{
		return bindingContext.FindSynonym(null, name.SchemaName.Value, name.ObjectName.Value, lexicalScope);
	}

	void IDropObjectStatement<ISynonym, SqlObjectIdentifier>.ExecuteDrop(BatchBindingContext bindingContext, ISynonym dropObject, LexicalScope lexicalScope)
	{
		bindingContext.SchemaToSchemaEx(dropObject.Schema).Synonyms.Drop(dropObject, lexicalScope);
	}

	internal SqlDropSynonymStatement(List<SqlObjectIdentifier> objects)
	{
		this.objects = new SqlObjectIdentifierCollection(objects);
		foreach (SqlObjectIdentifier @object in this.objects)
		{
			if (!SqlIdentifier.IsNullOrEmpty(@object.DatabaseName))
			{
				@object.DatabaseName.ReportParseError(SR.DbNameIsNotAllowedForDropSynonym);
			}
		}
	}
}
