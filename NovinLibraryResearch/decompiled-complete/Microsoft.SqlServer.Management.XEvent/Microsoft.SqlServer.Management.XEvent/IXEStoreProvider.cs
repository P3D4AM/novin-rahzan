using System.Collections.Generic;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.XEvent;

public interface IXEStoreProvider
{
	string DomainInstanceName { get; }

	ISfcExecutionEngine GetExecutionEngine();

	ISfcConnection GetConnection(SfcObjectQueryMode mode);

	IComparer<string> GetComparer();
}
