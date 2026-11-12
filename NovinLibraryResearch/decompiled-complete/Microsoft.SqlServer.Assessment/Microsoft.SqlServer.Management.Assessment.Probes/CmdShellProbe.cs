using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Assessment.Tools;
using Newtonsoft.Json;

namespace Microsoft.SqlServer.Management.Assessment.Probes;

internal class CmdShellProbe : SingleToolProbe<ICmdShellTool>
{
	internal const string LineColumnName = "stdout";

	[JsonProperty("command")]
	public string CommandText { get; set; }

	protected override async Task<IDataView> GetDataImpl(RunSpace runSpace, ICmdShellTool tool, IReadOnlyDictionary<string, object> parameters)
	{
		return new LinesView(await tool.ExecuteLines(CommandText).ConfigureAwait(continueOnCapturedContext: false), "stdout");
	}
}
