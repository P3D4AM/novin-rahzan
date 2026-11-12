using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.SqlServer.Management.Assessment.Tools;

public interface IAzureTool : ITool
{
	Task<IDataView> Query(string kustoQuery);

	Task<IDataView> Query(string query, IReadOnlyDictionary<string, object> parameters);
}
