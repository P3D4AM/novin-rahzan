using System;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlExecuteAsClause : SqlCodeObject
{
	private sealed class ExecuteAsCaller : SqlExecuteAsClause
	{
		public ExecuteAsCaller()
			: base(SqlModuleExecutionContextType.Caller)
		{
		}

		public override string ToString()
		{
			return "CALLER";
		}
	}

	private sealed class ExecuteAsOwner : SqlExecuteAsClause
	{
		public ExecuteAsOwner()
			: base(SqlModuleExecutionContextType.Owner)
		{
		}

		public override string ToString()
		{
			return "OWNER";
		}
	}

	private sealed class ExecuteAsSelf : SqlExecuteAsClause
	{
		public ExecuteAsSelf()
			: base(SqlModuleExecutionContextType.Self)
		{
		}

		public override string ToString()
		{
			return "SELF";
		}
	}

	private sealed class ExecuteAsUser : SqlExecuteAsClause
	{
		private readonly string userName;

		public override string UserName => userName;

		public ExecuteAsUser(string userName)
			: base(SqlModuleExecutionContextType.User)
		{
			this.userName = userName;
		}

		public override string ToString()
		{
			return "'" + userName + "'";
		}
	}

	private sealed class InvalidExecuteAs : SqlExecuteAsClause
	{
		private string name;

		internal override bool IsValid => false;

		public InvalidExecuteAs(string name)
			: base(SqlModuleExecutionContextType.Caller)
		{
			this.name = name;
		}

		public override string ToString()
		{
			return name;
		}
	}

	private IExecutionContext executionContext;

	private readonly SqlModuleExecutionContextType type;

	internal override bool IsVisitable => true;

	public override IMetadataObject BoundObject => executionContext;

	public IExecutionContext BoundExecutionContext => executionContext;

	public SqlModuleExecutionContextType Type => type;

	public virtual string UserName => null;

	internal virtual bool IsValid => true;

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

	internal override void Bind(IBindingContext bindingContext)
	{
		BatchBindingContext batch = bindingContext.Batch;
		IExecutionContextFactory executionContextFactory = batch.MetadataFactory.ExecutionContext;
		switch (Type)
		{
		case SqlModuleExecutionContextType.Caller:
			executionContext = executionContextFactory.CreateExecuteAsCaller();
			break;
		case SqlModuleExecutionContextType.Owner:
			executionContext = executionContextFactory.CreateExecuteAsOwner();
			break;
		case SqlModuleExecutionContextType.Self:
			executionContext = executionContextFactory.CreateExecuteAsSelf();
			break;
		case SqlModuleExecutionContextType.User:
		{
			IUser user = batch.FindUser(UserName, GetLexicalScope());
			if (user != null)
			{
				executionContext = executionContextFactory.CreateExecuteAsUser(user);
			}
			else
			{
				ReportBindError(SR.CannotFindUser(UserName), ErrorFlags.UnresolvedDatabaseObjectReference);
			}
			break;
		}
		}
		base.BindingState |= (BindingState)((executionContext != null) ? 1 : 2);
	}

	internal override void Unbind()
	{
		executionContext = null;
		base.Unbind();
	}

	private SqlExecuteAsClause(SqlModuleExecutionContextType type)
	{
		this.type = type;
	}

	internal static SqlExecuteAsClause Caller()
	{
		return new ExecuteAsCaller();
	}

	internal static SqlExecuteAsClause Owner()
	{
		return new ExecuteAsOwner();
	}

	internal static SqlExecuteAsClause Self()
	{
		return new ExecuteAsSelf();
	}

	internal static SqlExecuteAsClause User(SqlLiteralExpression literalExpression)
	{
		return new ExecuteAsUser(literalExpression.Value);
	}

	internal static SqlExecuteAsClause Invalid(string name)
	{
		return new InvalidExecuteAs(name);
	}
}
