using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlCreateSchemaStatement : SqlDdlStatement
{
	private IMutableSchema boundSchema;

	private readonly SqlIdentifier name;

	private readonly SqlIdentifier owner;

	private readonly SqlSchemaElementCollection schemaElements;

	internal override bool IsVisitable => true;

	internal sealed override string StatementPhrase => "CREATE SCHEMA";

	internal sealed override bool IsCreateStatement => true;

	internal sealed override bool IsAlterStatement => false;

	internal sealed override bool IsDropStatement => false;

	internal sealed override DdlTargetObjectType TargetObjectType => DdlTargetObjectType.Schema;

	public ISchema BoundSchema => boundSchema;

	public override IMetadataObject BoundObject => boundSchema;

	public SqlIdentifier Name => name;

	public SqlIdentifier Owner => owner;

	public SqlSchemaElementCollection SchemaElements => schemaElements;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			if (name != null)
			{
				yield return name;
			}
			if (owner != null)
			{
				yield return owner;
			}
			if (schemaElements == null)
			{
				yield break;
			}
			foreach (SqlStatement schemaElement in schemaElements)
			{
				yield return schemaElement;
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

	internal override void InitiateBindStatement(BatchBindingContext bindingContext, StatementBindMode bindMode)
	{
		if (!SqlIdentifier.IsNullOrEmpty(name))
		{
			LexicalScope lexicalScope = GetLexicalScope();
			if (bindingContext.FindSchema(null, name.Value, lexicalScope) != null)
			{
				name.ReportBindError(SR.DatabaseObjectExist(name.Value));
				base.BindingState |= BindingState.BindFailed;
				return;
			}
			DatabaseEx connectedDatabase = bindingContext.ConnectedDatabase;
			boundSchema = bindingContext.MetadataFactory.Schema.Create(connectedDatabase.ProxyObject, name.Value);
			connectedDatabase.Schemas.Create(boundSchema, this, bindMode);
			base.BindingState |= BindingState.BindSucceeded;
		}
		else
		{
			base.BindingState |= BindingState.BindFailed;
		}
	}

	internal override void FinalizeBindStatement(BatchBindingContext bindingContext, StatementBindMode bindMode)
	{
		IDatabasePrincipal databasePrincipal;
		if (!SqlIdentifier.IsNullOrEmpty(owner))
		{
			databasePrincipal = bindingContext.FindDatabasePrincipal(Owner.Value, GetLexicalScope());
			if (databasePrincipal == null)
			{
				owner.ReportBindError(SR.CannotFindUser(owner.Value), ErrorFlags.UnresolvedDatabaseObjectReference);
				base.BindingState |= BindingState.BindFailed;
			}
		}
		else
		{
			databasePrincipal = bindingContext.ConnectedDatabase.Owner;
		}
		if (boundSchema != null)
		{
			boundSchema.Owner = databasePrincipal;
		}
		if (schemaElements == null)
		{
			return;
		}
		SchemaEx schema = ((boundSchema == null) ? bindingContext.GetCDASchema(SqlIdentifier.RetrieveValue(name), GetLexicalScope()) : bindingContext.SchemaToSchemaEx(boundSchema));
		BatchBindingContext orCreateModuleBindingContext = bindingContext.GetOrCreateModuleBindingContext(schema);
		foreach (SqlStatement schemaElement in schemaElements)
		{
			schemaElement.Bind(orCreateModuleBindingContext);
			base.BindingState |= schemaElement.BindingState;
		}
	}

	internal override void Unbind()
	{
		boundSchema = null;
		base.Unbind();
	}

	internal SqlCreateSchemaStatement(SqlIdentifier name, SqlIdentifier owner, IEnumerable<SqlStatement> schemaElements)
	{
		if (name == null && owner == null)
		{
			ReportParseError(SR.NameOrAuthorizationKeywordRequired);
		}
		this.name = name;
		this.owner = owner;
		if (schemaElements != null)
		{
			this.schemaElements = new SqlSchemaElementCollection(schemaElements);
		}
	}
}
