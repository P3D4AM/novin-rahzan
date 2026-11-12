using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlDropTypeStatement : SqlDropStatement, IDropObjectStatement<IUserDefinedType, SqlObjectIdentifier>
{
	private SqlObjectIdentifier typeName;

	internal override bool IsVisitable => true;

	internal sealed override string StatementPhrase => "DROP TYPE";

	internal sealed override DdlTargetObjectType TargetObjectType => DdlTargetObjectType.Type;

	ISqlCodeCollection<SqlObjectIdentifier> IDropObjectStatement<IUserDefinedType, SqlObjectIdentifier>.DropObjects => new SqlObjectIdentifierCollection(new SqlObjectIdentifier[1] { TypeName });

	public SqlObjectIdentifier TypeName => typeName;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return typeName;
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

	IUserDefinedType IDropObjectStatement<IUserDefinedType, SqlObjectIdentifier>.FindMetadataObject(BatchBindingContext bindingContext, SqlObjectIdentifier name, LexicalScope lexicalScope)
	{
		return bindingContext.FindUserDefinedType(name.SchemaName.Value, name.ObjectName.Value, lexicalScope);
	}

	void IDropObjectStatement<IUserDefinedType, SqlObjectIdentifier>.ExecuteDrop(BatchBindingContext bindingContext, IUserDefinedType dropObject, LexicalScope lexicalScope)
	{
		SchemaEx schemaEx = bindingContext.SchemaToSchemaEx(dropObject.Schema);
		if (dropObject is IUserDefinedClrType droppedObject)
		{
			schemaEx.UserDefinedClrTypes.Drop(droppedObject, lexicalScope);
		}
		else if (dropObject is IUserDefinedDataType droppedObject2)
		{
			schemaEx.UserDefinedDataTypes.Drop(droppedObject2, lexicalScope);
		}
		else if (dropObject is IUserDefinedTableType droppedObject3)
		{
			schemaEx.UserDefinedTableTypes.Drop(droppedObject3, lexicalScope);
		}
	}

	internal SqlDropTypeStatement(SqlObjectIdentifier typeName)
	{
		this.typeName = typeName;
	}
}
