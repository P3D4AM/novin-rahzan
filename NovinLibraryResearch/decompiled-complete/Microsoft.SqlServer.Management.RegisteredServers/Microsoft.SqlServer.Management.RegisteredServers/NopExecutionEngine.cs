using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.RegisteredServers;

internal class NopExecutionEngine : ISfcExecutionEngine
{
	public object Execute(ISfcScript script)
	{
		return null;
	}
}
