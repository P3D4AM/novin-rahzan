using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlCreateRoleStatement : SqlDdlStatement
{
	private IMutableDatabaseRole boundRole;

	private readonly SqlIdentifier name;

	private readonly SqlIdentifier owner;

	internal override bool IsVisitable => true;

	internal sealed override string StatementPhrase => "CREATE ROLE";

	internal sealed override bool IsCreateStatement => true;

	internal sealed override bool IsAlterStatement => false;

	internal sealed override bool IsDropStatement => false;

	internal sealed override DdlTargetObjectType TargetObjectType => DdlTargetObjectType.Role;

	public IDatabaseRole BoundRole => boundRole;

	public override IMetadataObject BoundObject => boundRole;

	public SqlIdentifier Name => name;

	public SqlIdentifier Owner => owner;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return name;
			if (owner != null)
			{
				yield return owner;
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
		DatabaseEx connectedDatabase = bindingContext.ConnectedDatabase;
		boundRole = bindingContext.MetadataFactory.Role.CreateDatabaseRole(connectedDatabase.ProxyObject, name.Value);
		LexicalScope lexicalScope = GetLexicalScope();
		if (bindingContext.FindDatabasePrincipal(name.Value, lexicalScope) != null)
		{
			name.ReportBindError(SR.UserGroupOrRoleExists(name.Value));
			base.BindingState |= BindingState.BindFailed;
			return;
		}
		if (!name.IsEmpty)
		{
			connectedDatabase.Roles.Create(boundRole, this, bindMode);
		}
		base.BindingState |= BindingState.BindSucceeded;
	}

	internal override void FinalizeBindStatement(BatchBindingContext bindingContext, StatementBindMode bindMode)
	{
		if (!SqlIdentifier.IsNull(owner) && !owner.IsEmpty)
		{
			boundRole.Owner = bindingContext.FindDatabasePrincipal(owner.Value, GetLexicalScope());
			if (boundRole.Owner != null)
			{
				bindingContext.SetBoundObject(owner, boundRole.Owner);
				return;
			}
			owner.ReportBindError(SR.CannotFindUser(owner.Value), ErrorFlags.UnresolvedDatabaseObjectReference);
			base.BindingState |= BindingState.BindFailed;
		}
	}

	internal override void Unbind()
	{
		boundRole = null;
		base.Unbind();
	}

	internal SqlCreateRoleStatement(SqlIdentifier name, SqlIdentifier owner)
	{
		this.name = name;
		this.owner = owner;
	}
}
