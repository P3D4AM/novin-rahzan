using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.SqlServer.Management.Assessment.Tools;

internal interface ICmdShellTool : ITool
{
	Task<List<string>> ExecuteLines(string script);

	Task<LinesReader> ExecuteTextReader(string script);
}
