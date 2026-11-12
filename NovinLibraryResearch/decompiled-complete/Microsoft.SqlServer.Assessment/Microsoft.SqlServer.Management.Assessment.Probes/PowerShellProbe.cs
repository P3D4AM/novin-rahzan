using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Assessment.Tools;
using Newtonsoft.Json;

namespace Microsoft.SqlServer.Management.Assessment.Probes;

internal class PowerShellProbe : SingleToolProbe<IPowerShellTool>
{
	[JsonProperty("command")]
	public string CommandText { get; set; }

	protected override Task<IDataView> GetDataImpl(RunSpace runSpace, IPowerShellTool tool, IReadOnlyDictionary<string, object> parameters)
	{
		if (string.IsNullOrWhiteSpace(CommandText))
		{
			return Task.FromResult((IDataView)new MeasurementCollection());
		}
		return tool.Execute(CommandText, parameters);
	}
}
