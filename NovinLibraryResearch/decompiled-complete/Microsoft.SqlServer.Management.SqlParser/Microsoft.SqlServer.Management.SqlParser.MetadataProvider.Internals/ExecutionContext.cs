using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class ExecutionContext : MetadataObjectBase, IExecutionContext, IMetadataObject
{
	private readonly ExecutionContextType m_contextType;

	private readonly IUser m_user;

	private readonly ILogin m_login;

	public ExecutionContextType ContextType => m_contextType;

	public ILogin Login => m_login;

	public IUser User => m_user;

	public ExecutionContext(ExecutionContextType contextType)
		: this(contextType, null, null)
	{
	}

	public ExecutionContext(IUser user)
		: this(ExecutionContextType.ExecuteAsUser, user, null)
	{
	}

	public ExecutionContext(ILogin login)
		: this(ExecutionContextType.ExecuteAsLogin, null, login)
	{
	}

	private ExecutionContext(ExecutionContextType contextType, IUser user, ILogin login)
		: base(string.Empty)
	{
		m_contextType = contextType;
		m_user = user;
		m_login = login;
	}

	public override T Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}
}
