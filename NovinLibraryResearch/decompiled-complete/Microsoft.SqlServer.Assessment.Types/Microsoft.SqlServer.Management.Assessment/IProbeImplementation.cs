using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.SqlServer.Management.Assessment;

public interface IProbeImplementation
{
	string ServerName { get; set; }

	string TargetName { get; set; }

	string Urn { get; set; }

	IReadOnlyDictionary<string, object> Parameters { get; set; }

	Task<IDataView> GetDataAsync();
}
