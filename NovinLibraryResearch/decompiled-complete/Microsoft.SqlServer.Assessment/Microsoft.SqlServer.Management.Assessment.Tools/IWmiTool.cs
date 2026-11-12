using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.SqlServer.Management.Assessment.Tools;

internal interface IWmiTool : ITool
{
	Task<IDataView> Execute(string query, IReadOnlyDictionary<string, string> methods, IReadOnlyDictionary<string, object> parameters);
}
