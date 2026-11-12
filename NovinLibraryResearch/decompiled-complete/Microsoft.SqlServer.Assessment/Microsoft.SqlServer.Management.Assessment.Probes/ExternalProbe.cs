using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Assessment.Configuration;
using Microsoft.SqlServer.Management.Assessment.Properties;
using Newtonsoft.Json;

namespace Microsoft.SqlServer.Management.Assessment.Probes;

internal class ExternalProbe : IProbe
{
	public SqlObjectLocatorPattern Target { get; set; }

	public string FamilyName { get; set; }

	[JsonProperty("transform")]
	public IDataMorph Morph { get; set; }

	public ICharacteristicCollection Subject { get; }

	public ICharacteristicCollection Requirements { get; }

	public IEnumerable<Type> ToolTypes => Enumerable.Empty<Type>();

	public Type ImplementationType { get; set; }

	public string Configuration { get; set; }

	public async Task<IDataView> GetDataAsync(RunSpace runSpace, IReadOnlyDictionary<string, object> parameters)
	{
		if (!(Activator.CreateInstance(ImplementationType) is IProbeImplementation probeImplementation))
		{
			throw new SqlAssessmentProbeException(Resources.ErrorProbeInitialize, ImplementationType.FullName);
		}
		if (!string.IsNullOrWhiteSpace(Configuration))
		{
			JsonConvert.PopulateObject(Configuration, probeImplementation);
		}
		probeImplementation.ServerName = runSpace.Target?.ServerName;
		probeImplementation.TargetName = runSpace.Target?.Name;
		probeImplementation.Urn = runSpace.Target?.Urn;
		probeImplementation.Parameters = parameters;
		return await probeImplementation.GetDataAsync().ConfigureAwait(continueOnCapturedContext: false);
	}
}
