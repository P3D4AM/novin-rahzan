using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Assessment.Tools;
using Newtonsoft.Json;

namespace Microsoft.SqlServer.Management.Assessment.Probes;

internal class KustoProbe : SingleToolProbe<IAzureTool>
{
	[JsonProperty]
	public string Query { get; set; }

	protected override Task<IDataView> GetDataImpl(RunSpace runSpace, IAzureTool tool, IReadOnlyDictionary<string, object> parameters)
	{
		if (string.IsNullOrWhiteSpace(Query))
		{
			return Task.FromResult((IDataView)new MeasurementCollection());
		}
		return tool.Query(Query, parameters);
	}
}
