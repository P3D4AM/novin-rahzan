using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.SqlServer.Management.Assessment.Tools;

internal interface IPowerShellTool : ITool
{
	Task<IDataView> Execute(string script, IReadOnlyDictionary<string, object> parameters);
}
