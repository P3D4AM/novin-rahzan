using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Assessment.Tools;
using Newtonsoft.Json;

namespace Microsoft.SqlServer.Management.Assessment.Probes;

internal class WmiProbe : SingleToolProbe<IWmiTool>
{
	[JsonProperty]
	public string Query { get; set; }

	[JsonProperty]
	public Dictionary<string, string> Methods { get; set; }

	protected override Task<IDataView> GetDataImpl(RunSpace runSpace, IWmiTool tool, IReadOnlyDictionary<string, object> parameters)
	{
		if (string.IsNullOrWhiteSpace(Query))
		{
			return Task.FromResult((IDataView)new MeasurementCollection());
		}
		return tool.Execute(Query, Methods, parameters);
	}
}
