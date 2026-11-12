using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.SqlServer.Management.Assessment.Tools;

internal interface IRegistryTool : ITool
{
	Task<List<Dictionary<string, object>>> Execute(string hive, string key, IReadOnlyList<string> valueNames, bool instance);

	Task<Dictionary<string, string>> EnumKeys(string hive, string key, bool instance);
}
