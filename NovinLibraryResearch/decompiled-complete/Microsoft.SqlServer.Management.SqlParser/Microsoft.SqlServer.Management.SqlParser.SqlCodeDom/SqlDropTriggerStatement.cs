using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlDropTriggerStatement : SqlDropStatement
{
	private sealed class DropDatabaseTriggerHelper : IDropObjectStatement<IDatabaseDdlTrigger, SqlObjectIdentifier>
	{
		private readonly SqlDropTriggerStatement dropStatement;

		ISqlCodeCollection<SqlObjectIdentifier> IDropObjectStatement<IDatabaseDdlTrigger, SqlObjectIdentifier>.DropObjects => dropStatement.objects;

		public DropDatabaseTriggerHelper(SqlDropTriggerStatement dropStatement)
		{
			this.dropStatement = dropStatement;
		}

		IDatabaseDdlTrigger IDropObjectStatement<IDatabaseDdlTrigger, SqlObjectIdentifier>.FindMetadataObject(BatchBindingContext bindingContext, SqlObjectIdentifier name, LexicalScope lexicalScope)
		{
			return bindingContext.FindDatabaseTrigger(name.ObjectName.Value, lexicalScope);
		}

		void IDropObjectStatement<IDatabaseDdlTrigger, SqlObjectIdentifier>.ExecuteDrop(BatchBindingContext bindingContext, IDatabaseDdlTrigger dropObject, LexicalScope lexicalScope)
		{
			bindingContext.DatabaseToDatabaseEx(dropObject.Database).Triggers.Drop(dropObject, lexicalScope);
		}
	}

	private sealed class DropServerTriggerHelper : IDropObjectStatement<IServerDdlTrigger, SqlObjectIdentifier>
	{
		private readonly SqlDropTriggerStatement dropStatement;

		ISqlCodeCollection<SqlObjectIdentifier> IDropObjectStatement<IServerDdlTrigger, SqlObjectIdentifier>.DropObjects => dropStatement.objects;

		public DropServerTriggerHelper(SqlDropTriggerStatement dropStatement)
		{
			this.dropStatement = dropStatement;
		}

		IServerDdlTrigger IDropObjectStatement<IServerDdlTrigger, SqlObjectIdentifier>.FindMetadataObject(BatchBindingContext bindingContext, SqlObjectIdentifier name, LexicalScope lexicalScope)
		{
			return bindingContext.FindServerTrigger(name.ObjectName.Value, lexicalScope);
		}

		void IDropObjectStatement<IServerDdlTrigger, SqlObjectIdentifier>.ExecuteDrop(BatchBindingContext bindingContext, IServerDdlTrigger dropObject, LexicalScope lexicalScope)
		{
			bindingContext.GetServer().Triggers.Drop(dropObject, lexicalScope);
		}
	}

	private SqlDropTriggerTargetScope targetScope;

	private SqlObjectIdentifierCollection objects;

	internal override bool IsVisitable => true;

	internal sealed override string StatementPhrase => "DROP TRIGGER";

	internal sealed override DdlTargetObjectType TargetObjectType => DdlTargetObjectType.Trigger;

	public SqlDropTriggerTargetScope TargetScope => targetScope;

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
		switch (targetScope)
		{
		case SqlDropTriggerTargetScope.None:
			base.BindingState |= BindingState.BindFailed;
			break;
		case SqlDropTriggerTargetScope.AllServer:
			SqlDropStatement.BindDropObjectStatement(this, DropObjectBinderHelper.GetStandardHelper(new DropServerTriggerHelper(this)), bindingContext);
			break;
		case SqlDropTriggerTargetScope.Database:
			SqlDropStatement.BindDropObjectStatement(this, DropObjectBinderHelper.GetStandardHelper(new DropDatabaseTriggerHelper(this)), bindingContext);
			break;
		}
	}

	internal SqlDropTriggerStatement(List<SqlObjectIdentifier> objects, SqlDropTriggerTargetScope targetScope)
	{
		this.objects = new SqlObjectIdentifierCollection(objects);
		this.targetScope = targetScope;
	}
}
