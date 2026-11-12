using System;
using System.Collections.Generic;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlDropFunctionStatement : SqlDropStatement, IDropObjectStatement<IUserDefinedFunction, SqlObjectIdentifier>
{
	private SqlObjectIdentifierCollection objects;

	internal override bool IsVisitable => true;

	internal sealed override string StatementPhrase => "DROP FUNCTION";

	internal sealed override DdlTargetObjectType TargetObjectType => DdlTargetObjectType.Function;

	ISqlCodeCollection<SqlObjectIdentifier> IDropObjectStatement<IUserDefinedFunction, SqlObjectIdentifier>.DropObjects => objects;

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

	IUserDefinedFunction IDropObjectStatement<IUserDefinedFunction, SqlObjectIdentifier>.FindMetadataObject(BatchBindingContext bindingContext, SqlObjectIdentifier name, LexicalScope lexicalScope)
	{
		IUserDefinedFunction userDefinedFunction = bindingContext.FindScalarValuedFunction(name.DatabaseName.Value, name.SchemaName.Value, name.ObjectName.Value, lexicalScope);
		if (userDefinedFunction == null)
		{
			userDefinedFunction = bindingContext.FindTableValuedFunction(name.DatabaseName.Value, name.SchemaName.Value, name.ObjectName.Value, lexicalScope);
		}
		return userDefinedFunction;
	}

	void IDropObjectStatement<IUserDefinedFunction, SqlObjectIdentifier>.ExecuteDrop(BatchBindingContext bindingContext, IUserDefinedFunction dropObject, LexicalScope lexicalScope)
	{
		SchemaEx schemaEx = bindingContext.SchemaToSchemaEx(dropObject.Schema);
		if (dropObject is IScalarValuedFunction droppedObject)
		{
			schemaEx.ScalarValuedFunctions.Drop(droppedObject, lexicalScope);
		}
		else if (dropObject is ITableValuedFunction droppedObject2)
		{
			schemaEx.TableValuedFunctions.Drop(droppedObject2, lexicalScope);
		}
	}

	internal override IMetadataCollection<IMetadataObject> GetCompletionList(LexLocation caretLocation, CompletionContext completionContext)
	{
		return base.GetCompletionList(caretLocation, CompletionContext.DatabaseQualifiedScalarValuedFunctions | CompletionContext.TableValuedFunctions);
	}

	internal SqlDropFunctionStatement(List<SqlObjectIdentifier> objects)
	{
		this.objects = new SqlObjectIdentifierCollection(objects);
	}
}
