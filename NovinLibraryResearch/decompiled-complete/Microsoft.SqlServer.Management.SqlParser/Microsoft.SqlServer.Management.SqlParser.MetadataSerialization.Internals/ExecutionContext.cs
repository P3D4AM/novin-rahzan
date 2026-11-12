using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class ExecutionContext : IExecutionContext, IMetadataObject
{
	private readonly DeserializationModel model;

	private readonly ExecutionContextType contextType;

	private readonly int login;

	private readonly int user;

	ExecutionContextType IExecutionContext.ContextType => contextType;

	ILogin IExecutionContext.Login => model.GetObjectById<ILogin>(login);

	IUser IExecutionContext.User => model.GetObjectById<IUser>(user);

	string IMetadataObject.Name => null;

	public ExecutionContext(DeserializationModel model, ExecutionContextType contextType, int login, int user)
	{
		this.model = model;
		this.contextType = contextType;
		this.login = login;
		this.user = user;
	}

	public ExecutionContext(DeserializationModel model, object[] values)
		: this(model, (ExecutionContextType)values[0], (int)values[1], (int)values[2])
	{
	}

	T IMetadataObject.Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}
}
