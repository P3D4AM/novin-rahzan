using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public interface IExecutionContextFactory
{
	IExecutionContext CreateExecuteAsCaller();

	IExecutionContext CreateExecuteAsLogin(ILogin login);

	IExecutionContext CreateExecuteAsOwner();

	IExecutionContext CreateExecuteAsSelf();

	IExecutionContext CreateExecuteAsUser(IUser user);
}
