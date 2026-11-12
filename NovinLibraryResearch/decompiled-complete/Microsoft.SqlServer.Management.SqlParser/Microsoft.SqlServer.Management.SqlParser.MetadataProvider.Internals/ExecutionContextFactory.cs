using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class ExecutionContextFactory : ObjectFactoryBase<ExecutionContextFactory>, IExecutionContextFactory
{
	private readonly ExecutionContext m_caller;

	private readonly ExecutionContext m_owner;

	private readonly ExecutionContext m_self;

	public ExecutionContextFactory()
	{
		m_caller = new ExecutionContext(ExecutionContextType.Caller);
		m_owner = new ExecutionContext(ExecutionContextType.Owner);
		m_self = new ExecutionContext(ExecutionContextType.Self);
	}

	public IExecutionContext CreateExecuteAsCaller()
	{
		return m_caller;
	}

	public IExecutionContext CreateExecuteAsLogin(ILogin login)
	{
		if (login == null)
		{
			throw new ArgumentNullException("login");
		}
		return new ExecutionContext(login);
	}

	public IExecutionContext CreateExecuteAsOwner()
	{
		return m_owner;
	}

	public IExecutionContext CreateExecuteAsSelf()
	{
		return m_self;
	}

	public IExecutionContext CreateExecuteAsUser(IUser user)
	{
		if (user == null)
		{
			throw new ArgumentNullException("user");
		}
		return new ExecutionContext(user);
	}
}
